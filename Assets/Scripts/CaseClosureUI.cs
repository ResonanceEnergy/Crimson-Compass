using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaseClosureUI : MonoBehaviour
{
    public GameObject caseClosedPanel;
    public TextMeshProUGUI caseClosedText;
    public float displayDuration = 3f;

    void Start()
    {
        HideCaseClosed();
    }

    public void ShowCaseClosed()
    {
        caseClosedPanel.SetActive(true);
        if (caseClosedText != null)
        {
            caseClosedText.text = "CASE CLOSED";
        }
        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        HideCaseClosed();
    }

    void HideCaseClosed()
    {
        caseClosedPanel.SetActive(false);
    }
}