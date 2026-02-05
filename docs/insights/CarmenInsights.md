# Carmen Sandiego Insights Integration

## Key Design Lessons for Crimson Compass

### Core Loop & Deduction
- Triad deduction (WHO/HOW/WHERE) is central; ensure hypotheses produce exactly one disproof.
- Time as core balancing: travel costs, investigation costs.
- Warrant systems as attribute filters narrowing suspects.
- Episodic cases with randomized paths/clues for replayability.
- Promotion/rank system for long-term progression.

### Content Architecture
- Clues as templated sentences tied to location attributes.
- Procedural case generation: pick suspect → pick trail → generate consistent clues.
- Curate locations for iconic landmarks; avoid inaccuracies.
- Almanac/encyclopedia for learning loops.

### Mechanics
- Menu-driven UI for accessibility.
- Difficulty scaling by clue clarity or branching.
- Fail states (wrong warrant, time expired) make deduction meaningful.
- Gadget verbs for exploration depth.

### Educational Value
- Geography as investigation, not memorization.
- Knowledge graph: locations ↔ attributes ↔ clue sentences.
- Fit timed sessions (5-12 min micro-cases).

### Legal & Branding
- Avoid trademarked names (Carmen, ACME, VILE).
- Original IP for App Store compliance.
- Premium no-IAP positioning.

### Competitive Positioning
- Differentiate with deeper deduction or procedural elements.
- Dual modes (modern + retro) for nostalgia.
- Offline play, controller support.

## Integration into Crimson Compass
- **DisproofEngine**: Already implements one-disproof rule.
- **Case Data**: Expand with more suspects/methods/locations.
- **Insights DB**: Add more clues inspired by patterns.
- **UI**: Add notepad for disproved elements.
- **Agents**: Implement HELIX/OPTIMUS/ZTECH interactions.

## Next Steps
- Generate 50+ new clues based on Carmen patterns.
- Add procedural case generation.
- Implement time/heat mechanics.
- Create Unity scene with map UI.