using UnityEditor;
using UnityEngine;

public class CreateAudioDeltas
{
    [MenuItem("Tools/Create Episode Audio Deltas")]
    static void CreateDeltas()
    {
        // Create the delta library
        CCAudioDeltaLibrarySO library = ScriptableObject.CreateInstance<CCAudioDeltaLibrarySO>();
        AssetDatabase.CreateAsset(library, "Assets/Audio/CrimsonCompass/POLICY/CCAudioDeltaLibrary.asset");

        // Create 12 episode deltas (placeholder values)
        for (int i = 1; i <= 12; i++)
        {
            CCEpisodeAudioDeltaSO delta = ScriptableObject.CreateInstance<CCEpisodeAudioDeltaSO>();
            delta.EpisodeNumber = i;
            // Placeholder: set some basic deltas
            delta.BusVolumeDeltas[CCAudioBus.AMBIENCE] = 0f;
            delta.BusVolumeDeltas[CCAudioBus.SFX] = 0f;
            delta.BusVolumeDeltas[CCAudioBus.UI] = -0.5f;
            delta.BusVolumeDeltas[CCAudioBus.VO] = 0.5f;
            delta.BusVolumeDeltas[CCAudioBus.PRESS] = -1.5f + (i * 0.1f); // Slight increase per episode
            delta.BusVolumeDeltas[CCAudioBus.ECHO] = -2.5f;

            delta.HeatMultiplier = 1f;
            delta.TimeMultiplier = 1f;
            delta.LeadIntegrityMultiplier = 1f;

            AssetDatabase.CreateAsset(delta, $"Assets/Audio/CrimsonCompass/POLICY/Episode{i}Delta.asset");
            library.episodeDeltas.Add(delta);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Episode Audio Deltas created.");
    }
}