using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Catalog")]
public class AudioCatalog : ScriptableObject
{
    public List<AudioEvent> events = new();

    private Dictionary<string, AudioEvent> _map;

    public void Init()
    {
        _map = new Dictionary<string, AudioEvent>();
        foreach (var e in events)
            if (!string.IsNullOrEmpty(e.eventId))
                _map[e.eventId] = e;
    }

    public AudioEvent Get(string id)
    {
        if (_map == null) Init();
        return _map.TryGetValue(id, out var e) ? e : null;
    }
}
