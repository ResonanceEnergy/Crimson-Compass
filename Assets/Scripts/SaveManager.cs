using UnityEngine;
using CrimsonCompass.Runtime;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string SAVE_KEY = "CrimsonCompass_Save";

    [System.Serializable]
    public class SaveData
    {
        public GameState State;
        public string CurrentEpisodeId;
        public int CurrentSceneId;
        public string[] CompletedEpisodes;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame(GameState state, string episodeId, int sceneId, string[] completedEpisodes)
    {
        var saveData = new SaveData
        {
            State = state,
            CurrentEpisodeId = episodeId,
            CurrentSceneId = sceneId,
            CompletedEpisodes = completedEpisodes
        };

        var json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Game saved successfully");
    }

    public SaveData LoadGame()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return null;

        var json = PlayerPrefs.GetString(SAVE_KEY);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        Debug.Log("Save deleted");
    }
}
