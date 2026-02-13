using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string id;
    public string name;
    public string description;
    public Sprite icon;
}

public class InteractableObject : MonoBehaviour
{
    public string objectId;
    public string observeDescription;
    public bool canEngage = true;
    public bool canUseProtocol = false;
    public List<string> requiredItems = new List<string>(); // For combination locks

    public virtual void OnObserve()
    {
        Debug.Log("Observing " + objectId + ": " + observeDescription);
        // Show description UI - basic implementation
        // In a full implementation, this would show a UI panel with the description
        // For now, we use Debug.Log and could extend to show UI
    }

    public virtual void OnEngage()
    {
        if (!canEngage)
        {
            Debug.Log("Cannot engage with " + objectId);
            return;
        }

        Debug.Log("Engaging with " + objectId);
        // Basic engagement logic - can be overridden in derived classes
        // For example, open a door, activate a device, etc.
    }

    public virtual void OnProtocol()
    {
        if (!canUseProtocol)
        {
            Debug.Log("Cannot use protocol on " + objectId);
            return;
        }

        Debug.Log("Using protocol on " + objectId);
        // Basic protocol logic - can be overridden in derived classes
        // Protocol might involve hacking, bypassing security, etc.
    }

    public virtual bool CanCombineWith(Item item)
    {
        return requiredItems.Contains(item.id);
    }

    public virtual void OnCombine(Item item)
    {
        Debug.Log("Combining " + item.name + " with " + objectId);
        // Basic combination result - can be overridden in derived classes
        // For example, unlock a door, create a new item, etc.
    }
}