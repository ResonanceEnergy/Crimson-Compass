using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

namespace CrimsonCompass.Editor
{
    public class SceneSetupAutomator : EditorWindow
    {
        [MenuItem("Crimson Compass/Automate Scene Setup")]
        static void AutomateSceneSetup()
        {
            // Step 1: Ensure we're in MainScene
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            string scenePath = "Assets/Scenes/MainScene.unity";
            if (!System.IO.File.Exists(scenePath))
            {
                Debug.LogError("MainScene not found at " + scenePath);
                return;
            }

            EditorSceneManager.OpenScene(scenePath);

            // Step 2: Create GameManager GameObject
            GameObject gameManagerGO = new GameObject("GameManager");
            gameManagerGO.AddComponent<GameManager>();
            gameManagerGO.AddComponent<AgentManager>();
            gameManagerGO.AddComponent<TimeHeatManager>();
            gameManagerGO.AddComponent<SaveManager>();
            gameManagerGO.AddComponent<CrimsonCompass.Runtime.SeasonManager>();
            gameManagerGO.AddComponent<GasketManager>();

            // Step 3: Assign Data Assets
            GameManager gm = gameManagerGO.GetComponent<GameManager>();
            gm.caseJson = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/Cases/case_0001.json");
            gm.agentsJson = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/Agents/agents.json");
            gm.insightsJsonl = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/Insights/insights_0001_0500.jsonl");

            // Step 4: Create UI Canvas
            GameObject canvasGO = new GameObject("Canvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            canvasGO.AddComponent<GraphicRaycaster>();

            // Step 5: Create Hypothesis Input UI
            GameObject hypothesisPanel = new GameObject("HypothesisPanel");
            hypothesisPanel.transform.SetParent(canvasGO.transform);
            RectTransform panelRect = hypothesisPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.8f);
            panelRect.anchorMax = new Vector2(0.5f, 0.8f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(800, 100);
            panelRect.anchoredPosition = Vector2.zero;
            Image panelImage = hypothesisPanel.AddComponent<Image>();
            panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            // Create Dropdowns
            GameObject whoDropdownGO = CreateDropdown("WhoDropdown", canvasGO, new Vector2(-250, 0));
            GameObject howDropdownGO = CreateDropdown("HowDropdown", canvasGO, new Vector2(0, 0));
            GameObject whereDropdownGO = CreateDropdown("WhereDropdown", canvasGO, new Vector2(250, 0));

            // Create Submit Button
            GameObject submitButtonGO = new GameObject("SubmitButton");
            submitButtonGO.transform.SetParent(canvasGO.transform);
            RectTransform buttonRect = submitButtonGO.AddComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.7f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.7f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);
            buttonRect.sizeDelta = new Vector2(200, 50);
            buttonRect.anchoredPosition = Vector2.zero;

            Image buttonImage = submitButtonGO.AddComponent<Image>();
            buttonImage.color = Color.green;

            Button submitButton = submitButtonGO.AddComponent<Button>();
            TextMeshProUGUI buttonText = CreateTextElement("Submit", submitButtonGO);
            buttonText.alignment = TextAlignmentOptions.Center;

            // Step 6: Create Notepad UI
            GameObject notepadPanel = new GameObject("NotepadPanel");
            notepadPanel.transform.SetParent(canvasGO.transform);
            RectTransform notepadRect = notepadPanel.AddComponent<RectTransform>();
            notepadRect.anchorMin = new Vector2(0.1f, 0.1f);
            notepadRect.anchorMax = new Vector2(0.9f, 0.4f);
            notepadRect.pivot = new Vector2(0.5f, 0.5f);
            notepadRect.sizeDelta = new Vector2(-20, -20);
            notepadRect.anchoredPosition = Vector2.zero;
            Image notepadImage = notepadPanel.AddComponent<Image>();
            notepadImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            // Create Text Elements
            GameObject whoTextGO = CreateTextElementGO("WhoText", notepadPanel, new Vector2(0, 100));
            GameObject howTextGO = CreateTextElementGO("HowText", notepadPanel, new Vector2(0, 0));
            GameObject whereTextGO = CreateTextElementGO("WhereText", notepadPanel, new Vector2(0, -100));

            // Step 7: Create UI Manager GameObjects
            GameObject hypothesisInputGO = new GameObject("HypothesisInput");
            hypothesisInputGO.AddComponent<HypothesisInput>();

            GameObject notepadUIGO = new GameObject("NotepadUI");
            notepadUIGO.AddComponent<NotepadUI>();

            GameObject episodeUIGO = new GameObject("EpisodeUI");
            episodeUIGO.AddComponent<EpisodeUI>();

            // Step 8: Wire UI References
            HypothesisInput hypothesisInput = hypothesisInputGO.GetComponent<HypothesisInput>();
            hypothesisInput.whoDropdown = whoDropdownGO.GetComponent<TMP_Dropdown>();
            hypothesisInput.howDropdown = howDropdownGO.GetComponent<TMP_Dropdown>();
            hypothesisInput.whereDropdown = whereDropdownGO.GetComponent<TMP_Dropdown>();
            hypothesisInput.submitButton = submitButton;

            NotepadUI notepadUI = notepadUIGO.GetComponent<NotepadUI>();
            notepadUI.whoText = whoTextGO.GetComponent<TextMeshProUGUI>();
            notepadUI.howText = howTextGO.GetComponent<TextMeshProUGUI>();
            notepadUI.whereText = whereTextGO.GetComponent<TextMeshProUGUI>();

            gm.hypothesisInput = hypothesisInput;
            gm.notepadUI = notepadUI;
            gm.episodeUI = episodeUIGO.GetComponent<EpisodeUI>();

            // Step 9: Add to Build Settings
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[1];
            scenes[0] = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettings.scenes = scenes;

            // Save scene
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

            Debug.Log("Scene setup automation complete!");
        }

        static GameObject CreateDropdown(string name, GameObject parent, Vector2 position)
        {
            GameObject dropdownGO = new GameObject(name);
            dropdownGO.transform.SetParent(parent.transform);

            RectTransform rect = dropdownGO.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(200, 30);
            rect.anchoredPosition = position;

            TMP_Dropdown dropdown = dropdownGO.AddComponent<TMP_Dropdown>();
            Image dropdownImage = dropdownGO.AddComponent<Image>();
            dropdownImage.color = Color.white;

            // Create label
            GameObject labelGO = new GameObject("Label");
            labelGO.transform.SetParent(dropdownGO.transform);
            TextMeshProUGUI label = CreateTextElement("Select " + name.Replace("Dropdown", ""), labelGO);
            label.alignment = TextAlignmentOptions.Left;

            RectTransform labelRect = labelGO.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.sizeDelta = Vector2.zero;

            return dropdownGO;
        }

        static GameObject CreateTextElementGO(string name, GameObject parent, Vector2 position)
        {
            GameObject textGO = new GameObject(name);
            textGO.transform.SetParent(parent.transform);

            RectTransform rect = textGO.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(300, 50);
            rect.anchoredPosition = position;

            CreateTextElement("", textGO);

            return textGO;
        }

        static TextMeshProUGUI CreateTextElement(string text, GameObject parent)
        {
            TextMeshProUGUI tmp = parent.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 24;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.TopLeft;
            return tmp;
        }
    }
}