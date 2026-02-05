# Testing Guide

## Deduction Loop Test
1. Open MainScene in Unity.
2. Play scene.
3. Verify dropdowns populate with suspects/methods/locations.
4. Select hypothesis, click submit.
5. Check notepad updates with disproved elements.
6. Verify time decreases, heat increases appropriately.

## Edge Cases
- Submit invalid hypothesis (should not publish).
- Exceed time/heat limits (mission fail event).
- Test multiple hypotheses to narrow down.

## Agent Interactions
- Check console for HELIX recap on session start.
- Trigger hint offer, verify OPTIMUS messages.
- Select gadgets, verify ZTECH confirmation.

## Data Loading
- Switch case JSON in GameManager, verify loads correctly.
- Test procedural generation with CaseGenerator.

## Performance
- Monitor frame rate with multiple UI elements.
- Test on target device (Android emulator).