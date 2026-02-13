using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace CrimsonCompass.Editor
{
    public class Episode1TestAutomation
    {
        [MenuItem("CrimsonCompass/Automate Episode 1 Test")]
        static void AutomateEpisode1Test()
        {
            Debug.Log("Starting Episode 1 automation...");

            // Load the scene
            string scenePath = "Assets/Scenes/S01E01_AgencyBriefingRoom.unity";
            if (!System.IO.File.Exists(scenePath))
            {
                Debug.LogError($"Scene not found: {scenePath}");
                return;
            }

            EditorSceneManager.OpenScene(scenePath);
            Debug.Log("Scene loaded successfully");

            // Find or create a setup GameObject
            GameObject setupObject = GameObject.Find("Episode1TestSetup");
            if (setupObject == null)
            {
                setupObject = new GameObject("Episode1TestSetup");
                Debug.Log("Created Episode1TestSetup GameObject");
            }

            // Add a simple test component that will initialize the runtime components
            var testComponent = setupObject.AddComponent<Episode1TestInitializer>();
            Debug.Log("Added Episode1TestInitializer component");

            // Save the scene
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            Debug.Log("Scene saved with test components");

            Debug.Log("Episode 1 automation setup complete!");
            Debug.Log("Next steps:");
            Debug.Log("1. Press Play in Unity Editor to start the test");
            Debug.Log("2. The runtime components will initialize automatically");
            Debug.Log("Controls:");
            Debug.Log("- Click objects to interact based on selected verb");
            Debug.Log("- Use bottom verb bar to change interaction modes");
            Debug.Log("- KIT verb opens inventory");
            Debug.Log("- Click NPCs to start dialogue");
        }

        [MenuItem("CrimsonCompass/Stop Play Mode")]
        static void StopPlayMode()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.ExitPlaymode();
                Debug.Log("Exited Play Mode");
            }
            else
            {
                Debug.Log("Not in Play Mode");
            }
        }
    }
}