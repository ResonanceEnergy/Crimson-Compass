using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureGameManager : MonoBehaviour
{
    public static AdventureGameManager Instance;

    [Header("UI Components")]
    public VerbBarUI verbBarUI;
    public InventoryUI inventoryUI;
    public DialogueUI dialogueUI;
    public CaseClosureUI caseClosureUI;

    [Header("Game State")]
    public Verb currentVerb = Verb.MOVE;
    public List<InteractableObject> interactableObjects = new List<InteractableObject>();
    public List<Item> inventory = new List<Item>();
    public bool onboardingComplete = false;

    [Header("Onboarding")]
    public GameObject onboardingPrompt;
    public float onboardingStartTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartOnboarding();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                InteractableObject obj = hit.collider.GetComponent<InteractableObject>();
                if (obj != null)
                {
                    InteractWithObject(obj);
                }
                else
                {
                    // Handle walkable area
                    HandleMove(worldPoint);
                }
            }
        }
    }

    public void OnVerbSelected(Verb verb)
    {
        currentVerb = verb;
        Debug.Log("Selected verb: " + verb);
    }

    void InteractWithObject(InteractableObject obj)
    {
        switch (currentVerb)
        {
            case Verb.MOVE:
                HandleMove(obj.transform.position);
                break;
            case Verb.OBSERVE:
                obj.OnObserve();
                break;
            case Verb.ENGAGE:
                obj.OnEngage();
                break;
            case Verb.KIT:
                // Show inventory for combination
                inventoryUI.ShowInventoryForCombination(obj);
                break;
            case Verb.PROTOCOL:
                obj.OnProtocol();
                break;
        }
    }

    void HandleMove(Vector2 position)
    {
        // Move character to position
        Debug.Log("Moving to position: " + position);
        // TODO: Implement character movement
    }

    public void AddToInventory(Item item)
    {
        inventory.Add(item);
        inventoryUI.UpdateInventory();
    }

    public void RemoveFromInventory(Item item)
    {
        inventory.Remove(item);
        inventoryUI.UpdateInventory();
    }

    public bool HasItem(string itemId)
    {
        return inventory.Exists(i => i.id == itemId);
    }

    public void StartOnboarding()
    {
        onboardingStartTime = Time.time;
        if (onboardingPrompt != null)
        {
            onboardingPrompt.SetActive(true);
        }

        // Lock verbs except MOVE and OBSERVE
        verbBarUI.LockVerb(Verb.ENGAGE);
        verbBarUI.LockVerb(Verb.KIT);
        verbBarUI.LockVerb(Verb.PROTOCOL);
    }

    public void CompleteOnboardingPhase(string phase)
    {
        switch (phase)
        {
            case "engage_unlock":
                verbBarUI.UnlockVerb(Verb.ENGAGE);
                break;
            case "kit_unlock":
                verbBarUI.UnlockVerb(Verb.KIT);
                break;
            case "protocol_unlock":
                verbBarUI.UnlockVerb(Verb.PROTOCOL);
                break;
        }
    }

    public void TriggerUneaseTail()
    {
        // TODO: Implement UneaseTail audio/visual effect
        Debug.Log("UneaseTail triggered!");
    }

    public void CloseCase()
    {
        caseClosureUI.ShowCaseClosed();
        // TODO: Play motif puncture
        // TODO: Trigger UneaseTail after delay
        StartCoroutine(DelayedUneaseTail(2f));
    }

    IEnumerator DelayedUneaseTail(float delay)
    {
        yield return new WaitForSeconds(delay);
        TriggerUneaseTail();
    }
}