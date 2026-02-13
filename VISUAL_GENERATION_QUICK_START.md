# Crimson Compass Season 1 Visual Generation Quick Start

## Episode-by-Episode Visual Creation Workflow

This guide provides step-by-step instructions for generating all visual assets for Crimson Compass Season 1 using Grok Imagine.

---

## 🚀 Quick Start Commands

### Generate All Episode Assets
```bash
# Episode 1: Welcome Packet (9 assets)
.\scripts\generate_episode_assets.ps1 -Episode S01E01

# Episode 2: Badge & Borrow (6 assets)
.\scripts\generate_episode_assets.ps1 -Episode S01E02

# Episode 3: Clean Room (6 assets)
.\scripts\generate_episode_assets.ps1 -Episode S01E03

# Episode 4: Update Never Shipped (6 assets)
.\scripts\generate_episode_assets.ps1 -Episode S01E04

# Generate ALL episodes (87 total assets)
.\scripts\generate_episode_assets.ps1 -Episode ALL
```

### Dry Run (See What Would Be Generated)
```bash
# Preview Episode 1 generation
.\scripts\generate_episode_assets.ps1 -Episode S01E01 -DryRun

# Preview all episodes
.\scripts\generate_episode_assets.ps1 -Episode ALL -DryRun
```

---

## 📋 Episode Generation Checklist

### Episode 1: Welcome Packet (9 assets)
- [ ] **Backgrounds (2)**: Urban Rooftop Stakeout, Abandoned Warehouse District
- [ ] **Characters (5)**: Player (Compass Master), Optimus (Agent Y), Helix (Agent X), ZTech (Agent Z), Gasket (Agent G)
- [ ] **Objects (2)**: Crimson Compass, ZTech Multi-Tool Pen

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E01
.\scripts\generate_episode_assets.ps1 -Episode S01E01 -IncludeVideos
```

**Estimated Time:** 45-60 minutes

---

### Episode 2: Badge & Borrow (6 assets)
- [ ] **Backgrounds (2)**: Corporate Lobby, Executive Office
- [ ] **Characters (2)**: Corporate Executive, IT Administrator
- [ ] **Objects (2)**: Hardware Keys, Badge Scanner

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E02
```

**Estimated Time:** 30-45 minutes

---

### Episode 3: Clean Room (6 assets)
- [ ] **Backgrounds (2)**: Sterile Clean Room, Maintenance Corridor
- [ ] **Characters (2)**: Facility Manager, Insider Culprit
- [ ] **Objects (2)**: Micro-Components, Contamination Detector

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E03
```

**Estimated Time:** 30-45 minutes

---

### Episode 4: Update Never Shipped (6 assets)
- [ ] **Backgrounds (2)**: Server Room, Operations Center
- [ ] **Characters (2)**: IT Director, Hacker Culprit
- [ ] **Objects (2)**: Staging Server, Network Scanner Swarm

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E04
```

**Estimated Time:** 30-45 minutes

---

### Episode 5: Hostile Compliance (7 assets)
- [ ] **Backgrounds (2)**: Corporate Boardroom, Legal Library
- [ ] **Characters (3)**: Corporate Counsel, Legal Assistant, Judge (Virtual)
- [ ] **Objects (2)**: Legal Documents, Precedent Scanner

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E05
```

**Estimated Time:** 35-50 minutes

---

### Episode 6: Double Stamp (6 assets)
- [ ] **Backgrounds (2)**: Warehouse District, Subcontractor Facility
- [ ] **Characters (2)**: Subcontractor Leader, Warehouse Security
- [ ] **Objects (2)**: Sensor Module, Custody Forms

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E06
```

**Estimated Time:** 30-45 minutes

---

### Episode 7: Convenient Suspect (7 assets)
- [ ] **Backgrounds (2)**: Interrogation Room, Evidence Laboratory
- [ ] **Characters (3)**: Decoy Suspect, Real Culprit, Forensic Tech
- [ ] **Objects (2)**: False Evidence, Lie Detector

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E07
```

**Estimated Time:** 35-50 minutes

---

### Episode 8: Custody (7 assets)
- [ ] **Backgrounds (2)**: Secure Vault, Data Center
- [ ] **Characters (3)**: Security Director, Encryption Expert, Legal Counsel
- [ ] **Objects (2)**: Encrypted Files, Quantum Decryptor

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E08
```

**Estimated Time:** 35-50 minutes

---

### Episode 9: Exit Wound (6 assets)
- [ ] **Backgrounds (2)**: Facility Corridors, Security Checkpoint
- [ ] **Characters (2)**: Extraction Leader, Facility Director
- [ ] **Objects (2)**: Neural Interface Chip, Security Countermeasures

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E09
```

**Estimated Time:** 30-45 minutes

---

### Episode 10: Blow Cover (6 assets)
- [ ] **Backgrounds (2)**: Corporate Security Center, Alliance Negotiation Room
- [ ] **Characters (2)**: Ring Leader, Corrupt Executive
- [ ] **Objects (2)**: Network Mapping Display, Deception Detector

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E10
```

**Estimated Time:** 30-45 minutes

---

### Episode 11: Safe House (6 assets)
- [ ] **Backgrounds (2)**: Safe House Interior, Evidence Storage
- [ ] **Characters (2)**: Cleanup Leader, Legal Observer
- [ ] **Objects (2)**: Evidence Containers, Quantum Verifier

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E11
```

**Estimated Time:** 30-45 minutes

---

### Episode 12: Hindsight (6 assets)
- [ ] **Backgrounds (2)**: Final Confrontation Chamber, Prototype Vault
- [ ] **Characters (2)**: Final Culprit, Agency Director
- [ ] **Objects (2)**: Core Prototype Module, Pattern Synthesis Display

**Command:**
```powershell
.\scripts\generate_episode_assets.ps1 -Episode S01E12
```

**Estimated Time:** 1.5-2 hours

---

## 🎨 Manual Generation Process

For each asset, the script will:

1. **Open Grok Imagine** in your browser
2. **Display the prompt** to copy
3. **Show filename** and save location
4. **Wait for you to generate** the asset manually

### Generation Steps:
1. Copy the displayed prompt
2. Paste into Grok Imagine text box
3. Click "Generate"
4. Wait for result (typically 10-30 seconds)
5. Download and save with the specified filename
6. Press Enter in the script to continue

---

## 🔧 Quality Control

### Before Generation:
- [ ] Ensure Grok Imagine account is active
- [ ] Check available quota (free tier: limited, SuperGrok: unlimited)
- [ ] Close other browser tabs for better performance

### During Generation:
- [ ] Verify aspect ratios (9:16 backgrounds, 3:4 characters, 1:1 objects)
- [ ] Check style consistency (high contrast, atmospheric lighting)
- [ ] Ensure mobile-friendly file sizes (<1MB)
- [ ] Generate 3-4 variations per asset for selection

### After Generation:
- [ ] Run validation: `.\scripts\validate_setup.ps1`
- [ ] Import to Unity: `.\scripts\unity_workflow.ps1`
- [ ] Test in Unity editor

---

## 📁 File Organization

Generated assets are automatically organized:

```
GeneratedAssets/
├── S01E01_Welcome_Packet/
│   ├── S01E01_Urban_Rooftop_Stakeout_v01_20260212.png
│   ├── S01E01_Player_Compass_Master_v01_20260212.png
│   └── ...
├── S01E02_Badge_Borrow/
│   └── ...
├── S01E03_Clean_Room/
│   └── ...
└── S01E04_Update_Never_Shipped/
    └── ...
```

---

## ⚡ Performance Tips

### Speed Up Generation:
- Generate similar assets together (all backgrounds, then all characters)
- Use multiple browser tabs for parallel generation
- Generate during off-peak hours for faster processing

### Quality Optimization:
- Always include full prompts (don't truncate)
- Use specific aspect ratios in prompts
- Generate variations to find the best result
- Check results at full resolution before saving

### Batch Processing:
```powershell
# Generate only backgrounds for all episodes
.\scripts\generate_episode_assets.ps1 -Episode ALL -Filter "*Background*"

# Generate only characters
.\scripts\generate_episode_assets.ps1 -Episode ALL -Filter "*Character*"

# Custom variations (more than default 3)
.\scripts\generate_episode_assets.ps1 -Episode S01E01 -VariationsPerAsset 5
```

---

## 🔄 Unity Integration

After generating assets:

```powershell
# Validate generation
.\scripts\validate_setup.ps1

# Import to Unity
.\scripts\unity_workflow.ps1

# Check in Unity
# - Open Crimson Compass project
# - Check Assets/Sprites/ folders
# - Run: Crimson Compass > Process Imported Assets
# - Run: Crimson Compass > Generate Asset Report
```

---

## 📊 Progress Tracking

### Total Assets: 87
- Episode 1: 9 assets
- Episode 2: 6 assets
- Episode 3: 6 assets
- Episode 4: 6 assets
- Episode 5: 7 assets
- Episode 6: 6 assets
- Episode 7: 7 assets
- Episode 8: 7 assets
- Episode 9: 6 assets
- Episode 10: 6 assets
- Episode 11: 6 assets
- Episode 12: 6 assets

### Completion Checklist:
- [ ] Episode 1 assets generated
- [ ] Episode 2 assets generated
- [ ] Episode 3 assets generated
- [ ] Episode 4 assets generated
- [ ] Episode 5 assets generated
- [ ] Episode 6 assets generated
- [ ] Episode 7 assets generated
- [ ] Episode 8 assets generated
- [ ] Episode 9 assets generated
- [ ] Episode 10 assets generated
- [ ] Episode 11 assets generated
- [ ] Episode 12 assets generated
- [ ] Assets imported to Unity
- [ ] Unity project tested
- [ ] Quality assurance passed

---

## 🆘 Troubleshooting

### Common Issues:

**"Grok Imagine not loading"**
- Check internet connection
- Try different browser
- Clear browser cache

**"Assets too large"**
- Regenerate with compression prompts
- Use Unity's automatic compression
- Resize in image editor if needed

**"Style inconsistency"**
- Review prompts for consistent descriptors
- Regenerate with more specific styling
- Use reference images in prompts

**"Unity import fails"**
- Check file paths
- Ensure Unity project is closed
- Run validation script first

---

## 📚 Resources

- **Detailed Prompts**: `docs/Season1_Episode_Visual_Guide.md`
- **Grok Imagine Guide**: `Grok_Imagine_Integration_README.md`
- **Batch Scripts**: `scripts/` directory
- **Validation**: `.\scripts\validate_setup.ps1`

Happy generating! 🎨✨