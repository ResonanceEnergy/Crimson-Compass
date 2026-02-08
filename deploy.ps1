#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Crimson Compass Production Deployment Automation
.DESCRIPTION
    Automates the complete production deployment process after Unity setup:
    - Final validation
    - Production builds for Android and iOS
    - Build artifact management
    - Deployment preparation
.PARAMETER Target
    Build target: Android, iOS, or Both
.PARAMETER Clean
    Clean build artifacts before building
.EXAMPLE
    .\deploy.ps1 -Target Both
.EXAMPLE
    .\deploy.ps1 -Target Android -Clean
#>

param(
    [ValidateSet("Android", "iOS", "Both")]
    [string]$Target = "Both",
    [switch]$Clean
)

$ErrorActionPreference = "Stop"

Write-Host "🚀 Crimson Compass Production Deployment" -ForegroundColor Green
Write-Host "=========================================" -ForegroundColor Green

# Get project root
$ProjectRoot = Get-Location
$ToolsDir = Join-Path $ProjectRoot "Tools"
$BuildsDir = Join-Path $ProjectRoot "Builds"

# Function to clean build artifacts
function Clear-BuildArtifacts {
    Write-Host "`n🧹 Cleaning build artifacts..." -ForegroundColor Yellow

    if (Test-Path $BuildsDir) {
        Remove-Item $BuildsDir -Recurse -Force
        Write-Host "✅ Build directory cleaned" -ForegroundColor Green
    }

    # Clean Unity build artifacts
    $artifacts = @(
        "Library/Artifacts",
        "Library/il2cpp",
        "Library/ScriptAssemblies",
        "Temp",
        "obj"
    )

    foreach ($artifact in $artifacts) {
        $path = Join-Path $ProjectRoot $artifact
        if (Test-Path $path) {
            Remove-Item $path -Recurse -Force
            Write-Host "✅ Cleaned $artifact" -ForegroundColor Green
        }
    }
}

# Function to run final validation
function Test-FinalValidation {
    Write-Host "`n📋 Running final validation..." -ForegroundColor Yellow

    # Run episode data validation
    & python (Join-Path $ToolsDir "validate_episode_data.py")
    if ($LASTEXITCODE -ne 0) {
        throw "Episode data validation failed"
    }

    # Run audio validation
    & python (Join-Path $ToolsDir "validate_audio_events.py")
    if ($LASTEXITCODE -ne 0) {
        throw "Audio validation failed"
    }

    Write-Host "✅ All validations passed!" -ForegroundColor Green
}

# Function to build target
function Invoke-ProductionBuild {
    param([string]$BuildTarget)

    Write-Host "`n🏗️  Building for $BuildTarget..." -ForegroundColor Cyan

    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $targetDir = Join-Path $BuildsDir $BuildTarget

    # Create build directory
    New-Item -ItemType Directory -Force -Path $targetDir | Out-Null

    # Run build
    & python (Join-Path $ToolsDir "build.py") --target $BuildTarget
    if ($LASTEXITCODE -ne 0) {
        throw "$BuildTarget build failed"
    }

    # Check if build artifact exists
    $buildFiles = Get-ChildItem $targetDir -File
    if ($buildFiles.Count -eq 0) {
        throw "No build artifacts found in $targetDir"
    }

    Write-Host "✅ $BuildTarget build completed successfully!" -ForegroundColor Green
    Write-Host "📦 Build artifacts: $($buildFiles.Count) files" -ForegroundColor White

    # Show build size
    $totalSize = ($buildFiles | Measure-Object -Property Length -Sum).Sum
    $sizeMB = [math]::Round($totalSize / 1MB, 2)
    Write-Host "📊 Total size: $sizeMB MB" -ForegroundColor White
}

# Function to generate deployment manifest
function New-DeploymentManifest {
    Write-Host "`n📋 Generating deployment manifest..." -ForegroundColor Yellow

    $manifest = @{
        project = "Crimson Compass"
        version = "1.0.0"
        build_date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
        platforms = @()
        validation = @{
            episode_data = "passed"
            audio_system = "passed"
        }
    }

    # Check build artifacts
    if (Test-Path $BuildsDir) {
        $platforms = Get-ChildItem $BuildsDir -Directory
        foreach ($platform in $platforms) {
            $files = Get-ChildItem $platform.FullName -File -Recurse
            $totalSize = ($files | Measure-Object -Property Length -Sum).Sum

            $platformInfo = @{
                name = $platform.Name
                file_count = $files.Count
                total_size_bytes = $totalSize
                total_size_mb = [math]::Round($totalSize / 1MB, 2)
                build_path = $platform.FullName
            }
            $manifest.platforms += $platformInfo
        }
    }

    # Save manifest
    $manifestPath = Join-Path $BuildsDir "deployment_manifest.json"
    $manifest | ConvertTo-Json -Depth 10 | Out-File $manifestPath -Encoding UTF8

    Write-Host "✅ Deployment manifest created: $manifestPath" -ForegroundColor Green
}

# Function to show deployment summary
function Show-DeploymentSummary {
    Write-Host "`n🎉 Crimson Compass Production Deployment Complete!" -ForegroundColor Green
    Write-Host "==================================================" -ForegroundColor Green

    if (Test-Path $BuildsDir) {
        Write-Host "`n📦 Build Artifacts:" -ForegroundColor Cyan
        $platforms = Get-ChildItem $BuildsDir -Directory
        foreach ($platform in $platforms) {
            $files = Get-ChildItem $platform.FullName -File -Recurse
            $totalSize = ($files | Measure-Object -Property Length -Sum).Sum
            $sizeMB = [math]::Round($totalSize / 1MB, 2)
            Write-Host "  • $($platform.Name): $($files.Count) files, $sizeMB MB" -ForegroundColor White
        }
    }

    Write-Host "`n🚀 Ready for deployment!" -ForegroundColor Green
    Write-Host "  • Upload APK to Google Play Store" -ForegroundColor White
    Write-Host "  • Upload IPA to Apple App Store" -ForegroundColor White
    Write-Host "  • Check deployment_manifest.json for details" -ForegroundColor White
}

# Main execution
try {
    # Clean if requested
    if ($Clean) {
        Clear-BuildArtifacts
    }

    # Final validation
    Test-FinalValidation

    # Build targets
    if ($Target -eq "Both" -or $Target -eq "Android") {
        Invoke-ProductionBuild -BuildTarget "Android"
    }

    if ($Target -eq "Both" -or $Target -eq "iOS") {
        Invoke-ProductionBuild -BuildTarget "iOS"
    }

    # Generate manifest
    New-DeploymentManifest

    # Show summary
    Show-DeploymentSummary

}
catch {
    Write-Host "`n❌ Deployment failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}