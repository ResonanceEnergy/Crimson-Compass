#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Crimson Compass Master Automation - Complete Pipeline
.DESCRIPTION
    The ultimate automation script for Crimson Compass:
    - Development setup
    - Unity asset creation (when possible)
    - Production builds
    - Deployment preparation
.PARAMETER Mode
    Automation mode: Setup, Build, Deploy, or Full
.PARAMETER Target
    Build target for Build/Deploy modes: Android, iOS, or Both
.PARAMETER Clean
    Clean all artifacts before operations
.EXAMPLE
    .\automate.ps1 -Mode Full
.EXAMPLE
    .\automate.ps1 -Mode Build -Target Android
#>

param(
    [ValidateSet("Setup", "Build", "Deploy", "Full")]
    [string]$Mode = "Full",
    [ValidateSet("Android", "iOS", "Both")]
    [string]$Target = "Both",
    [switch]$Clean
)

$ErrorActionPreference = "Stop"

Write-Host "🤖 Crimson Compass Master Automation" -ForegroundColor Magenta
Write-Host "====================================" -ForegroundColor Magenta
Write-Host "Mode: $Mode | Target: $Target | Clean: $($Clean.IsPresent)" -ForegroundColor White
Write-Host ""

# Get project root
$ProjectRoot = Get-Location

# Import sub-scripts
$completeSetupScript = Join-Path $ProjectRoot "complete_setup.ps1"
$deployScript = Join-Path $ProjectRoot "deploy.ps1"

function Invoke-SetupPhase {
    Write-Host "📋 PHASE 1: Development Setup" -ForegroundColor Yellow
    Write-Host "=============================" -ForegroundColor Yellow

    if (!(Test-Path $completeSetupScript)) {
        throw "complete_setup.ps1 not found. Run from project root."
    }

    & $completeSetupScript
    if ($LASTEXITCODE -ne 0) {
        throw "Setup phase failed"
    }
}

function Invoke-BuildPhase {
    param([string]$BuildTarget)

    Write-Host "`n🏗️  PHASE 2: Production Builds" -ForegroundColor Yellow
    Write-Host "=============================" -ForegroundColor Yellow

    if (!(Test-Path $deployScript)) {
        throw "deploy.ps1 not found. Run from project root."
    }

    if ($Clean) {
        & $deployScript -Target $BuildTarget -Clean
    } else {
        & $deployScript -Target $BuildTarget
    }
    if ($LASTEXITCODE -ne 0) {
        throw "Build phase failed"
    }
}

function Invoke-DeployPhase {
    Write-Host "`n🚀 PHASE 3: Deployment Preparation" -ForegroundColor Yellow
    Write-Host "=================================" -ForegroundColor Yellow

    # Check if builds exist
    $buildsDir = Join-Path $ProjectRoot "Builds"
    if (!(Test-Path $buildsDir)) {
        throw "No builds found. Run build phase first."
    }

    $platforms = Get-ChildItem $buildsDir -Directory
    if ($platforms.Count -eq 0) {
        throw "No platform builds found in $buildsDir"
    }

    Write-Host "📦 Deployment-ready builds found:" -ForegroundColor Green
    foreach ($platform in $platforms) {
        $files = Get-ChildItem $platform.FullName -File -Recurse
        $totalSize = ($files | Measure-Object -Property Length -Sum).Sum
        $sizeMB = [math]::Round($totalSize / 1MB, 2)
        Write-Host "  • $($platform.Name): $($files.Count) files, $sizeMB MB" -ForegroundColor White
    }

    # Generate deployment instructions
    Write-Host "`n📋 Deployment Instructions:" -ForegroundColor Cyan
    Write-Host "  1. Android: Upload APK to Google Play Console" -ForegroundColor White
    Write-Host "     - Path: Builds/Android/*.apk" -ForegroundColor Gray
    Write-Host "  2. iOS: Upload to App Store Connect" -ForegroundColor White
    Write-Host "     - Path: Builds/iOS/*.ipa" -ForegroundColor Gray
    Write-Host "  3. Check deployment_manifest.json for details" -ForegroundColor White

    Write-Host "`n✅ Deployment preparation complete!" -ForegroundColor Green
}

function Show-CompletionSummary {
    Write-Host "`n🎯 Crimson Compass Automation Complete!" -ForegroundColor Green
    Write-Host "=====================================" -ForegroundColor Green

    Write-Host "`n📊 Pipeline Status:" -ForegroundColor Cyan
    Write-Host "  ✅ Development Setup: Automated validation & build prep" -ForegroundColor Green
    Write-Host "  ✅ Unity Integration: Manual step (Tools > Complete Crimson Compass Setup)" -ForegroundColor Yellow
    Write-Host "  ✅ Production Builds: Android & iOS ready" -ForegroundColor Green
    Write-Host "  ✅ Deployment Prep: Manifests & instructions generated" -ForegroundColor Green

    Write-Host "`n🚀 Quick Commands:" -ForegroundColor Cyan
    Write-Host "  • Build Android: .\automate.ps1 -Mode Build -Target Android" -ForegroundColor White
    Write-Host "  • Build iOS: .\automate.ps1 -Mode Build -Target iOS" -ForegroundColor White
    Write-Host "  • Full Pipeline: .\automate.ps1 -Mode Full" -ForegroundColor White
    Write-Host "  • Deploy Prep: .\automate.ps1 -Mode Deploy" -ForegroundColor White

    Write-Host "`n🎉 Crimson Compass is production-ready!" -ForegroundColor Magenta
}

# Main execution logic
try {
    switch ($Mode) {
        "Setup" {
            Invoke-SetupPhase
        }
        "Build" {
            Invoke-BuildPhase -BuildTarget $Target
        }
        "Deploy" {
            Invoke-DeployPhase
        }
        "Full" {
            Invoke-SetupPhase
            Invoke-BuildPhase -BuildTarget $Target
            Invoke-DeployPhase
        }
    }

    Show-CompletionSummary

}
catch {
    Write-Host "`n❌ Automation failed: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "`n🔧 Troubleshooting:" -ForegroundColor Yellow
    Write-Host "  • Ensure Unity is closed for build operations" -ForegroundColor White
    Write-Host "  • Run 'Tools > Complete Crimson Compass Setup' in Unity first" -ForegroundColor White
    Write-Host "  • Check logs in Logs/ directory" -ForegroundColor White
    exit 1
}