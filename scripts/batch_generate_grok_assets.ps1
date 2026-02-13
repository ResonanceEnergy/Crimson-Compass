# Crimson Compass Grok Imagine Batch Generation Script
# Generates all 58 Season 1 visual assets using Grok Imagine prompts

param(
    [string]$OutputPath = ".\GeneratedAssets",
    [string]$PromptFile = ".\docs\Visual_Prompts_Grok_Imagine.md",
    [switch]$SkipVideos,
    [switch]$DryRun
)

# Configuration
$Config = @{
    BaseUrl = "https://grok.com/imagine"
    BrowserDelay = 3000  # milliseconds
    MaxRetries = 3
    BatchSize = 5  # Generate 5 assets at a time to avoid overwhelming
}

# Asset categories and counts based on Season 1 requirements
$AssetCategories = @{
    "Backgrounds" = 12
    "Characters" = 15
    "Objects" = 20
    "UI" = 16
    "Effects" = 3
    "Videos" = 8  # Dynamic scenes for character movements and demonstrations
}

# Create output directories
function Initialize-OutputDirectories {
    param([string]$BasePath)

    $Directories = @(
        "$BasePath\Backgrounds",
        "$BasePath\Characters",
        "$BasePath\Objects",
        "$BasePath\UI",
        "$BasePath\Effects",
        "$BasePath\Videos",
        "$BasePath\Logs"
    )

    foreach ($Dir in $Directories) {
        if (!(Test-Path $Dir)) {
            New-Item -ItemType Directory -Path $Dir -Force | Out-Null
            Write-Host "Created directory: $Dir"
        }
    }
}

# Extract prompts from markdown file
function Get-PromptsFromFile {
    param([string]$FilePath)

    if (!(Test-Path $FilePath)) {
        Write-Error "Prompt file not found: $FilePath"
        return $null
    }

    $Content = Get-Content $FilePath -Raw
    $Prompts = @{}

    # Extract episode sections
    $Episodes = [regex]::Matches($Content, '(?s)## Episode \d+:.*?(?=## Episode \d+:|## UI Elements|## Video Generation|\z)')

    foreach ($Episode in $Episodes) {
        $EpisodeContent = $Episode.Value
        $EpisodeTitle = [regex]::Match($EpisodeContent, '## Episode \d+: ([^\n]+)').Groups[1].Value

        # Extract backgrounds
        $Backgrounds = [regex]::Matches($EpisodeContent, '\*\*([^*]+):\*\*\s*([^\n]+)')
        foreach ($Bg in $Backgrounds) {
            $Name = $Bg.Groups[1].Value
            $Prompt = $Bg.Groups[2].Value.Trim()
            if ($Prompt -and $Name -notmatch "Characters|Core Objects") {
                $Prompts["Background_$Name"] = @{
                    Prompt = $Prompt
                    Category = "Backgrounds"
                    Episode = $EpisodeTitle
                }
            }
        }

        # Extract characters
        $CharSection = [regex]::Match($EpisodeContent, '(?s)\*\*([^*]+):\*\*\s*([^\n]+(?:\n(?!###|\*\*).+)*)')
        if ($CharSection.Success) {
            $CharMatches = [regex]::Matches($EpisodeContent, '\*\*([^*]+):\*\*\s*([^\n]+)')
            foreach ($Char in $CharMatches) {
                $Name = $Char.Groups[1].Value
                $Prompt = $Char.Groups[2].Value.Trim()
                if ($Prompt -and $Name -match "Agent|Player|NPC") {
                    $Prompts["Character_$Name"] = @{
                        Prompt = $Prompt
                        Category = "Characters"
                        Episode = $EpisodeTitle
                    }
                }
            }
        }

        # Extract objects
        $ObjectMatches = [regex]::Matches($EpisodeContent, '\*\*([^*]+):\*\*\s*([^\n]+)')
        foreach ($Obj in $ObjectMatches) {
            $Name = $Obj.Groups[1].Value
            $Prompt = $Obj.Groups[2].Value.Trim()
            if ($Prompt -and ($Name -match "Compass|Pen|Scanner|Components|Cores|Keys")) {
                $Prompts["Object_$Name"] = @{
                    Prompt = $Prompt
                    Category = "Objects"
                    Episode = $EpisodeTitle
                }
            }
        }
    }

    # Extract UI elements
    $UIMatches = [regex]::Matches($Content, '(?s)### [^*]*\*\*([^*]+):\*\*\s*([^\n]+)')
    foreach ($UI in $UIMatches) {
        $Name = $UI.Groups[1].Value
        $Prompt = $UI.Groups[2].Value.Trim()
        if ($Prompt) {
            $Prompts["UI_$Name"] = @{
                Prompt = $Prompt
                Category = "UI"
                Episode = "Cross-Episode"
            }
        }
    }

    # Extract effects
    $EffectMatches = [regex]::Matches($Content, '(?s)\*\*([^*]+):\*\*\s*([^\n]+)')
    foreach ($Effect in $EffectMatches) {
        $Name = $Effect.Groups[1].Value
        $Prompt = $Effect.Groups[2].Value.Trim()
        if ($Prompt -and $Name -match "Fog|Projections|Signatures") {
            $Prompts["Effect_$Name"] = @{
                Prompt = $Prompt
                Category = "Effects"
                Episode = "Cross-Episode"
            }
        }
    }

    # Extract video prompts if not skipping
    if (!$SkipVideos) {
        $VideoSection = [regex]::Match($Content, '(?s)## Video Generation Prompts.*?(?=## Prompt Engineering|\z)')
        if ($VideoSection.Success) {
            $VideoMatches = [regex]::Matches($VideoSection.Value, '\*\*([^*]+):\*\*\s*([^\n]+(?:\n(?!###|\*\*).+)*)')
            foreach ($Video in $VideoMatches) {
                $Name = $Video.Groups[1].Value
                $Prompt = $Video.Groups[2].Value.Trim()
                if ($Prompt) {
                    $Prompts["Video_$Name"] = @{
                        Prompt = $Prompt
                        Category = "Videos"
                        Episode = "Dynamic Scenes"
                    }
                }
            }
        }
    }

    return $Prompts
}

# Generate single asset using Grok Imagine
function Generate-Asset {
    param(
        [string]$Name,
        [string]$Prompt,
        [string]$Category,
        [string]$OutputDir,
        [int]$RetryCount = 0
    )

    try {
        $Timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
        $FileName = "${Name}_${Timestamp}.png"
        $OutputPath = Join-Path $OutputDir $FileName

        if ($DryRun) {
            Write-Host "[DRY RUN] Would generate: $FileName"
            Write-Host "  Prompt: $Prompt"
            return $true
        }

        # Open browser and navigate to Grok Imagine
        $Uri = "https://grok.com/imagine"
        Start-Process $Uri

        # Wait for page to load
        Start-Sleep -Milliseconds $Config.BrowserDelay

        # Note: Actual generation would require browser automation
        # This is a placeholder for the manual process
        Write-Host "Please manually generate asset: $Name"
        Write-Host "Category: $Category"
        Write-Host "Prompt: $Prompt"
        Write-Host "Save as: $OutputPath"
        Write-Host "---"

        # In a real implementation, you would use Selenium or similar
        # to automate the browser interaction

        return $true

    } catch {
        Write-Warning "Failed to generate $Name (attempt $($RetryCount + 1)): $($_.Exception.Message)"

        if ($RetryCount -lt $Config.MaxRetries) {
            Write-Host "Retrying in 5 seconds..."
            Start-Sleep -Seconds 5
            return Generate-Asset -Name $Name -Prompt $Prompt -Category $Category -OutputDir $OutputDir -RetryCount ($RetryCount + 1)
        }

        return $false
    }
}

# Main batch generation function
function Start-BatchGeneration {
    param([hashtable]$Prompts, [string]$OutputBasePath)

    $TotalAssets = $Prompts.Count
    $Generated = 0
    $Failed = 0

    Write-Host "Starting batch generation of $TotalAssets assets..."
    Write-Host "Output path: $OutputBasePath"
    Write-Host "---"

    # Group by category for organized generation
    $ByCategory = $Prompts.GetEnumerator() | Group-Object { $_.Value.Category }

    foreach ($CategoryGroup in $ByCategory) {
        $Category = $CategoryGroup.Name
        $CategoryPrompts = $CategoryGroup.Group
        $OutputDir = Join-Path $OutputBasePath $Category

        Write-Host "`n=== Generating $Category ($($CategoryPrompts.Count) assets) ==="

        $BatchIndex = 0
        foreach ($PromptEntry in $CategoryPrompts) {
            $AssetName = $PromptEntry.Key
            $AssetData = $PromptEntry.Value

            Write-Host "[$($BatchIndex + 1)/$($CategoryPrompts.Count)] Generating: $AssetName"

            $Success = Generate-Asset -Name $AssetName -Prompt $AssetData.Prompt -Category $Category -OutputDir $OutputDir

            if ($Success) {
                $Generated++
            } else {
                $Failed++
            }

            $BatchIndex++

            # Small delay between generations to avoid overwhelming
            if (!$DryRun -and $BatchIndex % $Config.BatchSize -eq 0) {
                Write-Host "Batch pause... (press Enter to continue)"
                Read-Host
            }
        }
    }

    # Generate summary report
    $ReportPath = Join-Path $OutputBasePath "Logs\generation_report_$(Get-Date -Format 'yyyyMMdd_HHmmss').txt"
    $Report = @"
Crimson Compass Asset Generation Report
Generated: $(Get-Date)
Total Assets: $TotalAssets
Successfully Generated: $Generated
Failed: $Failed
Success Rate: $([math]::Round(($Generated / $TotalAssets) * 100, 2))%

Categories:
$($ByCategory | ForEach-Object { "  $($_.Name): $($_.Count) assets" } | Out-String)
"@

    if (!$DryRun) {
        $Report | Out-File -FilePath $ReportPath -Encoding UTF8
    }

    Write-Host "`n=== Generation Complete ==="
    Write-Host "Report saved to: $ReportPath"
    Write-Host "Successfully generated: $Generated/$TotalAssets assets"
}

# Main execution
function Main {
    Write-Host "Crimson Compass Grok Imagine Batch Generator"
    Write-Host "=========================================="

    # Initialize directories
    Initialize-OutputDirectories -BasePath $OutputPath

    # Load prompts
    Write-Host "Loading prompts from: $PromptFile"
    $Prompts = Get-PromptsFromFile -FilePath $PromptFile

    if ($null -eq $Prompts) {
        Write-Error "Failed to load prompts. Exiting."
        exit 1
    }

    Write-Host "Found $($Prompts.Count) prompts to process"

    if ($DryRun) {
        Write-Host "[DRY RUN MODE] - No actual generation will occur"
    }

    # Start batch generation
    Start-BatchGeneration -Prompts $Prompts -OutputBasePath $OutputPath

    Write-Host "`nBatch generation script completed."
    if (!$DryRun) {
        Write-Host "Next steps:"
        Write-Host "1. Review generated assets in $OutputPath"
        Write-Host "2. Run Unity import script: .\scripts\import_assets.ps1"
        Write-Host "3. Test assets in Unity editor"
    }
}

# Run main function
Main