using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    [Header("Onboarding Objects")]
    public GameObject clockInPrompt;
    public InteractableObject seamlineTag;
    public InteractableObject device;
    public NPC deskNPC;
    public InteractableObject anomalyObject;

    [Header("Timing")]
    public float engageUnlockTime = 120f; // 2:00
    public float kitUnlockTime = 300f; // 5:00
    public float caseClosureTime = 510f; // 8:30

    private float startTime;

    void Start()
    {
        startTime = Time.time;
        StartCoroutine(OnboardingSequence());
    }

    IEnumerator OnboardingSequence()
    {
        // 0:00–0:45 — Cold Open
        yield return new WaitForSeconds(45f);

        // 0:45–2:00 — Verb Bar Appears (Move + Observe only)
        // Already handled in VerbBarUI.Start()

        // 2:00–3:30 — "ENGAGE" Unlock + First Micro-Problem
        yield return new WaitForSeconds(engageUnlockTime - 45f);
        AdventureGameManager.Instance.CompleteOnboardingPhase("engage_unlock");

        // 3:30–5:00 — Micro Dialogue
        yield return new WaitForSeconds(kitUnlockTime - engageUnlockTime);
        AdventureGameManager.Instance.CompleteOnboardingPhase("kit_unlock");

        // 5:00–6:30 — "KIT" Unlock + First Combination
        // Already unlocked above

        // 6:30–8:30 — The First "Case Closure" Loop
        yield return new WaitForSeconds(caseClosureTime - kitUnlockTime);
        // Wait for player to complete the sequence, then trigger closure
        StartCoroutine(WaitForCaseClosure());
    }

    IEnumerator WaitForCaseClosure()
    {
        // In a real implementation, this would wait for specific player actions
        // For now, just trigger after a delay
        yield return new WaitForSeconds(30f);
        AdventureGameManager.Instance.CloseCase();
    }

    public void OnClockInTapped()
    {
        if (clockInPrompt != null)
        {
            clockInPrompt.SetActive(false);
        }
        // Start the actual game
    }
}