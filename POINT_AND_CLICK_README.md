# Crimson Compass - Point-and-Click Adventure System

This implementation adds a point-and-click adventure interface to Crimson Compass, following the design specifications for iPhone-optimized gameplay.

## Core Components

### Verb System
- **MOVE**: Navigate character to locations
- **OBSERVE**: Examine objects for descriptions
- **ENGAGE**: Interact with objects and NPCs
- **KIT**: Access inventory for combinations
- **PROTOCOL**: Apply special procedures

### Key Scripts

#### AdventureGameManager.cs
Main game controller handling:
- Verb selection and interaction routing
- Inventory management
- Onboarding sequence coordination
- Case closure logic

#### VerbBarUI.cs
Bottom UI bar with verb buttons:
- Visual feedback for selected verbs
- Progressive unlocking during onboarding
- Touch-optimized button layout

#### InteractableObject.cs
Base class for all interactive scene objects:
- Observe/Engage/Protocol actions
- Item combination logic
- Extensible for specific behaviors

#### InventoryUI.cs
Clean inventory interface (max 4 slots):
- Touch-friendly item selection
- Combination targeting
- Minimal, mobile-optimized design

#### DialogueUI.cs
NPC conversation system:
- Portrait + topic selection
- No free-form typing (mobile-friendly)
- Structured conversation flow

## Onboarding Sequence (10 Minutes)

### Phase 1: Cold Open (0:00-0:45)
- "Tap to clock in" prompt
- Normal workplace atmosphere
- Subtle absurd context clues

### Phase 2: Verb Introduction (0:45-2:00)
- MOVE and OBSERVE unlocked
- Highlighted hotspots for interaction
- Immediate feedback on actions

### Phase 3: ENGAGE Unlock (2:00-3:30)
- Unlock ENGAGE verb
- Simple pickup + device interaction
- Disproportionate "stability graph" result

### Phase 4: Dialogue (3:30-5:00)
- Meet desk NPC (Helix)
- 3 topics: Assignment, Protocol, Local Notes
- Unlocks case notebook access

### Phase 5: KIT Unlock (5:00-6:30)
- Unlock inventory system
- Tag + Seal = Authorized Tag combination
- Clean, limited inventory UI

### Phase 6: Case Closure (6:30-8:30)
- Complete interaction sequence
- CASE CLOSED stamp
- Motif puncture audio/visual

### Phase 7: UneaseTail (8:30-10:00)
- Subtle audio shift to UneaseTail
- Minor visual timing anomaly
- "Shadow Token: recorded" note

## Scene Setup

### Required Scene Objects
1. **ClockInPrompt**: Initial "Tap to clock in" UI
2. **SeamlineTag**: Pickup item for combination
3. **StabilityDevice**: Target for tag application
4. **DeskNPC**: Conversation partner
5. **RoutingAnomaly**: Final observation target

### UI Layout (Portrait, iPhone Optimized)
```
┌─────────────────┐
│ Case Header     │  ← 5-8% (S01_SEAMLINE · OBJECTIVE)
├─────────────────┤
│                 │
│   World View    │  ← 75-80% (interactive scene)
│                 │
├─────────────────┤
│ MOVE OBSERVE    │
│ ENGAGE KIT      │  ← 12-15% (verb bar)
│ PROTOCOL        │
└─────────────────┘
```

## Pricing Model

### Episodic Premium Structure
- **Free**: Episode 1 (onboarding case)
- **Paid**: Season Pass ($X.99) or 3-episode bundles
- **Optional**: Complete Series Bundle later

### Why This Fits
- Aligns with existing "seasons/episodes" content structure
- iOS appetite for story-driven games
- Premium pricing normal in mobile narrative space
- Trial reduces friction for touch-based adventure

## App Store Positioning

### Hook
"A point-and-click spy adventure where routine procedures quietly decide the fate of the world."

### Title Options
- **Primary**: Crimson Compass
- **Subtitle**: A Spy Adventure of Procedures and Consequences

### Feature Bullets
- Point-and-click verbs built for touch: Move, Observe, Engage, Kit, Protocol
- Episodic cases with a larger hidden thread: Each operation closes—almost completely
- Bright stylized spy world, serious tone: Absurd only by context
- Audio-driven tension states: Normal → Investigation → Extraction → UneaseTail

### Keywords
point and click, adventure, spy, detective, narrative, story, episodic, mystery, puzzles, choices

## Technical Implementation Notes

### Touch Optimization
- 44pt minimum hit targets
- Bottom/middle controls for thumb reach
- No critical interactions in top corners
- Latching verbs reduce mis-taps

### Performance
- Object pooling for inventory slots
- Efficient raycasting for interactions
- Minimal UI redraws during verb selection

### Audio Integration
- Uses existing CCAudioDirector system
- UneaseTail snapshots for tension states
- Motif punctures for case closure

## Setup Instructions

1. Add AdventureGameManager to scene
2. Create VerbBarUI with 5 verb buttons
3. Add InventoryUI with slot prefab
4. Create DialogueUI with topic buttons
5. Add CaseClosureUI for case completion
6. Populate scene with InteractableObject instances
7. Configure OnboardingManager timing
8. Set up SceneSetup for object placement

## Testing Checklist

- [ ] Verb bar appears with MOVE/OBSERVE unlocked
- [ ] ENGAGE unlocks at 2:00 mark
- [ ] Dialogue system shows 3 topics
- [ ] KIT unlocks inventory at 5:00
- [ ] Item combination creates Authorized Tag
- [ ] Case closes with stamp at sequence completion
- [ ] UneaseTail triggers with subtle anomaly
- [ ] All interactions work on touch
- [ ] UI fits iPhone portrait layout