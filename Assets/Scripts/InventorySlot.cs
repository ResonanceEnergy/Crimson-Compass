using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public Button slotButton;

    private Item item;
    private Action<Item> onItemSelected;

    public void Setup(Item item, Action<Item> onItemSelected)
    {
        this.item = item;
        this.onItemSelected = onItemSelected;

        if (itemIcon != null && item.icon != null)
        {
            itemIcon.sprite = item.icon;
        }

        if (itemName != null)
        {
            itemName.text = item.name;
        }

        if (slotButton != null)
        {
            slotButton.onClick.AddListener(OnSlotClicked);
        }
    }

    void OnSlotClicked()
    {
        if (onItemSelected != null)
        {
            onItemSelected(item);
        }
    }
}