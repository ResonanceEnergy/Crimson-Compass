using UnityEngine;

public static class CCAudioManual
{
    public static void PlayUI(string eventId)
    {
        if (CCAudioCanonGuardrails.IsAllowed(CCAudioBus.UI, eventId))
        {
            CCAudioDirector.Play(CCAudioBus.UI, eventId);
        }
    }

    public static void PlaySFX(string eventId)
    {
        if (CCAudioCanonGuardrails.IsAllowed(CCAudioBus.SFX, eventId))
        {
            CCAudioDirector.Play(CCAudioBus.SFX, eventId);
        }
    }

    public static void PlayPRESS(string eventId)
    {
        if (CCAudioCanonGuardrails.IsAllowed(CCAudioBus.PRESS, eventId))
        {
            CCAudioDirector.Play(CCAudioBus.PRESS, eventId);
        }
    }

    public static void PlayECHO(string eventId)
    {
        if (CCAudioCanonGuardrails.IsAllowed(CCAudioBus.ECHO, eventId))
        {
            CCAudioDirector.Play(CCAudioBus.ECHO, eventId);
        }
    }

    public static void PlayVO(string eventId)
    {
        if (CCAudioCanonGuardrails.IsAllowed(CCAudioBus.VO, eventId))
        {
            CCAudioDirector.Play(CCAudioBus.VO, eventId);
        }
    }

    public static void PlayAMBIENCE(string eventId)
    {
        if (CCAudioCanonGuardrails.IsAllowed(CCAudioBus.AMBIENCE, eventId))
        {
            CCAudioDirector.Play(CCAudioBus.AMBIENCE, eventId);
        }
    }
}