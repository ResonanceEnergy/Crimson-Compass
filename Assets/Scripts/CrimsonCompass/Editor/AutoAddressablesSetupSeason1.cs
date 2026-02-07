#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace CrimsonCompass.EditorTools
{
    public static class AutoAddressablesSetupSeason1
    {
        private const string DefaultCatalogPath = "Assets/AddressableAssetsData/CrimsonCompass/Season1/addressables_catalog.json";
        private const string DefaultGroupName = "CrimsonCompass_Season1";
        private const string DefaultLabel = "cc_s1";
        private const string CatalogAddressKey = "cc/s1/catalog";

        [MenuItem("CrimsonCompass/Addressables/Auto-Setup Season 1 (from Catalog)")]
        public static void Run()
        {
            try
            {
                var catalogAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(DefaultCatalogPath);
                if (catalogAsset == null)
                    throw new Exception($"Catalog not found at '{DefaultCatalogPath}'.");

                var settings = AddressableAssetSettingsDefaultObject.Settings;
                if (settings == null)
                    throw new Exception("Addressables Settings not found. Open Addressables Groups window once.");

                var catalog = JsonConvert.DeserializeObject<AddressablesCatalog>(catalogAsset.text);
                if (catalog == null || catalog.entries == null || catalog.entries.Count == 0)
                    throw new Exception("Catalog parsed but contains no entries.");

                EnsureLabel(settings, DefaultLabel);
                var group = GetOrCreateGroup(settings, DefaultGroupName);

                int processed = 0;
                int missing = 0;

                foreach (var entry in catalog.entries)
                {
                    var guid = AssetDatabase.AssetPathToGUID(entry.path);
                    if (string.IsNullOrEmpty(guid))
                    {
                        missing++;
                        Debug.LogWarning($"[CC] Missing asset: {entry.path} (key={entry.key})");
                        continue;
                    }

                    // Strict SHA-256 verification (recommended)
                    if (!string.IsNullOrEmpty(entry.sha256))
                    {
                        var ta = AssetDatabase.LoadAssetAtPath<TextAsset>(entry.path);
                        if (ta != null)
                        {
                            string actual = Sha256Utf8(ta.text);
                            if (!actual.Equals(entry.sha256, StringComparison.OrdinalIgnoreCase))
                                throw new Exception($"SHA mismatch for {entry.key}. Expected {entry.sha256}, got {actual}.");
                        }
                    }

                    var aaEntry = settings.CreateOrMoveEntry(guid, group);
                    aaEntry.address = entry.key;
                    aaEntry.SetLabel(DefaultLabel, true);
                    processed++;
                }

                MarkCatalogAsAddressable(settings, group, DefaultCatalogPath, CatalogAddressKey, DefaultLabel);

                settings.SetDirty(AddressableAssetSettings.ModificationEvent.BatchModification, group, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("CC Addressables Setup Complete", $"Processed: {processed}\nMissing: {missing}\nGroup: {DefaultGroupName}\nLabel: {DefaultLabel}", "OK");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                EditorUtility.DisplayDialog("CC Addressables Setup FAILED", ex.Message, "OK");
            }
        }

        private static void EnsureLabel(AddressableAssetSettings settings, string label)
        {
            if (!settings.GetLabels().Contains(label)) settings.AddLabel(label);
        }

        private static AddressableAssetGroup GetOrCreateGroup(AddressableAssetSettings settings, string groupName)
        {
            var group = settings.groups.FirstOrDefault(g => g != null && g.Name == groupName);
            if (group != null) return group;

            group = settings.CreateGroup(groupName, false, false, false, null,
                typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));

            var bundled = group.GetSchema<BundledAssetGroupSchema>();
            if (bundled != null)
            {
                bundled.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
                bundled.Compression = BundledAssetGroupSchema.BundleCompressionMode.LZ4;
            }
            return group;
        }

        private static void MarkCatalogAsAddressable(AddressableAssetSettings settings, AddressableAssetGroup group, string assetPath, string address, string label)
        {
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (string.IsNullOrEmpty(guid)) return;
            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = address;
            entry.SetLabel(label, true);
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

        [Serializable]
        private class AddressablesCatalog
        {
            public string catalog_version;
            public string generated_utc;
            public List<string> notes;
            public List<Entry> entries;
        }

        [Serializable]
        private class Entry
        {
            public string key;
            public string label;
            public string group;
            public string path;
            public string sha256;
            public long bytes;
        }
    }
}
#endif
