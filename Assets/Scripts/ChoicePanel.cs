using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrimsonCompass.Runtime;
using CrimsonCompass.Core;

namespace CrimsonCompass
{
    /// <summary>
    /// Presents advisor choices to the player
    /// </summary>
    public class ChoicePanel : MonoBehaviour
    {
        public GameObject panel;
        public Transform choiceContainer;
        public GameObject choiceButtonPrefab;

        private List<Button> choiceButtons = new List<Button>();

        void Start()
        {
            HideChoices();
        }

        public void ShowChoices(List<ChoiceDto> choices)
        {
            ClearChoices();

            foreach (var choice in choices)
            {
                GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
                Button button = buttonObj.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                buttonText.text = $"{choice.advisor}: {choice.label}";
                button.onClick.AddListener(() => OnChoiceSelected(choice));

                choiceButtons.Add(button);
            }

            panel.SetActive(true);
        }

        public void HideChoices()
        {
            panel.SetActive(false);
            ClearChoices();
        }

        void ClearChoices()
        {
            foreach (var button in choiceButtons)
            {
                Destroy(button.gameObject);
            }
            choiceButtons.Clear();
        }

        void OnChoiceSelected(ChoiceDto choice)
        {
            GameManager.Instance.eventBus.Publish(GameEventType.CHOICE_MADE, choice);
            HideChoices();
        }
    }
}