using UnityEngine;

public class CCAudioDeltaApplier : MonoBehaviour
{
    public static CCAudioDeltaApplier Instance { get; private set; }

    [SerializeField] public CCAudioDeltaLibrarySO deltaLibrary;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyEpisodeDelta(int episodeNumber)
    {
        if (deltaLibrary == null) return;

        CCEpisodeAudioDeltaSO delta = deltaLibrary.GetDeltaForEpisode(episodeNumber);
        if (delta != null)
        {
            delta.ApplyDelta();
        }
    }
}