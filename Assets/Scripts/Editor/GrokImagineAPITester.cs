#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CrimsonCompass;

namespace CrimsonCompass.Editor
{
    /// <summary>
    /// Editor window for testing Grok Imagine API integration
    /// </summary>
    public class GrokImagineAPITester : EditorWindow
    {
        private GrokImagineService service;
        private string apiKey = "";
        private string testPrompt = "Futurama Detective Noir + Uncharted Action-Adventure fusion, rainy city street at night, neon signs reflecting on wet pavement, atmospheric lighting, mobile game art, stylized";
        private string aspectRatio = "16:9";
        private int videoDuration = 8;
        private bool isGenerating = false;
        private string lastResult = "";

        [MenuItem("Crimson Compass/Grok Imagine API Tester")]
        static void Init()
        {
            GrokImagineAPITester window = (GrokImagineAPITester)EditorWindow.GetWindow(typeof(GrokImagineAPITester));
            window.titleContent = new GUIContent("Grok Imagine API Tester");
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Grok Imagine API Integration Test", EditorStyles.boldLabel);

            // API Key
            GUILayout.Label("API Configuration", EditorStyles.boldLabel);
            apiKey = EditorGUILayout.PasswordField("API Key", apiKey);

            if (GUILayout.Button("Set API Key"))
            {
                if (service == null)
                {
                    GameObject serviceGO = new GameObject("GrokImagineService");
                    service = serviceGO.AddComponent<GrokImagineService>();
                }
                // Note: In a real implementation, you'd want to store this securely
                service.GetType().GetField("apiKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .SetValue(service, apiKey);
                PlayerPrefs.SetString("GrokImagineApiKey", apiKey);
                Debug.Log("API Key set");
            }

            GUILayout.Space(20);

            // Test Generation
            GUILayout.Label("Test Generation", EditorStyles.boldLabel);

            testPrompt = EditorGUILayout.TextArea(testPrompt, GUILayout.Height(60));
            aspectRatio = EditorGUILayout.TextField("Aspect Ratio", aspectRatio);

            if (GUILayout.Button("Generate Image") && !isGenerating)
            {
                if (service == null)
                {
                    Debug.LogError("Please set API key first");
                    return;
                }

                isGenerating = true;
                lastResult = "Generating image...";

                service.GenerateImage(testPrompt, aspectRatio,
                    (url) => {
                        lastResult = $"Image generated: {url}";
                        isGenerating = false;
                        Debug.Log($"Image URL: {url}");
                    },
                    (error) => {
                        lastResult = $"Error: {error}";
                        isGenerating = false;
                        Debug.LogError($"Generation failed: {error}");
                    });
            }

            videoDuration = EditorGUILayout.IntField("Video Duration", videoDuration);

            if (GUILayout.Button("Generate Video") && !isGenerating)
            {
                if (service == null)
                {
                    Debug.LogError("Please set API key first");
                    return;
                }

                isGenerating = true;
                lastResult = "Generating video...";

                service.GenerateVideo(testPrompt, videoDuration, aspectRatio,
                    (url) => {
                        lastResult = $"Video generated: {url}";
                        isGenerating = false;
                        Debug.Log($"Video URL: {url}");
                    },
                    (error) => {
                        lastResult = $"Error: {error}";
                        isGenerating = false;
                        Debug.LogError($"Generation failed: {error}");
                    });
            }

            GUILayout.Space(10);

            // Status
            if (isGenerating)
            {
                EditorGUILayout.HelpBox("Generating... Please wait.", MessageType.Info);
            }

            if (!string.IsNullOrEmpty(lastResult))
            {
                EditorGUILayout.HelpBox(lastResult, MessageType.Info);
            }

            GUILayout.Space(20);

            // Quick Tests
            GUILayout.Label("Quick Tests", EditorStyles.boldLabel);

            if (GUILayout.Button("Test Background Generation"))
            {
                testPrompt = "Futurama Detective Noir + Uncharted Action-Adventure fusion, rainy city street at night, neon signs reflecting on wet pavement, atmospheric lighting, mobile game art, stylized";
                aspectRatio = "9:16";
            }

            if (GUILayout.Button("Test Character Generation"))
            {
                testPrompt = "Futurama Detective Noir + Uncharted Action-Adventure fusion, determined female detective in trench coat and fedora, dramatic lighting, mobile game art, stylized character portrait";
                aspectRatio = "1:1";
            }

            if (GUILayout.Button("Test Video Generation"))
            {
                testPrompt = "Dynamic scene of detective examining evidence, handheld camera, slow deliberate movement, atmospheric noir lighting";
                aspectRatio = "9:16";
                videoDuration = 8;
            }
        }

        void OnDestroy()
        {
            if (service != null && service.gameObject != null)
            {
                DestroyImmediate(service.gameObject);
            }
        }
    }
}
#endif