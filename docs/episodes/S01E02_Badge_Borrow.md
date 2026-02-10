# S01E02: Badge & Borrow

## Episode Overview
**Title:** Badge & Borrow  
**Case:** Social engineering theft of hardware keys  
**Case Token:** Keys recovered; culprit caught  
**Shadow Token:** Maintenance corridor exists on no blueprint  
**Theme:** Social engineering tactics, trust manipulation  

## Character Descriptions

### Player (The Closer)
- **Appearance:** Same as S01E01, now more confident in agency environment
- **Background:** Building rapport with team, learning their dynamics
- **Personality:** Growing trust in Helix's coordination, wary of ZTech's chaos

### Advisors (Same as S01E01)
- **Helix:** Leading social engineering countermeasures
- **Optimus:** Analyzing behavioral patterns
- **ZTech:** Technical security bypasses
- **Gasket:** Appears briefly, senses deception layers

### NPCs
- **Corporate Executive:** Mid-40s, charming manipulator, expensive watch
- **IT Administrator:** Early 30s, socially awkward, helpful but naive
- **Security Personnel:** Various, suspicious of outsiders
- **Thief (Culprit):** Late 20s, charismatic, uses borrowed identities

## Scene 1: Corporate Infiltration

**Background:** Sleek corporate lobby, glass elevators, security checkpoints. Employees in business attire, reception desk with digital displays.

**Narrative:** You've infiltrated the corporate office building posing as IT support. The suspect is using social engineering to gain access to restricted areas.

**Puzzle:** Badge Verification  
- **Objective:** Identify fake vs real employee badges  
- **Mechanics:** Scan badges with device, cross-reference with database  
- **Solution:** Fake badges lack proper encryption signatures  
- **Clue:** Holographic elements missing on counterfeit

**Dialogue:**

**Helix:** "Remember, Closer - people are the weakest link. And sometimes the strongest tool."

**IT Admin:** "Thanks for coming so quickly. The system keeps glitching on the executive floor."

**ZTech:** "Glitching? More like hacked! Their security is full of holes."

## Scene 2: Executive Confrontation

**Background:** Corner office with city view, mahogany desk, security monitors. Executive pacing nervously.

**Narrative:** You confront the executive who claims to be a legitimate contractor. The hardware keys are missing from the secure vault.

**Puzzle:** Timeline Reconstruction  
- **Objective:** Match access logs with witness statements  
- **Mechanics:** Drag-and-drop timeline events, identify inconsistencies  
- **Solution:** Executive's alibi has 15-minute gap  
- **Clue:** Security camera timestamp mismatch

**Dialogue:**

**Executive:** "I assure you, this is all a misunderstanding. I'm here on official business."

**Optimus:** "Your access card was used at 14:37. You claim to have been in a meeting. Inconsistency detected."

**Gasket:** "Not the first time. Won't be the last." (brief appearance)

## Puzzles & Mechanics

### Social Engineering Defense
- **Type:** Conversation Tree
- **Description:** Respond to suspect's manipulation attempts
- **Choices:** Direct confrontation, subtle probing, technical verification
- **Consequences:** Trust meter affects information gained

### Key Recovery Chase
- **Type:** Multi-path pursuit
- **Description:** Track thief through building floors
- **Choices:** Elevator, stairs, service corridors
- **Consequences:** Different encounter outcomes

## Visual Prompts (Midjourney)

### Corporate Lobby
```
/imagine prompt: sleek corporate lobby, glass elevators, security checkpoints, employees in business attire, reception desk with digital displays, modern architecture, professional atmosphere --ar 16:9 --v 5
```

### Executive Office
```
/imagine prompt: corner office city view, mahogany desk, security monitors, executive pacing nervously, modern office decor, tension atmosphere --ar 16:9 --v 5
```

### Hardware Keys
```
/imagine prompt: sleek hardware security keys, electronic components, keychain attachments, technological design, detailed engineering --ar 1:1 --v 5
```

## Audio Prompts (Unity)

### Ambient Audio
**Corporate Lobby:**
- Office bustle: typing, conversations, elevator dings
- Mix: CORPORATE_LOBBY_AMBIENCE
- Parameters: volume 0.5, spatial blend 0.7

**Executive Office:**
- Tense silence: air conditioning, distant phones
- Mix: EXECUTIVE_TENSION_AMBIENCE
- Parameters: volume 0.4, spatial blend 0.8

### Character Voices
**Corporate Executive:**
- Voice: Male, smooth, authoritative
- Sample: "I assure you, this is all a misunderstanding."
- Unity Audio: VO_EXECUTIVE_SMOOTH

**IT Administrator:**
- Voice: Male, nervous, technical
- Sample: "The system keeps glitching on the executive floor."
- Unity Audio: VO_IT_NERVOUS

### Sound Effects
**Badge Scanner:**
- Beep sequence: ELECTRONIC_BEEP + CONFIRMATION_CHIME
- Parameters: pitch varies by validity

**Door Access:**
- Magnetic lock: ELECTRIC_CLICK + DOOR_HISS
- Parameters: reverb 0.2, spatial blend 0.9

### Music Cues
**Investigation:**
- Track: MYSTERY_UNFOLDING
- Parameters: fade in 3s, loop, volume 0.3

**Confrontation:**
- Track: DRAMATIC_REVEAL
- Parameters: build tension, volume 0.5