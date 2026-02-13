using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

public class AutoCreateAudioAssets : MonoBehaviour
{
    [MenuItem("Tools/Auto Create All Audio Assets")]
    static void CreateAll()
    {
        // AudioMixer already created manually
        CreateAudioDeltas();
    }

    // static void CreateAudioMixer()
    // {
    //     string path = "Assets/Audio/CrimsonCompass/MIXER/CrimsonCompassAudioMixer.mixer";
    //     if (AssetDatabase.LoadAssetAtPath<AudioMixer>(path) != null) return;
    //     AudioMixer mixer = AudioMixer.CreateMixer("CrimsonCompassAudioMixer");
    //     AssetDatabase.CreateAsset(mixer, path);
    //     AssetDatabase.Refresh();
    //     Debug.Log("AudioMixer created at " + path);
    // }

    static void CreateAudioDeltas()
    {
        string libraryPath = "Assets/Resources/CCAudioDeltaLibrary.asset";
        if (AssetDatabase.LoadAssetAtPath<CCAudioDeltaLibrarySO>(libraryPath) != null) return;

        CCAudioDeltaLibrarySO library = ScriptableObject.CreateInstance<CCAudioDeltaLibrarySO>();
        AssetDatabase.CreateAsset(library, libraryPath);

        for (int i = 1; i <= 12; i++)
        {
            string deltaPath = $"Assets/Resources/Episode{i}Delta.asset";
            if (AssetDatabase.LoadAssetAtPath<CCEpisodeAudioDeltaSO>(deltaPath) != null) continue;

            CCEpisodeAudioDeltaSO delta = ScriptableObject.CreateInstance<CCEpisodeAudioDeltaSO>();
            delta.EpisodeNumber = i;
            // Initialize dictionary
            delta.BusVolumeDeltas = new System.Collections.Generic.Dictionary<CCAudioBus, float>();
            // Set placeholder deltas
            delta.BusVolumeDeltas[CCAudioBus.AMBIENCE] = 0f;
            delta.BusVolumeDeltas[CCAudioBus.SFX] = 0f;
            delta.BusVolumeDeltas[CCAudioBus.UI] = -0.5f;
            delta.BusVolumeDeltas[CCAudioBus.VO] = 0.5f;
            delta.BusVolumeDeltas[CCAudioBus.PRESS] = -1.5f + (i * 0.1f);
            delta.BusVolumeDeltas[CCAudioBus.ECHO] = -2.5f;
            delta.HeatMultiplier = 1f;
            delta.TimeMultiplier = 1f;
            delta.LeadIntegrityMultiplier = 1f;

            AssetDatabase.CreateAsset(delta, deltaPath);
            library.episodeDeltas.Add(delta);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Audio Deltas created.");
    }
}