#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Complete Crimson Compass production setup automation
.DESCRIPTION
    Automates the entire setup process for Crimson Compass:
    - Audio asset creation
    - Scene integration
    - Build validation
.PARAMETER SkipUnity
    Skip Unity Editor steps (for when Unity is not available)
.EXAMPLE
    .\complete_setup.ps1
.EXAMPLE
    .\complete_setup.ps1 -SkipUnity
#>

param(
    [switch]$SkipUnity
)

$ErrorActionPreference = "Stop"

Write-Host "🚀 Crimson Compass Complete Setup Automation" -ForegroundColor Green
Write-Host "=============================================" -ForegroundColor Green

# Get project root (script is in project root)
$ProjectRoot = Get-Location
$ToolsDir = Join-Path $ProjectRoot "Tools"

# Function to run validation
function Run-Validation {
    Write-Host "`n📋 Running pre-setup validation..." -ForegroundColor Yellow

    # Run episode data validation
    & python (Join-Path $ToolsDir "validate_episode_data.py")
    if ($LASTEXITCODE -ne 0) {
        throw "Episode data validation failed"
    }

    # Run audio validation (will show warnings, which is expected)
    & python (Join-Path $ToolsDir "validate_audio_events.py")
    # Don't fail on audio validation warnings - they're expected before setup
}

# Function to attempt Unity setup
function Run-UnitySetup {
    if ($SkipUnity) {
        Write-Host "`n⚠️  Skipping Unity setup as requested" -ForegroundColor Yellow
        return
    }

    Write-Host "`n🎵 Attempting Unity audio setup..." -ForegroundColor Cyan

    $UnityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"

    if (-not (Test-Path $UnityPath)) {
        Write-Host "❌ Unity not found at $UnityPath" -ForegroundColor Red
        Write-Host "Manual setup required in Unity Editor:" -ForegroundColor Yellow
        Write-Host "  1. Open the project in Unity" -ForegroundColor White
        Write-Host "  2. Go to Tools > Complete Crimson Compass Setup" -ForegroundColor White
        return
    }

    # Check if Unity is already running
    $unityProcesses = Get-Process -Name "Unity" -ErrorAction SilentlyContinue
    if ($unityProcesses) {
        Write-Host "⚠️  Unity is currently running. Cannot run batch mode setup." -ForegroundColor Yellow
        Write-Host "Manual setup required in Unity Editor:" -ForegroundColor Yellow
        Write-Host "  1. In Unity: Tools > Complete Crimson Compass Setup" -ForegroundColor White
        return
    }

    # Try batch mode setup
    try {
        Write-Host "Running Unity batch mode setup..." -ForegroundColor Cyan
        & $UnityPath -batchmode -nographics -quit -projectPath $ProjectRoot -executeMethod QuickAudioSetup.DoCompleteSetup -logFile (Join-Path $ProjectRoot "Logs/unity_setup.log")

        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Unity setup completed successfully!" -ForegroundColor Green
        } else {
            throw "Unity setup failed with exit code $LASTEXITCODE"
        }
    }
    catch {
        Write-Host "❌ Unity batch mode setup failed: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "Manual setup required in Unity Editor:" -ForegroundColor Yellow
        Write-Host "  1. Open the project in Unity" -ForegroundColor White
        Write-Host "  2. Go to Tools > Complete Crimson Compass Setup" -ForegroundColor White
    }
}

# Function to run build validation
function Run-BuildValidation {
    Write-Host "`n🏗️  Running build validation tests..." -ForegroundColor Cyan

    # Test Android build
    Write-Host "Testing Android build..." -ForegroundColor White
    & python (Join-Path $ToolsDir "build.py") --target Android --validate
    if ($LASTEXITCODE -ne 0) {
        throw "Android build validation failed"
    }

    # Test iOS build
    Write-Host "Testing iOS build..." -ForegroundColor White
    & python (Join-Path $ToolsDir "build.py") --target iOS --validate
    if ($LASTEXITCODE -ne 0) {
        throw "iOS build validation failed"
    }

    Write-Host "✅ All build validations passed!" -ForegroundColor Green
}

# Main execution
try {
    Push-Location $ProjectRoot

    # Step 1: Pre-validation
    Run-Validation

    # Step 2: Unity setup
    Run-UnitySetup

    # Step 3: Build validation
    Run-BuildValidation

    Write-Host "`n🎉 Crimson Compass setup automation completed successfully!" -ForegroundColor Green
    Write-Host "`nNext steps:" -ForegroundColor Cyan
    Write-Host "  • If Unity setup was manual: Close Unity and run full builds" -ForegroundColor White
    Write-Host "  • Production builds: python Tools/build.py --target Android" -ForegroundColor White
    Write-Host "  • Production builds: python Tools/build.py --target iOS" -ForegroundColor White

}
catch {
    Write-Host "`n❌ Setup automation failed: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Check the logs and try manual setup if needed." -ForegroundColor Yellow
    exit 1
}
finally {
    Pop-Location
}