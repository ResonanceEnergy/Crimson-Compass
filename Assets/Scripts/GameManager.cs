using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass;
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

    [Header("Systems")]
    public CrimsonCompass.Runtime.SeasonManager seasonManager;
    public CountermeasureSetup countermeasureSetup;
    public SaveManager saveManager;

    public CaseData currentCase;
    public GameState currentState;
    public List<string> shadowTokens = new List<string>();
    public List<string> completedEpisodes = new List<string>();
    public AgentManager agentManager;
    private DisproofEngine disproofEngine;
    public EventBus eventBus;

    private List<IntelSource> availableIntelSources = new List<IntelSource>();
    private int nextIntelIndex = 0;
    public int currentScore = 0;

    private Coroutine timeCoroutine;

    void Awake()
    {
        Instance = this;
        eventBus = new EventBus();
        disproofEngine = new DisproofEngine();

        // Initialize systems
        InitializeSystems();
    }

    private void InitializeSystems()
    {
        // Initialize season manager with dependencies
        if (seasonManager == null)
        {
            var loader = new CcSeason1RuntimeLoader();
            var deck = countermeasureSetup != null ? countermeasureSetup.GetDeck() : new CountermeasureDeck();

            var initialState = new GameState
            {
                TimeRemaining = 10,
                Heat = 0,
                LeadIntegrity = LeadIntegrity.Clean,
                Gasket = GasketState.Contained,
                Flag = FlagState.None,
                WarrantPressure = WarrantPressure.None
            };

            seasonManager = new CrimsonCompass.Runtime.SeasonManager(loader, deck, initialState);
        }

        // Connect UI to season manager
        if (episodeUI != null)
        {
            episodeUI.Initialize(seasonManager);
        }

        // Load saved game if exists
        if (saveManager != null)
        {
            var saveData = saveManager.LoadGame();
            if (saveData != null)
            {
                seasonManager.State = saveData.State;
                Debug.Log("Loaded saved game state");
            }
        }
    }

    void Start()
    {
        eventBus.Subscribe(GameEventType.HYPOTHESIS_SUBMITTED, OnHypothesisSubmitted);
        
        StartEpisode("S01E01");
    }

    private System.Collections.IEnumerator StartFirstEpisode()
    {
        yield return new WaitForEndOfFrame(); // Wait for everything to initialize
        StartEpisode("S01E01");
    }

    public void LoadCase(TextAsset caseAsset = null)
    {
        if (caseAsset != null)
        {
            currentCase = JsonUtility.FromJson<CaseData>(caseAsset.text);
        }
        else
        {
            // For now, create a test case since Resources loading seems to be failing
            currentCase = new CaseData
            {
                caseId = "TEST-CASE",
                title = "Test Case",
                tier = "test",
                hook = "Test case for debugging",
                timeBudget = 24,
                hintsPerMission = 2,
                suspects = new Suspect[] {
                    new Suspect { id = "test_suspect_1", name = "Test Suspect 1", traits = new string[] { "Test Trait" } },
                    new Suspect { id = "test_suspect_2", name = "Test Suspect 2", traits = new string[] { "Test Trait" } }
                },
                methods = new Method[] {
                    new Method { id = "test_method_1", name = "Test Method 1", signatures = new string[] { "Test Signature" } },
                    new Method { id = "test_method_2", name = "Test Method 2", signatures = new string[] { "Test Signature" } }
                },
                locations = new Location[] {
                    new Location { id = "test_location_1", country = "Test Country" },
                    new Location { id = "test_location_2", country = "Test Country" }
                },
                truth = new Truth {
                    whoId = "test_suspect_1",
                    howId = "test_method_1",
                    whereId = "test_location_1"
                }
            };
            Debug.Log("Created test case: " + currentCase.title);
        }

        // Validate that we have a case loaded
        if (currentCase == null)
        {
            Debug.LogError("Failed to load case data - currentCase is null");
            return;
        }

        // Validate truth data exists
        if (currentCase.truth == null)
        {
            Debug.LogError("Case data missing truth field - cannot initialize disproof engine");
            return;
        }

        var truth = new Hypothesis
        {
            whoId = currentCase.truth.whoId,
            howId = currentCase.truth.howId,
            whereId = currentCase.truth.whereId
        };
        disproofEngine.SetMissionTruth(truth);

        // Create meaningful intel sources that reveal information about the truth
        availableIntelSources.Clear();
        
        // Intel about WHO (suspects) - each reveals the correct suspect through different evidence
        availableIntelSources.Add(new IntelSource { 
            id = "witness_olive_coat", 
            axis = TriadAxis.WHO, 
            value = currentCase.truth.whoId, 
            description = "Witness saw suspect wearing olive coat" 
        });
        availableIntelSources.Add(new IntelSource { 
            id = "dna_fingerprint", 
            axis = TriadAxis.WHO, 
            value = currentCase.truth.whoId, 
            description = "DNA analysis matches suspect profile" 
        });
        
        // Intel about HOW (methods) - each reveals the correct method through different evidence  
        availableIntelSources.Add(new IntelSource { 
            id = "security_footage", 
            axis = TriadAxis.HOW, 
            value = currentCase.truth.howId, 
            description = "Security footage shows method used" 
        });
        availableIntelSources.Add(new IntelSource { 
            id = "tool_signature", 
            axis = TriadAxis.HOW, 
            value = currentCase.truth.howId, 
            description = "Tool signature analysis reveals method" 
        });
        
        // Intel about WHERE (locations) - each reveals the correct location through different evidence
        availableIntelSources.Add(new IntelSource { 
            id = "surveillance_log", 
            axis = TriadAxis.WHERE, 
            value = currentCase.truth.whereId, 
            description = "Surveillance log shows activity at location" 
        });
        availableIntelSources.Add(new IntelSource { 
            id = "witness_location", 
            axis = TriadAxis.WHERE, 
            value = currentCase.truth.whereId, 
            description = "Witness places suspect at location" 
        });

        // Start with just 1-2 intel sources revealed initially
        var initialSources = new List<IntelSource>();
        int initialCount = Mathf.Min(2, availableIntelSources.Count); // Start with fewer clues
        for (int i = 0; i < initialCount; i++)
        {
            initialSources.Add(availableIntelSources[i]);
        }
        nextIntelIndex = initialCount;

        disproofEngine.SetIntelSources(initialSources);

        eventBus.Publish(GameEventType.SESSION_OPEN);
    }

    void OnHypothesisSubmitted(object payload)
    {
        var hypothesis = (Hypothesis)payload;
        Debug.Log("Hypothesis submitted: WHO=" + hypothesis.whoId + ", HOW=" + hypothesis.howId + ", WHERE=" + hypothesis.whereId);

        // Check for victory - hypothesis matches the truth exactly
        if (hypothesis.whoId == disproofEngine.GetTruth().whoId &&
            hypothesis.howId == disproofEngine.GetTruth().howId &&
            hypothesis.whereId == disproofEngine.GetTruth().whereId)
        {
            currentScore += 100; // Bonus for solving the case
            Debug.Log("VICTORY! Case solved correctly!");
            eventBus.Publish(GameEventType.CASE_RESOLVED, hypothesis);
            return;
        }

        // Check for disproofs using all available intel sources
        bool anyDisproof = false;
        foreach (var source in disproofEngine.GetIntelSources())
        {
            var disproof = disproofEngine.Disprove(hypothesis, source.id);
            if (disproof != null)
            {
                currentScore += 10; // Points for each disproof
                Debug.Log("Disproof returned: " + disproof.axis + " disproved ID=" + disproof.disprovedId + " using source " + source.id);
                notepadUI.MarkDisproved(disproof.axis.ToString(), disproof.disprovedId);
                eventBus.Publish(GameEventType.DISPROOF_RETURNED, disproof);
                anyDisproof = true;
            }
        }

        if (!anyDisproof)
        {
            Debug.Log("No disproofs found - hypothesis may be partially correct or no relevant intel available");
            // Publish event for no disproof (could be used for feedback)
            var resultData = new System.Collections.Generic.Dictionary<string, object>
            {
                ["result"] = "no_disproof",
                ["hypothesis"] = hypothesis
            };
            eventBus.Publish(GameEventType.ACCUSATION_RESULT, resultData);

            // Reveal additional intel source if available
            RevealNextIntelSource();
        }
    }

    // Public method to start episodes (call from UI)
    public async void StartEpisode(string episodeId)
    {
        await seasonManager.StartEpisodeAsync(episodeId);
    }

    void OnApplicationQuit()
    {
        // Auto-save on quit
        if (saveManager != null && seasonManager != null)
        {
            saveManager.SaveGame(seasonManager.State, seasonManager.CurrentEpisodeId, seasonManager.CurrentSceneId, new string[0]);
        }
    }

    public void StartNewGame()
    {
        currentState = new GameState
        {
            timeBudget = currentCase.timeBudget,
            heat = 0,
            leadIntegrity = LeadIntegrity.Clean,
            gasket = GasketState.Contained,
            flag = FlagState.None,
            warrantPressure = WarrantPressure.None,
            tokens = new List<string>()
        };
        shadowTokens.Clear();
        completedEpisodes.Clear();

        // Reset score
        currentScore = 0;

        // Start time countdown
        if (timeCoroutine != null) StopCoroutine(timeCoroutine);
        timeCoroutine = StartCoroutine(TimeCountdown());

        Debug.Log("New game started");
    }

    public void LoadEpisode(string episodeId)
    {
        Debug.Log("Loading episode: " + episodeId);
        // Implementation would load episode data
    }

    private void RevealNextIntelSource()
    {
        if (nextIntelIndex < availableIntelSources.Count)
        {
            var newSource = availableIntelSources[nextIntelIndex];
            var currentSources = disproofEngine.GetIntelSources();
            currentSources.Add(newSource);
            disproofEngine.SetIntelSources(currentSources);
            nextIntelIndex++;

            Debug.Log($"New intel revealed: {newSource.description}");
            eventBus.Publish(GameEventType.HINT_OFFERED, newSource);
        }
    }

    private IEnumerator TimeCountdown()
    {
        while (currentState.TimeRemaining > 0)
        {
            yield return new WaitForSeconds(60f); // 1 minute per time segment
            currentState.TimeRemaining--;

            // Update UI
            if (episodeUI != null)
            {
                episodeUI.UpdateStateDisplay(currentState);
            }

            eventBus.Publish(GameEventType.TIME_CHANGED, currentState.TimeRemaining);

            // Check for timeout
            if (currentState.TimeRemaining <= 0)
            {
                Debug.Log("TIME'S UP! Case failed.");
                var timeoutData = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["success"] = false,
                    ["reason"] = "timeout"
                };
                eventBus.Publish(GameEventType.MISSION_COMPLETED, timeoutData);
                break;
            }
        }
    }
}
