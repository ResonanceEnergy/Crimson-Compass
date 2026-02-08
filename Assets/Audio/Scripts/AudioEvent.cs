using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Audio Event")]
public class AudioEvent : ScriptableObject
{
    [Header("Identity")]
    public string eventId;

    [Header("Clips")]
    public AudioClip[] clips;

    [Header("Playback")]
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitchRandom = 0f;
    public bool loop;

    [Header("Routing")]
    public AudioMixerGroup outputGroup;

    [Header("Snapshot")]
    public AudioMixerSnapshot snapshot;
    [Range(0f, 2f)] public float snapshotBlendTime = 0.25f;

    [Header("Concurrency")]
    public bool singleInstance;
}
