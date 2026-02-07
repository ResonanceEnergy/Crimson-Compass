# Crimson Compass — Audio Spine (Consolidated)

## Locked Canon / Constraints (Audio-relevant)
- Use the term **COUNTERMEASURE** everywhere.
- **No RNG**. Outcomes are state-driven (Heat/Time bands, LeadIntegrity, thresholds).
- Mandatory cross-off per episode: **Hypothesis → EXACTLY ONE Disproof → Eliminated**.
- Warrant ritual: **WHO / WHERE / HOW + CONFIDENCE (PRESS vs HOLD)**. If wrong, reveal **EXACTLY ONE axis**.
- **THE ARCHITECT** echo: max **1 subtle line/episode**; no explicit familiarity until **Ep7+**.

## Audio Architecture (Deterministic)
- Per-episode audio delta stack via ScriptableObjects:
  - `CCEpisodeAudioDeltaSO` (bus gain offsets, event gain offsets, event pitch offsets, stateBand gain offsets)
  - `CCAudioDeltaLibrarySO` (episodeNumber → delta)
  - `CCAudioDeltaApplier` (applies deltas based on `CCAudioContextProvider.episodeNumber`)
  - Optional debug cycler (`[` and `]`)
  - Editor generator for 12 starter delta assets + library

### Director math
- `totalDb = variant.gainDb + busDelta + eventDelta + stateBandDelta`
- `pitchCents = variant.pitchCents + eventPitchDelta`

## Buses (Locked)
- **AMBIENCE** (primary pressure carrier)
- **SFX** (physical truth)
- **UI** (system truth / COUNTERMEASURE)
- **VO** (clarity)
- **PRESS** (authority)
- **ECHO** (Architect-only, guarded)

## State Bands (Locked)
- **HEAT**: Low / Mid / High
- **TIME**: Normal / Tight / Critical
- **LEAD_INTEGRITY**: Solid / Shaky / Collapsing

## Tone Doctrine (Locked)
- Baseline: **Cold procedural with faint human fatigue** (overworked office, fluorescent, paper & screens)
- Accents: **Warm procedural** (token-only; aftermath / late-season consequence)

## Import / Pipeline Decisions
- **Always stream AMBIENCE** (folder-based rule)
- VO import policy:
  - `VO/Barks` → DecompressOnLoad
  - `VO/Dialogue` → CompressedInMemory
  - `VO/Narration` → Streaming

## Starter Assets
- Sample ambience loop WAV included.
- Download checklist PDF included.
- Unity toolkit included (importer, prefab builder, QA window).
