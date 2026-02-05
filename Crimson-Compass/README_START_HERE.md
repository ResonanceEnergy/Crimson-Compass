# Crimson Compass — One‑Drop Pack

## 1) Open in VS Code
- macOS/Linux: `./scripts/open_vscode.sh`
- Windows: `./scripts/open_vscode.ps1`

## 2) Push to GitHub
Create an empty GitHub repo, then:

- macOS/Linux:
  ```bash
  ./scripts/github_init_push.sh https://github.com/USERNAME/crimson-compass.git
  ```

- Windows PowerShell:
  ```powershell
  .\scripts\github_init_push.ps1 -RemoteUrl https://github.com/USERNAME/crimson-compass.git
  ```

## 3) Unity
Recommended Unity: **2022.3 LTS**.
Open `CrimsonCompass/` as the Unity project root.

## Included Data
- `Assets/Data/Insights/insights_0001_0500.jsonl`
- `Assets/Data/Insights/clue_0100.jsonl`
- `tools/SpyStoryKit/insights_400.jsonl`
