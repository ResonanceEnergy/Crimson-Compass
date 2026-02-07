#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using CrimsonCompass.Data;

namespace CrimsonCompass.EditorTools
{
    public class EpisodeScriptableImporterWindow : EditorWindow
    {
        private const string DefaultCatalogPath = "Assets/AddressableAssetsData/CrimsonCompass/Season1/addressables_catalog.json";
        private const string DefaultJsonFolder = "Assets/AddressableAssetsData/CrimsonCompass/Season1/Json";
        private const string DefaultOutputFolder = "Assets/CrimsonCompass/Season1/Episodes";

        [SerializeField] private TextAsset catalogJson;
        [SerializeField] private DefaultAsset jsonFolder;
        [SerializeField] private DefaultAsset outputFolder;

        [SerializeField] private bool verifySha256 = true;
        [SerializeField] private bool overwriteExisting = true;

        [MenuItem("CrimsonCompass/Import/Season 1 Episodes (ScriptableObjects)...")]
        public static void Open()
        {
            var w = GetWindow<EpisodeScriptableImporterWindow>("CC Episode Importer");
            w.minSize = new Vector2(520, 280);
            w.AutoAssignDefaults();
        }

        private void AutoAssignDefaults()
        {
            if (catalogJson == null)
                catalogJson = AssetDatabase.LoadAssetAtPath<TextAsset>(DefaultCatalogPath);

            if (jsonFolder == null)
                jsonFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(DefaultJsonFolder);

            if (outputFolder == null)
            {
                EnsureFolder(DefaultOutputFolder);
                outputFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(DefaultOutputFolder);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Crimson Compass — ScriptableObject Importer", EditorStyles.boldLabel);
            EditorGUILayout.Space(6);

            catalogJson = (TextAsset)EditorGUILayout.ObjectField("Addressables Catalog JSON", catalogJson, typeof(TextAsset), false);
            jsonFolder = (DefaultAsset)EditorGUILayout.ObjectField("Episode JSON Folder", jsonFolder, typeof(DefaultAsset), false);
            outputFolder = (DefaultAsset)EditorGUILayout.ObjectField("Output Folder", outputFolder, typeof(DefaultAsset), false);

            EditorGUILayout.Space(8);
            verifySha256 = EditorGUILayout.ToggleLeft("Verify SHA-256 using catalog", verifySha256);
            overwriteExisting = EditorGUILayout.ToggleLeft("Overwrite existing Episode assets", overwriteExisting);

            EditorGUILayout.Space(12);
            using (new EditorGUI.DisabledScope(catalogJson == null || jsonFolder == null || outputFolder == null))
            {
                if (GUILayout.Button("IMPORT Season 1 Episodes", GUILayout.Height(34)))
                {
                    try
                    {
                        ImportAll();
                        EditorUtility.DisplayDialog("Import Complete", "Season 1 episodes imported.", "OK");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                        EditorUtility.DisplayDialog("Import Failed", ex.Message, "OK");
                    }
                }
            }
        }

        private void ImportAll()
        {
            var catalog = JsonConvert.DeserializeObject<AddressablesCatalog>(catalogJson.text);
            if (catalog == null || catalog.entries == null || catalog.entries.Count == 0)
                throw new Exception("Catalog is empty/invalid.");

            string jsonFolderPath = AssetDatabase.GetAssetPath(jsonFolder);
            string outFolderPath = AssetDatabase.GetAssetPath(outputFolder);
            EnsureFolder(outFolderPath);

            var episodeEntries = catalog.entries
                .Where(e => e.key.StartsWith("cc/s1/S01E", StringComparison.OrdinalIgnoreCase))
                .OrderBy(e => e.key)
                .ToList();

            int imported = 0;

            foreach (var entry in episodeEntries)
            {
                string episodeId = entry.key.Replace("cc/s1/", "");
                string jsonPath = Path.Combine(jsonFolderPath, $"{episodeId}.json").Replace("\\", "/");

                var jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
                if (jsonAsset == null)
                    throw new Exception($"Missing episode JSON: {jsonPath}");

                if (verifySha256 && !string.IsNullOrEmpty(entry.sha256))
                {
                    string actual = Sha256Utf8(jsonAsset.text);
                    if (!actual.Equals(entry.sha256, StringComparison.OrdinalIgnoreCase))
                        throw new Exception($"SHA mismatch for {episodeId}. Expected {entry.sha256}, got {actual}.");
                }

                var wrapper = JsonConvert.DeserializeObject<SeasonExportWrapper>(jsonAsset.text);
                if (wrapper?.episode == null)
                    throw new Exception($"Episode payload missing in {episodeId}.json");

                string assetPath = Path.Combine(outFolderPath, $"{episodeId}.asset").Replace("\\", "/");
                var existing = AssetDatabase.LoadAssetAtPath<EpisodeAsset>(assetPath);

                EpisodeAsset asset;
                if (existing != null)
                {
                    if (!overwriteExisting) continue;
                    asset = existing;
                }
                else
                {
                    asset = ScriptableObject.CreateInstance<EpisodeAsset>();
                    AssetDatabase.CreateAsset(asset, assetPath);
                }

                ApplyEpisode(asset, wrapper.episode);
                EditorUtility.SetDirty(asset);
                imported++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[CC] Imported {imported} Episode assets into {outFolderPath}");
        }

        private static void ApplyEpisode(EpisodeAsset asset, EpisodeDto dto)
        {
            asset.episodeId = dto.episode_id;
            asset.title = dto.title;
            asset.arc = dto.arc;
            asset.blueprintFragment = dto.blueprint_fragment;
            asset.architectEcho = dto.architect_echo;
            asset.surfaceCrimePillar = dto.surface_crime_pillar;
            asset.factionFocus = dto.faction_focus;
            asset.villainTraits = dto.villain_traits ?? new List<string>();
            asset.primaryLearningAxis = dto.primary_learning_axis;
            asset.warrantPressure = dto.warrant_pressure;
            asset.neonSnapOpportunity = dto.neon_snap_opportunity;
            asset.endHook = dto.end_hook;
            asset.shipGate = dto.ship_gate;
            asset.packagePdf = dto.package_pdf;

            asset.scenes = new List<EpisodeAsset.SceneData>();
            if (dto.scenes != null)
            {
                foreach (var s in dto.scenes.OrderBy(x => x.scene_id))
                {
                    var scene = new EpisodeAsset.SceneData { sceneId = s.scene_id, choices = new List<EpisodeAsset.ChoiceData>() };
                    if (s.choices != null)
                    {
                        foreach (var c in s.choices.OrderBy(x => x.id))
                        {
                            scene.choices.Add(new EpisodeAsset.ChoiceData
                            {
                                id = c.id,
                                text = c.text,
                                primaryEffect = c.primary_effect,
                                shadowEffect = c.shadow_effect,
                                notes = c.notes,
                                deltas = new EpisodeAsset.DeltaData
                                {
                                    time = c.deltas?.time ?? 0,
                                    heat = c.deltas?.heat ?? 0,
                                    leadIntegrity = c.deltas?.lead_integrity ?? "no_change",
                                    gasket = c.deltas?.gasket ?? "no_change",
                                    flag = c.deltas?.flag ?? "no_change"
                                }
                            });
                        }
                    }
                    asset.scenes.Add(scene);
                }
            }

            asset.BuildIndexes();
        }

        private static string Sha256Utf8(string text)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(bytes);
            var sb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private static void EnsureFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath)) return;
            var parts = folderPath.Split('/');
            if (parts.Length == 0 || parts[0] != "Assets")
                throw new Exception($"Folder must be under Assets/. Got: {folderPath}");

            string current = "Assets";
            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }

        [Serializable] private class AddressablesCatalog { public List<Entry> entries; }
        [Serializable] private class Entry { public string key; public string path; public string sha256; }

        [Serializable] private class SeasonExportWrapper { public EpisodeDto episode; }

        [Serializable]
        private class EpisodeDto
        {
            public string episode_id;
            public string title;
            public string arc;
            public string blueprint_fragment;
            public string architect_echo;
            public string surface_crime_pillar;
            public string faction_focus;
            public List<string> villain_traits;
            public string primary_learning_axis;
            public string warrant_pressure;
            public string neon_snap_opportunity;
            public string end_hook;
            public string ship_gate;
            public string package_pdf;
            public List<SceneDto> scenes;
        }

        [Serializable] private class SceneDto { public int scene_id; public List<ChoiceDto> choices; }
        [Serializable] private class ChoiceDto { public string id; public string text; public string primary_effect; public string shadow_effect; public DeltaDto deltas; public string notes; }
        [Serializable] private class DeltaDto { public int time; public int heat; public string lead_integrity; public string gasket; public string flag; }
    }
}
#endif
