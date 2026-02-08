# Crimson Compass - Production Automation

## 🤖 Complete Automation Pipeline

Crimson Compass now features a fully automated development and deployment pipeline that handles everything from validation to production builds.

## 🚀 Quick Start

### One-Command Full Automation
```powershell
.\automate.ps1 -Mode Full
```

This single command will:
- ✅ Run all validations
- ✅ Build for Android & iOS
- ✅ Generate deployment manifests
- ✅ Provide deployment instructions

### Individual Pipeline Stages

```powershell
# Development Setup & Validation
.\automate.ps1 -Mode Setup

# Production Builds Only
.\automate.ps1 -Mode Build -Target Android
.\automate.ps1 -Mode Build -Target iOS
.\automate.ps1 -Mode Build -Target Both

# Deployment Preparation
.\automate.ps1 -Mode Deploy
```

## 📋 Pipeline Overview

### Phase 1: Development Setup
- **Episode Data Validation**: Validates all 12 Season 1 case files
- **Audio System Validation**: Checks audio asset structure
- **Build Pipeline Testing**: Validates Android/iOS build configurations
- **Unity Integration**: Guides through Unity Editor setup

### Phase 2: Production Builds
- **Clean Builds**: Optional artifact cleanup
- **Cross-Platform**: Android APK and iOS builds
- **Artifact Management**: Organized build outputs
- **Size Reporting**: Build size analysis

### Phase 3: Deployment Preparation
- **Manifest Generation**: JSON manifest with build details
- **Deployment Instructions**: Platform-specific guidance
- **Artifact Verification**: Confirms all builds completed

## 🎯 Manual Unity Step

**Required**: After running setup, complete this Unity Editor step:

1. Open the project in Unity
2. Go to `Tools > Complete Crimson Compass Setup`
3. This creates audio assets and integrates AudioService

## 📁 Project Structure

```
Crimson-Compass/
├── automate.ps1          # Master automation script
├── complete_setup.ps1    # Development setup automation
├── deploy.ps1           # Production deployment automation
├── Tools/
│   ├── build.py         # Python build orchestrator
│   ├── build.ps1        # PowerShell build wrapper
│   ├── validate_episode_data.py
│   └── validate_audio_events.py
├── Assets/
│   ├── Audio/           # Audio system (ScriptableObjects)
│   └── Scenes/          # MainScene.unity
└── Builds/              # Generated build artifacts
```

## 🛠️ Automation Scripts

### `automate.ps1` - Master Controller
- **Modes**: Setup, Build, Deploy, Full
- **Targets**: Android, iOS, Both
- **Options**: -Clean (remove artifacts)

### `complete_setup.ps1` - Development Setup
- Pre-build validations
- Unity setup guidance
- Build pipeline testing

### `deploy.ps1` - Production Deployment
- Production builds
- Artifact management
- Deployment manifests

## ✅ Validation & Quality Assurance

### Episode Data Validation
- 12 Season 1 case files validated
- Schema compliance checking
- Cross-reference validation (suspects → truth)

### Audio System Validation
- AudioEvent asset structure
- AudioCatalog configuration
- Mixer group assignments

### Build Validation
- Unity build pipeline testing
- Cross-platform compatibility
- Artifact generation verification

## 📦 Build Artifacts

Builds are organized in the `Builds/` directory:

```
Builds/
├── Android/
│   ├── CrimsonCompass_20260207_160000.apk
│   └── build_manifest.txt
├── iOS/
│   └── iOSBuild_20260207_160000/
└── deployment_manifest.json
```

## 🚀 Deployment Instructions

### Android (Google Play)
1. Upload `Builds/Android/*.apk` to Google Play Console
2. Configure store listing and screenshots
3. Submit for review

### iOS (App Store)
1. Use `Builds/iOS/iOSBuild_*.zip` for App Store Connect
2. Configure app metadata and screenshots
3. Submit for review

## 🔧 Troubleshooting

### Unity Is Running
- Close Unity Editor before running build operations
- Use `-Mode Setup` for validation-only runs

### Build Failures
- Check `Logs/unity_build_*.log` for Unity errors
- Ensure MainScene is enabled in Build Settings
- Verify audio assets were created in Unity

### Validation Errors
- Run `.\automate.ps1 -Mode Setup` to diagnose
- Check episode JSON files in `Assets/Data/Cases/`
- Verify audio setup completed in Unity

## 🎯 Pipeline Status

- ✅ **Development Setup**: Fully automated
- ✅ **Unity Integration**: One-click menu command
- ✅ **Production Builds**: Automated for both platforms
- ✅ **Deployment Prep**: Automated manifest generation
- ✅ **Quality Assurance**: Comprehensive validation suite

## 🚀 Production Ready

Crimson Compass is now **fully automated** from development to deployment!

**Final Command for Production**:
```powershell
.\automate.ps1 -Mode Full
```

This will take you from source code to production-ready mobile apps. 🎉📱