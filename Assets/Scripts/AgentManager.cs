using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Core;

public class AgentManager : MonoBehaviour
    {
        public TextAsset agentsJson;
        public TextAsset messagesJsonl;

        private AgentsData agentsData;

        void Start()
        {
            agentsData = JsonUtility.FromJson<AgentsData>(agentsJson.text);
            GameManager.Instance.eventBus.Subscribe(GameEventType.SESSION_OPEN, OnSessionOpen);
            GameManager.Instance.eventBus.Subscribe(GameEventType.HINT_OFFERED, OnHintOffered);
        }

        void OnSessionOpen(object payload)
        {
            // HELIX: Provide recap
            Debug.Log("HELIX: New mission started. Suspects: " + string.Join(", ", System.Array.ConvertAll(GameManager.Instance.currentCase.suspects, s => s.name)));
        }

        void OnHintOffered(object payload)
        {
            // OPTIMUS: Offer hints
            var hintOptions = agentsData.optimusHelp.offerChoices;
            Debug.Log("OPTIMUS: Choose a hint: " + string.Join(", ", hintOptions));
        }

        public void RequestHint(string choice)
        {
            // Process hint request based on choice
            string hint = GetHintForChoice(choice);
            GameManager.Instance.eventBus.Publish(GameEventType.HINT_USED, hint);
            Debug.Log($"OPTIMUS: Hint provided - {hint}");
        }

        private string GetHintForChoice(string choice)
        {
            // Provide contextual hints based on current game state
            var state = GameManager.Instance.currentState;

            switch (choice.ToLower())
            {
                case "general":
                    return $"Current status: Time {state.timeBudget}, Heat {state.heat}, Lead Integrity {state.leadIntegrity}. " +
                           $"Be cautious with high heat levels.";

                case "suspect":
                    return "Focus on suspects with the most connections to the crime scene. " +
                           "Check their alibis carefully.";

                case "evidence":
                    return "Physical evidence is more reliable than witness testimony. " +
                           "Look for patterns in the evidence collection.";

                case "timing":
                    return $"You have {state.timeBudget} time segments remaining. " +
                           "Some actions take more time than others.";

                default:
                    return "Trust your instincts, but verify everything. " +
                           "The case details are in the evidence.";
            }
        }

        public void SelectGadgets(List<string> selected)
        {
            // ZTECH: Confirm selection
            Debug.Log("ZTECH: Gadgets selected: " + string.Join(", ", selected));
        }
    }

    [System.Serializable]
    public class AgentsData
    {
        public Agent[] agents;
        public OptimusHelp optimusHelp;
        public ZtechLoadout ztechLoadout;
    }

    [System.Serializable]
    public class Agent { public string id; public string codename; public string role; public string visibility; }
    [System.Serializable]
    public class OptimusHelp { public int hintsPerMission; public HintCost hintCost; public string[] offerChoices; }
    [System.Serializable]
    public class ZtechLoadout { public int offered; public int select; }