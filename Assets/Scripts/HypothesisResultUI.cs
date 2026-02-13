using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrimsonCompass.Core;
using CrimsonCompass.Agents;

public class HypothesisResultUI : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public GameObject resultPanel;
    public Button continueButton;

    void Start()
    {
        if (resultPanel != null) resultPanel.SetActive(false);
        if (continueButton != null) continueButton.onClick.AddListener(HideResult);

        GameManager.Instance.eventBus.Subscribe(GameEventType.CASE_RESOLVED, OnCaseResolved);
        GameManager.Instance.eventBus.Subscribe(GameEventType.ACCUSATION_RESULT, OnAccusationResult);
        GameManager.Instance.eventBus.Subscribe(GameEventType.MISSION_COMPLETED, OnMissionCompleted);
    }

    void OnCaseResolved(object payload)
    {
        var hypothesis = (Hypothesis)payload;
        ShowResult("🎉 CASE SOLVED! 🎉\n\nYou correctly identified:\n" +
                  "WHO: " + GetSuspectName(hypothesis.whoId) + "\n" +
                  "HOW: " + GetMethodName(hypothesis.howId) + "\n" +
                  "WHERE: " + GetLocationName(hypothesis.whereId) + "\n\n" +
                  $"Final Score: {GameManager.Instance.currentScore}\n\n" +
                  "The case is closed!", Color.green);
    }

    void OnAccusationResult(object payload)
    {
        // Create a simple class to hold the result data instead of using dynamic
        var resultData = payload as System.Collections.Generic.Dictionary<string, object>;
        if (resultData != null && resultData.ContainsKey("result") && resultData["result"].ToString() == "no_disproof")
        {
            ShowResult("❓ No contradictions found\n\nYour hypothesis couldn't be disproven with current intel.\n" +
                      "You may be on the right track, or more investigation is needed.", Color.yellow);
        }
    }

    void ShowResult(string message, Color textColor)
    {
        if (resultText != null)
        {
            resultText.text = message;
            resultText.color = textColor;
        }
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }
    }

    void HideResult()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    string GetSuspectName(string id)
    {
        if (GameManager.Instance.currentCase?.suspects != null)
        {
            foreach (var suspect in GameManager.Instance.currentCase.suspects)
            {
                if (suspect.id == id) return suspect.name;
            }
        }
        return id;
    }

    string GetMethodName(string id)
    {
        if (GameManager.Instance.currentCase?.methods != null)
        {
            foreach (var method in GameManager.Instance.currentCase.methods)
            {
                if (method.id == id) return method.name;
            }
        }
        return id;
    }

    string GetLocationName(string id)
    {
        if (GameManager.Instance.currentCase?.locations != null)
        {
            foreach (var location in GameManager.Instance.currentCase.locations)
            {
                if (location.id == id) return location.country;
            }
        }
        return id;
    }

    void OnMissionCompleted(object payload)
    {
        var resultData = payload as System.Collections.Generic.Dictionary<string, object>;
        if (resultData != null && resultData.ContainsKey("success") && !(bool)resultData["success"])
        {
            string reason = resultData.ContainsKey("reason") && resultData["reason"].ToString() == "timeout" ? "TIME'S UP!" : "Mission Failed";
            ShowResult($"❌ {reason}\n\nThe case remains unsolved.\n\nBetter luck next time!", Color.red);
        }
    }
}