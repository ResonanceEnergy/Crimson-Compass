using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace CrimsonCompass
{
    /// <summary>
    /// Service for integrating with xAI's Grok Imagine API for image and video generation
    /// </summary>
    public class GrokImagineService : MonoBehaviour
    {
        private const string BASE_URL = "https://api.x.ai/v1";
        private const string IMAGE_GENERATION_ENDPOINT = "/images/generations";
        private const string VIDEO_GENERATION_ENDPOINT = "/videos/generations";

        [Header("API Configuration")]
        [SerializeField] private string apiKey = ""; // Set this in inspector or via code

        [Header("Generation Settings")]
        [SerializeField] private string defaultModel = "grok-imagine-image";
        [SerializeField] private string defaultAspectRatio = "16:9";
        [SerializeField] private int defaultDuration = 8; // For videos
        [SerializeField] private string defaultResolution = "720p"; // For videos

        // Events
        public event Action<string> OnImageGenerated;
        public event Action<string> OnVideoGenerated;
        public event Action<string> OnGenerationFailed;

        private void Awake()
        {
            // Load API key from secure storage if available
            if (string.IsNullOrEmpty(apiKey))
            {
                // Try to load from PlayerPrefs (not secure, use proper secure storage in production)
                apiKey = PlayerPrefs.GetString("GrokImagineApiKey", "");
            }
        }

        /// <summary>
        /// Generate an image from text prompt
        /// </summary>
        public void GenerateImage(string prompt, string aspectRatio = null, Action<string> onComplete = null, Action<string> onError = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                string error = "API key not set. Please configure the GrokImagineService.";
                OnGenerationFailed?.Invoke(error);
                onError?.Invoke(error);
                return;
            }

            var requestData = new ImageGenerationRequest
            {
                prompt = prompt,
                model = defaultModel,
                aspect_ratio = aspectRatio ?? defaultAspectRatio
            };

            StartCoroutine(GenerateImageCoroutine(requestData, onComplete, onError));
        }

        /// <summary>
        /// Generate a video from text prompt
        /// </summary>
        public void GenerateVideo(string prompt, int duration = 0, string aspectRatio = null, Action<string> onComplete = null, Action<string> onError = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                string error = "API key not set. Please configure the GrokImagineService.";
                OnGenerationFailed?.Invoke(error);
                onError?.Invoke(error);
                return;
            }

            var requestData = new VideoGenerationRequest
            {
                prompt = prompt,
                model = "grok-imagine-video",
                duration = duration > 0 ? duration : defaultDuration,
                aspect_ratio = aspectRatio ?? defaultAspectRatio,
                resolution = defaultResolution
            };

            StartCoroutine(GenerateVideoCoroutine(requestData, onComplete, onError));
        }

        /// <summary>
        /// Edit an existing image
        /// </summary>
        public void EditImage(string prompt, string imageUrl, Action<string> onComplete = null, Action<string> onError = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                string error = "API key not set. Please configure the GrokImagineService.";
                OnGenerationFailed?.Invoke(error);
                onError?.Invoke(error);
                return;
            }

            var requestData = new ImageGenerationRequest
            {
                prompt = prompt,
                model = defaultModel,
                image_url = imageUrl
            };

            StartCoroutine(GenerateImageCoroutine(requestData, onComplete, onError));
        }

        private IEnumerator GenerateImageCoroutine(ImageGenerationRequest requestData, Action<string> onComplete, Action<string> onError)
        {
            string jsonData = JsonUtility.ToJson(requestData);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest request = new UnityWebRequest(BASE_URL + IMAGE_GENERATION_ENDPOINT, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        var response = JsonUtility.FromJson<ImageGenerationResponse>(request.downloadHandler.text);
                        if (response != null && !string.IsNullOrEmpty(response.url))
                        {
                            OnImageGenerated?.Invoke(response.url);
                            onComplete?.Invoke(response.url);
                        }
                        else
                        {
                            string error = "Invalid response format";
                            OnGenerationFailed?.Invoke(error);
                            onError?.Invoke(error);
                        }
                    }
                    catch (Exception e)
                    {
                        string error = $"Failed to parse response: {e.Message}";
                        OnGenerationFailed?.Invoke(error);
                        onError?.Invoke(error);
                    }
                }
                else
                {
                    string error = $"Request failed: {request.error}";
                    OnGenerationFailed?.Invoke(error);
                    onError?.Invoke(error);
                }
            }
        }

        private IEnumerator GenerateVideoCoroutine(VideoGenerationRequest requestData, Action<string> onComplete, Action<string> onError)
        {
            // Step 1: Start generation
            string jsonData = JsonUtility.ToJson(requestData);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest startRequest = new UnityWebRequest(BASE_URL + VIDEO_GENERATION_ENDPOINT, "POST"))
            {
                startRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                startRequest.downloadHandler = new DownloadHandlerBuffer();
                startRequest.SetRequestHeader("Content-Type", "application/json");
                startRequest.SetRequestHeader("Authorization", $"Bearer {apiKey}");

                yield return startRequest.SendWebRequest();

                if (startRequest.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        var startResponse = JsonUtility.FromJson<VideoStartResponse>(startRequest.downloadHandler.text);
                        if (startResponse != null && !string.IsNullOrEmpty(startResponse.request_id))
                        {
                            // Step 2: Poll for completion
                            yield return StartCoroutine(PollVideoGeneration(startResponse.request_id, onComplete, onError));
                        }
                        else
                        {
                            string error = "Invalid start response format";
                            OnGenerationFailed?.Invoke(error);
                            onError?.Invoke(error);
                        }
                    }
                    catch (Exception e)
                    {
                        string error = $"Failed to parse start response: {e.Message}";
                        OnGenerationFailed?.Invoke(error);
                        onError?.Invoke(error);
                    }
                }
                else
                {
                    string error = $"Start request failed: {startRequest.error}";
                    OnGenerationFailed?.Invoke(error);
                    onError?.Invoke(error);
                }
            }
        }

        private IEnumerator PollVideoGeneration(string requestId, Action<string> onComplete, Action<string> onError)
        {
            const float pollInterval = 5f; // Poll every 5 seconds
            const float maxWaitTime = 600f; // Max 10 minutes
            float elapsedTime = 0f;

            while (elapsedTime < maxWaitTime)
            {
                using (UnityWebRequest pollRequest = UnityWebRequest.Get($"{BASE_URL}/videos/{requestId}"))
                {
                    pollRequest.SetRequestHeader("Authorization", $"Bearer {apiKey}");

                    yield return pollRequest.SendWebRequest();

                    if (pollRequest.result == UnityWebRequest.Result.Success)
                    {
                        try
                        {
                            var pollResponse = JsonUtility.FromJson<VideoPollResponse>(pollRequest.downloadHandler.text);
                            if (pollResponse != null)
                            {
                                if (pollResponse.status == "done" && pollResponse.video != null)
                                {
                                    OnVideoGenerated?.Invoke(pollResponse.video.url);
                                    onComplete?.Invoke(pollResponse.video.url);
                                    yield break;
                                }
                                else if (pollResponse.status == "expired")
                                {
                                    string error = "Video generation request expired";
                                    OnGenerationFailed?.Invoke(error);
                                    onError?.Invoke(error);
                                    yield break;
                                }
                                // If status is "pending", continue polling
                            }
                        }
                        catch (Exception e)
                        {
                            string error = $"Failed to parse poll response: {e.Message}";
                            OnGenerationFailed?.Invoke(error);
                            onError?.Invoke(error);
                            yield break;
                        }
                    }
                    else
                    {
                        string error = $"Poll request failed: {pollRequest.error}";
                        OnGenerationFailed?.Invoke(error);
                        onError?.Invoke(error);
                        yield break;
                    }
                }

                yield return new WaitForSeconds(pollInterval);
                elapsedTime += pollInterval;
            }

            string timeoutError = "Video generation timed out";
            OnGenerationFailed?.Invoke(timeoutError);
            onError?.Invoke(timeoutError);
        }

        /// <summary>
        /// Download and save an asset from URL
        /// </summary>
        public static IEnumerator DownloadAsset(string url, string savePath, Action<bool> onComplete)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    System.IO.File.WriteAllBytes(savePath, request.downloadHandler.data);
                    onComplete?.Invoke(true);
                }
                else
                {
                    Debug.LogError($"Failed to download asset: {request.error}");
                    onComplete?.Invoke(false);
                }
            }
        }

        // Data classes for API requests and responses
        [Serializable]
        private class ImageGenerationRequest
        {
            public string prompt;
            public string model;
            public string aspect_ratio;
            public string image_url; // Optional, for editing
        }

        [Serializable]
        private class VideoGenerationRequest
        {
            public string prompt;
            public string model;
            public int duration;
            public string aspect_ratio;
            public string resolution;
            public string image_url; // Optional, for image-to-video
        }

        [Serializable]
        private class ImageGenerationResponse
        {
            public string url;
            public bool respect_moderation;
            public string model;
        }

        [Serializable]
        private class VideoStartResponse
        {
            public string request_id;
        }

        [Serializable]
        private class VideoPollResponse
        {
            public string status; // "pending", "done", "expired"
            public VideoData video;
            public string model;
        }

        [Serializable]
        private class VideoData
        {
            public string url;
            public int duration;
            public bool respect_moderation;
        }
    }
}