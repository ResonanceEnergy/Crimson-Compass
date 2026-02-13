# Crimson Compass (working title)

Mobile-first **international spy whodunnit**.

**Cadence:** weekly ~60-minute missions delivered via 5–10 minute micro-episodes.

## Pillars
- Clue-style triad deduction: **WHO / HOW / WHERE**
- Globe chase & geography-as-clues
- Cliffhanger pacing ("one more")
- Agent trio:
  - **HELIX** (AGENT X) = persistent mission secretary/EA
  - **OPTIMUS** (AGENT Y) = mission coordinator + help valve
  - **ZTECH** (AGENT Z) = gadgets + toolkit progression

## Repo layout
- `Assets/Data/Cases/` → case/mission JSON
- `Assets/Data/Geo/` → geo facts
- `Assets/Data/Insights/` → insight databases (500 + Clue 100 + SpyStoryKit)
- `Assets/Data/Agents/` → agents, messages, triggers
- `docs/` → schema + doctrine + setup guides
  - `docs/SeriesFoundation_Season1Kit.md` → canonical narrative rules and Season 1 episode kit

## Narrative Foundation
> **You’re the sane detective brought in to close cases for an absurdly elite but dysfunctional covert agency that prevents global collapse through paperwork, gadgets, and luck—while a shadowy adversary is only ever inferred through the clues left behind, always slipping away.**

**Tone:** Mission: Impossible stakes + James Bond global cool + Ocean’s team chemistry + Futurama-style institutional absurdity.

**Core Rule:** 95% closure per episode, but the final 5% never sits right—dual pursuit of explicit crimes and implicit shadow adversary.

See `docs/SeriesFoundation_Season1Kit.md` for full details, team roles, Season 1 episode grid, and authoring guidelines.

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
