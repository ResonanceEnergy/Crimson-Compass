using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrimsonCompass
{
    /// <summary>
    /// Test runner for Episode 1 implementation
    /// </summary>
    public class Episode1TestRunner : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(InitializeEpisode1());
        }

        IEnumerator InitializeEpisode1()
        {
            Debug.Log("Initializing Episode 1: Welcome Packet");

            // Load the scene
            SceneManager.LoadScene("S01E01_AgencyBriefingRoom");

            yield return new WaitForSeconds(1f);

            // Setup the scene
            GameObject setup = new GameObject("SceneSetup");
            setup.AddComponent<Episode1SceneTestSetup>();

            Debug.Log("Episode 1 scene setup complete. Use mouse to interact with objects.");
            Debug.Log("Controls:");
            Debug.Log("- Click objects to interact based on selected verb");
            Debug.Log("- Use verb bar at bottom to change interaction mode");
            Debug.Log("- KIT verb opens inventory");
        }

        [ContextMenu("Start Episode 1 Test")]
        public void StartEpisode1Test()
        {
            StartCoroutine(InitializeEpisode1());
        }
    }
}