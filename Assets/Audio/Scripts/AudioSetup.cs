using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using System.IO;

public class AudioSetup
{
    [MenuItem("Audio/Setup Crimson Compass Audio System")]
    public static void SetupAudioSystem()
    {
        // Find the AudioMixer
        var mixerPath = "Assets/Audio/CrimsonCompass/MIXER/CrimsonCompassAudioMixer.mixer";
        var mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>(mixerPath);
        if (mixer == null)
        {
            Debug.LogError($"AudioMixer not found at {mixerPath}");
            return;
        }

        // Get mixer groups
        var masterGroup = mixer.FindMatchingGroups("MASTER")[0];
        var uiGroup = mixer.FindMatchingGroups("UI")[0];
        var sfxGroup = mixer.FindMatchingGroups("SFX")[0];
        var musGroup = mixer.FindMatchingGroups("MUS")[0];
        var voGroup = mixer.FindMatchingGroups("VO")[0];
        var ambGroup = mixer.FindMatchingGroups("AMB")[0];
        var gasketFxGroup = mixer.FindMatchingGroups("GASKET_FX")[0];

        // Get snapshots
        var normalSnapshot = mixer.FindSnapshot("Normal");
        var investigationSnapshot = mixer.FindSnapshot("Investigation");
        var extractionSnapshot = mixer.FindSnapshot("Extraction");
        var uneaseTailSnapshot = mixer.FindSnapshot("UneaseTail");

        // Create directories if they don't exist
        Directory.CreateDirectory("Assets/Audio/CrimsonCompass/EVENTS");

        // Create AudioEvent assets
        CreateAudioEvent("UI_CASE_CLOSED", uiGroup, normalSnapshot, "Assets/Audio/CrimsonCompass/EVENTS/UI_CASE_CLOSED.asset");
        CreateAudioEvent("UI_FILE_STAMP", uiGroup, normalSnapshot, "Assets/Audio/CrimsonCompass/EVENTS/UI_FILE_STAMP.asset");
        CreateAudioEvent("UNEASE_TAIL_A", sfxGroup, uneaseTailSnapshot, "Assets/Audio/CrimsonCompass/EVENTS/UNEASE_TAIL_A.asset");
        CreateAudioEvent("UNEASE_TAIL_B", sfxGroup, uneaseTailSnapshot, "Assets/Audio/CrimsonCompass/EVENTS/UNEASE_TAIL_B.asset");
        CreateAudioEvent("UNEASE_TAIL_C", sfxGroup, uneaseTailSnapshot, "Assets/Audio/CrimsonCompass/EVENTS/UNEASE_TAIL_C.asset");
        CreateAudioEvent("GF_S01_01", gasketFxGroup, normalSnapshot, "Assets/Audio/CrimsonCompass/EVENTS/GF_S01_01.asset");
        CreateAudioEvent("GF_S01_02", gasketFxGroup, normalSnapshot, "Assets/Audio/CrimsonCompass/EVENTS/GF_S01_02.asset");

        // Create AudioCatalog
        var catalog = ScriptableObject.CreateInstance<AudioCatalog>();
        AssetDatabase.CreateAsset(catalog, "Assets/Audio/CrimsonCompass/CATALOG/CrimsonCompassAudioCatalog.asset");

        // Load and add events to catalog
        var events = new System.Collections.Generic.List<AudioEvent>();
        var eventGuids = AssetDatabase.FindAssets("t:AudioEvent", new[] { "Assets/Audio/CrimsonCompass/EVENTS" });
        foreach (var guid in eventGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var audioEvent = AssetDatabase.LoadAssetAtPath<AudioEvent>(path);
            if (audioEvent != null)
            {
                events.Add(audioEvent);
            }
        }
        catalog.events = events;

        // Save catalog
        EditorUtility.SetDirty(catalog);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Crimson Compass Audio System setup complete!");
    }

    private static void CreateAudioEvent(string eventId, AudioMixerGroup outputGroup, AudioMixerSnapshot snapshot, string assetPath)
    {
        var audioEvent = ScriptableObject.CreateInstance<AudioEvent>();
        audioEvent.eventId = eventId;
        audioEvent.outputGroup = outputGroup;
        audioEvent.snapshot = snapshot;
        audioEvent.volume = 1f;
        audioEvent.pitchRandom = 0f;
        audioEvent.loop = false;
        audioEvent.singleInstance = false;

        AssetDatabase.CreateAsset(audioEvent, assetPath);
        Debug.Log($"Created AudioEvent: {eventId}");
    }

    [MenuItem("Audio/Create AudioService Prefab")]
    public static void CreateAudioServicePrefab()
    {
        // Find the AudioCatalog
        var catalog = AssetDatabase.LoadAssetAtPath<AudioCatalog>("Assets/Audio/CrimsonCompass/CATALOG/CrimsonCompassAudioCatalog.asset");
        if (catalog == null)
        {
            Debug.LogError("AudioCatalog not found. Run 'Setup Crimson Compass Audio System' first.");
            return;
        }

        // Create AudioService GameObject
        var go = new GameObject("AudioService");
        var audioService = go.AddComponent<AudioService>();
        audioService.catalog = catalog;

        // Create prefab
        var prefabPath = "Assets/Audio/CrimsonCompass/PREFABS/AudioService.prefab";
        PrefabUtility.SaveAsPrefabAsset(go, prefabPath);

        // Clean up
        Object.DestroyImmediate(go);

        Debug.Log("AudioService prefab created!");
    }
}