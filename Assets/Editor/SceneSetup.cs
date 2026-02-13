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
        // Assets loaded at runtime
        gmScript.caseJson = Resources.Load<TextAsset>("case_0001");
        gmScript.agentsJson = Resources.Load<TextAsset>("agents");
        gmScript.insightsJsonl = Resources.Load<TextAsset>("insights_0001_0500");

        // Create Camera
        var cameraObj = new GameObject("Main Camera");
        var camera = cameraObj.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.1f, 0.1f, 0.15f, 1f); // Match canvas background
        camera.orthographic = false;
        camera.fieldOfView = 60f;
        cameraObj.AddComponent<AudioListener>();
        var canvas = new GameObject("Canvas");
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        var canvasComp = canvas.GetComponent<Canvas>();
        canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;

        // Add canvas background
        var canvasImage = canvas.AddComponent<Image>();
        canvasImage.color = new Color(0.1f, 0.1f, 0.15f, 1f); // Dark blue-gray background

        // Add title text
        var titleTextObj = new GameObject("TitleText");
        titleTextObj.transform.SetParent(canvas.transform);
        var titleText = titleTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        titleText.text = "CRIMSON COMPASS\nMystery Case Game";
        titleText.color = Color.white;
        titleText.fontSize = 24;
        titleText.alignment = TMPro.TextAlignmentOptions.Center;
        var titleRect = titleTextObj.GetComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(400, 60);
        titleRect.anchoredPosition = new Vector2(0, 150);

        // Version text
        var versionTextObj = new GameObject("VersionText");
        versionTextObj.transform.SetParent(canvas.transform);
        var versionText = versionTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        var versionAsset = Resources.Load<TextAsset>("version");
        versionText.text = versionAsset != null ? $"v{versionAsset.text.Trim()}" : "v1.0.0";
        versionText.color = new Color(0.7f, 0.7f, 0.7f, 1f); // Light gray
        versionText.fontSize = 12;
        versionText.alignment = TMPro.TextAlignmentOptions.BottomRight;
        var versionRect = versionTextObj.GetComponent<RectTransform>();
        versionRect.sizeDelta = new Vector2(200, 20);
        versionRect.anchorMin = new Vector2(1, 0);
        versionRect.anchorMax = new Vector2(1, 0);
        versionRect.anchoredPosition = new Vector2(-10, 10); // Bottom right corner

        // Notepad Panel
        var notepadPanel = new GameObject("NotepadPanel");
        notepadPanel.transform.SetParent(canvas.transform);
        var panel = notepadPanel.AddComponent<Image>();
        panel.color = new Color(0.95f, 0.95f, 0.9f, 1f); // Light cream color
        var notepadRect = notepadPanel.GetComponent<RectTransform>();
        notepadRect.sizeDelta = new Vector2(400, 300);
        notepadRect.anchoredPosition = new Vector2(-200, 0);
        var notepadUI = notepadPanel.AddComponent<NotepadUI>();
        // Add Text components
        var whoText = new GameObject("WhoText");
        whoText.transform.SetParent(notepadPanel.transform);
        var whoTxt = whoText.AddComponent<TMPro.TextMeshProUGUI>();
        whoTxt.text = "WHO:\n[No case loaded]";
        whoTxt.color = Color.black;
        whoTxt.fontSize = 14;
        whoTxt.enableWordWrapping = true;
        var whoRect = whoText.GetComponent<RectTransform>();
        whoRect.sizeDelta = new Vector2(380, 80);
        whoRect.anchoredPosition = new Vector2(0, 100);
        notepadUI.whoText = whoTxt;

        var howText = new GameObject("HowText");
        howText.transform.SetParent(notepadPanel.transform);
        var howTxt = howText.AddComponent<TMPro.TextMeshProUGUI>();
        howTxt.text = "HOW:\n[No case loaded]";
        howTxt.color = Color.black;
        howTxt.fontSize = 14;
        howTxt.enableWordWrapping = true;
        var howRect = howText.GetComponent<RectTransform>();
        howRect.sizeDelta = new Vector2(380, 80);
        howRect.anchoredPosition = new Vector2(0, 0);
        notepadUI.howText = howTxt;

        var whereText = new GameObject("WhereText");
        whereText.transform.SetParent(notepadPanel.transform);
        var whereTxt = whereText.AddComponent<TMPro.TextMeshProUGUI>();
        whereTxt.text = "WHERE:\n[No case loaded]";
        whereTxt.color = Color.black;
        whereTxt.fontSize = 14;
        whereTxt.enableWordWrapping = true;
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
        if (btnRect == null)
        {
            btnRect = submitBtn.AddComponent<RectTransform>();
        }
        btnRect.sizeDelta = new Vector2(160, 30);
        btnRect.anchoredPosition = new Vector2(0, -50);
        hypUI.submitButton = btn;

        // Exit Button
        var exitButtonObj = new GameObject("ExitButton");
        exitButtonObj.transform.SetParent(canvas.transform);
        var exitButton = exitButtonObj.AddComponent<Button>();
        var exitButtonRect = exitButtonObj.GetComponent<RectTransform>();
        if (exitButtonRect == null)
        {
            exitButtonRect = exitButtonObj.AddComponent<RectTransform>();
        }
        exitButtonRect.sizeDelta = new Vector2(80, 30);
        exitButtonRect.anchoredPosition = new Vector2(350, 250);
        var exitButtonText = new GameObject("Text").AddComponent<TMPro.TextMeshProUGUI>();
        exitButtonText.transform.SetParent(exitButtonObj.transform);
        exitButtonText.text = "Exit";
        exitButtonText.color = Color.black;
        exitButtonText.fontSize = 14;
        exitButtonText.alignment = TMPro.TextAlignmentOptions.Center;
        var exitTextRect = exitButtonText.GetComponent<RectTransform>();
        exitTextRect.sizeDelta = new Vector2(80, 30);
        exitTextRect.anchoredPosition = new Vector2(0, 0);
        exitButton.onClick.AddListener(() => Application.Quit());

        // Hypothesis Result UI Panel
        var resultPanel = new GameObject("ResultPanel");
        resultPanel.transform.SetParent(canvas.transform);
        var resultImage = resultPanel.AddComponent<Image>();
        resultImage.color = new Color(0.9f, 0.9f, 0.9f, 0.95f);
        var resultRect = resultPanel.GetComponent<RectTransform>();
        resultRect.sizeDelta = new Vector2(500, 300);
        resultRect.anchoredPosition = new Vector2(0, 0);
        
        var resultUI = resultPanel.AddComponent<HypothesisResultUI>();
        
        // Result Text
        var resultTextObj = new GameObject("ResultText");
        resultTextObj.transform.SetParent(resultPanel.transform);
        var resultText = resultTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        resultText.text = "";
        resultText.color = Color.black;
        resultText.fontSize = 16;
        resultText.alignment = TMPro.TextAlignmentOptions.Center;
        resultText.enableWordWrapping = true;
        var resultTextRect = resultTextObj.GetComponent<RectTransform>();
        resultTextRect.sizeDelta = new Vector2(450, 200);
        resultTextRect.anchoredPosition = new Vector2(0, 20);
        resultUI.resultText = resultText;
        resultUI.resultPanel = resultPanel;
        
        // Continue Button
        var continueBtn = new GameObject("ContinueButton");
        continueBtn.transform.SetParent(resultPanel.transform);
        var contBtnRect = continueBtn.AddComponent<RectTransform>();
        contBtnRect.sizeDelta = new Vector2(120, 40);
        contBtnRect.anchoredPosition = new Vector2(0, -100);
        var contBtn = continueBtn.AddComponent<Button>();
        resultUI.continueButton = contBtn;
        
        var contBtnText = new GameObject("Text").AddComponent<TMPro.TextMeshProUGUI>();
        contBtnText.transform.SetParent(continueBtn.transform);
        contBtnText.text = "Continue";
        contBtnText.color = Color.black;
        contBtnText.fontSize = 14;
        contBtnText.alignment = TMPro.TextAlignmentOptions.Center;
        var contTextRect = contBtnText.GetComponent<RectTransform>();
        contTextRect.sizeDelta = new Vector2(120, 40);
        contTextRect.anchoredPosition = new Vector2(0, 0);

        // Episode UI Panel
        var episodePanel = new GameObject("EpisodePanel");
        episodePanel.transform.SetParent(canvas.transform);
        var epRect = episodePanel.AddComponent<RectTransform>();
        epRect.sizeDelta = new Vector2(600, 100);
        epRect.anchoredPosition = new Vector2(0, -200);
        var episodeUI = episodePanel.AddComponent<EpisodeUI>();
        episodeUI.episodePanel = episodePanel;
        
        // Episode Title Text
        var epTitleText = new GameObject("EpisodeTitleText");
        epTitleText.transform.SetParent(episodePanel.transform);
        var epTitleTxt = epTitleText.AddComponent<TMPro.TextMeshProUGUI>();
        epTitleTxt.text = "Episode Title";
        epTitleTxt.color = Color.white;
        epTitleTxt.fontSize = 18;
        epTitleTxt.alignment = TMPro.TextAlignmentOptions.Center;
        var epTitleRect = epTitleText.GetComponent<RectTransform>();
        epTitleRect.sizeDelta = new Vector2(580, 30);
        epTitleRect.anchoredPosition = new Vector2(0, 30);
        episodeUI.episodeTitleText = epTitleTxt;
        
        // Time Text
        var timeTextObj = new GameObject("TimeText");
        timeTextObj.transform.SetParent(episodePanel.transform);
        var timeText = timeTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        timeText.text = "Time: --";
        timeText.color = Color.white;
        timeText.fontSize = 14;
        timeText.alignment = TMPro.TextAlignmentOptions.Left;
        var timeRect = timeTextObj.GetComponent<RectTransform>();
        timeRect.sizeDelta = new Vector2(150, 20);
        timeRect.anchoredPosition = new Vector2(-200, -20);
        episodeUI.timeText = timeText;
        
        // Heat Text
        var heatTextObj = new GameObject("HeatText");
        heatTextObj.transform.SetParent(episodePanel.transform);
        var heatText = heatTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        heatText.text = "Heat: --";
        heatText.color = Color.white;
        heatText.fontSize = 14;
        heatText.alignment = TMPro.TextAlignmentOptions.Center;
        var heatRect = heatTextObj.GetComponent<RectTransform>();
        heatRect.sizeDelta = new Vector2(150, 20);
        heatRect.anchoredPosition = new Vector2(0, -20);
        episodeUI.heatText = heatText;
        
        // Score Text
        var scoreTextObj = new GameObject("ScoreText");
        scoreTextObj.transform.SetParent(episodePanel.transform);
        var scoreText = scoreTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        scoreText.text = "Score: --";
        scoreText.color = Color.white;
        scoreText.fontSize = 14;
        scoreText.alignment = TMPro.TextAlignmentOptions.Right;
        var scoreRect = scoreTextObj.GetComponent<RectTransform>();
        scoreRect.sizeDelta = new Vector2(150, 20);
        scoreRect.anchoredPosition = new Vector2(200, -20);
        episodeUI.scoreText = scoreText;

        // Scene Text
        var sceneTextObj = new GameObject("SceneText");
        sceneTextObj.transform.SetParent(episodePanel.transform);
        var sceneText = sceneTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        sceneText.text = "Scene content will appear here...";
        sceneText.color = Color.white;
        sceneText.fontSize = 16;
        sceneText.alignment = TMPro.TextAlignmentOptions.TopLeft;
        var sceneRect = sceneTextObj.GetComponent<RectTransform>();
        sceneRect.sizeDelta = new Vector2(580, 200);
        sceneRect.anchoredPosition = new Vector2(0, -60);
        episodeUI.sceneText = sceneText;

        // Choices Container
        var choicesContainerObj = new GameObject("ChoicesContainer");
        choicesContainerObj.transform.SetParent(episodePanel.transform);
        var choicesRect = choicesContainerObj.AddComponent<RectTransform>();
        choicesRect.sizeDelta = new Vector2(580, 100);
        choicesRect.anchoredPosition = new Vector2(0, -180);
        episodeUI.choicesContainer = choicesContainerObj.transform;

        // Choice Button Prefab (create a template)
        var choiceButtonObj = new GameObject("ChoiceButtonPrefab");
        choiceButtonObj.transform.SetParent(episodePanel.transform);
        var choiceButtonRect = choiceButtonObj.AddComponent<RectTransform>();
        choiceButtonRect.sizeDelta = new Vector2(200, 30);
        var choiceButton = choiceButtonObj.AddComponent<Button>();

        // Add text to button
        var choiceButtonTextObj = new GameObject("Text");
        choiceButtonTextObj.transform.SetParent(choiceButtonObj.transform);
        var choiceButtonText = choiceButtonTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        choiceButtonText.text = "Choice Text";
        choiceButtonText.color = Color.black;
        choiceButtonText.fontSize = 14;
        choiceButtonText.alignment = TMPro.TextAlignmentOptions.Center;
        var choiceButtonTextRect = choiceButtonTextObj.GetComponent<RectTransform>();
        choiceButtonTextRect.sizeDelta = new Vector2(180, 20);
        choiceButtonTextRect.anchoredPosition = Vector2.zero;

        episodeUI.choiceButtonPrefab = choiceButton;

        // Hide the prefab since it's just a template
        choiceButtonObj.SetActive(false);

        // Assign UI references to GameManager
        gmScript.episodeUI = episodeUI;
        gmScript.notepadUI = notepadUI;

        // Create Audio Components
        var audioContextProvider = new GameObject("CCAudioContextProvider");
        audioContextProvider.AddComponent<CCAudioContextProvider>();

        var audioDeltaApplier = new GameObject("CCAudioDeltaApplier");
        var deltaApplierComp = audioDeltaApplier.AddComponent<CCAudioDeltaApplier>();
        // Load the delta library if it exists
        var deltaLibrary = Resources.Load<CCAudioDeltaLibrarySO>("CCAudioDeltaLibrary");
        if (deltaLibrary != null)
        {
            deltaApplierComp.deltaLibrary = deltaLibrary;
        }

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