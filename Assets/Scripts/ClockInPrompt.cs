using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ClockInPrompt : MonoBehaviour
{
    public Button clockInButton;
    public TextMeshProUGUI promptText;
    public Action onClockInTapped;

    void Start()
    {
        if (clockInButton != null)
        {
            clockInButton.onClick.AddListener(OnClockInClicked);
        }

        if (promptText != null)
        {
            promptText.text = "Tap to clock in.";
        }
    }

    void OnClockInClicked()
    {
        onClockInTapped?.Invoke();
        gameObject.SetActive(false);
    }
}