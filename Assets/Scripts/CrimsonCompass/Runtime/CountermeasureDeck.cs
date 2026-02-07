using System;
using System.Collections.Generic;

namespace CrimsonCompass.Runtime
{
    public enum SnapType { CountermeasureActivation, MajorIntrusion, GasketBoilover }

    [Serializable]
    public class CountermeasureCard
    {
        public string Id;
        public string Name;
        public SnapType SnapType;

        // Deterministic trigger predicate (no RNG)
        public Func<GameState, ChoiceContext, bool> Trigger;

        public int TimeDelta;
        public int HeatDelta;
        public LeadIntegrity LeadIntegrity;
        public FlagState Flag;
        public GasketState Gasket;

        public string UiToast;
        public string LogLine;
    }

    public struct ChoiceContext
    {
        public string EpisodeId;
        public int SceneId;
        public string ChoiceId;

        public bool IsNetworkPull;
        public bool IsPressAction;
        public bool IsProcedural;
        public bool IsGasketOption;
    }

    public class CountermeasureDeck
    {
        private readonly List<CountermeasureCard> _cards = new();
        public IReadOnlyList<CountermeasureCard> Cards => _cards;

        public void Add(CountermeasureCard card) => _cards.Add(card);

        public List<CountermeasureCard> EvaluateTriggers(GameState state, ChoiceContext ctx)
        {
            var triggered = new List<CountermeasureCard>();
            foreach (var c in _cards)
                if (c.Trigger != null && c.Trigger(state, ctx))
                    triggered.Add(c);
            return triggered;
        }
    }
}
