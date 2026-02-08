using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CrimsonCompass.Runtime;
using CrimsonCompass.Core;

namespace CrimsonCompass
{
    /// <summary>
    /// Executes episode content and manages game flow
    /// </summary>
    public class EpisodeRunner : MonoBehaviour
    {
        public static EpisodeRunner Instance;

        [Header("UI Components")]
        public EpisodeUI episodeUI;
        public ChoicePanel choicePanel;
        public BackgroundAbsurdityManager absurdityManager;

        private EpisodeDto currentEpisode;
        private int currentSceneIndex = 0;
        private GameState gameState;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            gameState = GameManager.Instance.currentState;
            GameManager.Instance.eventBus.Subscribe(GameEventType.CHOICE_MADE, OnChoiceMade);
        }

        public async void StartEpisode(string episodeId)
        {
            currentEpisode = await EpisodeLoader.Instance.LoadEpisodeAsync(episodeId);
            if (currentEpisode != null)
            {
                currentSceneIndex = 0;
                StartScene(currentSceneIndex);

                // Start background absurdity if enabled
                if (currentEpisode.background_absurdity?.enabled == true)
                {
                    absurdityManager.StartAbsurdityLoop(currentEpisode.background_absurdity);
                }
            }
        }

        void StartScene(int sceneIndex)
        {
            if (sceneIndex < currentEpisode.scenes.Count)
            {
                var scene = currentEpisode.scenes[sceneIndex];
                episodeUI.DisplayScene(scene);

                // Check if scene has choices
                if (scene.choices != null && scene.choices.Count > 0)
                {
                    choicePanel.ShowChoices(scene.choices);
                }
            }
            else
            {
                // Episode complete
                CompleteEpisode();
            }
        }

        void OnChoiceMade(object payload)
        {
            var choice = (ChoiceDto)payload;
            ApplyChoiceConsequences(choice);

            // Advance to next scene
            currentSceneIndex++;
            StartScene(currentSceneIndex);
        }

        void ApplyChoiceConsequences(ChoiceDto choice)
        {
            // Apply deltas to game state
            gameState.timeBudget += choice.time_delta;
            gameState.heat += choice.heat_delta;

            // Award tokens
            if (choice.awards != null)
            {
                foreach (var award in choice.awards)
                {
                    gameState.AddToken(award);
                }
            }

            // Update UI
            episodeUI.UpdateStateDisplay(gameState);
        }

        void CompleteEpisode()
        {
            // Award shadow token
            if (!string.IsNullOrEmpty(currentEpisode.shadow_token))
            {
                GameManager.Instance.shadowTokens.Add(currentEpisode.shadow_token);
            }

            // Trigger case closure
            GameManager.Instance.eventBus.Publish(GameEventType.EPISODE_COMPLETED, currentEpisode);

            // Stop absurdity
            absurdityManager.StopAbsurdityLoop();

            // Return to HQ
            SceneManager.LoadScene("HQ");
        }
    }
}