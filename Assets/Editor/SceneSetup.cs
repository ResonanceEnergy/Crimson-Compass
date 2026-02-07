using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSetup
{
    [MenuItem("Tools/Setup Main Scene")]
    public static void SetupMainScene()
    {
        // Create new scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "MainScene";

        // Create GameManager
        var gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
        var gmScript = gm.GetComponent<GameManager>();
        // Assign TextAssets
        gmScript.caseJson = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/Cases/case_0001.json");
        gmScript.agentsJson = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/Agents/agents.json");
        gmScript.insightsJsonl = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/Insights/insights_0001_0500.jsonl");

        // Create UI Canvas
        var canvas = new GameObject("Canvas");
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        var canvasComp = canvas.GetComponent<Canvas>();
        canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;

        // Notepad Panel
        var notepadPanel = new GameObject("NotepadPanel");
        notepadPanel.transform.SetParent(canvas.transform);
        var panel = notepadPanel.AddComponent<Image>();
        panel.color = Color.white;
        var notepadRect = notepadPanel.GetComponent<RectTransform>();
        notepadRect.sizeDelta = new Vector2(400, 300);
        notepadRect.anchoredPosition = new Vector2(-200, 0);
        var notepadUI = notepadPanel.AddComponent<NotepadUI>();
        // Add Text components
        var whoText = new GameObject("WhoText");
        whoText.transform.SetParent(notepadPanel.transform);
        var whoTxt = whoText.AddComponent<Text>();
        whoTxt.text = "WHO:\n";
        var whoRect = whoText.GetComponent<RectTransform>();
        whoRect.sizeDelta = new Vector2(380, 80);
        whoRect.anchoredPosition = new Vector2(0, 100);
        notepadUI.whoText = whoTxt;

        var howText = new GameObject("HowText");
        howText.transform.SetParent(notepadPanel.transform);
        var howTxt = howText.AddComponent<Text>();
        howTxt.text = "HOW:\n";
        var howRect = howText.GetComponent<RectTransform>();
        howRect.sizeDelta = new Vector2(380, 80);
        howRect.anchoredPosition = new Vector2(0, 0);
        notepadUI.howText = howTxt;

        var whereText = new GameObject("WhereText");
        whereText.transform.SetParent(notepadPanel.transform);
        var whereTxt = whereText.AddComponent<Text>();
        whereTxt.text = "WHERE:\n";
        var whereRect = whereText.GetComponent<RectTransform>();
        whereRect.sizeDelta = new Vector2(380, 80);
        whereRect.anchoredPosition = new Vector2(0, -100);
        notepadUI.whereText = whereTxt;

        // Hypothesis Panel
        var hypothesisPanel = new GameObject("HypothesisPanel");
        hypothesisPanel.transform.SetParent(canvas.transform);
        var hypRect = hypothesisPanel.AddComponent<RectTransform>();
        hypRect.sizeDelta = new Vector2(400, 200);
        hypRect.anchoredPosition = new Vector2(200, 0);
        var hypUI = hypothesisPanel.AddComponent<HypothesisInput>();

        // Dropdowns
        var whoDD = new GameObject("WhoDropdown");
        whoDD.transform.SetParent(hypothesisPanel.transform);
        var whoDrop = whoDD.AddComponent<Dropdown>();
        var whoDDRect = whoDD.GetComponent<RectTransform>();
        whoDDRect.sizeDelta = new Vector2(160, 30);
        whoDDRect.anchoredPosition = new Vector2(-100, 50);
        hypUI.whoDropdown = whoDrop;

        var howDD = new GameObject("HowDropdown");
        howDD.transform.SetParent(hypothesisPanel.transform);
        var howDrop = howDD.AddComponent<Dropdown>();
        var howDDRect = howDD.GetComponent<RectTransform>();
        howDDRect.sizeDelta = new Vector2(160, 30);
        howDDRect.anchoredPosition = new Vector2(100, 50);
        hypUI.howDropdown = howDrop;

        var whereDD = new GameObject("WhereDropdown");
        whereDD.transform.SetParent(hypothesisPanel.transform);
        var whereDrop = whereDD.AddComponent<Dropdown>();
        var whereDDRect = whereDD.GetComponent<RectTransform>();
        whereDDRect.sizeDelta = new Vector2(160, 30);
        whereDDRect.anchoredPosition = new Vector2(0, 0);
        hypUI.whereDropdown = whereDrop;

        // Button
        var submitBtn = new GameObject("SubmitButton");
        submitBtn.transform.SetParent(hypothesisPanel.transform);
        var btn = submitBtn.AddComponent<Button>();
        var btnRect = submitBtn.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(160, 30);
        btnRect.anchoredPosition = new Vector2(0, -50);
        hypUI.submitButton = btn;

        // EventSystem
        var eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Save scene
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainScene.unity");

        // Add to build settings
        var buildSettings = EditorBuildSettings.scenes;
        var sceneList = new System.Collections.Generic.List<EditorBuildSettingsScene>(buildSettings);
        sceneList.Add(new EditorBuildSettingsScene("Assets/Scenes/MainScene.unity", true));
        EditorBuildSettings.scenes = sceneList.ToArray();

        Debug.Log("MainScene setup complete!");
    }
}