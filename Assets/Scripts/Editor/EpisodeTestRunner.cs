using UnityEngine;
using UnityEditor;
using CrimsonCompass.Runtime;

public class EpisodeTestRunner : EditorWindow
{
    private string episodeId = "S01E01";
    private CrimsonCompass.Runtime.SeasonManager seasonManager;
    private GameState initialState;

    [MenuItem("Tools/Test Episode Runner")]
    static void ShowWindow()
    {
        GetWindow<EpisodeTestRunner>("Episode Test Runner");
    }

    void OnGUI()
    {
        GUILayout.Label("Episode Testing", EditorStyles.boldLabel);

        episodeId = EditorGUILayout.TextField("Episode ID", episodeId);

        if (GUILayout.Button("Initialize Test Environment"))
        {
            InitializeTestEnvironment();
        }

        if (seasonManager != null)
        {
            EditorGUILayout.LabelField("Current State:", seasonManager.FlowState.ToString());
            EditorGUILayout.LabelField("Episode:", seasonManager.CurrentEpisodeId ?? "None");
            EditorGUILayout.LabelField("Scene:", seasonManager.CurrentSceneId.ToString());

            if (GUILayout.Button("Start Episode"))
            {
                StartTestEpisode();
            }

            if (seasonManager.FlowState == CrimsonCompass.Runtime.SeasonFlowState.SceneActive)
            {
                EditorGUILayout.LabelField("Available Choices:");
                var scene = seasonManager.CurrentEpisode.SceneById[seasonManager.CurrentSceneId];
                foreach (var choice in scene.Choices)
                {
                    if (GUILayout.Button($"{choice.Id}: {choice.Text}"))
                    {
                        ApplyTestChoice(choice.Id);
                    }
                }
            }
        }
    }

    private void InitializeTestEnvironment()
    {
        // Create test components
        var loader = new CcSeason1RuntimeLoader();
        var deck = new CountermeasureDeck();

        initialState = new GameState
        {
            TimeRemaining = 8,
            Heat = 20,
            LeadIntegrity = LeadIntegrity.Clean,
            Gasket = GasketState.Contained,
            Flag = FlagState.None,
            WarrantPressure = WarrantPressure.None
        };

        seasonManager = new CrimsonCompass.Runtime.SeasonManager(loader, deck, initialState);
        Debug.Log("Test environment initialized");
    }

    private async void StartTestEpisode()
    {
        try
        {
            await seasonManager.StartEpisodeAsync(episodeId);
            Debug.Log($"Started episode {episodeId}");
            Repaint();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to start episode: {e.Message}");
        }
    }

    private async void ApplyTestChoice(string choiceId)
    {
        var context = new ChoiceContext
        {
            EpisodeId = seasonManager.CurrentEpisodeId,
            SceneId = seasonManager.CurrentSceneId,
            ChoiceId = choiceId
        };

        await seasonManager.ApplyChoiceAsync(seasonManager.CurrentSceneId, choiceId, context);
        Debug.Log($"Applied choice {choiceId}");
        Repaint();
    }
}
