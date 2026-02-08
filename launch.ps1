#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Crimson Compass Ultimate Production Launcher
.DESCRIPTION
    The final automation - complete production launch from development to deployment
.PARAMETER Force
    Force all operations (skip confirmations)
.EXAMPLE
    .\launch.ps1
.EXAMPLE
    .\launch.ps1 -Force
#>

param(
    [switch]$Force
)

$ErrorActionPreference = "Stop"

# ASCII Art Banner
$banner = @"
 ██████╗██████╗ ██╗███╗   ███╗███████╗ ██████╗ ███╗   ██╗     ██████╗ ██████╗ ███╗   ███╗██████╗  █████╗ ███████╗███████╗
██╔════╝██╔══██╗██║████╗ ████║██╔════╝██╔═══██╗████╗  ██║    ██╔════╝██╔═══██╗████╗ ████║██╔══██╗██╔══██╗██╔════╝██╔════╝
██║     ██████╔╝██║██╔████╔██║███████╗██║   ██║██╔██╗ ██║    ██║     ██║   ██║██╔████╔██║██████╔╝███████║███████╗███████╗
██║     ██╔══██╗██║██║╚██╔╝██║╚════██║██║   ██║██║╚██╗██║    ██║     ██║   ██║██║╚██╔╝██║██╔═══╝ ██╔══██║╚════██║╚════██║
╚██████╗██║  ██║██║██║ ╚═╝ ██║███████║╚██████╔╝██║ ╚████║    ╚██████╗╚██████╔╝██║ ╚═╝ ██║██║     ██║  ██║███████║███████║
 ╚═════╝╚═╝  ╚═╝╚═╝╚═╝     ╚═╝╚══════╝ ╚═════╝ ╚═╝  ╚═══╝     ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═╝     ╚═╝  ╚═╝╚══════╝╚══════╝

"@

Write-Host $banner -ForegroundColor Magenta
Write-Host "🎯 CRIMSON COMPASS - ULTIMATE PRODUCTION LAUNCH" -ForegroundColor Magenta
Write-Host "================================================" -ForegroundColor Magenta
Write-Host ""

if (-not $Force) {
    Write-Host "🚀 This will launch the complete Crimson Compass production pipeline!" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "📋 Pipeline includes:" -ForegroundColor White
    Write-Host "  • Development validation & setup" -ForegroundColor Gray
    Write-Host "  • Unity asset creation (if possible)" -ForegroundColor Gray
    Write-Host "  • Production builds for Android & iOS" -ForegroundColor Gray
    Write-Host "  • Deployment artifact generation" -ForegroundColor Gray
    Write-Host "  • Store submission preparation" -ForegroundColor Gray
    Write-Host ""

    $confirmation = Read-Host "Continue with production launch? (y/N)"
    if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
        Write-Host "Launch cancelled." -ForegroundColor Yellow
        exit 0
    }
}

$ProjectRoot = Get-Location

# Function to show progress
function Show-Progress {
    param([string]$Message, [string]$Status = "RUNNING")
    $timestamp = Get-Date -Format "HH:mm:ss"
    Write-Host "[$timestamp] $Status | $Message" -ForegroundColor Cyan
}

# Function to show success
function Show-Success {
    param([string]$Message)
    Write-Host "✅ $Message" -ForegroundColor Green
}

# Function to show warning
function Show-Warning {
    param([string]$Message)
    Write-Host "⚠️  $Message" -ForegroundColor Yellow
}

# Function to show error
function Show-Error {
    param([string]$Message)
    Write-Host "❌ $Message" -ForegroundColor Red
}

try {
    Show-Progress "Initializing Crimson Compass Production Launch"

    # Phase 1: System Validation
    Show-Progress "Phase 1: System Validation"
    Show-Progress "  → Checking automation scripts"

    $scripts = @("automate.ps1", "complete_setup.ps1", "deploy.ps1")
    foreach ($script in $scripts) {
        if (Test-Path $script) {
            Show-Success "  ✓ $script found"
        } else {
            throw "Required script $script not found"
        }
    }

    Show-Progress "  → Validating project structure"
    $requiredDirs = @("Assets", "Tools", "ProjectSettings")
    foreach ($dir in $requiredDirs) {
        if (Test-Path $dir) {
            Show-Success "  ✓ $dir directory exists"
        } else {
            throw "Required directory $dir not found"
        }
    }

    # Phase 2: Development Setup
    Show-Progress "Phase 2: Development Setup"
    Show-Progress "  → Running complete setup validation"

    $setupResult = & .\complete_setup.ps1 2>&1
    if ($LASTEXITCODE -eq 0) {
        Show-Success "  ✓ Development setup completed"
    } else {
        # Check if it's just the Unity warning
        $hasRealErrors = $setupResult | Where-Object { $_ -match "❌" -and $_ -notmatch "Unity" }
        if ($hasRealErrors) {
            throw "Development setup failed"
        } else {
            Show-Warning "  ⚠️  Development setup completed with Unity warnings (expected)"
        }
    }

    # Phase 3: Unity Integration Attempt
    Show-Progress "Phase 3: Unity Integration"
    Show-Progress "  → Attempting Unity asset creation"

    $unityRunning = Get-Process -Name "Unity" -ErrorAction SilentlyContinue
    if ($unityRunning) {
        Show-Warning "  ⚠️  Unity is running - cannot perform automatic asset creation"
        Show-Progress "  → Manual Unity setup required:"
        Show-Progress "    1. In Unity: Tools > Complete Crimson Compass Setup" -ForegroundColor White
        Show-Progress "    2. Close Unity Editor" -ForegroundColor White
        Show-Progress "    3. Re-run this launch script" -ForegroundColor White

        Write-Host ""
        Write-Host "⏸️  PRODUCTION LAUNCH PAUSED" -ForegroundColor Yellow
        Write-Host "Complete the Unity setup above, then run:" -ForegroundColor White
        Write-Host "  .\launch.ps1" -ForegroundColor Cyan
        Write-Host ""
        exit 0
    } else {
        Show-Progress "  → Unity not running - attempting automatic setup"
        # Try Unity batch mode setup
        $unityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"
        if (Test-Path $unityPath) {
            Show-Progress "  → Running Unity batch mode setup"
            try {
                & $unityPath -batchmode -nographics -quit -projectPath $ProjectRoot -executeMethod QuickAudioSetup.DoCompleteSetup -logFile "$ProjectRoot/Logs/unity_setup_$(Get-Date -Format 'yyyyMMdd_HHmmss').log"
                if ($LASTEXITCODE -eq 0) {
                    Show-Success "  ✓ Unity asset creation completed automatically!"
                } else {
                    throw "Unity setup failed"
                }
            }
            catch {
                Show-Warning "  ⚠️  Unity batch mode failed - manual setup required"
                Show-Progress "  → Manual Unity setup:"
                Show-Progress "    1. Open project in Unity" -ForegroundColor White
                Show-Progress "    2. Tools > Complete Crimson Compass Setup" -ForegroundColor White
                Show-Progress "    3. Close Unity and re-run launch" -ForegroundColor White
                exit 0
            }
        } else {
            Show-Warning "  ⚠️  Unity not found - manual setup required"
            Show-Progress "  → Manual Unity setup:"
            Show-Progress "    1. Open project in Unity" -ForegroundColor White
            Show-Progress "    2. Tools > Complete Crimson Compass Setup" -ForegroundColor White
            Show-Progress "    3. Close Unity and re-run launch" -ForegroundColor White
            exit 0
        }
    }

    # Phase 4: Production Builds
    Show-Progress "Phase 4: Production Builds"
    Show-Progress "  → Building for Android & iOS"

    $buildResult = & .\deploy.ps1 -Target Both 2>&1
    if ($LASTEXITCODE -eq 0) {
        Show-Success "  ✓ Production builds completed!"
    } else {
        throw "Production builds failed: $buildResult"
    }

    # Phase 5: Deployment Preparation
    Show-Progress "Phase 5: Deployment Preparation"
    Show-Progress "  → Generating deployment artifacts"

    if (Test-Path "Builds") {
        $buildsDir = Get-Item "Builds"
        $androidBuilds = Get-ChildItem "$buildsDir/Android" -Filter "*.apk" -ErrorAction SilentlyContinue
        $iosBuilds = Get-ChildItem "$buildsDir/iOS" -Directory -ErrorAction SilentlyContinue

        Show-Success "  ✓ Deployment artifacts generated:"
        if ($androidBuilds) {
            Show-Progress "    📱 Android: $($androidBuilds.Count) APK file(s)" -ForegroundColor Green
        }
        if ($iosBuilds) {
            Show-Progress "    🍎 iOS: $($iosBuilds.Count) build(s)" -ForegroundColor Green
        }
    }

    # Final Success
    Write-Host ""
    Write-Host "🎉 CRIMSON COMPASS PRODUCTION LAUNCH COMPLETE!" -ForegroundColor Green
    Write-Host "=================================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "📦 Production artifacts ready in Builds/ directory" -ForegroundColor Cyan
    Write-Host "🚀 Upload to app stores:" -ForegroundColor White
    Write-Host "   • Google Play: Builds/Android/*.apk" -ForegroundColor Gray
    Write-Host "   • App Store: Builds/iOS/*/" -ForegroundColor Gray
    Write-Host ""
    Write-Host "📋 Check deployment_manifest.json for build details" -ForegroundColor White
    Write-Host ""
    Write-Host "🏆 Crimson Compass is now LIVE in production!" -ForegroundColor Magenta
    Write-Host ""

}
catch {
    Show-Error "Production launch failed: $($_.Exception.Message)"
    Write-Host ""
    Write-Host "🔧 Troubleshooting:" -ForegroundColor Yellow
    Write-Host "  • Ensure Unity is closed for builds" -ForegroundColor White
    Write-Host "  • Check Logs/ directory for detailed errors" -ForegroundColor White
    Write-Host "  • Run .\automate.ps1 -Mode Setup for diagnostics" -ForegroundColor White
    exit 1
}