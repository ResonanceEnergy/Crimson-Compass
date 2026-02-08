using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrimsonCompass.Core;
using CrimsonCompass.Runtime;

public class EpisodeUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI sceneText;
    public TextMeshProUGUI sceneDescription;
    public Transform choicesContainer;
    public Button choiceButtonPrefab;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI heatText;
    public TextMeshProUGUI leadIntegrityText;
    public TextMeshProUGUI gasketText;
    public TextMeshProUGUI flagText;

    [Header("Agent UI")]
    public TextMeshProUGUI agentMessageText;
    public Button hintButton;
    public Button gadgetButton;

    private List<Button> choiceButtons = new List<Button>();

    void Start()
    {
        GameManager.Instance.eventBus.Subscribe(GameEventType.SCENE_LOADED, OnSceneLoaded);
        GameManager.Instance.eventBus.Subscribe(GameEventType.STATE_CHANGED, OnStateChanged);
        GameManager.Instance.eventBus.Subscribe(GameEventType.HINT_OFFERED, OnHintOffered);
        GameManager.Instance.eventBus.Subscribe(GameEventType.AGENT_MESSAGE, OnAgentMessage);

        hintButton.onClick.AddListener(() => GameManager.Instance.agentManager.RequestHint("general"));
        gadgetButton.onClick.AddListener(() => GameManager.Instance.agentManager.SelectGadgets(new List<string> { "wire_cutter", "lockpick" }));
    }

    void OnSceneLoaded(object payload)
    {
        var scene = (SceneDto)payload;
        DisplayScene(scene);
    }

    void OnStateChanged(object payload)
    {
        UpdateStateDisplay();
    }

    void OnHintOffered(object payload)
    {
        hintButton.gameObject.SetActive(true);
    }

    void OnAgentMessage(object payload)
    {
        var message = (string)payload;
        agentMessageText.text = message;
        StartCoroutine(ClearAgentMessageAfterDelay(5f));
    }

    public void DisplayScene(SceneDto scene)
    {
        sceneText.text = scene.scene_text;
        sceneDescription.text = scene.scene_description;

        // Clear existing choice buttons
        foreach (var button in choiceButtons)
        {
            Destroy(button.gameObject);
        }
        choiceButtons.Clear();

        // Create new choice buttons
        foreach (var choice in scene.choices)
        {
            var button = Instantiate(choiceButtonPrefab, choicesContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice.label;
            button.onClick.AddListener(() => OnChoiceSelected(choice));
            choiceButtons.Add(button);
        }

        UpdateStateDisplay();
    }

    void OnChoiceSelected(ChoiceDto choice)
    {
        var context = new ChoiceContext
        {
            EpisodeId = GameManager.Instance.seasonManager.CurrentEpisodeId,
            SceneId = GameManager.Instance.seasonManager.CurrentSceneId,
            ChoiceId = choice.id,
            IsNetworkPull = false,
            IsPressAction = false
        };

        GameManager.Instance.seasonManager.ApplyChoiceAsync(
            GameManager.Instance.seasonManager.CurrentSceneId,
            choice.id,
            context);
    }

    void UpdateStateDisplay()
    {
        var state = GameManager.Instance.currentState;
        UpdateStateDisplay(state);
    }

    public void UpdateStateDisplay(GameState state)
    {
        timeText.text = $"Time: {state.timeBudget}";
        heatText.text = $"Heat: {state.heat}";
        leadIntegrityText.text = $"Lead Integrity: {state.leadIntegrity}";
        gasketText.text = $"Gasket: {state.gasket}";
        flagText.text = $"Flag: {state.flag}";
    }

    IEnumerator ClearAgentMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        agentMessageText.text = "";
    }
}