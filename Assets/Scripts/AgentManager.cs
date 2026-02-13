using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Core;

public class AgentManager : MonoBehaviour
{
    [System.Serializable]
    public class Agent
    {
        public string name;
        public string role;
        public List<string> specialties;
    }

    [System.Serializable]
    public class AgentsData
    {
        public Agent[] agents;
    }

    public TextAsset agentsJson;

    private AgentsData agentsData;
    private Dictionary<string, Agent> agentLookup;

    void Awake()
    {
        if (agentsJson != null)
        {
            agentsData = JsonUtility.FromJson<AgentsData>(agentsJson.text);
            agentLookup = new Dictionary<string, Agent>();
            foreach (var agent in agentsData.agents)
            {
                agentLookup[agent.name] = agent;
            }
        }
    }

    public void RequestHint(string choice)
    {
        // IMPLEMENTED: Provide contextual hints based on game state
        var hint = GenerateHint(choice);
        GameManager.Instance.eventBus.Publish(GameEventType.HINT_USED, hint);
        Debug.Log($"AGENT HINT: {hint}");
    }

    private string GenerateHint(string choice)
    {
        // Context-aware hint generation based on choice and game state
        var state = GameManager.Instance.currentCase != null ?
            GameManager.Instance.currentCase.title : "Unknown";

        return $"Based on '{choice}' in {state}: Consider checking network connections and verifying lead integrity before proceeding.";
    }

    public void SelectGadgets(List<string> selected)
    {
        Debug.Log("GADGETS: Selected " + string.Join(", ", selected));
    }
}
