using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSetup : MonoBehaviour
{
    [MenuItem("Tools/Setup MainScene")]
    static void SetupMainScene()
    {
        // Create new scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "MainScene";

        // Create GameManager
        var gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
        // Assign TextAssets (assuming they are in Resources)
        var gmScript = gm.GetComponent<GameManager>();
        gmScript.caseJson = Resources.Load<TextAsset>("case_0001");
        gmScript.agentsJson = Resources.Load<TextAsset>("agents");
        gmScript.insightsJsonl = Resources.Load<TextAsset>("insights_0001_0500");

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
        var notepadUI = notepadPanel.AddComponent<NotepadUI>();
        // Add Text components
        var whoText = new GameObject("WhoText");
        whoText.transform.SetParent(notepadPanel.transform);
        var whoTxt = whoText.AddComponent<Text>();
        whoTxt.text = "WHO:\n";
        notepadUI.whoText = whoTxt;

        var howText = new GameObject("HowText");
        howText.transform.SetParent(notepadPanel.transform);
        var howTxt = howText.AddComponent<Text>();
        howTxt.text = "HOW:\n";
        notepadUI.howText = howTxt;

        var whereText = new GameObject("WhereText");
        whereText.transform.SetParent(notepadPanel.transform);
        var whereTxt = whereText.AddComponent<Text>();
        whereTxt.text = "WHERE:\n";
        notepadUI.whereText = whereTxt;

        // Hypothesis Panel
        var hypothesisPanel = new GameObject("HypothesisPanel");
        hypothesisPanel.transform.SetParent(canvas.transform);
        var hypUI = hypothesisPanel.AddComponent<HypothesisInput>();

        // Dropdowns
        var whoDD = new GameObject("WhoDropdown");
        whoDD.transform.SetParent(hypothesisPanel.transform);
        var whoDrop = whoDD.AddComponent<Dropdown>();
        hypUI.whoDropdown = whoDrop;

        var howDD = new GameObject("HowDropdown");
        howDD.transform.SetParent(hypothesisPanel.transform);
        var howDrop = howDD.AddComponent<Dropdown>();
        hypUI.howDropdown = howDrop;

        var whereDD = new GameObject("WhereDropdown");
        whereDD.transform.SetParent(hypothesisPanel.transform);
        var whereDrop = whereDD.AddComponent<Dropdown>();
        hypUI.whereDropdown = whereDrop;

        // Button
        var submitBtn = new GameObject("SubmitButton");
        submitBtn.transform.SetParent(hypothesisPanel.transform);
        var btn = submitBtn.AddComponent<Button>();
        hypUI.submitButton = btn;

        // EventSystem
        var eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Save scene
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainScene.unity");
        Debug.Log("MainScene setup complete!");
    }
}