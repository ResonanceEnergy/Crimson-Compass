# Crimson Compass Grok Imagine + Unity Integration Workflow
# Complete pipeline from AI generation to Unity import

param(
    [string]$ProjectPath = ".",
    [string]$GeneratedAssetsPath = ".\GeneratedAssets",
    [string]$UnityAssetsPath = ".\Assets\Sprites",
    [switch]$SkipGeneration,
    [switch]$SkipUnityImport,
    [switch]$DryRun
)

# Configuration
$Config = @{
    UnityExecutable = "C:\Program Files\Unity\Hub\Editor\2021.3.16f1\Editor\Unity.exe"
    ProjectPath = $ProjectPath
    GeneratedPath = $GeneratedAssetsPath
    UnityAssetsPath = $UnityAssetsPath
    BackupPath = ".\Backups\$(Get-Date -Format 'yyyyMMdd_HHmmss')"
}

# Asset mapping for Unity import
$UnityAssetMapping = @{
    "Backgrounds" = "Sprites/Backgrounds"
    "Characters" = "Sprites/Characters"
    "Objects" = "Sprites/Objects"
    "UI" = "Sprites/UI"
    "Effects" = "Sprites/Effects"
    "Videos" = "Videos"  # Videos go to different folder
}

# Workflow phases
enum WorkflowPhase {
    Preparation
    AssetGeneration
    QualityCheck
    UnityImport
    Optimization
    Testing
    Completion
}

# Initialize workflow
function Initialize-Workflow {
    Write-Host "Crimson Compass Grok Imagine + Unity Workflow"
    Write-Host "============================================"

    # Validate paths
    if (!(Test-Path $Config.ProjectPath)) {
        Write-Error "Project path not found: $($Config.ProjectPath)"
        exit 1
    }

    if (!(Test-Path $Config.UnityExecutable)) {
        Write-Warning "Unity executable not found at: $($Config.UnityExecutable)"
        Write-Warning "Please update the path in the script configuration"
    }

    # Create backup
    if (!$DryRun -and !$SkipUnityImport) {
        Write-Host "Creating backup of current assets..."
        Create-Backup
    }

    Write-Host "Workflow initialized successfully"
}

# Create backup of existing assets
function Create-Backup {
    $BackupDir = $Config.BackupPath
    if (!(Test-Path $BackupDir)) {
        New-Item -ItemType Directory -Path $BackupDir -Force | Out-Null
    }

    # Backup Unity assets
    $UnityAssetsFullPath = Join-Path $Config.ProjectPath $Config.UnityAssetsPath
    if (Test-Path $UnityAssetsFullPath) {
        Copy-Item -Path $UnityAssetsFullPath -Destination $BackupDir -Recurse -Force
        Write-Host "Backup created: $BackupDir"
    }
}

# Phase 1: Asset Generation
function Start-AssetGeneration {
    Write-Host "`n=== Phase 1: Asset Generation ==="

    if ($SkipGeneration) {
        Write-Host "Skipping asset generation (requested)"
        return
    }

    $ScriptPath = Join-Path $PSScriptRoot "batch_generate_grok_assets.ps1"
    if (!(Test-Path $ScriptPath)) {
        Write-Error "Batch generation script not found: $ScriptPath"
        return
    }

    Write-Host "Running batch generation script..."
    if ($DryRun) {
        Write-Host "[DRY RUN] Would execute: $ScriptPath -OutputPath $($Config.GeneratedPath) -DryRun"
    } else {
        & $ScriptPath -OutputPath $Config.GeneratedPath
    }
}

# Phase 2: Quality Check
function Start-QualityCheck {
    Write-Host "`n=== Phase 2: Quality Check ==="

    $GeneratedPath = $Config.GeneratedPath
    if (!(Test-Path $GeneratedPath)) {
        Write-Warning "Generated assets path not found: $GeneratedPath"
        return
    }

    # Check file counts
    $Categories = Get-ChildItem -Path $GeneratedPath -Directory
    $TotalFiles = 0

    foreach ($Category in $Categories) {
        $Files = Get-ChildItem -Path $Category.FullName -File -Recurse
        $FileCount = $Files.Count
        $TotalFiles += $FileCount

        Write-Host "  $($Category.Name): $FileCount files"

        # Check file sizes (should be reasonable for mobile)
        $LargeFiles = $Files | Where-Object { $_.Length -gt 2MB }
        if ($LargeFiles) {
            Write-Warning "  Large files detected in $($Category.Name):"
            $LargeFiles | ForEach-Object {
                Write-Warning "    $($_.Name): $([math]::Round($_.Length / 1MB, 2)) MB"
            }
        }
    }

    Write-Host "Total generated files: $TotalFiles"

    # Expected counts
    $ExpectedCounts = @{
        "Backgrounds" = 12
        "Characters" = 15
        "Objects" = 20
        "UI" = 16
        "Effects" = 3
        "Videos" = 8
    }

    $TotalExpected = ($ExpectedCounts.Values | Measure-Object -Sum).Sum
    Write-Host "Expected total: $TotalExpected"

    if ($TotalFiles -lt $TotalExpected) {
        Write-Warning "Generated files ($TotalFiles) less than expected ($TotalExpected)"
        Write-Host "Consider re-running generation for missing assets"
    }
}

# Phase 3: Unity Import
function Start-UnityImport {
    Write-Host "`n=== Phase 3: Unity Import ==="

    if ($SkipUnityImport) {
        Write-Host "Skipping Unity import (requested)"
        return
    }

    $GeneratedPath = $Config.GeneratedPath
    $UnityAssetsPath = Join-Path $Config.ProjectPath $Config.UnityAssetsPath

    if (!(Test-Path $GeneratedPath)) {
        Write-Error "Generated assets not found: $GeneratedPath"
        return
    }

    # Create Unity directories
    foreach ($Mapping in $UnityAssetMapping.GetEnumerator()) {
        $UnityDir = Join-Path $UnityAssetsPath $Mapping.Value
        if (!(Test-Path $UnityDir)) {
            if (!$DryRun) {
                New-Item -ItemType Directory -Path $UnityDir -Force | Out-Null
            }
            Write-Host "Created Unity directory: $UnityDir"
        }
    }

    # Copy assets to Unity
    $Categories = Get-ChildItem -Path $GeneratedPath -Directory
    $ImportedCount = 0

    foreach ($Category in $Categories) {
        $SourcePath = $Category.FullName
        $UnityTarget = $UnityAssetMapping[$Category.Name]

        if ($UnityTarget) {
            $TargetPath = Join-Path $UnityAssetsPath $UnityTarget

            Write-Host "Importing $($Category.Name) to: $UnityTarget"

            if (!$DryRun) {
                # Copy files
                $Files = Get-ChildItem -Path $SourcePath -File -Recurse
                foreach ($File in $Files) {
                    $RelativePath = $File.FullName.Replace($SourcePath, "").TrimStart("\")
                    $DestFile = Join-Path $TargetPath $RelativePath
                    $DestDir = Split-Path $DestFile -Parent

                    if (!(Test-Path $DestDir)) {
                        New-Item -ItemType Directory -Path $DestDir -Force | Out-Null
                    }

                    Copy-Item -Path $File.FullName -Destination $DestFile -Force
                    $ImportedCount++
                }
            } else {
                Write-Host "[DRY RUN] Would copy files from $SourcePath to $TargetPath"
            }
        }
    }

    Write-Host "Imported $ImportedCount assets to Unity"

    # Generate Unity import script
    if (!$DryRun) {
        Generate-UnityImportScript -ImportedCount $ImportedCount
    }
}

# Generate Unity editor script for asset processing
function Generate-UnityImportScript {
    param([int]$ImportedCount)

    $ScriptPath = Join-Path $Config.ProjectPath "Assets\Editor\CrimsonCompassAssetImporter.cs"

    $ScriptContent = @"
// Crimson Compass Asset Importer
// Automatically processes imported Grok Imagine assets

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class CrimsonCompassAssetImporter : AssetPostprocessor
{
    private static readonly string[] TextureFolders = {
        "Assets/Sprites/Backgrounds",
        "Assets/Sprites/Characters",
        "Assets/Sprites/Objects",
        "Assets/Sprites/UI",
        "Assets/Sprites/Effects"
    };

    private void OnPreprocessTexture()
    {
        TextureImporter importer = assetImporter as TextureImporter;

        // Check if this is a Crimson Compass asset
        bool isCrimsonAsset = TextureFolders.Any(folder =>
            assetPath.StartsWith(folder));

        if (isCrimsonAsset)
        {
            // Configure for mobile optimization
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Bilinear;
            importer.textureCompression = TextureImporterCompression.Compressed;

            // Set compression based on folder
            if (assetPath.Contains("Backgrounds"))
            {
                importer.maxTextureSize = 2048;
                importer.compressionQuality = 50;
            }
            else if (assetPath.Contains("Characters") || assetPath.Contains("Objects"))
            {
                importer.maxTextureSize = 1024;
                importer.compressionQuality = 75;
            }
            else
            {
                importer.maxTextureSize = 512;
                importer.compressionQuality = 80;
            }

            Debug.Log($"Processed Crimson Compass asset: {assetPath}");
        }
    }

    [MenuItem("Crimson Compass/Process Imported Assets")]
    private static void ProcessImportedAssets()
    {
        AssetDatabase.Refresh();
        Debug.Log("Crimson Compass assets processed and optimized for mobile");
    }

    [MenuItem("Crimson Compass/Generate Asset Report")]
    private static void GenerateAssetReport()
    {
        string report = "Crimson Compass Asset Import Report\n";
        report += "==================================\n\n";

        foreach (string folder in TextureFolders)
        {
            if (Directory.Exists(folder))
            {
                string[] assets = Directory.GetFiles(folder, "*.png", SearchOption.AllDirectories);
                report += $"{Path.GetFileName(folder)}: {assets.Length} assets\n";
            }
        }

        string reportPath = "Assets/CrimsonCompass_Asset_Report.txt";
        File.WriteAllText(reportPath, report);
        AssetDatabase.Refresh();

        Debug.Log($"Asset report generated: {reportPath}");
    }
}
"@

    if (!(Test-Path (Split-Path $ScriptPath -Parent))) {
        New-Item -ItemType Directory -Path (Split-Path $ScriptPath -Parent) -Force | Out-Null
    }

    $ScriptContent | Out-File -FilePath $ScriptPath -Encoding UTF8
    Write-Host "Generated Unity import script: $ScriptPath"
}

# Phase 4: Optimization
function Start-Optimization {
    Write-Host "`n=== Phase 4: Optimization ==="

    # Run Unity asset processing
    if (!$DryRun -and (Test-Path $Config.UnityExecutable)) {
        Write-Host "Running Unity asset optimization..."

        $UnityArgs = @(
            "-projectPath", $Config.ProjectPath,
            "-executeMethod", "CrimsonCompassAssetImporter.ProcessImportedAssets",
            "-quit"
        )

        if (!$DryRun) {
            & $Config.UnityExecutable $UnityArgs
        } else {
            Write-Host "[DRY RUN] Would execute: $($Config.UnityExecutable) $($UnityArgs -join ' ')"
        }
    }
}

# Phase 5: Testing
function Start-Testing {
    Write-Host "`n=== Phase 5: Testing ==="

    # Basic validation tests
    $UnityAssetsPath = Join-Path $Config.ProjectPath $Config.UnityAssetsPath

    if (Test-Path $UnityAssetsPath) {
        $TotalAssets = (Get-ChildItem -Path $UnityAssetsPath -File -Recurse | Measure-Object).Count
        Write-Host "Unity assets directory contains: $TotalAssets files"

        # Check for required categories
        foreach ($Mapping in $UnityAssetMapping.GetEnumerator()) {
            $Path = Join-Path $UnityAssetsPath $Mapping.Value
            if (Test-Path $Path) {
                $Count = (Get-ChildItem -Path $Path -File -Recurse | Measure-Object).Count
                Write-Host "  $($Mapping.Key): $Count assets"
            } else {
                Write-Warning "Missing category directory: $($Mapping.Value)"
            }
        }
    }

    Write-Host "Basic validation completed"
}

# Main workflow execution
function Main {
    $Phases = @(
        @{ Name = "Preparation"; Function = ${function:Initialize-Workflow} },
        @{ Name = "Asset Generation"; Function = ${function:Start-AssetGeneration} },
        @{ Name = "Quality Check"; Function = ${function:Start-QualityCheck} },
        @{ Name = "Unity Import"; Function = ${function:Start-UnityImport} },
        @{ Name = "Optimization"; Function = ${function:Start-Optimization} },
        @{ Name = "Testing"; Function = ${function:Start-Testing} }
    )

    $StartTime = Get-Date
    Write-Host "Starting Crimson Compass workflow at $StartTime"

    foreach ($Phase in $Phases) {
        try {
            & $Phase.Function
        } catch {
            Write-Error "Error in phase '$($Phase.Name)': $($_.Exception.Message)"
            if (!$DryRun) {
                Write-Host "Workflow interrupted. Check logs for details."
                exit 1
            }
        }
    }

    $EndTime = Get-Date
    $Duration = $EndTime - $StartTime

    Write-Host "`n=== Workflow Complete ==="
    Write-Host "Duration: $($Duration.TotalMinutes.ToString("F2")) minutes"
    Write-Host "Started: $StartTime"
    Write-Host "Completed: $EndTime"

    if (!$DryRun) {
        Write-Host "`nNext steps:"
        Write-Host "1. Open Unity project and check imported assets"
        Write-Host "2. Run: Crimson Compass > Generate Asset Report (in Unity)"
        Write-Host "3. Test assets in your game scenes"
        Write-Host "4. Adjust prompts if needed and re-run workflow"
    }
}

# Run main workflow
Main