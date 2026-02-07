using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Core;
using CrimsonCompass.Runtime;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_KEY = "CrimsonCompass_Save";

    [System.Serializable]
    private class SaveData
    {
        public GameState state;
        public string currentEpisodeId;
        public int currentSceneIndex;
        public List<string> completedEpisodes;
        public Dictionary<string, object> customData;
    }

    void Start()
    {
        GameManager.Instance.eventBus.Subscribe(GameEventType.STATE_CHANGED, OnStateChanged);
        GameManager.Instance.eventBus.Subscribe(GameEventType.EPISODE_COMPLETED, OnEpisodeCompleted);
    }

    public void SaveGame()
    {
        var saveData = new SaveData
        {
            state = GameManager.Instance.currentState,
            currentEpisodeId = GameManager.Instance.seasonManager?.CurrentEpisodeId,
            currentSceneIndex = GameManager.Instance.seasonManager?.CurrentSceneId ?? 1,
            completedEpisodes = new List<string>(GameManager.Instance.completedEpisodes),
            customData = new Dictionary<string, object>()
        };

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Game saved successfully");
    }

    public bool LoadGame()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            Debug.Log("No save data found");
            return false;
        }

        string json = PlayerPrefs.GetString(SAVE_KEY);
        var saveData = JsonUtility.FromJson<SaveData>(json);

        GameManager.Instance.currentState = saveData.state;
        GameManager.Instance.completedEpisodes = new HashSet<string>(saveData.completedEpisodes);

        if (!string.IsNullOrEmpty(saveData.currentEpisodeId))
        {
            // Load the episode and set the scene index
            GameManager.Instance.LoadEpisode(saveData.currentEpisodeId, saveData.currentSceneIndex);
        }

        GameManager.Instance.eventBus.Publish(GameEventType.GAME_LOADED, null);
        Debug.Log("Game loaded successfully");
        return true;
    }

    public void NewGame()
    {
        GameManager.Instance.currentState = new GameState();
        GameManager.Instance.completedEpisodes.Clear();
        GameManager.Instance.currentCase = null;

        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();

        GameManager.Instance.eventBus.Publish(GameEventType.NEW_GAME_STARTED, null);
        Debug.Log("New game started");
    }

    public bool HasSaveData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }

    void OnStateChanged(object payload)
    {
        // Auto-save on state changes
        SaveGame();
    }

    void OnEpisodeCompleted(object payload)
    {
        SaveGame();
    }

    void OnDestroy()
    {
        SaveGame();
    }
}