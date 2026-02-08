using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Image npcPortrait;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI dialogueText;
    public Transform topicsContainer;
    public Button topicButtonPrefab;

    private List<Button> topicButtons = new List<Button>();
    private NPC currentNPC;

    void Start()
    {
        HideDialogue();
    }

    public void ShowDialogue(NPC npc)
    {
        currentNPC = npc;
        dialoguePanel.SetActive(true);

        if (npcPortrait != null && npc.portrait != null)
        {
            npcPortrait.sprite = npc.portrait;
        }

        if (npcName != null)
        {
            npcName.text = npc.name;
        }

        ShowTopics(npc.GetAvailableTopics());
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        currentNPC = null;

        // Clear topic buttons
        foreach (var button in topicButtons)
        {
            Destroy(button.gameObject);
        }
        topicButtons.Clear();
    }

    void ShowTopics(List<string> topics)
    {
        foreach (var topic in topics)
        {
            Button topicButton = Instantiate(topicButtonPrefab, topicsContainer);
            TextMeshProUGUI buttonText = topicButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = topic;
            }
            topicButton.onClick.AddListener(() => OnTopicSelected(topic));
            topicButtons.Add(topicButton);
        }
    }

    void OnTopicSelected(string topic)
    {
        if (currentNPC != null)
        {
            string response = currentNPC.GetResponse(topic);
            dialogueText.text = response;

            // Handle special topics
            if (topic == "Assignment")
            {
                // Unlock case notebook or similar
                AdventureGameManager.Instance.CompleteOnboardingPhase("dialogue_complete");
            }
        }
    }
}