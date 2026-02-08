using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    [Header("Scene Objects")]
    public GameObject clockInPrompt;
    public GameObject seamlineTagPrefab;
    public GameObject devicePrefab;
    public GameObject npcPrefab;
    public GameObject anomalyPrefab;

    void Start()
    {
        SetupOnboardingScene();
    }

    void SetupOnboardingScene()
    {
        // Create clock in prompt
        if (clockInPrompt != null)
        {
            clockInPrompt.SetActive(true);
            ClockInPrompt prompt = clockInPrompt.GetComponent<ClockInPrompt>();
            if (prompt != null)
            {
                prompt.onClockInTapped += OnClockInTapped;
            }
        }

        // Create Seamline Tag
        GameObject tagObj = Instantiate(seamlineTagPrefab, new Vector3(-2f, 0f, 0f), Quaternion.identity);
        InteractableObject tag = tagObj.GetComponent<InteractableObject>();
        if (tag != null)
        {
            tag.objectId = "seamline_tag";
            tag.observeDescription = "A small electronic tag with seamline markings.";
            tag.canEngage = true;
        }

        // Create Device
        GameObject deviceObj = Instantiate(devicePrefab, new Vector3(2f, 0f, 0f), Quaternion.identity);
        DeviceInteractable device = deviceObj.GetComponent<DeviceInteractable>();
        if (device != null)
        {
            device.objectId = "stability_device";
            device.observeDescription = "A monitoring device showing global stability metrics.";
            device.canEngage = true;
            device.requiredItems.Add("seamline_tag");
        }

        // Create NPC
        GameObject npcObj = Instantiate(npcPrefab, new Vector3(0f, -2f, 0f), Quaternion.identity);
        NPC npc = npcObj.GetComponent<NPC>();
        if (npc != null)
        {
            npc.npcName = "Helix";
            npc.availableTopics.Add("Assignment");
            npc.availableTopics.Add("Protocol");
            npc.availableTopics.Add("Local Notes");
            npc.dialogueTopics["Assignment"] = "Your assignment is to verify the seamline tag and stabilize the custody routing.";
            npc.dialogueTopics["Protocol"] = "Follow standard protocol: observe, engage, and close the case.";
            npc.dialogueTopics["Local Notes"] = "Local notes indicate some unusual timing in recent operations.";
        }

        // Create Anomaly
        GameObject anomalyObj = Instantiate(anomalyPrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
        InteractableObject anomaly = anomalyObj.GetComponent<InteractableObject>();
        if (anomaly != null)
        {
            anomaly.objectId = "routing_anomaly";
            anomaly.observeDescription = "The custody routing binder shows an unusual entry.";
            anomaly.canEngage = true;
            anomaly.canUseProtocol = true;
        }
    }

    void OnClockInTapped()
    {
        // Start the onboarding sequence
        OnboardingManager onboarding = FindObjectOfType<OnboardingManager>();
        if (onboarding != null)
        {
            onboarding.OnClockInTapped();
        }
    }
}