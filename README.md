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

## Next Steps
- Set up scene in Unity
- Test deduction loop
- Build for Android
- Add assets and polish
