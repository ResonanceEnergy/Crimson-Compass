using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractableObject
{
    public string npcName;
    public Sprite portrait;
    public Dictionary<string, string> dialogueTopics = new Dictionary<string, string>();
    public List<string> availableTopics = new List<string>();

    public override void OnEngage()
    {
        base.OnEngage();
        // Show dialogue UI
        if (AdventureGameManager.Instance != null && AdventureGameManager.Instance.dialogueUI != null)
        {
            AdventureGameManager.Instance.dialogueUI.ShowDialogue(this);
        }
    }

    public List<string> GetAvailableTopics()
    {
        return availableTopics;
    }

    public string GetResponse(string topic)
    {
        if (dialogueTopics.ContainsKey(topic))
        {
            return dialogueTopics[topic];
        }
        return "I don't have anything to say about that.";
    }
}