# Crimson Compass (working title)

Mobile-first **international spy whodunnit**.

**Cadence:** weekly ~5-minute micro-episodes (2-3 scenes each) delivered as bite-sized investigations.

## Pillars
- Clue-style triad deduction: **WHO / HOW / WHERE**
- Globe chase & geography-as-clues
- Cliffhanger pacing ("one more")
- Agent foundation:
  - **OMEGA** (AGENT Y) = Executive Director, precision organizational genius outpaced by technology
  - **HELIX** (AGENT X) = Admin librarian, mission coordinator, big sister glue
  - **ZTECH** (AGENT Z) = Tech genius, gadget creator, ZBRANCH commander with unlimited resources
  - **GASKET** (AGENT G) = Former agent (post-incident), unofficial wildcard presence

## Repo layout
- `Assets/Data/Cases/` → case/mission JSON
- `Assets/Data/Geo/` → geo facts
- `Assets/Data/Insights/` → insight databases (500 + Clue 100 + SpyStoryKit)
- `Assets/Data/Agents/` → agents, messages, triggers
- `docs/` → schema + doctrine + setup guides
  - `docs/SeriesFoundation_Season1Kit.md` → canonical narrative rules and Season 1 episode kit

## Narrative Foundation
> **"You are a new Detective brought into a dysfunctional international shadowy covert espionage agency that 'prevents global collapse, crime, chaos, villains through - top level funding and clearance.. but vague on where it comes from that's top secret, gadgets - 2100 future proof over engineered for max absurdity and function in all areas of espionage and spy life and everyday living at the agency, and luck - they survive because of the dynamic, Agents Y, X, Z form the foundation of agency, Y is the head (Omega), he is a bumbling boomer who doesn't understand the technology he is forced to use to keep up, he is a top level executive director with precision organizational investigational and analytical skills a genius outpaced by time, Agent x (Helix) is the big sister glue that holds the agency together she is the admin librarian archivist, mission planner, mission executer under Y, coordinator and dispatch, Agent Z of ZBRANCH is a tech genius and engineering super agent, with unlimited budget and resources z is on the cutting edge of technology and innovation, nerd through and through with the determination of a front line soldier, Z makes the gadgets and handles all the it and tech for the agency.. his lab is 100% AI Robots and agents with over 250 agents and 5000 robots at his secret production and testing facility that no one knows about. Agent G (Gasket) was involved in an incident and was taken out of service, he is still around but not officially on or off team, '—but the real investigation begins when you discover that the agency's most powerful tool is haunted by the ghost of its former master."**

**Tone Formula:** Professionally absurd and absurdly professional - Futurama Detective Noir + Uncharted Action-Adventure + Institutional Irony.

**Two Core Hooks (every episode must serve both):**
1) **Mission Hook:** close the case using the Crimson Compass
2) **Team Hook:** survive the compass's psychological toll and Gasket's secrets.

**Signature Mechanic:** 95% closure with 5% unease—every case is solved, but the Crimson Compass always leaves a haunting anomaly.

**The Architect:** Faceless mastermind criminal running the shadowy international syndicate, always one step ahead, motives unknown.

**HQ:** Regular office building above (boring, bland), international underground spy agency below (Batman cave meets DARPA meets Skunkworks).

See `docs/SeriesFoundation_Season1Kit.md` for full details, team roles, Season 1 episode grid, and authoring guidelines.

## Content Foundation
The project includes a complete 12-season content foundation with 144 episodes, each delivered as ~5-minute micro-episodes with 2-3 scenes featuring unique thematic mechanics and progressive complexity:

### Season Overview
- **Season 1** (Episodes 0001-0012): "The Replacement" - Player replaces Gasket, discovers Crimson Compass
- **Season 2** (Episodes 0013-0024): "Compass Calibration" - Learning compass mechanics and basic investigations
- **Season 3** (Episodes 0025-0036): "Truth Vectors" - Advanced compass usage and anomaly recognition
- **Season 4** (Episodes 0037-0048): "The 5% Unease" - Psychological toll and first Architect hints
- **Season 5** (Episodes 0049-0060): "Gasket's Shadow" - Investigating Gasket's incident and compass origins
- **Season 6** (Episodes 0061-0072): "Architect's Web" - First direct confrontations with syndicate operations
- **Season 7** (Episodes 0073-0084): "Network of Deception" - Interconnected conspiracies revealed
- **Season 8** (Episodes 0085-0096): "Fractured Reality" - Compass-induced illusions vs. actual threats
- **Season 9** (Episodes 0097-0108): "ZBRANCH Unleashed" - Maximum absurdity with unlimited tech resources
- **Season 10** (Episodes 0109-0120): "Omega's Burden" - Executive challenges and technological adaptation
- **Season 11** (Episodes 0121-0132): "Helix's Network" - Administrative mastery and coordination complexity
- **Season 12** (Episodes 0133-0144): "The Final Calibration" - Confronting The Architect and compass mastery

### Core Mechanics (All Seasons)
- **WHO/HOW/WHERE Deduction**: Triad-based clue resolution system
- **Crimson Compass Navigation**: Truth vector scanning and anomaly detection
- **Time & Heat Management**: Resource constraints with risk/reward tradeoffs
- **Agent Interactions**: Four specialized agents (OMEGA, HELIX, ZTECH, GASKET)
- **The Architect**: Faceless antagonist creating psychological tension
- **95% Closure, 5% Unease**: Signature mechanic with compass anomalies
- **HQ Operations**: Regular office above, spy agency below with absurd professionalism

### Advanced Mechanics (Season-Specific)
Each season introduces unique advanced mechanics that build upon the core foundation:
- **Season 4**: Decoy deployment and network disruption
- **Season 5**: Consequence mapping and ripple effect analysis
- **Season 6**: Mirror analysis and authenticity verification

### Technical Specifications
- **Data Format**: JSON-based case files with comprehensive mechanics structures
- **Validation**: Automated integrity checking for all 144 episodes
- **Unity Ready**: Complete technical specifications for game implementation
- **Micro-Episode Format**: 5-minute episodes with 2-3 scenes each
- **Progressive Complexity**: Each season increases mechanical depth while maintaining accessibility

See individual season documentation in `docs/` for detailed mechanics specifications.

## Development Status
Recommended: Unity **2022.3 LTS**.

**Build Status: ✅ WORKING** - Automated build pipeline functional with scene setup and asset management.

## Build Automation
This project uses a deterministic local build pipeline that mirrors Unity Build Automation batchmode flows.

### Quick Start
- **VS Code:** `Ctrl+Shift+B` → **Build (Win64 Dev)**
- **Terminal:** `python Tools/build.py --target Win64 --dev`

### Available Commands
```bash
# Validators only
python Tools/build.py --validate-only

# Build with preflight validation
python Tools/build.py --target Win64 --dev --preflight

# Build specific platforms
python Tools/build.py --target Android
python Tools/build.py --target iOS
```

### VS Code Tasks
- **Preflight (Validators)**: Run episode data and audio event validators
- **Build (Win64 Dev)**: Fast iteration build for Windows development
- **Preflight + Build (Win64 Dev)**: Full validation + build pipeline
- **Build Android/iOS**: Production builds for mobile platforms

Build outputs go to `Builds/` directory with timestamps. Logs are in `Logs/`.

## Autogen Tooling
The project includes automated scaffolding generation for rapid development.

### Generate/Update Scaffolding
```bash
# Generate build + VS Code scaffolding
python Tools/cc_autogen.py init --unity-version 2022.3.62f3

# Generate audio system scaffolding
python Tools/cc_autogen.py audio

# Generate everything
python Tools/cc_autogen.py all --unity-version 2022.3.62f3
```

### What Gets Generated
- **Build System**: CLI build scripts, PowerShell wrappers, Python orchestrators
- **VS Code Tasks**: Integrated build and validation tasks
- **Audio System**: AudioEvent, AudioCatalog, AudioService scripts with mixer guidance
- **Validators**: Episode data and audio event validation stubs

The autogen tool supports safe editing with autogen blocks for preserving manual changes.

## Next Steps
- Set up scene in Unity
- Test deduction loop
- Build for Android
- Add assets and polish
