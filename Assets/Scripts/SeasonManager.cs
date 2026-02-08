using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CrimsonCompass.Core;

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
            string dataPath = $"Assets/Data/Cases/case_{i + 1:000}.json";
            episodeDataPaths[caseId] = dataPath;
        }
    }

    public async void StartEpisodeAsync(string episodeId, int startSceneIndex = 0)
    {
        Debug.Log($"Starting episode: {episodeId}");

        // Load episode data
        if (episodeDataPaths.TryGetValue(episodeId, out string dataPath))
        {
            TextAsset caseJson = Resources.Load<TextAsset>(dataPath.Replace("Assets/Data/Cases/", "").Replace(".json", ""));
            if (caseJson != null)
            {
                GameManager.Instance.caseJson = caseJson;
                GameManager.Instance.LoadCase();
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
    }

    void SetupEpisodeAtmosphere(string episodeId)
    {
        // Set music based on episode
        if (AudioManager.Instance != null)
        {
            // Default investigation music
            AudioManager.Instance.PlayMusic("investigation_theme", true);

            // Episode-specific ambience
            switch (episodeId)
            {
                case "CASE-0001":
                    AudioManager.Instance.PlayAmbience("office_ambience");
                    break;
                case "CASE-0002":
                    AudioManager.Instance.PlayAmbience("urban_ambience");
                    break;
                case "CASE-0010":
                    AudioManager.Instance.PlayAmbience("tension_ambience");
                    break;
                case "CASE-0012":
                    AudioManager.Instance.PlayAmbience("climax_ambience");
                    break;
                default:
                    AudioManager.Instance.PlayAmbience("default_ambience");
                    break;
            }
        }

        // TODO: Set visual atmosphere (lighting, UI themes, etc.)
    }

    public void NextEpisode()
    {
        if (currentEpisodeIndex < episodeIds.Length - 1)
        {
            currentEpisodeIndex++;
            StartEpisodeAsync(episodeIds[currentEpisodeIndex]);
        }
        else
        {
            Debug.Log("Season 1 completed!");
            // TODO: Season completion logic
        }
    }

    public void LoadEpisodeByIndex(int index)
    {
        if (index >= 0 && index < episodeIds.Length)
        {
            currentEpisodeIndex = index;
            StartEpisodeAsync(episodeIds[index]);
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
    public string CurrentSceneId => "main_scene"; // Placeholder
    public int CurrentSceneIndex => 0; // Placeholder

    public async void ApplyChoiceAsync(string choiceId)
    {
        // Placeholder for choice application
        Debug.Log($"Applying choice: {choiceId}");
        await System.Threading.Tasks.Task.Delay(100); // Simulate async
    }

    public void ResetSeason()
    {
        currentEpisodeIndex = 0;
        GameManager.Instance.completedEpisodes.Clear();
        GameManager.Instance.shadowTokens.Clear();
        if (GasketManager.Instance != null)
        {
            GasketManager.Instance.fragments.Clear();
        }
        Debug.Log("Season reset");
    }
}