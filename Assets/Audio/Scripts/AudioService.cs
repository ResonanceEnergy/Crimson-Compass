using UnityEngine;

public class AudioService : MonoBehaviour
{
    public static AudioService I;
    public AudioCatalog catalog;

    private readonly System.Collections.Generic.HashSet<string> _playingSingle = new();

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        if (catalog != null) catalog.Init();
    }

    public void Play(string eventId)
    {
        if (catalog == null) { Debug.LogWarning("AudioService: catalog not assigned."); return; }
        var ev = catalog.Get(eventId);
        if (ev == null) { Debug.LogWarning($"AudioEvent not found: {eventId}"); return; }

        if (ev.singleInstance && _playingSingle.Contains(eventId))
            return;

        var go = new GameObject($"Audio_{eventId}");
        var src = go.AddComponent<AudioSource>();

        var clips = ev.clips;
        if (clips == null || clips.Length == 0) { Destroy(go); Debug.LogWarning($"AudioEvent has no clips: {eventId}"); return; }

        src.clip = clips[Random.Range(0, clips.Length)];
        src.volume = ev.volume;
        src.pitch = 1f + Random.Range(-ev.pitchRandom, ev.pitchRandom);
        src.outputAudioMixerGroup = ev.outputGroup;
        src.loop = ev.loop;

        if (ev.snapshot != null)
            ev.snapshot.TransitionTo(ev.snapshotBlendTime);

        if (ev.singleInstance) _playingSingle.Add(eventId);

        src.Play();

        if (!ev.loop)
            Destroy(go, src.clip.length + 0.1f);
        else
            DontDestroyOnLoad(go);

        if (ev.singleInstance && !ev.loop)
            StartCoroutine(ClearSingleAfter(eventId, src.clip.length));
    }

    private System.Collections.IEnumerator ClearSingleAfter(string id, float t)
    {
        yield return new WaitForSeconds(t + 0.1f);
        _playingSingle.Remove(id);
    }
}
