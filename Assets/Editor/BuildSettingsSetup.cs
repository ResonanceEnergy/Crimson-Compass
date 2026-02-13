using UnityEditor;
using UnityEngine;

public class BuildSettingsSetup
{
    [MenuItem("Tools/Setup Build Settings")]
    public static void SetupBuildSettings()
    {
        // Get all scenes in Assets/Scenes/
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        var sceneList = new System.Collections.Generic.List<EditorBuildSettingsScene>();

        foreach (string guid in sceneGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path))
            {
                sceneList.Add(new EditorBuildSettingsScene(path, true));
                Debug.Log($"Added scene to build settings: {path}");
            }
        }

        EditorBuildSettings.scenes = sceneList.ToArray();
        Debug.Log($"Build settings updated with {sceneList.Count} scenes.");
    }
}