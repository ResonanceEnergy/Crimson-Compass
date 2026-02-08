using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CrimsonCompass
{
    /// <summary>
    /// Simple test component to start EP01 for demonstration
    /// </summary>
    public class EpisodeTestStarter : MonoBehaviour
    {
        public Button startEpisodeButton;
        public TextMeshProUGUI statusText;

        void Start()
        {
            if (startEpisodeButton != null)
            {
                startEpisodeButton.onClick.AddListener(StartEP01);
            }

            UpdateStatus("Ready to start EP01");
        }

        void StartEP01()
        {
            UpdateStatus("Starting EP01...");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadEpisode("EP01");
                UpdateStatus("EP01 started!");
            }
            else
            {
                UpdateStatus("GameManager not found!");
            }
        }

        void UpdateStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log(message);
        }
    }
}