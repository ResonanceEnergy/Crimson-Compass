using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public class QuickAudioSetup
{
    [MenuItem("Tools/Complete Crimson Compass Setup")]
    public static void DoCompleteSetup()
    {
        // Step 1: Create audio assets
        Debug.Log("Step 1: Creating audio assets...");
        AudioSetup.SetupAudioSystem();

        // Step 2: Create AudioService prefab
        Debug.Log("Step 2: Creating AudioService prefab...");
        AudioSetup.CreateAudioServicePrefab();

        // Step 3: Add AudioService to main scene
        Debug.Log("Step 3: Adding AudioService to MainScene...");
        AddAudioServiceToScene();

        Debug.Log("🎉 Complete Crimson Compass Setup Finished!");
        Debug.Log("Ready for production builds. Close Unity and run:");
        Debug.Log("python Tools/build.py --target Android --validate");
        Debug.Log("python Tools/build.py --target iOS --validate");
    }

    [MenuItem("Tools/Quick Audio Setup")]
    public static void DoQuickSetup()
    {
        // Run the full audio setup
        AudioSetup.SetupAudioSystem();

        // Create AudioService prefab
        AudioSetup.CreateAudioServicePrefab();

        Debug.Log("Quick Audio Setup Complete! AudioEvent assets, AudioCatalog, and AudioService prefab have been created.");
    }

    private static void AddAudioServiceToScene()
    {
        // Find the AudioService prefab
        string prefabPath = "Assets/Audio/CrimsonCompass/PREFABS/AudioService.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError($"AudioService prefab not found at {prefabPath}");
            return;
        }

        // Open MainScene if not already open
        string scenePath = "Assets/Scenes/MainScene.unity";
        Scene scene = EditorSceneManager.GetActiveScene();
        if (scene.path != scenePath)
        {
            scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        // Instantiate the prefab in the scene
        GameObject audioServiceInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (audioServiceInstance != null)
        {
            Debug.Log("AudioService prefab added to MainScene");
            // Mark scene as dirty so it gets saved
            EditorSceneManager.MarkSceneDirty(scene);
        }
        else
        {
            Debug.LogError("Failed to instantiate AudioService prefab");
        }
    }
}