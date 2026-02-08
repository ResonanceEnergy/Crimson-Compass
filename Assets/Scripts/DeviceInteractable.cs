using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInteractable : InteractableObject
{
    public override void OnCombine(Item item)
    {
        base.OnCombine(item);

        if (item.id == "seamline_tag")
        {
            // Create authorized tag
            Item authorizedTag = new Item
            {
                id = "authorized_tag",
                name = "Authorized Tag",
                description = "A properly authorized seamline tag."
            };

            AdventureGameManager.Instance.AddToInventory(authorizedTag);
            Debug.Log("Created Authorized Tag from combination");

            // Update stability graph (visual effect)
            // TODO: Implement stability graph update
        }
    }

    public override void OnEngage()
    {
        base.OnEngage();

        if (AdventureGameManager.Instance.HasItem("authorized_tag"))
        {
            // Apply the tag to close the case
            Debug.Log("Applying authorized tag to device");
            AdventureGameManager.Instance.CloseCase();
        }
        else
        {
            Debug.Log("Need an authorized tag to engage with this device");
        }
    }
}