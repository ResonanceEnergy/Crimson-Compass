using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonCompass
{
    /// <summary>
    /// Runtime initializer for Episode 1 test setup
    /// Added by the editor automation script
    /// </summary>
    public class Episode1TestInitializer : MonoBehaviour
    {
        void Awake()
        {
            Debug.Log("Episode1TestInitializer: Initializing Episode 1 test components...");

            // Add the scene test setup component
            var testSetup = gameObject.AddComponent<Episode1SceneTestSetup>();
            Debug.Log("Added Episode1SceneTestSetup component");

            // Add the test runner component
            var testRunner = gameObject.AddComponent<Episode1TestRunner>();
            Debug.Log("Added Episode1TestRunner component");

            Debug.Log("Episode 1 test initialization complete!");
            Debug.Log("The scene should now be ready for testing.");
        }
    }
}