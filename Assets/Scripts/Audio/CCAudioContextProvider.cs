using UnityEngine;

public class CCAudioContextProvider : MonoBehaviour
{
    public static CCAudioContextProvider Instance { get; private set; }

    public int EpisodeNumber { get; set; }
    public float Heat { get; set; } // 0-100
    public float Time { get; set; } // 0-100
    public float LeadIntegrity { get; set; } // 0-100

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

    public void SetStateBands(float heat, float time, float leadIntegrity)
    {
        Heat = Mathf.Clamp(heat, 0, 100);
        Time = Mathf.Clamp(time, 0, 100);
        LeadIntegrity = Mathf.Clamp(leadIntegrity, 0, 100);
    }
}