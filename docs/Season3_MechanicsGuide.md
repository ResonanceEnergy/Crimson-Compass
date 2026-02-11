# Crimson Compass Season 3: Enhanced Mechanics Documentation

## Overview
Season 3 "Recovery Paradoxes" introduces advanced gameplay mechanics that build upon the core WHO/HOW/WHERE deduction system. These mechanics create tension through paradoxes, timing manipulation, and jurisdictional conflicts.

## New Core Mechanics

### Paradox System
**Purpose:** Creates uncertainty and tension by introducing contradictory evidence and reality-bending elements.

**Implementation:**
- `paradoxLevel`: Integer (1-5) indicating paradox intensity per case
- `paradoxEvent`: Triggered by specific clues, requires resolution through network mapping
- Escalates through Season 3, peaking at level 5 in final episodes

**Gameplay Impact:**
- Higher paradox levels increase risk of "reality distortion" failures
- Paradox events can temporarily scramble evidence
- Resolution provides bonus insights but risks heat accumulation

### Recovery Scanner
**Purpose:** Detects hidden modifications in recovered assets and personnel.

**Gadget Details:**
- **Name:** RECOVERYSCANNER
- **Function:** Reveals implants, tracking devices, and modifications
- **Usage:** Activated on clues with `recoveryScanner` field

**Scanner Properties:**
- `detects`: Type of threat (reset-protocol-implant, neural-implant, etc.)
- `difficulty`: Scanning challenge level (easy/medium/hard)
- Success reveals hidden evidence, failure triggers security alerts

### Timing Analyzer
**Purpose:** Identifies and resolves temporal anomalies in custody transfers and operations.

**Gadget Details:**
- **Name:** TIMINGANALYZER
- **Function:** Calculates timing offsets and predicts adversary actions
- **Usage:** Puzzle mechanic for timing-based clues

**Analyzer Properties:**
- `reveals`: Information type (protocol-activation-window, transfer-timing)
- `puzzleType`: Interaction style (offset-calculation, sequence-timing)
- Critical for resolving 11-minute offset anomalies

### Jurisdictional Puzzles
**Purpose:** Resolves authority conflicts between agencies and jurisdictions.

**Implementation:**
- `jurisdictionConflicts`: Array of authority disputes per case
- `jurisdictionPuzzle`: Interactive resolution mechanic

**Puzzle Properties:**
- `conflictType`: Type of dispute (authority-overlap, jurisdiction-void)
- `authorities`: Involved parties (Federal, State, Local, International)
- `solution`: Required resolution method (Federal-override, treaty-citation)

**Gameplay Impact:**
- Wrong jurisdiction choices create paradoxes
- Correct resolution unlocks evidence chains
- Builds tension through bureaucratic complexity

### Network Mapping Interfaces
**Purpose:** Visualizes connections between cases, recovery operations, and paradox networks.

**Gadget Details:**
- **Name:** PARADOXMAPPER
- **Function:** Reveals hidden connections and predicts paradox spread
- **Usage:** Strategic overview tool for Season 3

**Mapping Properties:**
- `networkConnections`: Links between cases and recovery hubs
- `connectionType`: Node relationship (recovery-hub, redirection-node)
- `paradoxRisk`: Danger level of following connections

**Additional Tools:**
- **RESETCOUNTER**: Prevents adversary escape protocols
- Enhanced **CLEANER**: Removes paradox residue from evidence

## Enhanced Clue System

### Multi-Modal Clues
Season 3 clues support multiple interaction types:

```json
{
  "id": "C1",
  "text": "Recovery logs show perfect execution with 11-minute timing anomaly.",
  "leadsTo": ["S1", "M1"],
  "recoveryScanner": {
    "detects": "reset-protocol-implant",
    "difficulty": "medium"
  },
  "timingAnalyzer": {
    "reveals": "protocol-activation-window",
    "puzzleType": "offset-calculation"
  }
}
```

### Clue Enhancement Types
- **recoveryScanner**: Implant detection mechanics
- **timingAnalyzer**: Temporal puzzle interactions
- **jurisdictionPuzzle**: Authority conflict resolution
- **networkMapping**: Connection visualization
- **paradoxEvent**: Reality distortion triggers

## Enhanced Method System

### Method Augmentations
Methods now include specialized analysis data:

```json
{
  "id": "M1",
  "name": "Reset Protocol Activation",
  "signatures": ["protocol:escape-sequence"],
  "timingAnalysis": {
    "anomalyType": "reset-trigger",
    "offsetMinutes": 11,
    "severity": "critical"
  }
}
```

### Method Enhancement Types
- **timingAnalysis**: Temporal behavior patterns
- **recoveryScan**: Threat detection signatures
- **networkMapping**: Connection relationship data

## Progression & Difficulty

### Paradox Escalation
- **Episodes 1-4:** Paradox Level 1-2 (Introduction)
- **Episodes 5-8:** Paradox Level 2-3 (Escalation)
- **Episodes 9-12:** Paradox Level 3-5 (Convergence)

### Network Building
- Cases connect progressively through `networkConnections`
- Paradox risk increases with network complexity
- Final episodes reveal the complete paradox network

## Unity Implementation Notes

### UI Requirements
- **Paradox Meter:** Visual gauge showing paradox accumulation
- **Network Map:** Interactive node graph for connections
- **Timing Interface:** Clock manipulation mini-games
- **Jurisdiction Panel:** Authority selection interface

### Puzzle Mechanics
- **Scanner Mini-Game:** Precision-based implant detection
- **Timing Puzzles:** Offset calculation and sequence alignment
- **Jurisdiction Resolver:** Authority conflict logic puzzles
- **Network Tracer:** Connection pathfinding challenges

### Audio/Visual Feedback
- Paradox distortion effects during high-level events
- Scanner sweep animations and detection sounds
- Timing anomaly warning chimes
- Jurisdiction resolution confirmation effects

## Testing Guidelines

### Paradox Balance
- Test paradox accumulation doesn't break gameplay
- Verify resolution mechanics provide fair challenge
- Ensure high paradox levels don't become frustrating

### Mechanic Integration
- Confirm gadgets enhance rather than replace core deduction
- Test multi-modal clue interactions flow naturally
- Validate network connections create meaningful choices

### Performance Impact
- Monitor frame rate during network mapping operations
- Test memory usage with enhanced clue data
- Ensure mobile performance with complex puzzle states

## Future Expansion
These mechanics establish a foundation for:
- Multi-episode paradox chains
- Cross-season network connections
- Advanced gadget specialization
- Dynamic difficulty based on paradox management</content>
<parameter name="filePath">/Users/gripandripphdd/Crimson-Compass/docs/Season3_MechanicsGuide.md