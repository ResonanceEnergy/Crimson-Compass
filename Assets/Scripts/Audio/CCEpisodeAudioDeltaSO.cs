using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CCEpisodeAudioDelta", menuName = "Crimson Compass/Episode Audio Delta")]
public class CCEpisodeAudioDeltaSO : ScriptableObject
{
    public int EpisodeNumber;

    // Bus volume deltas (additive)
    public Dictionary<CCAudioBus, float> BusVolumeDeltas = new Dictionary<CCAudioBus, float>();

    // Event specific overrides
    public Dictionary<string, AudioClip> EventClipOverrides = new Dictionary<string, AudioClip>();

    // State band multipliers
    public float HeatMultiplier = 1f;
    public float TimeMultiplier = 1f;
    public float LeadIntegrityMultiplier = 1f;

    public void ApplyDelta()
    {
        // Apply bus volume deltas to mixer
        // Placeholder
        Debug.Log("Applying delta for episode " + EpisodeNumber);

        // Apply state band adjustments
        // e.g., adjust mixer parameters based on state bands
    }
}