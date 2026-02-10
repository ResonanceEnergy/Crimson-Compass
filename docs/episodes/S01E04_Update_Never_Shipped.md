# S01E04: The Update That Never Shipped

## Episode Overview
**Title:** The Update That Never Shipped  
**Case:** Firmware staging server hijacked  
**Case Token:** Server reclaimed; breach sealed  
**Shadow Token:** Deployment timestamps show the same skew  
**Theme:** Cyber security, digital forensics  

## Character Descriptions

### Player (The Closer)
- **Appearance:** Focused on computer screens, technical analysis mode
- **Background:** Adapting to digital investigation techniques
- **Personality:** Patient with technical details, frustrated by digital complexity

### Advisors
- **Helix:** Coordinating with IT staff
- **Optimus:** Digital pattern analysis
- **ZTech:** Technical breach investigation
- **Gasket:** Not present

### NPCs
- **IT Director:** Mid-40s, stressed, casual tech attire
- **Network Admin:** Late 20s, defensive, hoodie
- **Security Analyst:** Early 30s, detail-focused, glasses
- **Hacker (Culprit):** Mid-20s, arrogant, remote presence

## Scene 1: Server Room Investigation

**Background:** Climate-controlled server room, racks of blinking servers, monitoring consoles. Cool air, humming fans, status lights.

**Narrative:** You've accessed the server room where firmware staging occurs. Network logs show unusual traffic patterns, but the server appears normal.

**Puzzle:** Traffic Pattern Analysis  
- **Objective:** Identify injection points in network traffic  
- **Mechanics:** Graph visualization, anomaly detection  
- **Solution:** Unauthorized connection from maintenance port  
- **Clue:** Timestamp metadata manipulation

**Dialogue:**

**IT Director:** "Our systems are air-gapped! This shouldn't be possible."

**ZTech:** "Air-gapped? Please. I could hack this with a USB drive and a dream."

**Network Admin:** "The logs show normal traffic. I don't see anything unusual."

## Scene 2: Digital Confrontation

**Background:** Operations center with multiple screens, analysts at workstations. Alert notifications, system status displays.

**Narrative:** The malicious firmware update is set to deploy in minutes. You need to stop it while preserving evidence.

**Puzzle:** Deployment Timer  
- **Objective:** Calculate and interrupt deployment sequence  
- **Mechanics:** Real-time countdown, multi-stage interruption  
- **Solution:** Override at authentication checkpoint  
- **Clue:** Backdoor credentials in system cache

**Dialogue:**

**Security Analyst:** "Deployment sequence initiated! We have 4 minutes!"

**Optimus:** "Probability of successful override: 67%. Adjusting parameters."

**Helix:** "Talk me through it, people. What do you need?"

## Puzzles & Mechanics

### Network Forensics
- **Type:** Log Analysis
- **Description:** Trace hacker's digital footprint
- **Solution:** IP address leads to compromised workstation
- **Reward:** Breach method identification

### Deployment Interruption
- **Type:** Timing Puzzle
- **Description:** Stop firmware deployment without corruption
- **Choices:** Emergency shutdown, selective blocking, authentication override
- **Consequences:** Evidence preservation vs system integrity

## Visual Prompts (Midjourney)

### Server Room
```
/imagine prompt: climate-controlled server room, racks of blinking servers, monitoring consoles, cool air visualization, humming fans, technological atmosphere --ar 16:9 --v 5
```

### Operations Center
```
/imagine prompt: high-tech operations center, multiple screens, analysts at workstations, alert notifications, system status displays, cyber security atmosphere --ar 16:9 --v 5
```

### Firmware Update
```
/imagine prompt: digital firmware update visualization, code streams, deployment progress, cyber security interface, technological graphics --ar 16:9 --v 5
```

## Audio Prompts (Unity)

### Ambient Audio
**Server Room:**
- Cooling systems: FAN_HUM + COOLING_UNIT
- Mix: SERVER_ROOM_AMBIENCE
- Parameters: volume 0.5, low-frequency emphasis

**Operations Center:**
- Computer activity: KEYBOARD_TYPING + MOUSE_CLICKS
- Mix: TECH_CENTER_AMBIENCE
- Parameters: volume 0.6, mid-range focus

### Sound Effects
**Server Access:**
- Authentication: ELECTRONIC_BEEP + ACCESS_GRANTED
- Parameters: pitch confirmation

**Deployment Alert:**
- Warning: URGENT_ALARM + VOICE_WARNING
- Parameters: volume 0.8, reverb 0.3

### Music Cues
**Digital Investigation:**
- Track: CYBER_TENSION
- Parameters: electronic elements, volume 0.4

**Race Against Time:**
- Track: TECHNICAL_COUNTDOWN
- Parameters: increasing tempo, volume 0.6