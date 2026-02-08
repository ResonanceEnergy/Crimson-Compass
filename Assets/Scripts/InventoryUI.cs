using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform itemContainer;
    public GameObject itemSlotPrefab;
    public Button combineButton;
    public Button cancelButton;

    private List<GameObject> itemSlots = new List<GameObject>();
    private InteractableObject currentTargetObject;
    private Item selectedItem;

    void Start()
    {
        combineButton.onClick.AddListener(OnCombineClicked);
        cancelButton.onClick.AddListener(HideInventory);
        HideInventory();
    }

    public void UpdateInventory()
    {
        // Clear existing slots
        foreach (var slot in itemSlots)
        {
            Destroy(slot);
        }
        itemSlots.Clear();

        // Create new slots
        foreach (var item in AdventureGameManager.Instance.inventory)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemContainer);
            InventorySlot slotComponent = slot.GetComponent<InventorySlot>();
            if (slotComponent != null)
            {
                slotComponent.Setup(item, OnItemSelected);
            }
            itemSlots.Add(slot);
        }
    }

    public void ShowInventoryForCombination(InteractableObject targetObject)
    {
        currentTargetObject = targetObject;
        UpdateInventory();
        inventoryPanel.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
        currentTargetObject = null;
        selectedItem = null;
    }

    void OnItemSelected(Item item)
    {
        selectedItem = item;
        combineButton.interactable = (currentTargetObject != null && currentTargetObject.CanCombineWith(item));
    }

    void OnCombineClicked()
    {
        if (currentTargetObject != null && selectedItem != null)
        {
            currentTargetObject.OnCombine(selectedItem);
            AdventureGameManager.Instance.RemoveFromInventory(selectedItem);
            HideInventory();
        }
    }
}