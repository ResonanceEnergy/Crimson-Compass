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
        // TODO: Show description UI
    }

    public virtual void OnEngage()
    {
        if (!canEngage)
        {
            Debug.Log("Cannot engage with " + objectId);
            return;
        }

        Debug.Log("Engaging with " + objectId);
        // TODO: Implement specific engagement logic
    }

    public virtual void OnProtocol()
    {
        if (!canUseProtocol)
        {
            Debug.Log("Cannot use protocol on " + objectId);
            return;
        }

        Debug.Log("Using protocol on " + objectId);
        // TODO: Implement protocol logic
    }

    public virtual bool CanCombineWith(Item item)
    {
        return requiredItems.Contains(item.id);
    }

    public virtual void OnCombine(Item item)
    {
        Debug.Log("Combining " + item.name + " with " + objectId);
        // TODO: Implement combination result
    }
}