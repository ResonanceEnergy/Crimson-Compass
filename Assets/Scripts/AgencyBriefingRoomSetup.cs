using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CrimsonCompass
{
    /// <summary>
    /// Sets up the Agency Briefing Room scene for Episode 1
    /// </summary>
    public class AgencyBriefingRoomSetup : MonoBehaviour
    {
        [Header("Scene Objects")]
        public GameObject deskTerminal;
        public GameObject filingCabinet;
        public GameObject coffeeMachine;
        public GameObject doorToHallway;

        [Header("NPCs")]
        public GameObject helixNPC;
        public GameObject optimusNPC;
        public GameObject ztechNPC;
        public GameObject gasketNPC;

        [Header("UI Elements")]
        public GameObject onboardingPrompt;
        public TextMeshProUGUI onboardingText;

        void Start()
        {
            SetupInteractiveObjects();
            SetupNPCs();
            StartOnboardingSequence();
        }

        void SetupInteractiveObjects()
        {
            // Desk Terminal
            var terminal = deskTerminal.AddComponent<InteractableObject>();
            terminal.objectId = "desk_terminal";
            terminal.observeDescription = "A computer terminal with access to the agency database. The screen shows login credentials and case files.";
            terminal.canEngage = true;
            terminal.canUseProtocol = false;

            var terminalScript = deskTerminal.AddComponent<DeskTerminal>();
            terminalScript.observeDescription = terminal.observeDescription;

            // Filing Cabinet
            var cabinet = filingCabinet.AddComponent<InteractableObject>();
            cabinet.objectId = "filing_cabinet";
            cabinet.observeDescription = "Metal filing cabinets containing case files and personnel records. Some drawers are locked.";
            cabinet.canEngage = true;
            cabinet.canUseProtocol = false;

            // Coffee Machine
            var coffee = coffeeMachine.AddComponent<InteractableObject>();
            coffee.objectId = "coffee_machine";
            coffee.observeDescription = "A standard office coffee machine. The pot is half full and there's a stack of paper cups nearby.";
            coffee.canEngage = true;
            coffee.canUseProtocol = false;

            var coffeeScript = coffeeMachine.AddComponent<CoffeeMachine>();
            coffeeScript.observeDescription = coffee.observeDescription;

            // Door to Hallway
            var door = doorToHallway.AddComponent<InteractableObject>();
            door.objectId = "door_hallway";
            door.observeDescription = "A reinforced door leading to the main corridor. It requires security clearance to open.";
            door.canEngage = true;
            door.canUseProtocol = true;

            var doorScript = doorToHallway.AddComponent<SceneTransitionDoor>();
            doorScript.targetScene = "S01E01_UrbanRooftop";
            doorScript.observeDescription = door.observeDescription;
        }

        void SetupNPCs()
        {
            // Helix - Administrative Coordinator
            var helix = helixNPC.AddComponent<NPC>();
            helix.npcName = "Helix";
            helix.portrait = Resources.Load<Sprite>("Portraits/helix");
            helix.dialogueTopics.Add("greeting", "Welcome to Crimson Compass, Closer. I'm Helix - I coordinate operations and keep everyone from going rogue.");
            helix.dialogueTopics.Add("case_info", "We've got a contractor ring stealing prototype tech. Your first assignment.");
            helix.dialogueTopics.Add("team_info", "You'll work with Optimus (intel), ZTech (tech), and sometimes Gasket (unofficial consultant).");
            helix.availableTopics.AddRange(new string[] { "case_info", "team_info" });

            // Optimus - Intelligence Analyst
            var optimus = optimusNPC.AddComponent<NPC>();
            optimus.npcName = "Optimus";
            optimus.portrait = Resources.Load<Sprite>("Portraits/optimus");
            optimus.dialogueTopics.Add("greeting", "Access logs show 47 unauthorized entries. Pattern suggests insider involvement.");
            optimus.dialogueTopics.Add("evidence", "Vehicle registrations cross-reference with contractor database. One match found.");
            optimus.dialogueTopics.Add("warning", "Probability of armed resistance: 23%. Exercise caution.");
            optimus.availableTopics.AddRange(new string[] { "evidence", "warning" });

            // ZTech - Technical Specialist
            var ztech = ztechNPC.AddComponent<NPC>();
            ztech.npcName = "ZTech";
            ztech.portrait = Resources.Load<Sprite>("Portraits/ztech");
            ztech.dialogueTopics.Add("greeting", "Thermal imaging shows building overheating. Prototype cooling system compromised.");
            ztech.dialogueTopics.Add("tech_scan", "Their security is outdated. I can bypass it, but it'll leave traces.");
            ztech.dialogueTopics.Add("gadgets", "I've got scanners, lockpicks, and surveillance drones ready.");
            ztech.availableTopics.AddRange(new string[] { "tech_scan", "gadgets" });

            // Gasket - Unofficial Consultant
            var gasket = gasketNPC.AddComponent<NPC>();
            gasket.npcName = "Gasket";
            gasket.portrait = Resources.Load<Sprite>("Portraits/gasket");
            gasket.dialogueTopics.Add("greeting", "Something's wrong here. The setup feels like a trap.");
            gasket.dialogueTopics.Add("intuition", "Trust your instincts. Not everything is as it seems.");
            gasket.dialogueTopics.Add("experience", "I've seen operations like this before. They always go sideways.");
            gasket.availableTopics.AddRange(new string[] { "intuition", "experience" });
        }

        void StartOnboardingSequence()
        {
            if (onboardingPrompt != null)
            {
                onboardingPrompt.SetActive(true);
                StartCoroutine(OnboardingSequence());
            }
        }

        IEnumerator OnboardingSequence()
        {
            // Phase 1: Movement
            onboardingText.text = "Welcome to Crimson Compass!\n\nUse the MOVE verb to navigate around the room.\nClick on the floor to move your character.";
            yield return new WaitForSeconds(5f);

            // Phase 2: Observation
            onboardingText.text = "Use OBSERVE to examine objects in the room.\nTry observing the desk terminal and filing cabinet.";
            AdventureGameManager.Instance.CompleteOnboardingPhase("observe_unlock");
            yield return new WaitForSeconds(5f);

            // Phase 3: Engagement
            onboardingText.text = "Use ENGAGE to interact with people and objects.\nTry talking to the team members around the room.";
            AdventureGameManager.Instance.CompleteOnboardingPhase("engage_unlock");
            yield return new WaitForSeconds(5f);

            // Phase 4: Inventory
            onboardingText.text = "Use KIT to access your inventory.\nYou'll find your initial equipment there.";
            AdventureGameManager.Instance.CompleteOnboardingPhase("kit_unlock");
            yield return new WaitForSeconds(5f);

            // Phase 5: Protocol
            onboardingText.text = "Use PROTOCOL for special actions.\nSome objects require specific protocols to interact with.";
            AdventureGameManager.Instance.CompleteOnboardingPhase("protocol_unlock");
            yield return new WaitForSeconds(3f);

            // Complete onboarding
            onboardingPrompt.SetActive(false);
            AdventureGameManager.Instance.onboardingComplete = true;
        }
    }

    // Specific object implementations
    public class DeskTerminal : InteractableObject
    {
        public override void OnObserve()
        {
            base.OnObserve();
            // Show database access interface
            Debug.Log("Opening agency database...");
        }

        public override void OnEngage()
        {
            base.OnEngage();
            // Access case files
            Debug.Log("Accessing case files...");
        }
    }

    public class CoffeeMachine : InteractableObject
    {
        public override void OnEngage()
        {
            base.OnEngage();
            // Provide morale boost
            Debug.Log("Coffee consumed - morale boosted!");
            // TODO: Implement morale system
        }
    }

    public class SceneTransitionDoor : InteractableObject
    {
        public string targetScene;

        public override void OnObserve()
        {
            base.OnObserve();
        }

        public override void OnEngage()
        {
            base.OnEngage();
            // Check if player has required items/clearance
            if (AdventureGameManager.Instance.HasItem("agency_id_badge"))
            {
                Debug.Log("Access granted. Transitioning to " + targetScene);
                // TODO: Load next scene
            }
            else
            {
                Debug.Log("Access denied. Security clearance required.");
            }
        }

        public override void OnProtocol()
        {
            base.OnProtocol();
            // Use security protocol
            Debug.Log("Using security protocol...");
        }
    }
}