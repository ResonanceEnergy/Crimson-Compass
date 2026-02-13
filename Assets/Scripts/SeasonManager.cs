using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CrimsonCompass.Core;
using CrimsonCompass.Runtime;

public enum SeasonFlowState
{
    Idle,
    EpisodeLoading,
    SceneActive,
    EpisodeComplete,
    EpisodeEnd
}

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance;

    [Header("Season Configuration")]
    public string[] episodeIds = {
        "CASE-0001", "CASE-0002", "CASE-0003", "CASE-0004", "CASE-0005",
        "CASE-0006", "CASE-0007", "CASE-0008", "CASE-0009", "CASE-0010",
        "CASE-0011", "CASE-0012"
    };

    public string[] episodeTitles = {
        "Welcome Packet", "Badge & Borrow", "Clean Room", "The Update That Never Shipped", "Hostile Compliance",
        "Double Stamp", "The Convenient Suspect", "Custody", "Exit Wound", "Blow Cover",
        "Safe House", "Hindsight"
    };

    private int currentEpisodeIndex = 0;
    private Dictionary<string, string> episodeDataPaths = new Dictionary<string, string>();

    // Missing properties
    public System.Action<SeasonFlowState> OnFlowChanged;
    public System.Action<string> OnEpisodeEnded;
    public GameState State;
    public SeasonFlowState FlowState { get; private set; } = SeasonFlowState.Idle;
    public EpisodeDto _episode;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeEpisodeData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeEpisodeData()
    {
        for (int i = 0; i < episodeIds.Length; i++)
        {
            string caseId = episodeIds[i];
            string dataPath = $"Assets/Data/Cases/case_{i + 1:0000}.json";
            episodeDataPaths[caseId] = dataPath;
        }
    }

    public async System.Threading.Tasks.Task StartEpisodeAsync(string episodeId, int startSceneIndex = 0)
    {
        Debug.Log($"Starting episode: {episodeId}");

        // Load episode data
        if (episodeDataPaths.TryGetValue(episodeId, out string dataPath))
        {
            TextAsset caseJson = Resources.Load<TextAsset>("Data/Cases/" + dataPath.Replace("Assets/Data/Cases/", "").Replace(".json", ""));
            if (caseJson != null)
            {
                GameManager.Instance.LoadCase(caseJson);
                Debug.Log($"Episode {episodeId} data loaded");
            }
            else
            {
                Debug.LogError($"Failed to load case data for {episodeId} from {dataPath}");
            }
        }

        // Set up episode-specific audio/visuals
        SetupEpisodeAtmosphere(episodeId);

        // Start the episode
        GameManager.Instance.eventBus.Publish(GameEventType.EPISODE_COMPLETED, episodeId);

        await System.Threading.Tasks.Task.CompletedTask;
    }

    void SetupEpisodeAtmosphere(string episodeId)
    {
        // Set visual atmosphere
        SetVisualAtmosphere(episodeId);

        // Set audio atmosphere (music/snapshots)
        // TODO: Implement audio atmosphere setup
        Debug.Log($"Setting up atmosphere for episode: {episodeId}");
    }

    void SetVisualAtmosphere(string episodeId)
    {
        // Basic visual atmosphere implementation
        // In a full game, this would change lighting, post-processing, UI themes, etc.
        switch (episodeId)
        {
            case "CASE-0001":
                // Welcome - neutral office lighting
                RenderSettings.ambientLight = new Color(0.8f, 0.8f, 0.9f);
                break;
            case "CASE-0010":
                // Blow Cover - tense, red-tinted
                RenderSettings.ambientLight = new Color(0.9f, 0.7f, 0.7f);
                break;
            case "CASE-0012":
                // Hindsight - dramatic climax lighting
                RenderSettings.ambientLight = new Color(0.6f, 0.6f, 1.0f);
                break;
            default:
                RenderSettings.ambientLight = new Color(0.8f, 0.8f, 0.8f);
                break;
        }
    }

    public async void NextEpisode()
    {
        if (currentEpisodeIndex < episodeIds.Length - 1)
        {
            currentEpisodeIndex++;
            await StartEpisodeAsync(episodeIds[currentEpisodeIndex]);
        }
        else
        {
            Debug.Log("Season 1 completed!");
            // TODO: Season completion logic
            // Basic implementation: show completion message and reset or end game
            OnSeasonCompleted();
        }
    }

    public async void LoadEpisodeByIndex(int index)
    {
        if (index >= 0 && index < episodeIds.Length)
        {
            currentEpisodeIndex = index;
            await StartEpisodeAsync(episodeIds[index]);
        }
    }

    public string GetCurrentEpisodeTitle()
    {
        if (currentEpisodeIndex < episodeTitles.Length)
        {
            return episodeTitles[currentEpisodeIndex];
        }
        return "Unknown Episode";
    }

    public int GetCurrentEpisodeNumber()
    {
        return currentEpisodeIndex + 1;
    }

    public string CurrentEpisodeId => episodeIds[currentEpisodeIndex];
    public int CurrentSceneId { get; private set; } = 0; // Current scene index
    public int CurrentSceneIndex => 0; // Placeholder

    public async void ApplyChoiceAsync(string choiceId)
    {
        // Placeholder for choice application
        Debug.Log($"Applying choice: {choiceId}");
        await System.Threading.Tasks.Task.Delay(100); // Simulate async
    }

    public async System.Threading.Tasks.Task ApplyChoiceAsync(int sceneId, string choiceId, object context)
    {
        // Overload for choice application with scene and context
        Debug.Log($"Applying choice: {choiceId} in scene {sceneId} with context {context}");
        await System.Threading.Tasks.Task.Delay(100); // Simulate async
    }

    void OnSeasonCompleted()
    {
        // Basic season completion logic
        // In a full game, this might show credits, unlock next season, save progress, etc.
        Debug.Log("Congratulations! Season 1 completed.");
        // Could load a completion scene or show UI
        // For now, just log and perhaps reset or quit
    }
}