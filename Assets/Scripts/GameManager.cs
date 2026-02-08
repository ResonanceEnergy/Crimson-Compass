using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Agents;
using CrimsonCompass.Core;
using CrimsonCompass.Runtime;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Data")]
    public TextAsset caseJson;
    public TextAsset agentsJson;
    public TextAsset insightsJsonl;

    [Header("UI")]
    public NotepadUI notepadUI;
    public HypothesisInput hypothesisInput;
    public EpisodeUI episodeUI;

    [Header("Managers")]
    public CrimsonCompass.Runtime.SeasonManager seasonManager;
    public AgentManager agentManager;
    public SaveManager saveManager;
    public GasketManager gasketManager;

    public CaseData currentCase;
    public GameState currentState;
    public HashSet<string> completedEpisodes = new HashSet<string>();
    public List<string> shadowTokens = new List<string>();
    private DisproofEngine disproofEngine;
    public EventBus eventBus;

        void Awake()
        {
            Instance = this;
            eventBus = new EventBus();
            disproofEngine = new DisproofEngine();
            currentState = new GameState();

            // Initialize managers if not set
            if (seasonManager == null) seasonManager = GetComponent<CrimsonCompass.Runtime.SeasonManager>();
            if (agentManager == null) agentManager = GetComponent<AgentManager>();
            if (saveManager == null) saveManager = GetComponent<SaveManager>();
            if (gasketManager == null) gasketManager = GetComponent<GasketManager>();

            // Debug logs for data loading
            if (caseJson != null) Debug.Log("Case JSON loaded, length: " + caseJson.text.Length);
            else Debug.LogError("Case JSON is null!");
            if (agentsJson != null) Debug.Log("Agents JSON loaded, length: " + agentsJson.text.Length);
            else Debug.LogError("Agents JSON is null!");
            if (insightsJsonl != null) Debug.Log("Insights JSONL loaded, length: " + insightsJsonl.text.Length);
            else Debug.LogError("Insights JSONL is null!");

            LoadCase();
        }

        void Start()
        {
            eventBus.Subscribe(GameEventType.HYPOTHESIS_SUBMITTED, OnHypothesisSubmitted);
        }

        public void LoadCase()
        {
            currentCase = JsonUtility.FromJson<CaseData>(caseJson.text);
            Debug.Log("Case loaded: " + currentCase.title);
            // Set truth for disproof engine
            var truth = new Hypothesis
            {
                whoId = currentCase.truth.whoId,
                howId = currentCase.truth.howId,
                whereId = currentCase.truth.whereId
            };
            disproofEngine.SetMissionTruth(truth);
            Debug.Log("Truth set: WHO=" + truth.whoId + ", HOW=" + truth.howId + ", WHERE=" + truth.whereId);

            // Set intel sources (simplified: each suspect/method/location has an intel source)
            var sources = new List<IntelSource>();
            foreach (var s in currentCase.suspects)
            {
                sources.Add(new IntelSource { id = s.id, axis = TriadAxis.WHO, value = s.id, description = $"Intel on {s.name}" });
            }
            foreach (var m in currentCase.methods)
            {
                sources.Add(new IntelSource { id = m.id, axis = TriadAxis.HOW, value = m.id, description = $"Intel on {m.name}" });
            }
            foreach (var l in currentCase.locations)
            {
                sources.Add(new IntelSource { id = l.id, axis = TriadAxis.WHERE, value = l.id, description = $"Intel on {l.id}" });
            }
            disproofEngine.SetIntelSources(sources);

            eventBus.Publish(GameEventType.SESSION_OPEN);
        }

        void OnHypothesisSubmitted(object payload)
        {
            var hypothesis = (Hypothesis)payload;
            Debug.Log("Hypothesis submitted: WHO=" + hypothesis.whoId + ", HOW=" + hypothesis.howId + ", WHERE=" + hypothesis.whereId);
            // For now, use first intel source
            var disproof = disproofEngine.Disprove(hypothesis, currentCase.suspects[0].id);
            if (disproof != null)
            {
                Debug.Log("Disproof returned: " + disproof.axis + " disproved ID=" + disproof.disprovedId);
                notepadUI.MarkDisproved(disproof.axis.ToString(), disproof.disprovedId);
                eventBus.Publish(GameEventType.DISPROOF_RETURNED, disproof);
            }
            else
            {
                Debug.Log("No disproof - hypothesis correct!");
                // Case resolved: 95% closure
                if (!string.IsNullOrEmpty(currentCase.caseToken))
                {
                    Debug.Log("Case Token: " + currentCase.caseToken);
                    // TODO: Display case resolution UI
                }
                // Add shadow token for 5% unease
                if (!string.IsNullOrEmpty(currentCase.shadowToken))
                {
                    shadowTokens.Add(currentCase.shadowToken);
                    Debug.Log("Shadow Token added: " + currentCase.shadowToken);
                    // TODO: Display unease/anomaly
                }
                // Handle Gasket appearance
                if (currentCase.gasket && gasketManager != null)
                {
                    gasketManager.TriggerFragment(currentCase.caseId);
                }
                eventBus.Publish(GameEventType.CASE_RESOLVED, currentCase.caseId);
            }
        }

        public async void LoadEpisode(string episodeId, int startSceneIndex = 0)
        {
            if (seasonManager != null)
            {
                await seasonManager.StartEpisodeAsync(episodeId);
            }
            else
            {
                Debug.LogError("SeasonManager not found!");
            }
        }

        public void StartNewGame()
        {
            currentState = new GameState();
            completedEpisodes.Clear();
            shadowTokens.Clear();
            currentCase = null;

            if (saveManager != null)
            {
                saveManager.NewGame();
            }

            if (seasonManager != null)
            {
                seasonManager.ResetSeason();
            }

            // Load the first episode
            LoadEpisode("CASE-0001");

            eventBus.Publish(GameEventType.NEW_GAME_STARTED, null);
        }

        public void LoadSavedGame()
        {
            if (saveManager != null && saveManager.LoadGame())
            {
                Debug.Log("Game loaded successfully");
            }
            else
            {
                Debug.Log("No save data found, starting new game");
                StartNewGame();
            }
        }
    }

    [System.Serializable]
    public class CaseData
    {
        public string caseId;
        public string title;
        public string tier;
        public string hook;
        public int timeBudget;
        public int hintsPerMission;
        public HintCost hintCost;
        public string[] gadgetsOffered;
        public int gadgetsSelectable;
        public Suspect[] suspects;
        public Method[] methods;
        public Location[] locations;
        public Truth truth;
        public string caseToken;
        public string shadowToken;
        public bool gasket;
        public bool catastrophicChoice;
    }

    public class HintCost { public int timeHours; public int heat; }
    [System.Serializable]
    public class Suspect { public string id; public string name; public string[] traits; }
    [System.Serializable]
    public class Method { public string id; public string name; public string[] signatures; }
    [System.Serializable]
    public class Location { public string id; public string country; }
    [System.Serializable]
    public class Truth { public string whoId; public string howId; public string whereId; }