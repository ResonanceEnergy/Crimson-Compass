using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource voiceSource;
    public AudioSource ambienceSource;

    [Header("Audio Mixers")]
    public AudioMixer masterMixer;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup voiceGroup;
    public AudioMixerGroup ambienceGroup;

    [Header("Audio Clips")]
    public AudioClip[] musicTracks;
    public AudioClip[] sfxClips;
    public AudioClip[] voiceClips;
    public AudioClip[] ambienceClips;

    private Dictionary<string, AudioClip> audioLibrary = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioLibrary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeAudioLibrary()
    {
        // Load all audio clips into dictionary for easy access
        foreach (var clip in musicTracks)
        {
            if (clip != null) audioLibrary[clip.name] = clip;
        }
        foreach (var clip in sfxClips)
        {
            if (clip != null) audioLibrary[clip.name] = clip;
        }
        foreach (var clip in voiceClips)
        {
            if (clip != null) audioLibrary[clip.name] = clip;
        }
        foreach (var clip in ambienceClips)
        {
            if (clip != null) audioLibrary[clip.name] = clip;
        }

        Debug.Log($"Audio library initialized with {audioLibrary.Count} clips");
    }

    // Music playback
    public void PlayMusic(string clipName, bool loop = true)
    {
        if (audioLibrary.TryGetValue(clipName, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
            Debug.Log($"Playing music: {clipName}");
        }
        else
        {
            Debug.LogWarning($"Music clip not found: {clipName}");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void FadeMusic(float targetVolume, float duration)
    {
        StartCoroutine(FadeAudio(musicSource, targetVolume, duration));
    }

    // SFX playback
    public void PlaySFX(string clipName, float volume = 1f)
    {
        if (audioLibrary.TryGetValue(clipName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip, volume);
            Debug.Log($"Playing SFX: {clipName}");
        }
        else
        {
            Debug.LogWarning($"SFX clip not found: {clipName}");
        }
    }

    // Effect playback (alias for SFX)
    public void PlayEffect(string effectName, float volume = 1f)
    {
        PlaySFX(effectName, volume);
    }

    // Voice playback
    public void PlayVoice(string clipName)
    {
        if (audioLibrary.TryGetValue(clipName, out AudioClip clip))
        {
            voiceSource.clip = clip;
            voiceSource.Play();
            Debug.Log($"Playing voice: {clipName}");
        }
        else
        {
            Debug.LogWarning($"Voice clip not found: {clipName}");
        }
    }

    // Ambience playback
    public void PlayAmbience(string clipName, bool loop = true)
    {
        if (audioLibrary.TryGetValue(clipName, out AudioClip clip))
        {
            ambienceSource.clip = clip;
            ambienceSource.loop = loop;
            ambienceSource.Play();
            Debug.Log($"Playing ambience: {clipName}");
        }
        else
        {
            Debug.LogWarning($"Ambience clip not found: {clipName}");
        }
    }

    public void StopAmbience()
    {
        ambienceSource.Stop();
    }

    // Gasket fragment audio (sensory)
    public void PlayGasketFragment(string fragmentId)
    {
        switch (fragmentId)
        {
            case "GF_S01_01":
                // Double stamp THUNK-THUNK
                PlaySFX("gasket_stamp_double");
                // Ink crescent smear
                PlaySFX("gasket_ink_smear", 0.7f);
                // Spoken: "...we were already late—"
                StartCoroutine(PlayFragmentSequence(new[] { "gasket_voice_late" }, 1.5f));
                break;
            case "GF_S01_02":
                // Tinnitus ring
                PlaySFX("gasket_tinnitus");
                // Elevator chime wrong note
                PlaySFX("gasket_elevator_wrong", 0.8f);
                // Spoken: "Not like this."
                StartCoroutine(PlayFragmentSequence(new[] { "gasket_voice_not_like_this" }, 2f));
                break;
        }
    }

    private IEnumerator PlayFragmentSequence(string[] clips, float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var clip in clips)
        {
            PlayVoice(clip);
            yield return new WaitForSeconds(voiceSource.clip.length + 0.5f);
        }
    }

    // Volume controls
    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void SetVoiceVolume(float volume)
    {
        masterMixer.SetFloat("VoiceVolume", Mathf.Log10(volume) * 20);
    }

    public void SetAmbienceVolume(float volume)
    {
        masterMixer.SetFloat("AmbienceVolume", Mathf.Log10(volume) * 20);
    }

    // Fade coroutine
    private IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }
}