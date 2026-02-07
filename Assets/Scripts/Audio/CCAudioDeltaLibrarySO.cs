using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CCAudioDeltaLibrary", menuName = "Crimson Compass/Audio Delta Library")]
public class CCAudioDeltaLibrarySO : ScriptableObject
{
    [SerializeField] public List<CCEpisodeAudioDeltaSO> episodeDeltas = new List<CCEpisodeAudioDeltaSO>();

    public CCEpisodeAudioDeltaSO GetDeltaForEpisode(int episodeNumber)
    {
        return episodeDeltas.Find(delta => delta.EpisodeNumber == episodeNumber);
    }
}