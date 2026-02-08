using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Runtime;

namespace CrimsonCompass
{
    /// <summary>
    /// Manages background absurdity loops - non-interactive environmental humor
    /// </summary>
    public class BackgroundAbsurdityManager : MonoBehaviour
    {
        public static BackgroundAbsurdityManager Instance;

        [System.Serializable]
        public class AbsurdityEvent
        {
            public string patternId;
            public float duration;
            public GameObject[] affectedObjects;
            public AudioClip soundEffect;
        }

        [Header("Absurdity Patterns")]
        public AbsurdityEvent[] hqPatterns;
        public AbsurdityEvent[] corporatePatterns;
        public AbsurdityEvent[] fieldSitePatterns;

        private Coroutine currentLoop;
        private bool isRunning = false;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartAbsurdityLoop(BackgroundAbsurdityDto absurdity)
        {
            if (isRunning) StopAbsurdityLoop();

            AbsurdityEvent[] patterns = GetPatternsForLocation(absurdity.location_type);
            if (patterns.Length > 0)
            {
                currentLoop = StartCoroutine(AbsurdityLoop(patterns, absurdity.loop_minutes * 60f));
                isRunning = true;
            }
        }

        public void StopAbsurdityLoop()
        {
            if (currentLoop != null)
            {
                StopCoroutine(currentLoop);
                currentLoop = null;
            }
            isRunning = false;
        }

        AbsurdityEvent[] GetPatternsForLocation(string locationType)
        {
            switch (locationType.ToUpper())
            {
                case "HQ":
                case "AGENCY_HOME_BASE":
                    return hqPatterns;
                case "CORPORATE_OFFICE":
                    return corporatePatterns;
                case "FIELD_SITE":
                case "ACTIVE_OPERATION":
                    return fieldSitePatterns;
                default:
                    return hqPatterns; // fallback
            }
        }

        IEnumerator AbsurdityLoop(AbsurdityEvent[] patterns, float loopDuration)
        {
            float elapsed = 0f;

            while (elapsed < loopDuration)
            {
                // Pick a random pattern
                AbsurdityEvent pattern = patterns[Random.Range(0, patterns.Length)];

                // Execute the absurdity
                yield return StartCoroutine(ExecuteAbsurdity(pattern));

                // Wait before next pattern
                float waitTime = Mathf.Min(30f, loopDuration - elapsed); // Max 30s between patterns
                yield return new WaitForSeconds(Random.Range(waitTime * 0.5f, waitTime));

                elapsed += waitTime;
            }
        }

        IEnumerator ExecuteAbsurdity(AbsurdityEvent pattern)
        {
            // Play sound if available
            if (pattern.soundEffect != null)
            {
                AudioSource.PlayClipAtPoint(pattern.soundEffect, Camera.main.transform.position, 0.3f);
            }

            // Animate affected objects (subtle, non-blocking)
            foreach (var obj in pattern.affectedObjects)
            {
                if (obj != null)
                {
                    StartCoroutine(SubtleAnimation(obj, pattern.duration));
                }
            }

            yield return new WaitForSeconds(pattern.duration);
        }

        IEnumerator SubtleAnimation(GameObject obj, float duration)
        {
            // Example: slight color shift or position wobble
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color originalColor = renderer.material.color;
                Color targetColor = new Color(
                    Mathf.Clamp01(originalColor.r + Random.Range(-0.1f, 0.1f)),
                    Mathf.Clamp01(originalColor.g + Random.Range(-0.1f, 0.1f)),
                    Mathf.Clamp01(originalColor.b + Random.Range(-0.1f, 0.1f))
                );

                float elapsed = 0f;
                while (elapsed < duration)
                {
                    renderer.material.color = Color.Lerp(originalColor, targetColor, elapsed / duration);
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                renderer.material.color = originalColor;
            }
        }
    }
}