# Crimson Compass Episode-by-Episode Visual Generator
# Automated generation for each Season 1 episode

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("S01E01", "S01E02", "S01E03", "S01E04", "S01E05", "S01E06", "S01E07", "S01E08", "S01E09", "S01E10", "S01E11", "S01E12", "ALL")]
    [string]$Episode,

    [string]$OutputPath = ".\GeneratedAssets",
    [int]$VariationsPerAsset = 3,
    [switch]$IncludeVideos,
    [switch]$DryRun
)

# Episode configurations
$EpisodeConfigs = @{
    "S01E01" = @{
        Name = "Welcome Packet"
        Theme = "Corporate Contractor Access Ring"
        Assets = @(
            @{ Type = "Background"; Name = "Urban_Rooftop_Stakeout"; Variations = 3 }
            @{ Type = "Background"; Name = "Warehouse_District"; Variations = 3 }
            @{ Type = "Character"; Name = "Player_Compass_Master"; Variations = 4 }
            @{ Type = "Character"; Name = "Optimus_Agent_Y"; Variations = 3 }
            @{ Type = "Character"; Name = "Helix_Agent_X"; Variations = 3 }
            @{ Type = "Character"; Name = "ZTech_Agent_Z"; Variations = 4 }
            @{ Type = "Character"; Name = "Gasket_Agent_G"; Variations = 3 }
            @{ Type = "Object"; Name = "Crimson_Compass"; Variations = 3 }
            @{ Type = "Object"; Name = "Multi_Tool_Pen"; Variations = 4 }
        )
    }
    "S01E02" = @{
        Name = "Badge & Borrow"
        Theme = "Social Engineering Hardware Keys"
        Assets = @(
            @{ Type = "Background"; Name = "Corporate_Lobby"; Variations = 3 }
            @{ Type = "Background"; Name = "Executive_Office"; Variations = 3 }
            @{ Type = "Character"; Name = "Corporate_Executive"; Variations = 3 }
            @{ Type = "Character"; Name = "IT_Administrator"; Variations = 3 }
            @{ Type = "Object"; Name = "Hardware_Keys"; Variations = 3 }
            @{ Type = "Object"; Name = "Badge_Scanner"; Variations = 3 }
        )
    }
    "S01E03" = @{
        Name = "Clean Room"
        Theme = "Insider Micro-Component Smuggling"
        Assets = @(
            @{ Type = "Background"; Name = "Clean_Room"; Variations = 3 }
            @{ Type = "Background"; Name = "Maintenance_Corridor"; Variations = 3 }
            @{ Type = "Character"; Name = "Facility_Manager"; Variations = 3 }
            @{ Type = "Character"; Name = "Insider_Culprit"; Variations = 4 }
            @{ Type = "Object"; Name = "Micro_Components"; Variations = 3 }
            @{ Type = "Object"; Name = "Contamination_Detector"; Variations = 4 }
        )
    }
    "S01E04" = @{
        Name = "Update Never Shipped"
        Theme = "Air-Gapped Firmware Hijacking"
        Assets = @(
            @{ Type = "Background"; Name = "Server_Room"; Variations = 3 }
            @{ Type = "Background"; Name = "Operations_Center"; Variations = 3 }
            @{ Type = "Character"; Name = "IT_Director"; Variations = 3 }
            @{ Type = "Character"; Name = "Hacker_Culprit"; Variations = 4 }
            @{ Type = "Object"; Name = "Staging_Server"; Variations = 3 }
            @{ Type = "Object"; Name = "Network_Scanner"; Variations = 4 }
        )
    }
    "S01E05" = @{
        Name = "Hostile Compliance"
        Theme = "Legal Warfare Bureaucratic Obstruction"
        Assets = @(
            @{ Type = "Background"; Name = "Corporate_Boardroom"; Variations = 3 }
            @{ Type = "Background"; Name = "Legal_Library"; Variations = 3 }
            @{ Type = "Character"; Name = "Corporate_Counsel"; Variations = 3 }
            @{ Type = "Character"; Name = "Legal_Assistant"; Variations = 3 }
            @{ Type = "Character"; Name = "Judge_Virtual"; Variations = 3 }
            @{ Type = "Object"; Name = "Legal_Documents"; Variations = 3 }
            @{ Type = "Object"; Name = "Precedent_Scanner"; Variations = 3 }
        )
    }
    "S01E06" = @{
        Name = "Double Stamp"
        Theme = "Subcontractor Cell Prototype Theft"
        Assets = @(
            @{ Type = "Background"; Name = "Warehouse_District"; Variations = 3 }
            @{ Type = "Background"; Name = "Subcontractor_Facility"; Variations = 3 }
            @{ Type = "Character"; Name = "Subcontractor_Leader"; Variations = 3 }
            @{ Type = "Character"; Name = "Warehouse_Security"; Variations = 3 }
            @{ Type = "Object"; Name = "Sensor_Module"; Variations = 3 }
            @{ Type = "Object"; Name = "Custody_Forms"; Variations = 3 }
        )
    }
    "S01E07" = @{
        Name = "Convenient Suspect"
        Theme = "Decoy Culprit False Flag Frame"
        Assets = @(
            @{ Type = "Background"; Name = "Interrogation_Room"; Variations = 3 }
            @{ Type = "Background"; Name = "Evidence_Laboratory"; Variations = 3 }
            @{ Type = "Character"; Name = "Decoy_Suspect"; Variations = 3 }
            @{ Type = "Character"; Name = "Real_Culprit"; Variations = 3 }
            @{ Type = "Character"; Name = "Forensic_Tech"; Variations = 3 }
            @{ Type = "Object"; Name = "False_Evidence"; Variations = 3 }
            @{ Type = "Object"; Name = "Lie_Detector"; Variations = 3 }
        )
    }
    "S01E08" = @{
        Name = "Custody"
        Theme = "Encrypted Design Files Digital Security"
        Assets = @(
            @{ Type = "Background"; Name = "Secure_Vault"; Variations = 3 }
            @{ Type = "Background"; Name = "Data_Center"; Variations = 3 }
            @{ Type = "Character"; Name = "Security_Director"; Variations = 3 }
            @{ Type = "Character"; Name = "Encryption_Expert"; Variations = 3 }
            @{ Type = "Character"; Name = "Legal_Counsel"; Variations = 3 }
            @{ Type = "Object"; Name = "Encrypted_Files"; Variations = 3 }
            @{ Type = "Object"; Name = "Quantum_Decryptor"; Variations = 3 }
        )
    }
    "S01E09" = @{
        Name = "Exit Wound"
        Theme = "Physical Security Extraction Operations"
        Assets = @(
            @{ Type = "Background"; Name = "Facility_Corridors"; Variations = 3 }
            @{ Type = "Background"; Name = "Security_Checkpoint"; Variations = 3 }
            @{ Type = "Character"; Name = "Extraction_Leader"; Variations = 3 }
            @{ Type = "Character"; Name = "Facility_Director"; Variations = 3 }
            @{ Type = "Object"; Name = "Neural_Interface_Chip"; Variations = 3 }
            @{ Type = "Object"; Name = "Security_Countermeasures"; Variations = 3 }
        )
    }
    "S01E10" = @{
        Name = "Blow Cover"
        Theme = "Deep Infiltration Catastrophic Choices"
        Assets = @(
            @{ Type = "Background"; Name = "Corporate_Security_Center"; Variations = 3 }
            @{ Type = "Background"; Name = "Alliance_Negotiation_Room"; Variations = 3 }
            @{ Type = "Character"; Name = "Ring_Leader"; Variations = 3 }
            @{ Type = "Character"; Name = "Corrupt_Executive"; Variations = 3 }
            @{ Type = "Object"; Name = "Network_Mapping_Display"; Variations = 3 }
            @{ Type = "Object"; Name = "Deception_Detector"; Variations = 3 }
        )
    }
    "S01E11" = @{
        Name = "Safe House"
        Theme = "Evidence Preservation Cleanup Operations"
        Assets = @(
            @{ Type = "Background"; Name = "Safe_House_Interior"; Variations = 3 }
            @{ Type = "Background"; Name = "Evidence_Storage"; Variations = 3 }
            @{ Type = "Character"; Name = "Cleanup_Leader"; Variations = 3 }
            @{ Type = "Character"; Name = "Legal_Observer"; Variations = 3 }
            @{ Type = "Object"; Name = "Evidence_Containers"; Variations = 3 }
            @{ Type = "Object"; Name = "Quantum_Verifier"; Variations = 3 }
        )
    }
    "S01E12" = @{
        Name = "Hindsight"
        Theme = "Season Culmination Pattern Revelation"
        Assets = @(
            @{ Type = "Background"; Name = "Final_Confrontation_Chamber"; Variations = 3 }
            @{ Type = "Background"; Name = "Prototype_Vault"; Variations = 3 }
            @{ Type = "Character"; Name = "Final_Culprit"; Variations = 3 }
            @{ Type = "Character"; Name = "Agency_Director"; Variations = 3 }
            @{ Type = "Object"; Name = "Core_Prototype_Module"; Variations = 3 }
            @{ Type = "Object"; Name = "Pattern_Synthesis_Display"; Variations = 3 }
        )
    }
}

# Load prompts from the Grok Imagine file
function Get-PromptsFromFile {
    $PromptFile = ".\docs\Visual_Prompts_Grok_Imagine.md"
    if (!(Test-Path $PromptFile)) {
        Write-Error "Prompt file not found: $PromptFile"
        exit 1
    }

    $Content = Get-Content $PromptFile -Raw
    $Prompts = @{}

    # Extract episode sections
    $EpisodeSections = [regex]::Matches($Content, '(?s)## Episode \d+:.*?(?=## Episode \d+:|## UI Elements|## Video Generation|\z)')

    foreach ($Section in $EpisodeSections) {
        $SectionContent = $Section.Value
        $EpisodeMatch = [regex]::Match($SectionContent, '## Episode (\d+):')
        if ($EpisodeMatch.Success) {
            $EpisodeNum = "S01E$($EpisodeMatch.Groups[1].Value.PadLeft(2,'0'))"

            # Extract all prompts from this section - look for **Name:** pattern
            $PromptMatches = [regex]::Matches($SectionContent, '\*\*([^*]+):\*\*\s*([^\n]+(?:\n(?!###|\*\*).+)*)')

            foreach ($Match in $PromptMatches) {
                $AssetName = $Match.Groups[1].Value.Trim()
                $Prompt = $Match.Groups[2].Value.Trim()

                if ($Prompt -and $AssetName) {
                    # Create a mapping from file names to config names
                    $NameMapping = @{
                        # Episode 1
                        "Urban Rooftop Stakeout" = "Urban_Rooftop_Stakeout"
                        "Abandoned Warehouse District" = "Warehouse_District"
                        "Warehouse Interior" = "Warehouse_Interior"
                        "Player (Compass Master)" = "Player_Compass_Master"
                        "Optimus (Agent Y)" = "Optimus_Agent_Y"
                        "Helix (Agent X)" = "Helix_Agent_X"
                        "ZTech (Agent Z)" = "ZTech_Agent_Z"
                        "Gasket (Agent G)" = "Gasket_Agent_G"
                        "Ring Leader (NPC)" = "Ring_Leader_NPC"
                        "Crimson Compass" = "Crimson_Compass"
                        "Prototype Components" = "Prototype_Components"
                        "Encrypted Data Cores" = "Encrypted_Data_Cores"
                        "ZTech Multi-Tool Pen" = "Multi_Tool_Pen"
                        "Thermal Scanner" = "Thermal_Scanner"
                        "Dialogue System" = "Dialogue_System"
                        "Evidence Display" = "Evidence_Display"
                        # Episode 2
                        "Corporate Lobby" = "Corporate_Lobby"
                        "Executive Office" = "Executive_Office"
                        "Security Center" = "Security_Center"
                        "Corporate Executive" = "Corporate_Executive"
                        "IT Administrator" = "IT_Administrator"
                        "Thief Culprit" = "Thief_Culprit"
                        "Hardware Keys" = "Hardware_Keys"
                        "Badge Scanner" = "Badge_Scanner"
                        "Badge Multi-Tool" = "Badge_Multi_Tool"
                        "Deception Scan" = "Deception_Scan"
                        "Timeline UI" = "Timeline_UI"
                        # Episode 3
                        "Sterile Clean Room" = "Clean_Room"
                        "Maintenance Corridor" = "Maintenance_Corridor"
                        "Containment Area" = "Containment_Area"
                        "Facility Manager" = "Facility_Manager"
                        "Insider Culprit" = "Insider_Culprit"
                        "Micro-Components" = "Micro_Components"
                        "Contamination Detector" = "Contamination_Detector"
                        "Security Containers" = "Security_Containers"
                        "Contamination UI" = "Contamination_UI"
                        "Protocol Override" = "Protocol_Override"
                        # Episode 4
                        "Server Room" = "Server_Room"
                        "Operations Center" = "Operations_Center"
                        "Isolation Chamber" = "Isolation_Chamber"
                        "IT Director" = "IT_Director"
                        "Hacker Culprit" = "Hacker_Culprit"
                        "Staging Server" = "Staging_Server"
                        "Network Scanner Swarm" = "Network_Scanner"
                        "Firmware Modules" = "Firmware_Modules"
                        "Network Analysis" = "Network_Analysis"
                        "Temporal Anomaly" = "Temporal_Anomaly"
                        # Episode 5
                        "Corporate Boardroom" = "Corporate_Boardroom"
                        "Legal Library" = "Legal_Library"
                        "Corporate Counsel" = "Corporate_Counsel"
                        "Legal Assistant" = "Legal_Assistant"
                        "Judge (Virtual)" = "Judge_Virtual"
                        "Legal Documents" = "Legal_Documents"
                        "Precedent Scanner" = "Precedent_Scanner"
                        # Episode 6
                        "Warehouse District" = "Warehouse_District"
                        "Subcontractor Facility" = "Subcontractor_Facility"
                        "Subcontractor Leader" = "Subcontractor_Leader"
                        "Warehouse Security" = "Warehouse_Security"
                        "Sensor Module" = "Sensor_Module"
                        "Custody Forms" = "Custody_Forms"
                        # Episode 7
                        "Interrogation Room" = "Interrogation_Room"
                        "Evidence Laboratory" = "Evidence_Laboratory"
                        "Decoy Suspect" = "Decoy_Suspect"
                        "Real Culprit" = "Real_Culprit"
                        "Forensic Tech" = "Forensic_Tech"
                        "False Evidence" = "False_Evidence"
                        "Lie Detector" = "Lie_Detector"
                        # Episode 8
                        "Secure Vault" = "Secure_Vault"
                        "Data Center" = "Data_Center"
                        "Security Director" = "Security_Director"
                        "Encryption Expert" = "Encryption_Expert"
                        "Legal Counsel" = "Legal_Counsel"
                        "Encrypted Files" = "Encrypted_Files"
                        "Quantum Decryptor" = "Quantum_Decryptor"
                        # Episode 9
                        "Facility Corridors" = "Facility_Corridors"
                        "Security Checkpoint" = "Security_Checkpoint"
                        "Extraction Leader" = "Extraction_Leader"
                        "Facility Director" = "Facility_Director"
                        "Neural Interface Chip" = "Neural_Interface_Chip"
                        "Security Countermeasures" = "Security_Countermeasures"
                        # Episode 10
                        "Corporate Security Center" = "Corporate_Security_Center"
                        "Alliance Negotiation Room" = "Alliance_Negotiation_Room"
                        "Ring Leader" = "Ring_Leader"
                        "Corrupt Executive" = "Corrupt_Executive"
                        "Network Mapping Display" = "Network_Mapping_Display"
                        "Deception Detector" = "Deception_Detector"
                        # Episode 11
                        "Safe House Interior" = "Safe_House_Interior"
                        "Evidence Storage" = "Evidence_Storage"
                        "Cleanup Leader" = "Cleanup_Leader"
                        "Legal Observer" = "Legal_Observer"
                        "Evidence Containers" = "Evidence_Containers"
                        "Quantum Verifier" = "Quantum_Verifier"
                        # Episode 12
                        "Final Confrontation Chamber" = "Final_Confrontation_Chamber"
                        "Prototype Vault" = "Prototype_Vault"
                        "Final Culprit" = "Final_Culprit"
                        "Agency Director" = "Agency_Director"
                        "Core Prototype Module" = "Core_Prototype_Module"
                        "Pattern Synthesis Display" = "Pattern_Synthesis_Display"
                    }

                    # Find the config name that matches this asset name
                    $ConfigName = $NameMapping[$AssetName]
                    if ($ConfigName) {
                        $PromptKey = "${EpisodeNum}_${ConfigName}"
                    }

                    if ($PromptKey) {
                        $Prompts[$PromptKey] = @{
                            Prompt = $Prompt
                            Episode = $EpisodeNum
                            Type = "Unknown"
                        }
                    }
                }
            }
        }
    }

    # Extract video prompts
    $VideoSection = [regex]::Match($Content, '(?s)## Video Generation Prompts.*?(?=## Prompt Engineering|\z)')
    if ($VideoSection.Success) {
        $VideoMatches = [regex]::Matches($VideoSection.Value, '\*\*([^*]+):\*\*\s*([^\n]+(?:\n(?!###|\*\*).+)*)')
        foreach ($Match in $VideoMatches) {
            $AssetName = $Match.Groups[1].Value.Trim()
            $Prompt = $Match.Groups[2].Value.Trim()

            if ($Prompt) {
                # Determine episode from asset name patterns
                $EpisodeKey = switch ($AssetName) {
                    {$_ -match "Compass_Awakening|Team_Introduction"} { "S01E01" }
                    {$_ -match "Identity_Reveal|Badge_Scan_Demo"} { "S01E02" }
                    {$_ -match "Clean_Room_Breach|Drone_Swarm_Analysis"} { "S01E03" }
                    {$_ -match "Server_Breach|Hacker_Confrontation"} { "S01E04" }
                    default { "Unknown" }
                }

                $CleanAssetName = $AssetName -replace '[^a-zA-Z0-9_]', '_' -replace '_+', '_'
                $Key = "${EpisodeKey}_${CleanAssetName}"
                $Prompts[$Key] = @{
                    Prompt = $Prompt
                    Episode = $EpisodeKey
                    Type = "Video"
                }
            }
        }
    }

    return $Prompts
}

# Generate single asset with variations
function Generate-Asset {
    param(
        [string]$Episode,
        [string]$AssetName,
        [string]$AssetType,
        [string]$Prompt,
        [int]$Variations,
        [string]$OutputDir
    )

    Write-Host "`n=== Generating $AssetType`: $AssetName ($Variations variations) ==="

    for ($i = 1; $i -le $Variations; $i++) {
        $Timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
        $Variation = "v$i".PadLeft(2, '0')
        $FileName = "${Episode}_${AssetName}_${Variation}_${Timestamp}.png"

        if ($AssetType -eq "Video") {
            $FileName = $FileName -replace "\.png$", ".mp4"
        }

        $OutputPath = Join-Path $OutputDir $FileName

        if ($DryRun) {
            Write-Host "[DRY RUN] Would generate: $FileName"
            Write-Host "  Prompt: $($Prompt.Substring(0, [Math]::Min(100, $Prompt.Length)))..."
        } else {
            # Open Grok Imagine in browser
            Start-Process "https://grok.com/imagine"

            # Wait for page load
            Start-Sleep -Seconds 3

            Write-Host "Please generate asset manually:"
            Write-Host "  File: $FileName"
            Write-Host "  Type: $AssetType"
            Write-Host "  Prompt: $Prompt"
            Write-Host "  Save to: $OutputPath"
            Write-Host ""

            # Wait for user input before continuing
            if ($Variations -gt 1) {
                Read-Host "Press Enter when ready for next variation (or Ctrl+C to stop)"
            }
        }
    }
}

# Main generation function
function Start-EpisodeGeneration {
    param([string]$TargetEpisode)

    Write-Host "Crimson Compass Episode Visual Generator"
    Write-Host "======================================="
    Write-Host "Episode: $TargetEpisode"

    # Get episode configuration
    $Config = $EpisodeConfigs[$TargetEpisode]
    if (!$Config) {
        Write-Error "Invalid episode: $TargetEpisode"
        exit 1
    }

    Write-Host "Name: $($Config.Name)"
    Write-Host "Theme: $($Config.Theme)"
    Write-Host "Assets to generate: $($Config.Assets.Count)"
    Write-Host ""

    # Load prompts
    Write-Host "Loading prompts..."
    $AllPrompts = Get-PromptsFromFile

    # Create output directory
    $EpisodeOutputDir = Join-Path $OutputPath $TargetEpisode
    if (!(Test-Path $EpisodeOutputDir)) {
        New-Item -ItemType Directory -Path $EpisodeOutputDir -Force | Out-Null
    }

    # Generate assets
    $Generated = 0
    foreach ($Asset in $Config.Assets) {
        $AssetName = $Asset.Name
        $AssetType = $Asset.Type
        $Variations = if ($VariationsPerAsset -and $AssetType -ne "Video") { $VariationsPerAsset } else { $Asset.Variations }

        # Skip videos if not requested
        if ($AssetType -eq "Video" -and !$IncludeVideos) {
            continue
        }

        # Find prompt
        $PromptKey = "${TargetEpisode}_$AssetName"
        $PromptData = $AllPrompts[$PromptKey]

        if (!$PromptData) {
            Write-Warning "No prompt found for: $PromptKey"
            continue
        }

        Generate-Asset -Episode $TargetEpisode -AssetName $AssetName -AssetType $AssetType -Prompt $PromptData.Prompt -Variations $Variations -OutputDir $EpisodeOutputDir
        $Generated++
    }

    Write-Host "`n=== Episode $TargetEpisode Complete ==="
    Write-Host "Generated $Generated assets"
    Write-Host "Output directory: $EpisodeOutputDir"

    if (!$DryRun) {
        Write-Host "`nNext steps:"
        Write-Host "1. Review generated assets in $EpisodeOutputDir"
        Write-Host "2. Run quality check: .\scripts\validate_setup.ps1"
        Write-Host "3. Import to Unity: .\scripts\unity_workflow.ps1"
    }
}

# Handle episode selection
if ($Episode -eq "ALL") {
    foreach ($Ep in @("S01E01", "S01E02", "S01E03", "S01E04")) {
        Start-EpisodeGeneration -TargetEpisode $Ep
        if (!$DryRun) {
            Read-Host "`nPress Enter to continue to next episode"
        }
    }
} else {
    Start-EpisodeGeneration -TargetEpisode $Episode
}