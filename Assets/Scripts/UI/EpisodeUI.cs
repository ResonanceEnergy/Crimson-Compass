using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrimsonCompass.Runtime;

public class EpisodeUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI episodeTitleText;
    public TextMeshProUGUI sceneText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI heatText;
    public TextMeshProUGUI scoreText;
    public Transform choicesContainer;
    public Button choiceButtonPrefab;
    public GameObject episodePanel;

    private CrimsonCompass.Runtime.SeasonManager seasonManager;

    void Start()
    {
        episodePanel.SetActive(false);
    }

    public void Initialize(CrimsonCompass.Runtime.SeasonManager manager)
    {
        seasonManager = manager;
        seasonManager.OnFlowChanged += OnFlowChanged;
        seasonManager.OnEpisodeEnded += OnEpisodeEnded;
    }

    private void OnFlowChanged(CrimsonCompass.Runtime.SeasonFlowState state)
    {
        switch (state)
        {
            case CrimsonCompass.Runtime.SeasonFlowState.SceneActive:
                ShowCurrentScene();
                break;
            case CrimsonCompass.Runtime.SeasonFlowState.EpisodeEnd:
                episodePanel.SetActive(false);
                break;
        }
    }

    private void OnEpisodeEnded(string episodeId)
    {
        Debug.Log($"Episode {episodeId} completed!");
    }

    private void ShowCurrentScene()
    {
        if (seasonManager.CurrentEpisode == null) return;

        episodePanel.SetActive(true);

        var currentScene = seasonManager.CurrentEpisode.SceneById[seasonManager.CurrentSceneId];
        sceneText.text = currentScene.SceneText;

        // Clear previous choices
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }

        // Create choice buttons
        foreach (var choice in currentScene.Choices)
        {
            var button = Instantiate(choiceButtonPrefab, choicesContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;
            button.onClick.AddListener(() => OnChoiceSelected(choice.Id));
        }
    }

    private async void OnChoiceSelected(string choiceId)
    {
        var context = new ChoiceContext
        {
            EpisodeId = seasonManager.CurrentEpisodeId,
            SceneId = seasonManager.CurrentSceneId,
            ChoiceId = choiceId
        };

        await seasonManager.ApplyChoiceAsync(seasonManager.CurrentSceneId, choiceId, context);
    }

    public void UpdateStateDisplay(GameState state)
    {
        // Update UI with current game state
        if (timeText != null)
        {
            timeText.text = $"Time: {state.TimeRemaining}";
            timeText.color = state.TimeRemaining <= 2 ? Color.red : state.TimeRemaining <= 5 ? Color.yellow : Color.white;
        }

        if (heatText != null)
        {
            heatText.text = $"Heat: {state.Heat}";
            heatText.color = state.Heat >= 75 ? Color.red : state.Heat >= 50 ? Color.yellow : Color.white;
        }

        if (scoreText != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.currentScore}";
        }

        Debug.Log($"State updated: Time={state.TimeRemaining}, Heat={state.Heat}, Score={GameManager.Instance.currentScore}");
    }

    public void DisplayScene(string sceneText, string[] choiceLabels)
    {
        Debug.Log($"Displaying scene: {sceneText} with choices: {string.Join(", ", choiceLabels)}");
    }
}
