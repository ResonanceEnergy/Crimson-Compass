using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Runtime;

namespace CrimsonCompass
{
    /// <summary>
    /// Loads and manages episode data from Addressables
    /// </summary>
    public class EpisodeLoader : MonoBehaviour
    {
        public static EpisodeLoader Instance;

        private Dictionary<string, EpisodeDto> loadedEpisodes = new Dictionary<string, EpisodeDto>();

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

        public async System.Threading.Tasks.Task<EpisodeDto> LoadEpisodeAsync(string episodeId)
        {
            if (loadedEpisodes.TryGetValue(episodeId, out var cached))
            {
                return cached;
            }

            string path = $"Episodes/{episodeId}";
            TextAsset textAsset = Resources.Load<TextAsset>(path);

            if (textAsset != null)
            {
                string json = textAsset.text;
                EpisodeDto episode = JsonUtility.FromJson<EpisodeDto>(json);
                loadedEpisodes[episodeId] = episode;
                Resources.UnloadAsset(textAsset);
                return episode;
            }
            else
            {
                Debug.LogError($"Failed to load episode {episodeId} from Resources path {path}");
                return null;
            }
        }

        public void UnloadEpisode(string episodeId)
        {
            loadedEpisodes.Remove(episodeId);
        }
    }
}