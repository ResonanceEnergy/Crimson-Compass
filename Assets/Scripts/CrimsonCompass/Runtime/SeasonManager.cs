using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CrimsonCompass.Runtime
{
    /// <summary>
    /// Deterministic season runtime: executes episodes/scenes/choices using state-driven rules,
    /// cross-off gating, warrant ritual, and countermeasure snaps.
    /// </summary>
    public class SeasonManager
    {
        public SeasonFlowState FlowState { get; private set; } = SeasonFlowState.Boot;
        public GameState State;

        public string CurrentEpisodeId { get; private set; }
        public int CurrentSceneId { get; private set; } = 1;

        public event Action<SeasonFlowState> OnFlowChanged;
        public event Action<SnapType, string> OnSnap;
        public event Action<HardFailReason> OnHardFail;
        public event Action<string> OnEpisodeEnded;

        private readonly CcSeason1RuntimeLoader _loader;
        private readonly CountermeasureDeck _deck;
        private CcSeason1RuntimeLoader.EpisodeData _episode;

        public SeasonManager(CcSeason1RuntimeLoader loader, CountermeasureDeck deck, GameState initialState)
        {
            _loader = loader;
            _deck = deck;
            State = initialState;
        }

        public async Task StartEpisodeAsync(string episodeId, bool verifySha = true)
        {
            SetFlow(SeasonFlowState.LoadingEpisode);
            CurrentEpisodeId = episodeId;
            CurrentSceneId = 1;

            _episode = await _loader.LoadEpisodeAsync(episodeId, verifySha256: verifySha, useCache: true);
            _episode.BuildIndexes();

            // Audio setup
            int episodeNumber = int.Parse(episodeId.Split('_')[1]);
            if (CCAudioContextProvider.Instance != null)
            {
                CCAudioContextProvider.Instance.EpisodeNumber = episodeNumber;
                CCAudioContextProvider.Instance.SetStateBands(State.Heat, MapTimeToBand(State.TimeRemaining), MapLeadIntegrityToBand(State.LeadIntegrity));
            }
            if (CCAudioDeltaApplier.Instance != null)
            {
                CCAudioDeltaApplier.Instance.ApplyEpisodeDelta(episodeNumber);
            }
            CCAudioCanonGuardrails.ResetForNewEpisode();

            SetFlow(SeasonFlowState.SceneActive);
        }

        public async Task ApplyChoiceAsync(int sceneId, string choiceId, ChoiceContext ctx)
        {
            if (FlowState != SeasonFlowState.SceneActive)
                throw new InvalidOperationException($"Cannot apply choice in state {FlowState}");

            SetFlow(SeasonFlowState.ChoiceResolving);

            if (!_episode.ChoiceByKey.TryGetValue((sceneId, choiceId), out var choice))
                throw new Exception($"Choice not found: {CurrentEpisodeId} scene {sceneId} choice {choiceId}");

            ApplyDeltas(choice.Deltas);

            // Update audio state bands
            if (CCAudioContextProvider.Instance != null)
            {
                CCAudioContextProvider.Instance.SetStateBands(State.Heat, MapTimeToBand(State.TimeRemaining), MapLeadIntegrityToBand(State.LeadIntegrity));
            }

            if (State.IsTimeOut())
            {
                TriggerHardFail(HardFailReason.TimeOut);
                return;
            }

            // Shadow effects are presently informational; prefer encoding effects as deltas or countermeasures.
            EvaluateShadowEffect(choice.ShadowEffect, ctx);

            var triggered = _deck.EvaluateTriggers(State, ctx);
            if (triggered.Count > 0)
            {
                SetFlow(SeasonFlowState.SnapHandling);
                foreach (var card in triggered)
                {
                    OnSnap?.Invoke(card.SnapType, card.UiToast);
                    ApplyCountermeasure(card);
                }

                if (State.IsTimeOut())
                {
                    TriggerHardFail(HardFailReason.TimeOut);
                    return;
                }
            }

            AdvanceSceneOrEnd(sceneId);
            await Task.Yield();
        }

        public void OpenWarrant()
        {
            if (!WarrantRules.CanOpenWarrant(State))
                throw new InvalidOperationException("Warrant not available at current WarrantPressure.");
            SetFlow(SeasonFlowState.WarrantRitual);
        }

        public void CommitWarrant(WarrantSelection selection, Func<WarrantSelection, (bool ok, HardFailReason? reason)> validator)
        {
            if (selection.Confidence == WarrantConfidence.Hold)
                State.TimeRemaining -= WarrantRules.HoldTimeCostSegments;

            if (State.IsTimeOut())
            {
                TriggerHardFail(HardFailReason.TimeOut);
                return;
            }

            var (ok, reason) = validator(selection);
            if (!ok)
            {
                TriggerHardFail(reason ?? HardFailReason.WrongWarrant_WHO);
                return;
            }

            SetFlow(SeasonFlowState.EpisodeEnd);
            OnEpisodeEnded?.Invoke(CurrentEpisodeId);
        }

        private void ApplyDeltas(CcSeason1RuntimeLoader.DeltaData d)
        {
            State.TimeRemaining += d.Time;
            State.Heat = Mathf.Clamp(State.Heat + d.Heat, 0, 100);

            if (!string.IsNullOrEmpty(d.LeadIntegrity) && d.LeadIntegrity != "no_change")
                State.LeadIntegrity = ParseLead(d.LeadIntegrity);

            if (!string.IsNullOrEmpty(d.Gasket) && d.Gasket != "no_change")
                State.Gasket = ParseGasket(d.Gasket);

            if (!string.IsNullOrEmpty(d.Flag) && d.Flag != "no_change")
                State.Flag = ParseFlag(d.Flag);
        }

        private void ApplyCountermeasure(CountermeasureCard card)
        {
            State.TimeRemaining += card.TimeDelta;
            State.Heat = Mathf.Clamp(State.Heat + card.HeatDelta, 0, 100);

            if (card.LeadIntegrity != LeadIntegrity.NoChange)
                State.LeadIntegrity = card.LeadIntegrity;
            if (card.Gasket != GasketState.NoChange)
                State.Gasket = card.Gasket;
            if (card.Flag != FlagState.NoChange)
                State.Flag = card.Flag;
        }

        private void EvaluateShadowEffect(string shadowEffect, ChoiceContext ctx)
        {
            if (string.IsNullOrWhiteSpace(shadowEffect)) return;
            Debug.Log($"[ShadowEffect] {CurrentEpisodeId} S{ctx.SceneId} {ctx.ChoiceId}: {shadowEffect}");
        }

        private void AdvanceSceneOrEnd(int currentScene)
        {
            int nextScene = currentScene + 1;

            if (_episode.SceneById.ContainsKey(nextScene))
            {
                CurrentSceneId = nextScene;
                SetFlow(SeasonFlowState.SceneActive);
                return;
            }

            SetFlow(SeasonFlowState.CrossOff);
            SetFlow(SeasonFlowState.EpisodeEnd);
            OnEpisodeEnded?.Invoke(CurrentEpisodeId);
        }

        private void TriggerHardFail(HardFailReason reason)
        {
            SetFlow(SeasonFlowState.HardFail);
            OnHardFail?.Invoke(reason);
            SetFlow(SeasonFlowState.SeasonEnd);
        }

        private void SetFlow(SeasonFlowState next)
        {
            FlowState = next;
            OnFlowChanged?.Invoke(next);
        }

        private static LeadIntegrity ParseLead(string s) => s switch
        {
            "clean" => LeadIntegrity.Clean,
            "tainted" => LeadIntegrity.Tainted,
            "burned" => LeadIntegrity.Burned,
            _ => LeadIntegrity.NoChange
        };

        private static GasketState ParseGasket(string s) => s switch
        {
            "contained" => GasketState.Contained,
            "uncontained" => GasketState.Uncontained,
            _ => GasketState.NoChange
        };

        private static FlagState ParseFlag(string s) => s switch
        {
            "none" => FlagState.None,
            "tailed" => FlagState.Tailed,
            "sticky_heat" => FlagState.StickyHeat,
            "route_collapsed" => FlagState.RouteCollapsed,
            _ => FlagState.NoChange
        };

        private static float MapTimeToBand(int timeRemaining)
        {
            // Assume max time is 10 segments, map to 0-100
            return Mathf.Clamp((timeRemaining / 10f) * 100f, 0, 100);
        }

        private static float MapLeadIntegrityToBand(LeadIntegrity integrity)
        {
            return integrity switch
            {
                LeadIntegrity.Clean => 100f,
                LeadIntegrity.Tainted => 66f,
                LeadIntegrity.Burned => 33f,
                _ => 50f
            };
        }
    }
}
