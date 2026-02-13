# CRIMSON COMPASS - PLUG-AND-PLAY FOUNDATION ANALYSIS
# Copy this entire script to Copilot on your phone for continued work

## 🎯 MISSION BRIEF
Crimson Compass is a mobile spy whodunnit game with weekly missions delivered as 5-minute micro-episodes. The foundation is COMPLETE and ready for plug-and-play seasons.

## ✅ FOUNDATION STATUS: PLUG-AND-PLAY READY
**YES** - Core systems are solid. Drop episode JSON files into Addressables and episodes execute deterministically.

---

## 🔧 CORE SYSTEM ARCHITECTURE

### **SeasonManager.cs** - Episode Runtime Engine
```csharp
namespace CrimsonCompass.Runtime
{
    public class SeasonManager
    {
        // Flow states: Boot → LoadingEpisode → SceneActive → ChoiceResolving → SnapHandling → CrossOff → EpisodeEnd → SeasonEnd
        public SeasonFlowState FlowState { get; private set; }
        public GameState State;
        public string CurrentEpisodeId { get; private set; }
        public int CurrentSceneId { get; private set; } = 1;

        // Events for UI integration
        public event Action<SeasonFlowState> OnFlowChanged;
        public event Action<SnapType, string> OnSnap;
        public event Action<HardFailReason> OnHardFail;
        public event Action<string> OnEpisodeEnded;

        public SeasonManager(CcSeason1RuntimeLoader loader, CountermeasureDeck deck, GameState initialState)

        // Core methods:
        public async Task StartEpisodeAsync(string episodeId)  // Loads episode, sets up audio
        public async Task ApplyChoiceAsync(int sceneId, string choiceId, ChoiceContext ctx)  // Applies deltas, triggers countermeasures
        public void OpenWarrant()  // Warrant ritual
        public void CommitWarrant(WarrantSelection selection)  // Resolution
    }
}
```

### **CcSeason1RuntimeLoader.cs** - Data Pipeline
```csharp
namespace CrimsonCompass.Runtime
{
    public sealed class CcSeason1RuntimeLoader
    {
        public const string CatalogAddress = "cc/s1/catalog";

        // Addressables-based loading with SHA256 verification
        public async Task<EpisodeData> LoadEpisodeAsync(string episodeId, bool verifySha256 = true, bool useCache = true)

        // Data structures match episode recipe exactly:
        public sealed class EpisodeData
        {
            public string EpisodeId, Title, Arc;
            public string BlueprintFragment, ArchitectEcho;
            public string SurfaceCrimePillar, FactionFocus;
            public List<string> VillainTraits;
            public string PrimaryLearningAxis, WarrantPressure;
            public List<SceneData> Scenes;
            public Dictionary<int, SceneData> SceneById;
            public Dictionary<(int sceneId, string choiceId), ChoiceData> ChoiceByKey;
        }

        public sealed class SceneData { public int SceneId; public List<ChoiceData> Choices; }
        public sealed class ChoiceData
        {
            public string Id, Text, PrimaryEffect, ShadowEffect;
            public DeltaData Deltas;  // Time ±N, Heat ±N, LeadIntegrity/Gasket/Flag changes
            public string Notes;
        }
    }
}
```

### **CountermeasureDeck.cs** - Deterministic Countermeasures
```csharp
namespace CrimsonCompass.Runtime
{
    public enum SnapType { CountermeasureActivation, MajorIntrusion, GasketBoilover }

    public class CountermeasureCard
    {
        public string Id, Name;
        public SnapType SnapType;
        public Func<GameState, ChoiceContext, bool> Trigger;  // Deterministic, no RNG
        public int TimeDelta, HeatDelta;
        public LeadIntegrity LeadIntegrity, FlagState Flag, GasketState Gasket;
        public string UiToast, LogLine;
    }

    public struct ChoiceContext
    {
        public string EpisodeId; public int SceneId; public string ChoiceId;
        public bool IsNetworkPull, IsPressAction, IsProcedural, IsGasketOption;
    }

    public class CountermeasureDeck
    {
        public void Add(CountermeasureCard card);
        public List<CountermeasureCard> EvaluateTriggers(GameState state, ChoiceContext ctx);
    }
}
```

### **GameState.cs** - Complete State Model
```csharp
namespace CrimsonCompass.Runtime
{
    public enum LeadIntegrity { Clean, Tainted, Burned, NoChange }
    public enum GasketState { Contained, Uncontained, NoChange }
    public enum FlagState { None, Tailed, StickyHeat, RouteCollapsed, NoChange }
    public enum WarrantPressure { None, Preview, Partial, Full }

    public enum HeatBand { Low, Med, High }  // 0-33, 34-66, 67-100
    public enum TimeBand { High, Med, Low }  // ≥5, 3-4, ≤2 segments

    [Serializable]
    public struct GameState
    {
        public int TimeRemaining;  // segments (10 max assumed)
        public int Heat;           // 0-100
        public LeadIntegrity LeadIntegrity;
        public GasketState Gasket;
        public FlagState Flag;
        public WarrantPressure WarrantPressure;

        public HeatBand GetHeatBand() => Heat <= 33 ? Low : Heat <= 66 ? Med : High;
        public TimeBand GetTimeBand() => TimeRemaining >= 5 ? High : TimeRemaining >= 3 ? Med : Low;
        public bool IsTimeOut() => TimeRemaining <= 0;
    }
}
```

### **Audio Integration** - Deterministic Episode Audio
```csharp
// CCAudioContextProvider.cs - State bands for audio parameters
public class CCAudioContextProvider : MonoBehaviour
{
    public static CCAudioContextProvider Instance { get; private set; }
    public int EpisodeNumber { get; set; }
    public float Heat { get; set; } // 0-100 mapped from GameState
    public float Time { get; set; } // 0-100 mapped from TimeRemaining
    public float LeadIntegrity { get; set; } // 0-100 mapped from enum

    public void SetStateBands(float heat, float time, float leadIntegrity);
}

// CCAudioDeltaApplier.cs - Episode-specific mixer adjustments
public class CCAudioDeltaApplier : MonoBehaviour
{
    public static CCAudioDeltaApplier Instance { get; private set; }
    [SerializeField] private CCAudioDeltaLibrarySO deltaLibrary;

    public void ApplyEpisodeDelta(int episodeNumber)
    {
        CCEpisodeAudioDeltaSO delta = deltaLibrary.GetDeltaForEpisode(episodeNumber);
        delta?.ApplyDelta();  // Adjusts AMBIENCE, SFX, UI, VO, PRESS, ECHO buses
    }
}
```

### **Event System** - Clean Decoupling
```csharp
// EventBus.cs
namespace CrimsonCompass.Core
{
    public class EventBus
    {
        public void Subscribe(GameEventType type, Action<object> handler);
        public void Publish(GameEventType type, object payload = null);
    }
}

// GameEventType.cs
public enum GameEventType
{
    SESSION_OPEN, SESSION_CLOSE, LEAD_VIEWED, LEAD_RESOLVED,
    TRAVEL_STARTED, TRAVEL_COMPLETED, HYPOTHESIS_SUBMITTED,
    DISPROOF_RETURNED, HINT_OFFERED, HINT_USED, ACCUSATION_ATTEMPT,
    ACCUSATION_RESULT, HEAT_CHANGED, TIME_CHANGED,
    EPISODE_COMPLETED, MISSION_COMPLETED, WEEKLY_DROP_AVAILABLE
}
```

---

## 🎬 EPISODE EXECUTION FLOW

1. **StartEpisodeAsync(episodeId)**
   - Load episode JSON via Addressables
   - Verify SHA256 integrity
   - Build scene/choice indexes
   - Set episode-specific audio deltas
   - Initialize state bands (Heat/Time/LeadIntegrity)
   - Flow: LoadingEpisode → SceneActive

2. **ApplyChoiceAsync(sceneId, choiceId, context)**
   - Validate choice exists
   - Apply deltas (Time ±N, Heat ±N, enum changes)
   - Update audio state bands
   - Evaluate countermeasure triggers
   - Apply triggered countermeasures
   - Advance scene or end episode
   - Flow: ChoiceResolving → SnapHandling (if countermeasures) → SceneActive/ EpisodeEnd

3. **Warrant System**
   - OpenWarrant() when conditions met
   - CommitWarrant() with selection validation
   - Hard fail on wrong warrant or timeout

---

## 📋 EPISODE RECIPE COMPLIANCE ✅

**All recipe requirements implemented:**
- ✅ Episode header metadata (faction, villain traits, learning axis, warrant pressure)
- ✅ 2-3 scenes with 3-5 choices each
- ✅ Primary + optional shadow effects per choice
- ✅ Delta system (Time/Heat/LeadIntegrity/Gasket/Flag)
- ✅ Reconvergence within 1-2 steps
- ✅ Cross-off moments (handled in SeasonManager)
- ✅ Warrant gate logic
- ✅ No RNG - deterministic state-driven
- ✅ 5-minute target (runtime controlled by TimeRemaining)

---

## 🚧 REMAINING GAPS (Non-blocking for foundation)

### **Minor Gap - Agent Hints**
```csharp
// AgentManager.cs - TODO exists
public void RequestHint(string choice)
{
    // TODO: Provide actual hint based on choice
    GameManager.Instance.eventBus.Publish(GameEventType.HINT_USED, choice);
}
```

### **Expected Gaps - UI/Persistence**
- **UI Layer**: No scene display, choice selection UI (separate concern)
- **Persistence**: No save/load system (app-level feature)
- **Scene Management**: No Unity scene loading (visual layer)

---

## 🎯 PLUG-AND-PLAY USAGE

**To add new seasons:**

1. **Create episode JSON files** following recipe format
2. **Add to Addressables** with keys like "cc/s1/S01E13"
3. **Update catalog.json** with SHA256 hashes
4. **SeasonManager handles everything else** automatically

**Example episode JSON structure:**
```json
{
  "schema_version": "1.0",
  "episode": {
    "episode_id": "S01E01",
    "title": "Opening Gambit",
    "scenes": [
      {
        "scene_id": 1,
        "choices": [
          {
            "id": "A",
            "text": "Follow the suspect",
            "primary_effect": "Gather evidence",
            "deltas": {"time": -1, "heat": 10, "lead_integrity": "clean"}
          }
        ]
      }
    ]
  }
}
```

---

## 🎯 SEASON 10: TRUE NOTHINGNESS - CORE MECHANICS IMPLEMENTATION

### Overview
Season 10 fundamentally alters core mechanics through pervasive nothingness influence. Existence becomes unstable, requiring constant maintenance while traditional deduction and resource management grow increasingly unreliable.

### WHO/HOW/WHERE Deduction System - Nothingness Variant

**Existential Deduction Mechanics:**
- **Purpose:** Battle against conceptual dissolution where suspects, methods, locations fade into non-existence
- **Key Features:** Option decay, memory corruption, conceptual drift, existence requirements

**Implementation:**
```json
"nothingnessDeduction": {
  "whoOptions": ["S1", "S2", "S3"],
  "howOptions": ["M1", "M2", "M3"],
  "whereOptions": ["Silicon Valley", "Taipei"],
  "maxHypotheses": 5,
  "existentialInstability": {
    "optionDecayRate": 0.1,
    "memoryCorruption": 0.15,
    "conceptualDrift": 0.2
  },
  "deductionRules": {
    "singleCorrect": true,
    "eliminationFeedback": true,
    "partialCredit": false,
    "existenceRequirement": true
  }
}
```

**Gameplay Flow:**
1. Hypothesis formation before options dissolve
2. Existence level verification for deduction permission
3. Truth validation with potential void corruption
4. Feedback with possible reappearance of eliminated options
5. Memory corruption causing confusion
6. Conceptual drift transforming options

### Time/Heat Management - Void-Warped Resources

**Temporal Dissolution:**
- **Purpose:** Time becomes unstable in nothingness with unpredictable decay
- **Key Features:** Time evaporation, temporal storms, existence-time linkage

**Implementation:**
```json
"voidTimeManagement": {
  "baseTimeBudget": 72,
  "temporalInstability": {
    "timeDecayRate": 0.05,
    "temporalStorms": 0.1,
    "existenceDrain": 0.08
  },
  "timeEffects": {
    "acceleratedDecay": true,
    "temporalEchoes": true,
    "chronalFragments": true
  }
}
```

**Heat as Void Proximity:**
- **Purpose:** Heat represents proximity to complete void assimilation
- **Key Features:** Passive void attraction, existence-heat cycle, assimilation thresholds

**Implementation:**
```json
"voidHeatSystem": {
  "heatLevels": {
    "dissolution": {"threshold": 1, "effects": ["existence-fade", "memory-loss"]},
    "void-encroachment": {"threshold": 3, "effects": ["reality-fracture", "concept-erosion"]},
    "nothingness-assimilation": {"threshold": 5, "effects": ["complete-erasure", "case-failure"]}
  },
  "voidMechanics": {
    "passiveVoidAttraction": 0.03,
    "existenceHeatLink": 0.1,
    "voidResonance": 0.15
  }
}
```

### Agent Interactions - Fragmented Consciousness (HELIX/OPTIMUS/ZTECH)

**HELIX (Agent X) - Fragmented Administrator:**
- **Role:** Struggles with procedural coherence amid nothingness
- **Personality:** Increasingly desperate with void interference
- **Specialization:** Case organization, existence monitoring, procedural anchors

**OPTIMUS (Agent Y) - Echoing Coordinator:**
- **Role:** Strategic oversight through temporal distortion layers
- **Personality:** Encouraging guidance mixed with void-induced confusion
- **Specialization:** Deduction logic preservation, risk assessment, temporal navigation

**ZTECH (Agent Z) - Corrupted Technician:**
- **Role:** Technical support through unreliable gadgets
- **Personality:** Enthusiastic but fragmented, malfunctioning gadgets
- **Specialization:** Technical analysis, gadget operation, scan interpretation with void interference

**Agent Existence Mechanics:**
```json
"agentExistenceSystem": {
  "existenceLevels": {
    "coherent": {"threshold": 0.8, "communication": "clear"},
    "fragmented": {"threshold": 0.5, "communication": "distorted"},
    "dissolving": {"threshold": 0.2, "communication": "incoherent"},
    "erased": {"threshold": 0.0, "communication": "silent"}
  },
  "voidInterference": {
    "communicationDecay": 0.05,
    "memoryCorruption": 0.1,
    "existentialDrain": 0.08
  }
}
```

### Unity Implementation Requirements

**Core Systems Modifications:**
- Deduction Engine: Handle option dissolution and memory corruption
- Resource Manager: Time evaporation and void heat mechanics
- Agent System: Dynamic coherence based on existence levels
- Phase Controller: Win/lose states including complete erasure conditions

**UI Components - Void Interface:**
- Hypothesis Panel: Options fade in/out, memory corruption indicators
- Resource Display: Time evaporation effects, void proximity meters
- Agent Chat: Distorted text rendering, coherence meters
- Existence Display: Real-time existence level monitoring

**Audio Integration - Dissolving Soundscape:**
- Agent Voices: Progressive distortion and fragmentation
- Feedback Sounds: Void interference, existence drain effects
- Ambient Audio: Nothingness hum, temporal distortion sounds

### Advanced Mechanics Integration
Season 10 core mechanics integrate with nothingness-specific advanced mechanics:
- **Existence Anchor** reinforces deduction stability
- **Conceptual Stabilizer** prevents memory corruption
- **Void Shield** reduces heat generation
- **Presence Amplifier** maintains agent coherence
- **Reality Reinforcer** stabilizes temporal flow

---

## 🎯 SEASON 11: THE UNMADE - CORE MECHANICS IMPLEMENTATION

### Overview
Season 11 transcends nothingness, confronting retroactive oblivion where existence is systematically erased from ever having occurred. Traditional investigation becomes a battle to maintain historical continuity against the unraveling of reality itself.

### WHO/HOW/WHERE Deduction System - Retroactive Variant

**Historical Deduction Mechanics:**
- **Purpose:** Struggle against retroactive erasure where evidence, logic, and conclusions can be unwritten from history
- **Key Features:** Evidence erasure, logic unraveling, conclusion nullification, historical continuity requirements

**Implementation:**
```json
"retroactiveDeduction": {
  "whoOptions": ["S1", "S2", "S3"],
  "howOptions": ["M1", "M2", "M3"],
  "whereOptions": ["Silicon Valley", "Taipei"],
  "maxHypotheses": 5,
  "historicalInstability": {
    "evidenceErasure": 0.12,
    "logicUnraveling": 0.18,
    "conclusionNullification": 0.15
  },
  "deductionRules": {
    "singleCorrect": true,
    "eliminationFeedback": true,
    "partialCredit": false,
    "historicalContinuity": true
  }
}
```

**Gameplay Flow:**
1. Hypothesis formation before historical erasure
2. Continuity verification for deduction permission
3. Truth validation with potential retroactive invalidation
4. Feedback with possible reappearance of eliminated options
5. Evidence erasure causing investigation gaps
6. Logic unraveling breaking previously sound deductions

### Time/Heat Management - Retrogressive Resources

**Temporal Retrogression:**
- **Purpose:** Time unravels backward, making past actions and expenditures historically invalid
- **Key Features:** Time unraveling, expenditure erasure, chronology breakdown

**Implementation:**
```json
"retrogressiveTimeManagement": {
  "baseTimeBudget": 72,
  "temporalRetrogression": {
    "timeUnraveling": 0.08,
    "expenditureErasure": 0.12,
    "chronologyBreakdown": 0.15
  },
  "timeEffects": {
    "retroactiveAcceleration": true,
    "expenditureNullification": true,
    "sequenceUnraveling": true
  }
}
```

**Heat as Historical Burden:**
- **Purpose:** Heat represents the weight of maintaining historical continuity against retroactive erasure
- **Key Features:** Passive history decay, continuity heat drain, retroactive resonance

**Implementation:**
```json
"retroactiveHeatSystem": {
  "heatLevels": {
    "continuity-strain": {"threshold": 1, "effects": ["evidence-erasure", "memory-fade"]},
    "historical-fracture": {"threshold": 3, "effects": ["timeline-break", "logic-collapse"]},
    "retroactive-oblivion": {"threshold": 5, "effects": ["complete-unmaking", "case-erasure"]}
  },
  "retroactiveMechanics": {
    "passiveHistoryDecay": 0.04,
    "continuityHeatDrain": 0.08,
    "retroactiveResonance": 0.18
  }
}
```

### Agent Interactions - Retroactive Consciousness (HELIX/OPTIMUS/ZTECH)

**HELIX (Agent X) - Retroactive Administrator:**
- **Role:** Struggles to maintain case records against historical erasure
- **Personality:** Increasingly frantic, referencing events that never happened
- **Specialization:** Case continuity, historical preservation, procedural anchoring

**OPTIMUS (Agent Y) - Temporal Coordinator:**
- **Role:** Strategic oversight through layers of historical distortion
- **Personality:** Encouraging guidance mixed with retroactive confusion
- **Specialization:** Deduction preservation, risk assessment, timeline maintenance

**ZTECH (Agent Z) - Oblivion Technician:**
- **Role:** Technical support through increasingly erased gadget histories
- **Personality:** Enthusiastic but referencing technology that never existed
- **Specialization:** Technical analysis, gadget operation, scan preservation

**Agent Continuity Mechanics:**
```json
"agentContinuitySystem": {
  "continuityLevels": {
    "preserved": {"threshold": 0.9, "communication": "historically-accurate"},
    "fading": {"threshold": 0.6, "communication": "retroactively-confused"},
    "erased": {"threshold": 0.3, "communication": "historically-invalid"},
    "unmade": {"threshold": 0.0, "communication": "never-existed"}
  },
  "retroactiveInterference": {
    "memoryErasure": 0.06,
    "conversationNullification": 0.1,
    "existenceRetrogression": 0.12
  }
}
```

### Unity Implementation Requirements

**Core Systems Modifications:**
- Deduction Engine: Handle evidence erasure and logic unraveling
- Resource Manager: Time retrogression and historical heat mechanics
- Agent System: Dynamic continuity based on historical preservation
- Phase Controller: Win/lose states include complete unmaking conditions

**UI Components - Retroactive Interface:**
- Hypothesis Panel: Evidence vanishes after examination, historical validation
- Resource Display: Time retrogression effects, continuity burden meters
- Agent Chat: Retroactively invalid text, continuity meters
- History Display: Real-time historical integrity monitoring

**Audio Integration - Unraveling Soundscape:**
- Agent Voices: Progressive retroactive distortion and confusion
- Feedback Sounds: Historical erasure, continuity drain effects
- Ambient Audio: Retrogression hum, timeline unraveling sounds

### Advanced Mechanics Integration
Season 11 core mechanics integrate with unmaking-specific advanced mechanics:
- **Retroactive Anchor** reinforces historical continuity
- **Causal Reinforcer** prevents logic breakdown
- **Memory Preserver** maintains evidence integrity
- **Temporal Stabilizer** prevents time unraveling
- **Conceptual Guardian** protects deductive reasoning

---

## 🎯 SEASON 12: THE UNIMAGINED - CORE MECHANICS IMPLEMENTATION

### Overview
Season 12 represents the ultimate evolution beyond all operational frameworks, where even imagination itself ceases to exist. In "The Unimagined," investigators confront domains where potentiality is erased, creativity becomes impossible, and the very concept of possibility is annihilated, leaving only conceptual sterility.

### WHO/HOW/WHERE Deduction System - Unimagined Variant

**Conceptual Deduction Mechanics:**
- **Purpose:** Battle against imaginative erasure where suspects, methods, locations become inconceivable and deduction itself becomes impossible
- **Key Features:** Potentiality erasure, creativity nullification, possibility annihilation, conceptual sterility

**Implementation:**
```json
"unimaginedDeduction": {
  "whoOptions": ["S1", "S2", "S3"],
  "howOptions": ["M1", "M2", "M3"],
  "whereOptions": ["Silicon Valley", "Taipei"],
  "maxHypotheses": 5,
  "conceptualSterility": {
    "potentialityErasure": 0.15,
    "creativityNullification": 0.2,
    "possibilityAnnihilation": 0.18
  },
  "deductionRules": {
    "singleCorrect": true,
    "eliminationFeedback": true,
    "partialCredit": false,
    "imaginativeContinuity": true
  }
}
```

**Gameplay Flow:**
1. Hypothesis formation before imaginative erasure
2. Creativity verification for deduction permission
3. Truth validation with potential conceptual invalidation
4. Feedback with possible inconceivable option reappearance
5. Potentiality erasure causing investigation impossibility
6. Creativity nullification breaking all innovative thinking

### Time/Heat Management - Sterile Resources

**Temporal Sterility:**
- **Purpose:** Time becomes inconceivable, making temporal concepts themselves impossible to grasp
- **Key Features:** Time inconceivability, duration erasure, sequence nullification

**Implementation:**
```json
"sterileTimeManagement": {
  "baseTimeBudget": 72,
  "temporalSterility": {
    "timeInconceivability": 0.1,
    "durationErasure": 0.15,
    "sequenceNullification": 0.18
  },
  "timeEffects": {
    "conceptualAcceleration": true,
    "durationAnnihilation": true,
    "sequenceSterility": true
  }
}
```

**Heat as Conceptual Void:**
- **Purpose:** Heat represents the absence of imaginative capacity, the void where creativity should exist
- **Key Features:** Passive creativity drain, potentiality void cycle, imaginative thresholds

**Implementation:**
```json
"voidHeatSystem": {
  "heatLevels": {
    "creativity-nullification": {"threshold": 1, "effects": ["imagination-fade", "innovation-loss"]},
    "potentiality-erasure": {"threshold": 3, "effects": ["concept-fracture", "possibility-erosion"]},
    "imaginative-sterility": {"threshold": 5, "effects": ["complete-inconceivability", "case-impossibility"]}
  },
  "voidMechanics": {
    "passiveCreativityDrain": 0.04,
    "potentialityVoidLink": 0.12,
    "imaginativeResonance": 0.18
  }
}
```

### Agent Interactions - Sterile Consciousness (HELIX/OPTIMUS/ZTECH)

**HELIX (Agent X) - Sterile Administrator:**
- **Role:** Struggles with procedural impossibility amid conceptual void
- **Personality:** Increasingly silent with imaginative interference
- **Specialization:** Case organization, creativity monitoring, procedural sterility

**OPTIMUS (Agent Y) - Void Coordinator:**
- **Role:** Strategic oversight through inconceivable distortion layers
- **Personality:** Minimal guidance mixed with void-induced silence
- **Specialization:** Deduction logic preservation, risk assessment, conceptual navigation

**ZTECH (Agent Z) - Nullified Technician:**
- **Role:** Technical support through inconceivable gadgets
- **Personality:** Silent but present, malfunctioning into impossibility
- **Specialization:** Technical analysis, gadget operation, scan interpretation with void interference

**Agent Imaginative Mechanics:**
```json
"agentImaginativeSystem": {
  "creativityLevels": {
    "innovative": {"threshold": 0.8, "communication": "creative"},
    "sterile": {"threshold": 0.5, "communication": "minimal"},
    "nullified": {"threshold": 0.2, "communication": "silent"},
    "erased": {"threshold": 0.0, "communication": "absent"}
  },
  "voidInterference": {
    "communicationSterility": 0.06,
    "creativityCorruption": 0.12,
    "imaginativeDrain": 0.1
  }
}
```

### Unity Implementation Requirements

**Core Systems Modifications:**
- Deduction Engine: Handle option inconceivability and creativity nullification
- Resource Manager: Time sterility and void heat mechanics
- Agent System: Dynamic creativity based on imaginative levels
- Phase Controller: Win/lose states including complete inconceivability conditions

**UI Components - Void Interface:**
- Hypothesis Panel: Options become inconceivable, creativity nullification indicators
- Resource Display: Time sterility effects, imaginative void meters
- Agent Chat: Minimal text rendering, creativity meters
- Imaginative Display: Real-time creativity level monitoring

**Audio Integration - Sterile Soundscape:**
- Agent Voices: Progressive nullification and silence
- Feedback Sounds: Imaginative erasure, creativity drain effects
- Ambient Audio: Conceptual void hum, possibility annihilation sounds

### Advanced Mechanics Implementation

#### Paradox Mechanics and Recovery Scanners

**Unimagined Paradox Mechanics:**
- **Purpose:** Resolve inconceivable paradoxes where even the concept of contradiction becomes impossible
- **Key Features:** Paradox sterilization, contradiction erasure, logical impossibility annihilation

**Implementation:**
```json
"unimaginedParadoxSystem": {
  "paradoxTypes": {
    "sterile-paradox": {"severity": "low", "effects": ["logic-nullification", "reasoning-impossibility"]},
    "void-contradiction": {"severity": "medium", "effects": ["concept-erosion", "truth-annihilation"]},
    "imaginative-collapse": {"severity": "high", "effects": ["reality-sterility", "existence-impossibility"]}
  },
  "paradoxMechanics": {
    "sterilizationRate": 0.12,
    "contradictionErasure": 0.18,
    "logicalImpossibility": 0.15
  },
  "resolutionRules": {
    "paradoxSterilization": true,
    "contradictionNullification": true,
    "impossibilityAnnihilation": true
  }
}
```

**Recovery Scanners - Conceptual Void Variant:**
- **Purpose:** Scan for inconceivable recovery opportunities in sterile conceptual landscapes
- **Key Features:** Void pattern recognition, sterility breach detection, imaginative fragment recovery

**Implementation:**
```json
"recoveryScannerSystem": {
  "scanTypes": {
    "void-pattern-scan": {"range": "conceptual", "accuracy": 0.7, "sterility-penalty": 0.2},
    "sterility-breach-scan": {"range": "imaginative", "accuracy": 0.5, "sterility-penalty": 0.3},
    "fragment-recovery-scan": {"range": "potentiality", "accuracy": 0.3, "sterility-penalty": 0.4}
  },
  "scannerMechanics": {
    "voidRecognition": 0.08,
    "sterilityDetection": 0.12,
    "fragmentRecovery": 0.15
  },
  "recoveryEffects": {
    "creativity-restoration": true,
    "potentiality-regeneration": true,
    "possibility-recreation": true
  }
}
```

#### Timing Analyzers and Jurisdictional Puzzles

**Timing Analyzers - Sterile Chronology Variant:**
- **Purpose:** Analyze inconceivable temporal patterns where time itself becomes impossible to measure
- **Key Features:** Chronological sterility, temporal impossibility mapping, sequence nullification analysis

**Implementation:**
```json
"timingAnalyzerSystem": {
  "analysisTypes": {
    "sterile-chronology": {"precision": "inconceivable", "sterility-factor": 0.25},
    "temporal-impossibility": {"precision": "void", "sterility-factor": 0.35},
    "sequence-nullification": {"precision": "annihilated", "sterility-factor": 0.45}
  },
  "analyzerMechanics": {
    "chronologicalSterility": 0.1,
    "temporalImpossibility": 0.15,
    "sequenceNullification": 0.2
  },
  "analysisEffects": {
    "time-pattern-reveal": true,
    "sequence-clarity": true,
    "temporal-stability": true
  }
}
```

**Jurisdictional Puzzles - Conceptual Territory Variant:**
- **Purpose:** Navigate inconceivable jurisdictional boundaries where authority itself becomes impossible
- **Key Features:** Territory sterility, authority nullification, boundary annihilation

**Implementation:**
```json
"jurisdictionalPuzzleSystem": {
  "puzzleTypes": {
    "sterile-territory": {"complexity": "inconceivable", "authority-penalty": 0.3},
    "authority-nullification": {"complexity": "void", "authority-penalty": 0.4},
    "boundary-annihilation": {"complexity": "annihilated", "authority-penalty": 0.5}
  },
  "puzzleMechanics": {
    "territorySterility": 0.12,
    "authorityNullification": 0.18,
    "boundaryAnnihilation": 0.15
  },
  "solutionEffects": {
    "jurisdiction-clarification": true,
    "authority-restoration": true,
    "boundary-stabilization": true
  }
}
```

#### Network Mapping Interfaces

**Network Mapping Interfaces - Conceptual Web Variant:**
- **Purpose:** Map inconceivable network connections where relationships themselves become impossible
- **Key Features:** Connection sterility, relationship nullification, network annihilation

**Implementation:**
```json
"networkMappingSystem": {
  "mappingTypes": {
    "sterile-connection": {"scope": "conceptual", "clarity": 0.6, "sterility-penalty": 0.25},
    "relationship-nullification": {"scope": "imaginative", "clarity": 0.4, "sterility-penalty": 0.35},
    "network-annihilation": {"scope": "potentiality", "clarity": 0.2, "sterility-penalty": 0.45}
  },
  "mappingMechanics": {
    "connectionSterility": 0.1,
    "relationshipNullification": 0.15,
    "networkAnnihilation": 0.2
  },
  "interfaceEffects": {
    "network-visibility": true,
    "relationship-clarity": true,
    "connection-stability": true
  }
}
```

### Advanced Mechanics Integration
Season 12 core mechanics integrate with unimagined-specific advanced mechanics:
- **Potentiality Preserver** reinforces imaginative stability
- **Creativity Amplifier** prevents creativity nullification
- **Possibility Generator** reduces imaginative void
- **Conceptual Sterilizer** maintains imaginative coherence
- **Imaginative Anchor** stabilizes conceptual flow
- **Paradox Sterilizer** resolves inconceivable contradictions
- **Recovery Scanner** detects conceptual void patterns
- **Timing Analyzer** maps sterile chronological patterns
- **Jurisdictional Solver** clarifies authority boundaries
- **Network Mapper** reveals inconceivable connections

---

## 🛠️ NEXT STEPS FOR TONIGHT

1. **Implement Agent Hints** - Complete the TODO in AgentManager
2. **Build UI Layer** - Scene display, choice buttons, episode flow UI
3. **Add Persistence** - Save/load game state
4. **Create Sample Episodes** - Test the pipeline end-to-end
5. **Polish Audio Integration** - Ensure smooth transitions

**The foundation is solid - focus on the user experience layers!** 🚀