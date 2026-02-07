using UnityEngine;
using UnityEngine.Audio;

public class CCAudioDirector : MonoBehaviour
{
    public static CCAudioDirector Instance { get; private set; }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup ambienceGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup uiGroup;
    [SerializeField] private AudioMixerGroup voGroup;
    [SerializeField] private AudioMixerGroup pressGroup;
    [SerializeField] private AudioMixerGroup echoGroup;

    private AudioSource ambienceSource;
    private AudioSource sfxSource;
    private AudioSource uiSource;
    private AudioSource voSource;
    private AudioSource pressSource;
    private AudioSource echoSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create AudioSources
        ambienceSource = CreateAudioSource(ambienceGroup);
        sfxSource = CreateAudioSource(sfxGroup);
        uiSource = CreateAudioSource(uiGroup);
        voSource = CreateAudioSource(voGroup);
        pressSource = CreateAudioSource(pressGroup);
        echoSource = CreateAudioSource(echoGroup);
    }

    private AudioSource CreateAudioSource(AudioMixerGroup group)
    {
        GameObject go = new GameObject("AudioSource_" + group.name);
        go.transform.parent = transform;
        AudioSource source = go.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = group;
        return source;
    }

    private void Update()
    {
        // Update mixer parameters based on state bands
        if (CCAudioContextProvider.Instance != null)
        {
            mixer.SetFloat("Heat", CCAudioContextProvider.Instance.Heat);
            mixer.SetFloat("Time", CCAudioContextProvider.Instance.Time);
            mixer.SetFloat("LeadIntegrity", CCAudioContextProvider.Instance.LeadIntegrity);
        }
    }

    public static void Play(CCAudioBus bus, string eventId)
    {
        if (Instance == null) return;

        AudioClip clip = LoadClip(eventId);
        if (clip == null) return;

        AudioSource source = GetSource(bus);
        source.clip = clip;
        source.Play();
    }

    private static AudioSource GetSource(CCAudioBus bus)
    {
        switch (bus)
        {
            case CCAudioBus.AMBIENCE: return Instance.ambienceSource;
            case CCAudioBus.SFX: return Instance.sfxSource;
            case CCAudioBus.UI: return Instance.uiSource;
            case CCAudioBus.VO: return Instance.voSource;
            case CCAudioBus.PRESS: return Instance.pressSource;
            case CCAudioBus.ECHO: return Instance.echoSource;
            default: return null;
        }
    }

    private static AudioClip LoadClip(string eventId)
    {
        // Placeholder: load from Resources or Addressables
        // For now, return null
        Debug.Log("Loading clip for: " + eventId);
        return null;
    }
}