# MainScene Setup Guide

## Unity Scene Creation
1. Open Unity 2022.3 LTS.
2. Create new scene: File > New Scene > Basic (Built-in).
3. Save as `Assets/Scenes/MainScene.unity`.

## Hierarchy Setup
- **GameManager** (Empty GameObject)
  - Attach `GameManager.cs`
  - Assign:
    - Case Json: `Assets/Data/Cases/case_0001.json`
    - Agents Json: `Assets/Data/Agents/agents.json`
    - Insights Jsonl: `Assets/Data/Insights/insights_0001_0500.jsonl`

- **UI Canvas** (UI > Canvas)
  - **NotepadPanel** (UI > Panel)
    - Attach `NotepadUI.cs`
    - Add 3 Text components for WHO/HOW/WHERE
    - Assign to NotepadUI public fields

  - **HypothesisPanel** (UI > Panel)
    - Attach `HypothesisInput.cs`
    - Add 3 Dropdowns for WHO/HOW/WHERE
    - Add 1 Button for Submit
    - Assign to HypothesisInput public fields

## Event System
- Ensure EventSystem exists (UI > Event System).

## Testing
- Play scene: Dropdowns populate, submit hypothesis, notepad updates with disproofs.