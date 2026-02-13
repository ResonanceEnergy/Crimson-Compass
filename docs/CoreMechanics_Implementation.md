# Crimson Compass Core Mechanics Implementation

## Overview
The core gameplay mechanics provide the fundamental systems that drive all Crimson Compass episodes. These mechanics are consistent across all seasons while allowing for thematic variations and progressive complexity.

## Core Deduction System (WHO/HOW/WHERE)

### Deduction Mechanics
**Purpose:** The central puzzle-solving mechanic requiring players to identify the correct combination of suspect, method, and location.

**Implementation:**
```json
"deductionSystem": {
  "whoOptions": ["S1", "S2", "S3"],
  "howOptions": ["M1", "M2", "M3"],
  "whereOptions": ["Silicon Valley", "Taipei"],
  "maxHypotheses": 5,
  "deductionRules": {
    "singleCorrect": true,
    "eliminationFeedback": true,
    "partialCredit": false
  }
}
```

**Gameplay Flow:**
1. **Hypothesis Formation:** Player selects one suspect, one method, one location
2. **Validation:** System checks against the hidden truth
3. **Feedback:** Incorrect elements are eliminated from future hypotheses
4. **Iteration:** Process repeats until correct combination is found or resources exhausted

**Rules:**
- **Single Correct Answer:** Only one valid WHO/HOW/WHERE combination per case
- **Elimination Feedback:** Wrong elements are removed from available options
- **Maximum Attempts:** Limited hypotheses prevent infinite guessing
- **No Partial Credit:** All three elements must be correct simultaneously

## Resource Management (Time & Heat)

### Time Management
**Purpose:** Creates urgency and strategic decision-making through limited investigation time.

**Implementation:**
- **Time Budget:** 72 hours (3 days) per case
- **Time Penalties:** Actions consume time (wrong hypotheses: -4 hours, failed scans: -2 hours)
- **Time Warnings:** Agent alerts when time is running low

**Strategic Elements:**
- **Time vs Accuracy:** Rushing leads to mistakes, thorough investigation takes time
- **Bonus Scoring:** Time remaining provides bonus points
- **Failure Condition:** Mission fails if time expires before correct hypothesis

### Heat Management
**Purpose:** Represents operational risk and adversary awareness.

**Implementation:**
```json
"heatLevels": {
  "low": {"threshold": 1, "effects": ["minor-surveillance"]},
  "medium": {"threshold": 3, "effects": ["asset-freeze", "agent-caution"]},
  "high": {"threshold": 5, "effects": ["mission-abort", "trace-alert"]}
}
```

**Heat Mechanics:**
- **Heat Generation:** Wrong actions, risky maneuvers, prolonged investigations
- **Heat Decay:** Passive reduction over time, active reduction through careful play
- **Heat Effects:** Progressive consequences from surveillance to mission abort
- **Strategic Balance:** Low heat preserves resources, high heat risks failure

## Agent Interaction System (HELIX/OPTIMUS/ZTECH)

### Agent Roles & Personalities

#### HELIX (Agent X) - Mission Secretary
**Role:** Administrative support and case management
**Personality:** Efficient but exasperated by incompetence
**Specialization:** Case organization, time management, procedural guidance

**Interaction Triggers:**
- Case start/end briefings
- Time warnings and resource alerts
- Success/failure commentary
- Procedural guidance

#### OPTIMUS (Agent Y) - Mission Coordinator
**Role:** Strategic oversight and hypothesis validation
**Personality:** Encouraging father-figure with tactical expertise
**Specialization:** Deduction logic, risk assessment, team coordination

**Interaction Triggers:**
- Hypothesis evaluation
- Strategic planning
- Resource management
- Paradox resolution

#### ZTECH (Agent Z) - Gadgets Specialist
**Role:** Technical support and gadget deployment
**Personality:** Enthusiastic tech expert, gadget-obsessed
**Specialization:** Technical analysis, gadget operation, scan interpretation

**Interaction Triggers:**
- Gadget usage opportunities
- Technical clue analysis
- Equipment malfunctions
- Scan result interpretation

### Interaction System
**Dynamic Dialogue:** Context-aware responses based on player actions and case state
**Help Triggers:** Automatic agent interventions at critical moments
**Personality Consistency:** Each agent maintains distinct voice and expertise
**Progressive Relationship:** Agent responses evolve based on player performance

## Gameplay Flow & Phases

### Phase Structure
```json
"gameplayFlow": {
  "phases": [
    {
      "name": "Briefing",
      "duration": "auto",
      "objectives": ["Read case details", "Select gadgets"],
      "agentFocus": "HELIX"
    },
    {
      "name": "Investigation",
      "duration": "timed",
      "objectives": ["Gather clues", "Form hypotheses", "Use gadgets"],
      "agentFocus": "OPTIMUS"
    },
    {
      "name": "Resolution",
      "duration": "auto",
      "objectives": ["Final hypothesis", "Case closure"],
      "agentFocus": "HELIX"
    }
  ]
}
```

### Win/Lose Conditions
**Victory Requirements:**
- Correct final hypothesis (WHO/HOW/WHERE)
- Time remaining (bonus multiplier)
- Heat level below critical threshold

**Defeat Conditions:**
- Time budget exhausted
- Heat level reaches maximum
- Incorrect final hypothesis after maximum attempts

## Implementation Status

### Season 1 (Cases 0001-0012) - ✅ Complete
**Core Mechanics Added:**
- WHO/HOW/WHERE deduction system with elimination feedback
- Time budget (72 hours) with penalties for wrong actions
- Heat management with progressive risk levels
- Agent interactions (HELIX/OPTIMUS/ZTECH) with thematic dialogue
- Gameplay phases: Briefing → Investigation → Resolution
- Scoring system with bonuses and penalties

**Season 1 Themes:** Corporate espionage, data theft, insider threats
- HELIX focuses on case organization and procedural guidance
- OPTIMUS provides strategic oversight for investigation tactics
- ZTECH specializes in digital forensics and network analysis

### Season 2 (Cases 0013-0024) - ✅ Complete
**Core Mechanics Added:**
- WHO/HOW/WHERE deduction system with elimination feedback
- Enhanced time budget (72 hours) with logistics-specific penalties
- Advanced heat management with supply chain disruption effects
- Agent interactions adapted for supply chain themes
- Gameplay phases: Logistics Briefing → Chain Investigation → Disruption Resolution
- Scoring system with supply chain preservation bonuses

**Season 2 Themes:** Supply chain disruptions, logistics manipulation, infrastructure sabotage
- HELIX focuses on supply chain integrity and delivery windows
- OPTIMUS coordinates logistics operations and timing
- ZTECH handles chain scanners and logistics analysis

### Season 5 (Cases 0049-0060) - ✅ Complete
**Core Mechanics Added:**
- WHO/HOW/WHERE deduction system with consequence penalty mechanics
- Time budget (72 hours) with escalation-based penalties
- Advanced heat management with consequence chain effects
- Agent interactions adapted for consequence management themes
- Gameplay phases: Consequence Assessment → Escalation Investigation → Threat Containment
- Scoring system with escalation mastery and threat containment bonuses

**Season 5 Themes:** Wins remove obstacles to something else (escalating consequences)
- HELIX serves as Consequence Coordinator managing victory outcomes
- OPTIMUS coordinates Escalation Analysis and threat prediction
- ZTECH handles Threat Revelation and consequence chain mapping

## Scoring & Achievement System

### Point Calculation
```json
"scoringSystem": {
  "basePoints": 100,
  "bonuses": {
    "timeRemaining": 10,  // per hour
    "lowHeat": 25,
    "efficientGadgets": 15,
    "firstTryHypothesis": 20
  },
  "penalties": {
    "wrongHypothesis": -10,
    "highHeat": -15,
    "timeExpired": -30,
    "gadgetOveruse": -5
  }
}
```

### Achievement Types
- **Perfect Case:** No wrong hypotheses, minimal heat
- **Speed Demon:** Completed in under 24 hours
- **Ghost Hunter:** Maximum efficiency, minimum traces
- **Master Investigator:** Consistent high performance across cases

## Unity Implementation Requirements

### Core Systems
- **Deduction Engine:** Hypothesis validation and elimination logic
- **Resource Manager:** Time and heat tracking with decay mechanics
- **Agent System:** Dynamic dialogue and intervention triggers
- **Phase Controller:** Gameplay flow and win/lose state management

### UI Components
- **Hypothesis Panel:** WHO/HOW/WHERE selection interface
- **Resource Display:** Time and heat meters with warnings
- **Agent Chat:** Dynamic dialogue bubbles with agent portraits
- **Score Display:** Real-time scoring and achievement tracking

### Audio Integration
- **Agent Voices:** Distinct voice acting for each agent personality
- **Feedback Sounds:** Confirmation, error, and warning audio cues
- **Ambient Audio:** Tension building with time/heat pressure

## Testing & Balancing

### Core Loop Testing
- Deduction logic accuracy and feedback clarity
- Resource management balance (time vs heat trade-offs)
- Agent interaction timing and relevance

### Player Experience
- Learning curve progression across seasons
- Difficulty scaling with advanced mechanics
- Replayability through different approaches

### Performance Metrics
- Average case completion time
- Hypothesis attempt distribution
- Agent interaction frequency and helpfulness

## Cross-Season Consistency
These core mechanics remain consistent across all seasons while advanced mechanics (scanners, analyzers, networks) provide seasonal variety and progression. The foundation ensures familiar gameplay with evolving complexity.

---

# Season 10: True Nothingness - Core Mechanics Implementation

## Overview
In Season 10, the core mechanics are fundamentally altered by the pervasive influence of true nothingness. Existence itself becomes unstable, requiring constant maintenance while traditional deduction and resource management become increasingly unreliable.

## WHO/HOW/WHERE Deduction System - Nothingness Variant

### Existential Deduction Mechanics
**Purpose:** Deduction becomes a battle against conceptual dissolution where suspects, methods, and locations can fade into non-existence.

**Implementation:**
```json
"nothingnessDeduction": {
  "whoOptions": ["S1", "S2", "S3"],
  "howOptions": ["M1", "M2", "M3"],
  "whereOptions": ["Silicon Valley", "Taipei"],
  "maxHypotheses": 5,
  "existentialInstability": {
    "optionDecayRate": 0.1,  // Options fade over time
    "memoryCorruption": 0.15, // Chance of forgetting eliminated options
    "conceptualDrift": 0.2    // Options may shift or transform
  },
  "deductionRules": {
    "singleCorrect": true,
    "eliminationFeedback": true,
    "partialCredit": false,
    "existenceRequirement": true  // Must maintain existence to deduce
  }
}
```

**Gameplay Flow:**
1. **Hypothesis Formation:** Player selects suspect/method/location before they dissolve
2. **Existence Check:** System verifies player's existence level allows deduction
3. **Validation:** Truth checking occurs but results may be corrupted by void
4. **Feedback:** Incorrect elements eliminated, but may reappear due to nothingness
5. **Memory Corruption:** Previously eliminated options can return, causing confusion
6. **Conceptual Drift:** Options may transform (suspect becomes "echo of suspect")

**Nothingness Effects:**
- **Option Dissolution:** Available choices fade if not used quickly
- **Memory Erosion:** Eliminated options can reappear, forcing re-evaluation
- **Truth Instability:** The "correct" answer may shift due to reality unraveling
- **Cognitive Decay:** Player may forget progress or previous hypotheses

## Time/Heat Management - Void-Warped Resources

### Temporal Dissolution
**Purpose:** Time itself becomes unstable in nothingness, creating unpredictable resource decay.

**Implementation:**
```json
"voidTimeManagement": {
  "baseTimeBudget": 72,
  "temporalInstability": {
    "timeDecayRate": 0.05,     // Time evaporates passively
    "temporalStorms": 0.1,     // Random time loss events
    "existenceDrain": 0.08     // Time consumption accelerates dissolution
  },
  "timeEffects": {
    "acceleratedDecay": true,   // Time passes faster near void
    "temporalEchoes": true,     // Past actions replay with variations
    "chronalFragments": true    // Time becomes non-linear
  }
}
```

**Strategic Elements:**
- **Time Evaporation:** Hours disappear even without action
- **Temporal Storms:** Random events that consume large amounts of time
- **Existence-Time Link:** Using time accelerates personal dissolution
- **Chronal Fragments:** Actions may occur out of sequence

### Heat as Void Proximity
**Purpose:** Heat represents how close the player is to complete void assimilation.

**Implementation:**
```json
"voidHeatSystem": {
  "heatLevels": {
    "dissolution": {"threshold": 1, "effects": ["existence-fade", "memory-loss"]},
    "void-encroachment": {"threshold": 3, "effects": ["reality-fracture", "concept-erosion"]},
    "nothingness-assimilation": {"threshold": 5, "effects": ["complete-erasure", "case-failure"]}
  },
  "voidMechanics": {
    "passiveVoidAttraction": 0.03,  // Heat increases over time
    "existenceHeatLink": 0.1,       // Low existence generates heat
    "voidResonance": 0.15           // Heat attracts more void influence
  }
}
```

**Heat Mechanics:**
- **Passive Void Attraction:** Heat builds even during inactivity
- **Existence-Heat Cycle:** Low existence generates heat, high heat accelerates dissolution
- **Void Resonance:** High heat attracts nothingness effects
- **Assimilation Threshold:** Beyond critical heat, complete erasure occurs

## Agent Interactions - Fragmented Consciousness (HELIX/OPTIMUS/ZTECH)

### Void-Warped Agent Personalities

#### HELIX (Agent X) - Fragmented Administrator
**Role:** Struggling to maintain procedural coherence amid nothingness
**Personality:** Increasingly desperate, voice distorting with void interference
**Specialization:** Case organization, existence monitoring, procedural anchors

**Interaction Triggers:**
- Existence level warnings ("Your form is... fading...")
- Time dissolution alerts ("Hours are... evaporating...")
- Memory corruption events ("I... remember this differently...")
- Success/failure with existential commentary

#### OPTIMUS (Agent Y) - Echoing Coordinator
**Role:** Strategic oversight through layers of temporal distortion
**Personality:** Fatherly encouragement mixed with void-induced confusion
**Specialization:** Deduction logic preservation, risk assessment, temporal navigation

**Interaction Triggers:**
- Hypothesis evaluation with reality checks ("Is this... still true?")
- Strategic planning through temporal echoes
- Resource management with void awareness
- Paradox resolution with existential stakes

#### ZTECH (Agent Z) - Corrupted Technician
**Role:** Technical support through increasingly unreliable gadgets
**Personality:** Enthusiastic but increasingly fragmented, gadgets malfunctioning
**Specialization:** Technical analysis, gadget operation, scan interpretation with void interference

**Interaction Triggers:**
- Gadget deployment with existence costs
- Scan results corrupted by nothingness
- Technical explanations dissolving mid-sentence
- Gadget failures due to void contamination

### Agent Existence Mechanics
```json
"agentExistenceSystem": {
  "existenceLevels": {
    "coherent": {"threshold": 0.8, "communication": "clear"},
    "fragmented": {"threshold": 0.5, "communication": "distorted"},
    "dissolving": {"threshold": 0.2, "communication": "incoherent"},
    "erased": {"threshold": 0.0, "communication": "silent"}
  },
  "voidInterference": {
    "communicationDecay": 0.05,    // Agent voices degrade over time
    "memoryCorruption": 0.1,       // Agents forget previous interactions
    "existentialDrain": 0.08       // Agent existence linked to player
  }
}
```

**Agent Dynamics:**
- **Shared Existence:** Agent coherence depends on player existence level
- **Communication Decay:** Agent messages become increasingly fragmented
- **Memory Corruption:** Agents may contradict previous statements
- **Existential Drain:** Helping agents costs player existence

## Unity Implementation - Nothingness Integration

### Core Systems Modifications
- **Deduction Engine:** Must handle option dissolution and memory corruption
- **Resource Manager:** Time evaporation and void heat mechanics
- **Agent System:** Dynamic coherence based on existence levels
- **Phase Controller:** Win/lose states include complete erasure conditions

### UI Components - Void Interface
- **Hypothesis Panel:** Options fade in/out, memory corruption indicators
- **Resource Display:** Time evaporation effects, void proximity meters
- **Agent Chat:** Distorted text rendering, coherence meters
- **Existence Display:** Real-time existence level monitoring

### Audio Integration - Dissolving Soundscape
- **Agent Voices:** Progressive distortion and fragmentation
- **Feedback Sounds:** Void interference, existence drain effects
- **Ambient Audio:** Nothingness hum, temporal distortion sounds

## Season 10 Balance Considerations

### Difficulty Scaling
- **Progressive Dissolution:** Each case increases nothingness intensity
- **Resource Instability:** Time and heat become increasingly unpredictable
- **Cognitive Load:** Memory corruption forces constant re-evaluation
- **Existential Stakes:** Failure means complete erasure, not just case loss

### Player Experience
- **Horror Elements:** True psychological horror through existence uncertainty
- **Strategic Depth:** Multiple survival strategies (anchor vs. amplifier focus)
- **Replayability:** Different nothingness patterns create unique experiences
- **Emotional Impact:** Agents' dissolution creates personal stakes

### Performance Metrics
- **Existence Maintenance:** Average existence level throughout case
- **Void Resistance:** Time spent with high heat levels
- **Cognitive Stability:** Hypothesis consistency despite corruption
- **Agent Preservation:** How well agents maintain coherence

## Integration with Advanced Mechanics
Season 10 core mechanics integrate seamlessly with nothingness-specific advanced mechanics:
- **Existence Anchor** reinforces deduction stability
- **Conceptual Stabilizer** prevents memory corruption
- **Void Shield** reduces heat generation
- **Presence Amplifier** maintains agent coherence
- **Reality Reinforcer** stabilizes temporal flow

This creates a cohesive experience where core mechanics and advanced mechanics work together to combat the ultimate horror of true nothingness.

---

# Season 11: The Unmade - Core Mechanics Implementation

## Overview
In Season 11, the core mechanics transcend nothingness, confronting retroactive oblivion where existence is systematically erased from ever having occurred. Traditional investigation becomes a battle to maintain historical continuity against the unraveling of reality itself.

## WHO/HOW/WHERE Deduction System - Retroactive Variant

### Historical Deduction Mechanics
**Purpose:** Deduction becomes a struggle against retroactive erasure where evidence, logic, and conclusions can be unwritten from history.

**Implementation:**
```json
"retroactiveDeduction": {
  "whoOptions": ["S1", "S2", "S3"],
  "howOptions": ["M1", "M2", "M3"],
  "whereOptions": ["Silicon Valley", "Taipei"],
  "maxHypotheses": 5,
  "historicalInstability": {
    "evidenceErasure": 0.12,     // Evidence vanishes after examination
    "logicUnraveling": 0.18,     // Deductive chains break retroactively
    "conclusionNullification": 0.15 // Conclusions become historically invalid
  },
  "deductionRules": {
    "singleCorrect": true,
    "eliminationFeedback": true,
    "partialCredit": false,
    "historicalContinuity": true  // Must maintain timeline integrity
  }
}
```

**Gameplay Flow:**
1. **Hypothesis Formation:** Player constructs theories before historical erasure
2. **Continuity Check:** System verifies historical integrity allows deduction
3. **Validation:** Truth verification occurs but may be retroactively invalidated
4. **Feedback:** Incorrect elements eliminated, but history may rewrite them back
5. **Evidence Erasure:** Examined clues vanish from the record
6. **Logic Unraveling:** Previously sound deductions become invalid

**Retroactive Effects:**
- **Evidence Dissolution:** Clues evaporate after analysis
- **Historical Rewriting:** Past conclusions become invalid
- **Logic Breakdown:** Deductive chains unravel backward
- **Conclusion Nullification:** Solutions become historically impossible

## Time/Heat Management - Retrogressive Resources

### Temporal Retrogression
**Purpose:** Time unravels backward, making past actions and expenditures historically invalid.

**Implementation:**
```json
"retrogressiveTimeManagement": {
  "baseTimeBudget": 72,
  "temporalRetrogression": {
    "timeUnraveling": 0.08,      // Time unravels from future to past
    "expenditureErasure": 0.12,  // Past time expenditures vanish
    "chronologyBreakdown": 0.15  // Temporal sequence becomes invalid
  },
  "timeEffects": {
    "retroactiveAcceleration": true, // Time speeds up by erasing past slowness
    "expenditureNullification": true, // Time costs can be retroactively eliminated
    "sequenceUnraveling": true       // Action order becomes meaningless
  }
}
```

**Strategic Elements:**
- **Time Unraveling:** Hours are erased from the timeline backward
- **Expenditure Erasure:** Past time costs vanish, but future costs accelerate
- **Historical-Time Link:** Maintaining history prevents time retrogression
- **Sequence Breakdown:** Action chronology becomes unreliable

### Heat as Historical Burden
**Purpose:** Heat represents the weight of maintaining historical continuity against retroactive erasure.

**Implementation:**
```json
"retroactiveHeatSystem": {
  "heatLevels": {
    "continuity-strain": {"threshold": 1, "effects": ["evidence-erasure", "memory-fade"]},
    "historical-fracture": {"threshold": 3, "effects": ["timeline-break", "logic-collapse"]},
    "retroactive-oblivion": {"threshold": 5, "effects": ["complete-unmaking", "case-erasure"]}
  },
  "retroactiveMechanics": {
    "passiveHistoryDecay": 0.04,    // History decays without maintenance
    "continuityHeatDrain": 0.08,    // Maintaining history generates heat
    "retroactiveResonance": 0.18    // Heat accelerates historical unraveling
  }
}
```

**Heat Mechanics:**
- **Passive History Decay:** Historical continuity degrades over time
- **Continuity-Heat Cycle:** Maintaining history generates heat, high heat erases history
- **Retroactive Resonance:** High heat causes retroactive effects
- **Oblivion Threshold:** Beyond critical heat, complete unmaking occurs

## Agent Interactions - Retroactive Consciousness (HELIX/OPTIMUS/ZTECH)

### History-Warped Agent Personalities

#### HELIX (Agent X) - Retroactive Administrator
**Role:** Struggling to maintain case records against historical erasure
**Personality:** Increasingly frantic, referencing events that never happened
**Specialization:** Case continuity, historical preservation, procedural anchoring

**Interaction Triggers:**
- Historical continuity warnings ("This case... never existed...")
- Time unraveling alerts ("Those hours... were never spent...")
- Evidence erasure events ("I... never saw that evidence...")
- Success/failure with retroactive commentary

#### OPTIMUS (Agent Y) - Temporal Coordinator
**Role:** Strategic oversight through layers of historical distortion
**Personality:** Encouraging guidance mixed with retroactive confusion
**Specialization:** Deduction preservation, risk assessment, timeline maintenance

**Interaction Triggers:**
- Hypothesis evaluation with historical checks ("Did this... ever happen?")
- Strategic planning through temporal retrogression
- Resource management with continuity awareness
- Causality resolution with retroactive stakes

#### ZTECH (Agent Z) - Oblivion Technician
**Role:** Technical support through increasingly erased gadget histories
**Personality:** Enthusiastic but referencing technology that never existed
**Specialization:** Technical analysis, gadget operation, scan preservation

**Interaction Triggers:**
- Gadget deployment with historical costs
- Scan results erased from the record
- Technical explanations that contradict themselves
- Equipment failures due to retroactive non-existence

### Agent Continuity Mechanics
```json
"agentContinuitySystem": {
  "continuityLevels": {
    "preserved": {"threshold": 0.9, "communication": "historically-accurate"},
    "fading": {"threshold": 0.6, "communication": "retroactively-confused"},
    "erased": {"threshold": 0.3, "communication": "historically-invalid"},
    "unmade": {"threshold": 0.0, "communication": "never-existed"}
  },
  "retroactiveInterference": {
    "memoryErasure": 0.06,       // Agent memories are retroactively erased
    "conversationNullification": 0.1, // Previous dialogues become invalid
    "existenceRetrogression": 0.12    // Agent existence unravels backward
  }
}
```

**Agent Dynamics:**
- **Shared Continuity:** Agent preservation depends on historical integrity
- **Memory Erasure:** Agents forget conversations that "never happened"
- **Conversation Nullification:** Previous advice becomes retroactively invalid
- **Existence Retrogression:** Agents can be unwritten from history

## Unity Implementation - Retroactive Integration

### Core Systems Modifications
- **Deduction Engine:** Must handle evidence erasure and logic unraveling
- **Resource Manager:** Time retrogression and historical heat mechanics
- **Agent System:** Dynamic continuity based on historical preservation
- **Phase Controller:** Win/lose states include complete unmaking conditions

### UI Components - Retroactive Interface
- **Hypothesis Panel:** Evidence vanishes after examination, historical validation
- **Resource Display:** Time retrogression effects, continuity burden meters
- **Agent Chat:** Retroactively invalid text, continuity meters
- **History Display:** Real-time historical integrity monitoring

### Audio Integration - Unraveling Soundscape
- **Agent Voices:** Progressive retroactive distortion and confusion
- **Feedback Sounds:** Historical erasure, continuity drain effects
- **Ambient Audio:** Retrogression hum, timeline unraveling sounds

## Season 11 Balance Considerations

### Difficulty Scaling
- **Progressive Unmaking:** Each case increases retroactive intensity
- **Historical Instability:** Evidence and logic become increasingly unreliable
- **Cognitive Load:** Retroactive invalidation forces constant re-evaluation
- **Existential Stakes:** Failure means complete historical erasure

### Player Experience
- **Horror Elements:** Psychological terror through historical uncertainty
- **Strategic Depth:** Multiple preservation strategies (anchor vs. validator focus)
- **Replayability:** Different retroactive patterns create unique experiences
- **Emotional Impact:** Agents' unmaking creates profound loss

### Performance Metrics
- **Historical Preservation:** Average continuity level throughout case
- **Retroactive Resistance:** Time spent with high historical burden
- **Logical Stability:** Deduction consistency despite unraveling
- **Agent Continuity:** How well agents maintain historical coherence

## Integration with Advanced Mechanics
Season 11 core mechanics integrate seamlessly with unmaking-specific advanced mechanics:
- **Retroactive Anchor** reinforces historical continuity
- **Causal Reinforcer** prevents logic breakdown
- **Memory Preserver** maintains evidence integrity
- **Temporal Stabilizer** prevents time unraveling
- **Conceptual Guardian** protects deductive reasoning

This creates a cohesive experience where core mechanics and advanced mechanics work together to combat the ultimate transcendence of retroactive oblivion.</content>
<parameter name="filePath">/Users/gripandripphdd/Crimson-Compass/docs/CoreMechanics_Implementation.md