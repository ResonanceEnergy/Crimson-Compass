using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Core;

public class GasketManager : MonoBehaviour
{
    public static GasketManager Instance;

    public List<string> fragments = new List<string>();
    public bool isActive = false;

    void Awake()
    {
        Instance = this;
        GameManager.Instance.eventBus.Subscribe(GameEventType.CASE_RESOLVED, OnCaseResolved);
    }

    void OnCaseResolved(object payload)
    {
        string caseId = (string)payload;
        if (GameManager.Instance.currentCase.gasket && GameManager.Instance.currentCase.caseId == caseId)
        {
            TriggerFragment(caseId);
        }
        if (GameManager.Instance.currentCase.catastrophicChoice && GameManager.Instance.currentCase.caseId == caseId)
        {
            // EP10 has catastrophic choice available
            Debug.Log("GASKET: Catastrophic choice available - choose wisely!");
        }
    }

    public void TriggerFragment(string caseId)
    {
        string fragment = GetFragmentForCase(caseId);
        if (!string.IsNullOrEmpty(fragment))
        {
            fragments.Add(fragment);
            Debug.Log("GASKET FRAGMENT: " + fragment);            // Trigger audio fragment
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayGasketFragment(fragment.Split(':')[0].Trim());
            }            // TODO: Display fragment UI (sensory, non-linear)
        }
    }

    private string GetFragmentForCase(string caseId)
    {
        switch (caseId)
        {
            case "CASE-0002": return "GF_S01_01: Trigger: custody form / case closure stamp. Sensory: double stamp THUNK-THUNK; ink crescent smear. Spoken: '...we were already late—'";
            case "CASE-0006": return "GF_S01_01"; // EP06
            case "CASE-0010": return "GF_S01_02: Trigger: timing skew + siren tone. Sensory: tinnitus ring; elevator chime wrong note. Spoken: 'Not like this.'";
            default: return null;
        }
    }

    public void ChooseCatastrophicOption()
    {
        // Gasket choice: massive heat/time spike, cover blown, safe house reset
        var state = GameManager.Instance.currentState;
        state.heat += 50; // Massive spike
        state.timeBudget -= 24; // Massive time loss
        Debug.Log("GASKET CHOICE: Cover blown! Forced extraction to safe house. Investigation reset.");
        // TODO: Reset board, move to safe house scene
        GameManager.Instance.eventBus.Publish(GameEventType.SAFE_HOUSE_RESET, null);
    }
}