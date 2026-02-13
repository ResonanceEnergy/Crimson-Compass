# Crimson Compass - Build & Automation Features

## Overview
Crimson Compass now includes automated build pipelines, version display, and testing capabilities.

## Features

### 🎯 Version Display
- **Location**: Bottom-right corner of the game UI
- **Source**: `Assets/Resources/version.txt`
- **Format**: Plain text version string (e.g., "1.0.0")
- **Styling**: White text with black outline, 14pt font

### 🖥️ Desktop Shortcut
- **Location**: Windows Desktop as "Crimson Compass.lnk"
- **Auto-updating**: Automatically updates to latest build
- **Icon**: Uses the game's executable icon
- **Description**: "Crimson Compass - Espionage Strategy Game v1.0.0"

### 🧪 Automated Testing
- **Purpose**: Validates built executables run correctly
- **Duration**: 5-second runtime test
- **Checks**: Process starts, runs, and terminates gracefully
- **Integration**: Runs automatically after successful builds

### 🔄 Automated Builds
- **Schedule**: Nightly at 2:00 AM (when set up)
- **Process**: Preflight validation + build + automated testing
- **Logs**: Stored in `Logs/` directory
- **Output**: Latest build in `Builds/Win64/`

## Usage

### Manual Build with Testing
```bash
# Full pipeline: validation + build + test
python Tools/build.py --target Win64 --dev --preflight --test

# Just build and test
python Tools/build.py --target Win64 --dev --test

# Just validation
python Tools/build.py --preflight
```

### Version Management
- Edit `Assets/Resources/version.txt` to change the displayed version
- Version updates automatically appear in new builds

### Desktop Shortcut
- **Automatic**: Created/updated after every build
- **Manual Creation**: Run `python Tools/create_desktop_shortcut.py`
- **Location**: Appears on Windows Desktop as "Crimson Compass"
- **Always Current**: Points to the latest successful build

### Setting Up Nightly Builds
1. **Run as Administrator**:
   ```powershell
   .\setup_nightly_builds.ps1
   ```

2. **Manual Setup** (if PowerShell script fails):
   - Open Windows Task Scheduler
   - Create new task: "CrimsonCompass_NightlyBuild"
   - Trigger: Daily at 2:00 AM
   - Action: Start program
     - Program: `python`
     - Arguments: `Tools/build.py --preflight --test`
     - Start in: `C:\Path\To\Your\Project`
   - Check "Run with highest privileges"

### Build Options
- `--target Win64`: Build for Windows 64-bit
- `--dev`: Development build (includes debug symbols)
- `--preflight`: Run validation checks before building
- `--test`: Run automated testing after building

## File Structure
```
Tools/
├── build.py              # Main build orchestration
├── test_executable.py    # Automated testing script
├── setup_nightly_builds.py  # Python setup script
├── validate_*.py         # Validation scripts
└── build.ps1            # Unity build script

setup_nightly_builds.ps1  # PowerShell setup script

Assets/Resources/
└── version.txt          # Version display data

Builds/Win64/            # Build outputs
Logs/                    # Build and test logs
```

## Troubleshooting

### Build Failures
- Check `Logs/unity_build_*.log` for Unity errors
- Ensure Unity Editor is properly installed
- Verify all assets are present and not corrupted

### Test Failures
- Check if executable launches manually
- Verify Windows permissions for running executables
- Check `Logs/test_*.log` for detailed error messages

### Nightly Build Issues
- Ensure Task Scheduler has proper permissions
- Check that Python is in system PATH
- Verify project paths are correct in scheduled task

### Version Display Issues
- Confirm `Assets/Resources/version.txt` exists
- Check Unity console for Resources.Load errors
- Verify TextMeshPro is properly imported

## Logs
- **Build Logs**: `Logs/unity_build_*.log`
- **Test Logs**: `Logs/test_*.log`
- **Validation Logs**: Console output during preflight

## Dependencies
- Python 3.8+
- Unity 2022.3.62f3
- TextMeshPro (for version display)
- Windows Task Scheduler (for automation)