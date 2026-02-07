using UnityEngine;

public static class CCAudioCanonGuardrails
{
    private static bool echoPlayedThisEpisode = false;
    private static bool disproofPlayedThisEpisode = false;

    public static void ResetForNewEpisode()
    {
        echoPlayedThisEpisode = false;
        disproofPlayedThisEpisode = false;
    }

    public static bool IsAllowed(CCAudioBus bus, string eventId)
    {
        switch (bus)
        {
            case CCAudioBus.ECHO:
                if (echoPlayedThisEpisode)
                {
                    Debug.LogWarning("ECHO already played this episode. Ignoring: " + eventId);
                    return false;
                }
                echoPlayedThisEpisode = true;
                return true;
            case CCAudioBus.UI:
                // Check if disproof related
                if (eventId.Contains("disproof"))
                {
                    if (disproofPlayedThisEpisode)
                    {
                        Debug.LogWarning("Disproof already played this episode. Ignoring: " + eventId);
                        return false;
                    }
                    disproofPlayedThisEpisode = true;
                }
                return true;
            default:
                return true;
        }
    }
}