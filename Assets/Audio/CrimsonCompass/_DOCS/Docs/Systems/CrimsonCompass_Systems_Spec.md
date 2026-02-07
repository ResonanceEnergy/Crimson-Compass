# Crimson Compass — Systems Spec (After Audio)

## 1) Narrative State (Single Source of Truth)
**Must include**: episodeNumber, episodePhase, currentHypothesisId, disproofUsed, eliminatedHypothesisIds, heatBand, timeBand, leadIntegrityBand,
warrantWho/Where/How + confidence (PRESS/HOLD), warrantResult (CORRECT or WRONG_AXIS_WHO/WHERE/HOW), architectEchoUsed, architectFamiliarityUnlocked.

**Rule:** One controller writes state; all systems subscribe.

## 2) Warrant Evaluation Engine (Deterministic)
- Evaluate WHO/WHERE/HOW deterministically.
- If wrong, reveal EXACTLY ONE axis using fixed priority (no RNG).
- Apply PRESS vs HOLD consequences deterministically.
- Disproof is an explicit single beat; never auto-create multiple disproof beats.

## 3) UI Consistency Contract
- Wrong warrant highlights exactly one axis.
- UI must not imply multiple disproof moments.
- UI must not create early Architect familiarity (pre-Ep7).
- Use COUNTERMEASURE terminology exactly.

## 4) Production Roadmap
Spine test scene → NarrativeState → Warrant Engine → Episode template → Episodes 1–3.
