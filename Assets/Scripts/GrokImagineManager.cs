using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass;

namespace CrimsonCompass
{
    /// <summary>
    /// Manager for handling Grok Imagine asset generation in the Unity editor and runtime
    /// </summary>
    public class GrokImagineManager : MonoBehaviour
    {
        [Header("Service Reference")]
        [SerializeField] private GrokImagineService grokService;

        [Header("Generation Settings")]
        [SerializeField] private string outputFolder = "Assets/GeneratedAssets/GrokImagine";

        private void Awake()
        {
            if (grokService == null)
            {
                grokService = FindObjectOfType<GrokImagineService>();
                if (grokService == null)
                {
                    Debug.LogError("GrokImagineService not found in scene. Please add it to a GameObject.");
                }
            }

            // Ensure output folder exists
            if (!System.IO.Directory.Exists(outputFolder))
            {
                System.IO.Directory.CreateDirectory(outputFolder);
            }
        }

        private void OnEnable()
        {
            if (grokService != null)
            {
                grokService.OnImageGenerated += OnImageGenerated;
                grokService.OnVideoGenerated += OnVideoGenerated;
                grokService.OnGenerationFailed += OnGenerationFailed;
            }
        }

        private void OnDisable()
        {
            if (grokService != null)
            {
                grokService.OnImageGenerated -= OnImageGenerated;
                grokService.OnVideoGenerated -= OnVideoGenerated;
                grokService.OnGenerationFailed -= OnGenerationFailed;
            }
        }

        /// <summary>
        /// Generate an image asset for the visual production pipeline
        /// </summary>
        public void GenerateImageAsset(string prompt, string assetName, string aspectRatio = "16:9")
        {
            if (grokService == null) return;

            Debug.Log($"Generating image: {assetName}");
            grokService.GenerateImage(prompt, aspectRatio,
                (url) => StartCoroutine(DownloadAndSaveImage(url, assetName)),
                (error) => Debug.LogError($"Failed to generate image {assetName}: {error}")
            );
        }

        /// <summary>
        /// Generate a video asset for the visual production pipeline
        /// </summary>
        public void GenerateVideoAsset(string prompt, string assetName, int duration = 8, string aspectRatio = "16:9")
        {
            if (grokService == null) return;

            Debug.Log($"Generating video: {assetName}");
            grokService.GenerateVideo(prompt, duration, aspectRatio,
                (url) => StartCoroutine(DownloadAndSaveVideo(url, assetName)),
                (error) => Debug.LogError($"Failed to generate video {assetName}: {error}")
            );
        }

        /// <summary>
        /// Edit an existing image
        /// </summary>
        public void EditImageAsset(string prompt, string sourceImagePath, string assetName)
        {
            if (grokService == null) return;

            // Convert local path to URL (for now, assume it's already a URL or handle local files)
            string imageUrl = sourceImagePath;
            if (!sourceImagePath.StartsWith("http"))
            {
                // For local files, we'd need to upload them first or use base64
                // This is a simplified example
                Debug.LogWarning("Local file editing not fully implemented. Use image URLs.");
                return;
            }

            Debug.Log($"Editing image: {assetName}");
            grokService.EditImage(prompt, imageUrl,
                (url) => StartCoroutine(DownloadAndSaveImage(url, assetName)),
                (error) => Debug.LogError($"Failed to edit image {assetName}: {error}")
            );
        }

        private IEnumerator DownloadAndSaveImage(string url, string assetName)
        {
            string fileName = $"{assetName}.png";
            string filePath = System.IO.Path.Combine(outputFolder, fileName);

            yield return GrokImagineService.DownloadAsset(url, filePath, (success) =>
            {
                if (success)
                {
                    Debug.Log($"Image saved: {filePath}");
                    // Refresh Unity asset database
                    #if UNITY_EDITOR
                    UnityEditor.AssetDatabase.Refresh();
                    #endif
                }
                else
                {
                    Debug.LogError($"Failed to save image: {filePath}");
                }
            });
        }

        private IEnumerator DownloadAndSaveVideo(string url, string assetName)
        {
            string fileName = $"{assetName}.mp4";
            string filePath = System.IO.Path.Combine(outputFolder, fileName);

            yield return GrokImagineService.DownloadAsset(url, filePath, (success) =>
            {
                if (success)
                {
                    Debug.Log($"Video saved: {filePath}");
                    // Refresh Unity asset database
                    #if UNITY_EDITOR
                    UnityEditor.AssetDatabase.Refresh();
                    #endif
                }
                else
                {
                    Debug.LogError($"Failed to save video: {filePath}");
                }
            });
        }

        private void OnImageGenerated(string url)
        {
            Debug.Log($"Image generated successfully: {url}");
        }

        private void OnVideoGenerated(string url)
        {
            Debug.Log($"Video generated successfully: {url}");
        }

        private void OnGenerationFailed(string error)
        {
            Debug.LogError($"Generation failed: {error}");
        }

        // Example methods for batch generation based on the project's visual prompts

        /// <summary>
        /// Generate background assets
        /// </summary>
        public void GenerateBackgroundAssets()
        {
            // This would load prompts from docs/Visual_Prompts_Grok_Imagine.md
            // For now, example prompts
            GenerateImageAsset(
                "Futurama Detective Noir + Uncharted Action-Adventure fusion, rainy city street at night, neon signs reflecting on wet pavement, atmospheric lighting, mobile game art, stylized",
                "Background_CityStreet_Night",
                "9:16"
            );
        }

        /// <summary>
        /// Generate character assets
        /// </summary>
        public void GenerateCharacterAssets()
        {
            GenerateImageAsset(
                "Futurama Detective Noir + Uncharted Action-Adventure fusion, determined female detective in trench coat and fedora, dramatic lighting, mobile game art, stylized character portrait",
                "Character_Detective_Portrait",
                "1:1"
            );
        }

        /// <summary>
        /// Generate UI assets
        /// </summary>
        public void GenerateUIAssets()
        {
            GenerateImageAsset(
                "Futurama Detective Noir + Uncharted Action-Adventure fusion, detective notepad interface, case files and clues display, mobile UI design, dark theme",
                "UI_NotePad_Interface",
                "9:16"
            );
        }
    }
}