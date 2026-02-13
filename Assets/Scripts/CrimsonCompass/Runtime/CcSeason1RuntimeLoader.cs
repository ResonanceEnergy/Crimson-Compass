using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CrimsonCompass.Runtime
{
    /// <summary>
    /// Runtime loader that reads the Addressables catalog from cc/s1/catalog,
    /// then loads per-episode JSON by key (cc/s1/S01E01 ... cc/s1/S01E12).
    /// </summary>
    public sealed class CcSeason1RuntimeLoader
    {
        public const string CatalogAddress = "cc/s1/catalog";

        private AddressablesCatalog _catalog;
        private readonly Dictionary<string, EpisodeData> _episodeCache = new();

        public async Task<AddressablesCatalog> LoadCatalogAsync()
        {
            if (_catalog != null) return _catalog;

            TextAsset catalogText = await LoadTextAssetAsync(CatalogAddress);
            if (catalogText == null)
                throw new Exception($"Catalog missing at address '{CatalogAddress}'. Ensure addressables_catalog.json is addressable.");

            _catalog = JsonConvert.DeserializeObject<AddressablesCatalog>(catalogText.text);
            if (_catalog == null || _catalog.Entries == null)
                throw new Exception("Catalog JSON parsed but was empty/invalid.");

            _catalog.BuildIndex();
            return _catalog;
        }

        public async Task<EpisodeData> LoadEpisodeAsync(string episodeId, bool verifySha256 = true, bool useCache = true)
        {
            if (string.IsNullOrWhiteSpace(episodeId))
                throw new ArgumentException("episodeId is required");

            if (useCache && _episodeCache.TryGetValue(episodeId, out var cached))
                return cached;

            if (_catalog == null) await LoadCatalogAsync();

            string key = $"cc/s1/{episodeId}";
            if (!_catalog.EntryByKey.TryGetValue(key, out var entry))
                throw new Exception($"No catalog entry for key '{key}'.");

            TextAsset episodeText = await LoadTextAssetAsync(key);
            if (episodeText == null)
                throw new Exception($"Episode JSON missing at address '{key}'.");

            if (verifySha256)
            {
                string actual = Sha256Utf8(episodeText.text);
                if (!string.Equals(actual, entry.Sha256, StringComparison.OrdinalIgnoreCase))
                    throw new Exception($"SHA mismatch for {episodeId}. Expected {entry.Sha256}, got {actual}.");
            }

            var wrapper = JsonConvert.DeserializeObject<EpisodeWrapper>(episodeText.text);
            if (wrapper?.Episode == null)
                throw new Exception($"Episode wrapper invalid for {episodeId} (missing 'episode').");

            wrapper.Episode.BuildIndexes();

            if (useCache)
                _episodeCache[episodeId] = wrapper.Episode;

            return wrapper.Episode;
        }

        public async Task<Dictionary<string, EpisodeData>> LoadAllEpisodesAsync(bool verifySha256 = true, bool useCache = true)
        {
            if (_catalog == null) await LoadCatalogAsync();

            var ids = _catalog.Entries
                .Select(e => e.Key)
                .Where(k => k.StartsWith("cc/s1/S01E", StringComparison.OrdinalIgnoreCase))
                .Select(k => k.Replace("cc/s1/", ""))
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var result = new Dictionary<string, EpisodeData>();
            foreach (var id in ids)
                result[id] = await LoadEpisodeAsync(id, verifySha256, useCache);

            return result;
        }

        private static async Task<TextAsset> LoadTextAssetAsync(string address)
        {
            // For now, load directly from Resources path instead of Addressables
            if (address == "cc/s1/catalog")
            {
                var textAsset = Resources.Load<TextAsset>("AddressableAssetsData/CrimsonCompass/Season1/addressables_catalog");
                if (textAsset != null) return textAsset;
            }
            else if (address.StartsWith("cc/s1/"))
            {
                string episodeId = address.Replace("cc/s1/", "");
                var textAsset = Resources.Load<TextAsset>($"AddressableAssetsData/CrimsonCompass/Season1/{episodeId}");
                if (textAsset != null) return textAsset;
            }

            // Fallback to Addressables if available
            AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(address);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
                return handle.Result;
            
            return null;
        }

        private static string Sha256Utf8(string text)
        {
            using var sha = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            byte[] hash = sha.ComputeHash(bytes);
            var sb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        [Serializable]
        public sealed class AddressablesCatalog
        {
            [JsonProperty("catalog_version")] public string CatalogVersion;
            [JsonProperty("generated_utc")] public string GeneratedUtc;
            [JsonProperty("notes")] public List<string> Notes;
            [JsonProperty("entries")] public List<AddressablesEntry> Entries;

            [JsonIgnore] public Dictionary<string, AddressablesEntry> EntryByKey;

            public void BuildIndex()
            {
                EntryByKey = new Dictionary<string, AddressablesEntry>(StringComparer.OrdinalIgnoreCase);
                if (Entries == null) return;
                foreach (var e in Entries) EntryByKey[e.Key] = e;
            }
        }

        [Serializable]
        public sealed class AddressablesEntry
        {
            [JsonProperty("key")] public string Key;
            [JsonProperty("label")] public string Label;
            [JsonProperty("group")] public string Group;
            [JsonProperty("path")] public string Path;
            [JsonProperty("sha256")] public string Sha256;
            [JsonProperty("bytes")] public long Bytes;
        }

        [Serializable]
        public sealed class EpisodeWrapper
        {
            [JsonProperty("schema_version")] public string SchemaVersion;
            [JsonProperty("episode")] public EpisodeData Episode;
            [JsonProperty("constraints")] public List<object> Constraints;
        }

        [Serializable]
        public sealed class EpisodeData
        {
            [JsonProperty("episode_id")] public string EpisodeId;
            [JsonProperty("title")] public string Title;
            [JsonProperty("arc")] public string Arc;

            [JsonProperty("blueprint_fragment")] public string BlueprintFragment;
            [JsonProperty("architect_echo")] public string ArchitectEcho;

            [JsonProperty("surface_crime_pillar")] public string SurfaceCrimePillar;
            [JsonProperty("faction_focus")] public string FactionFocus;
            [JsonProperty("villain_traits")] public List<string> VillainTraits;

            [JsonProperty("primary_learning_axis")] public string PrimaryLearningAxis;
            [JsonProperty("warrant_pressure")] public string WarrantPressure;
            [JsonProperty("neon_snap_opportunity")] public string NeonSnapOpportunity;

            [JsonProperty("end_hook")] public string EndHook;
            [JsonProperty("ship_gate")] public string ShipGate;
            [JsonProperty("package_pdf")] public string PackagePdf;

            [JsonProperty("scenes")] public List<SceneData> Scenes;

            [JsonIgnore] public Dictionary<int, SceneData> SceneById;
            [JsonIgnore] public Dictionary<(int sceneId, string choiceId), ChoiceData> ChoiceByKey;

            public void BuildIndexes()
            {
                SceneById = new Dictionary<int, SceneData>();
                ChoiceByKey = new Dictionary<(int, string), ChoiceData>();
                if (Scenes == null) return;
                foreach (var scene in Scenes)
                {
                    SceneById[scene.SceneId] = scene;
                    if (scene.Choices == null) continue;
                    foreach (var choice in scene.Choices)
                        ChoiceByKey[(scene.SceneId, choice.Id)] = choice;
                }
            }
        }

        [Serializable]
        public sealed class SceneData
        {
            [JsonProperty("scene_id")] public int SceneId;
            [JsonProperty("scene_text")] public string SceneText;
            [JsonProperty("choices")] public List<ChoiceData> Choices;
        }

        [Serializable]
        public sealed class ChoiceData
        {
            [JsonProperty("id")] public string Id;
            [JsonProperty("text")] public string Text;
            [JsonProperty("primary_effect")] public string PrimaryEffect;
            [JsonProperty("shadow_effect")] public string ShadowEffect;
            [JsonProperty("deltas")] public DeltaData Deltas;
            [JsonProperty("notes")] public string Notes;
        }

        [Serializable]
        public sealed class DeltaData
        {
            [JsonProperty("time")] public int Time;
            [JsonProperty("heat")] public int Heat;
            [JsonProperty("lead_integrity")] public string LeadIntegrity;
            [JsonProperty("gasket")] public string Gasket;
            [JsonProperty("flag")] public string Flag;
        }
    }
}
