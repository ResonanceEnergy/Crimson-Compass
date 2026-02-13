# Crimson Compass - Automated Workflow

## 🎯 Overview

This document describes the automated workflow for Crimson Compass development, from setup to deployment.

## 🚀 Quick Start

### Option 1: Full Automation (Recommended)
Run the complete automated workflow:

**VS Code**: Terminal → Run Task → "Workflow Automation (Full)"
**Command Line**: `.\workflow_automation.ps1`

This will:
1. ✅ Run preflight validation (episode data, audio systems)
2. 🎮 Open Unity Editor and guide you through scene setup
3. 🏗️ Build the project for Win64
4. 🧪 Test the build functionality
5. 📊 Generate a comprehensive status report

### Option 2: Build-Only Automation
For CI/CD or when Unity is already set up:

**VS Code**: Terminal → Run Task → "Workflow Automation (Build Only)"
**Command Line**: `.\workflow_automation.ps1 -SkipUnity -SkipTest`

## 📋 Manual Workflow Steps

If you prefer manual control, follow these steps:

### 1. Unity Editor Setup
```bash
# Open Unity Editor
start "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe" -projectPath "."
```

**In Unity Editor:**
- Wait for project to load
- Go to: **Tools → Setup Main Scene**
- This creates the UI scene with all components
- Save the scene when prompted

### 2. Build System
```bash
# Run preflight validation + build
python Tools/build.py --target Win64 --dev --preflight

# Or just build
python Tools/build.py --target Win64 --dev
```

**VS Code Tasks:**
- Terminal → Run Task → "Preflight + Build (Win64 Dev)"
- Terminal → Run Task → "Build (Win64 Dev)"

### 3. Gameplay Testing
- Launch the built executable from `Builds/Win64/`
- Test core systems:
  - UI loads without errors
  - Can navigate menus
  - No immediate crashes
  - Episode progression works

### 4. Development Continuation
- All core systems are functional
- Full codebase ready for continued development
- Use existing automation scripts for additional features

## 🔧 Automation Script Details

### `workflow_automation.ps1`

**Parameters:**
- `-SkipUnity`: Skip opening Unity Editor
- `-SkipBuild`: Skip build process
- `-SkipTest`: Skip build testing

**Phases:**
1. **Preflight Validation**: Episode data and audio system checks
2. **Unity Editor Setup**: Opens Unity and guides scene setup
3. **Production Build**: Creates Win64 executable
4. **Build Testing**: Launches and verifies build functionality
5. **Status Report**: Comprehensive development status

### VS Code Integration

**Default Build Task**: "Workflow Automation (Full)" - marked as default
**Build Tasks Available**:
- Preflight (Validators)
- Build (Win64 Dev)
- Preflight + Build (Win64 Dev)
- Workflow Automation (Full)
- Workflow Automation (Build Only)

## 📊 Status Reporting

The automation provides detailed status reporting:

```
🎯 CRIMSON COMPASS - DEVELOPMENT STATUS
========================================
✅ Preflight Validation: PASSED
✅ Unity Editor Setup: COMPLETED
✅ Production Build: SUCCESSFUL
✅ Build Testing: PASSED

🏆 OVERALL STATUS:
   🎉 ALL SYSTEMS GO! Ready for development and deployment!
```

## 🛠️ Troubleshooting

### Build Fails Despite Success Message
- This is a known PowerShell script issue
- Check `Builds/Win64/` for the actual executable
- Build logs are in `Logs/` directory

### Unity Editor Won't Open
- Verify Unity 2022.3.62f3 is installed
- Check Unity Hub for proper installation
- Ensure project path is correct

### Validation Fails
- Check Python installation
- Verify Tools directory contains validation scripts
- Check episode data files in project

### Scene Setup Issues
- Ensure Unity Editor is fully loaded
- Check that Tools menu contains "Setup Main Scene"
- Verify all required assets are present

## 📁 Project Structure

```
Crimson-Compass/
├── Assets/                 # Unity assets
├── Builds/                 # Build outputs
├── Logs/                   # Build and validation logs
├── Tools/                  # Build and validation scripts
├── workflow_automation.ps1 # Main automation script
└── .vscode/
    └── tasks.json         # VS Code task definitions
```

## 🔗 Related Files

- `README.md` - General project documentation
- `docs/BuildGuide.md` - Detailed build instructions
- `docs/TestingGuide.md` - Testing procedures
- `Tools/build.py` - Build script
- `Tools/validate_*.py` - Validation scripts

## 🎮 Development Ready

The automated workflow ensures:
- ✅ Consistent development environment
- ✅ Automated UI scene setup
- ✅ Reliable build process
- ✅ Functional testing verification
- ✅ Status reporting and next steps

**The Crimson Compass project is now fully automated and ready for development!**