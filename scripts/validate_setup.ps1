# Crimson Compass Grok Imagine Workflow Validator
# Tests the integration setup and validates all components

param(
    [switch]$FixIssues,
    [switch]$Verbose
)

Write-Host "Crimson Compass Grok Imagine Validator"
Write-Host "====================================="

$Issues = @()
$Warnings = @()

# Test 1: Check required files exist
Write-Host "`n1. Checking required files..."
$RequiredFiles = @(
    "docs\Visual_Prompts_Grok_Imagine.md",
    "docs\Season1_Visual_Production_Plan.md",
    "scripts\batch_generate_grok_assets.ps1",
    "scripts\unity_workflow.ps1",
    "Grok_Imagine_Integration_README.md"
)

foreach ($File in $RequiredFiles) {
    if (Test-Path $File) {
        if ($Verbose) { Write-Host "  ✓ $File" }
    } else {
        $Issues += "Missing required file: $File"
        Write-Host "  ✗ $File"
    }
}

# Test 2: Check PowerShell execution policy
Write-Host "`n2. Checking PowerShell execution policy..."
$ExecutionPolicy = Get-ExecutionPolicy
if ($ExecutionPolicy -eq "Restricted") {
    $Issues += "PowerShell execution policy is Restricted. Scripts cannot run."
    Write-Host "  ✗ Execution policy: $ExecutionPolicy"
} else {
    Write-Host "  ✓ Execution policy: $ExecutionPolicy"
}

# Test 3: Check Unity installation
Write-Host "`n3. Checking Unity installation..."
$UnityPaths = @(
    "C:\Program Files\Unity\Hub\Editor\*\Editor\Unity.exe",
    "C:\Program Files\Unity\*\Editor\Unity.exe"
)

$UnityFound = $false
foreach ($Path in $UnityPaths) {
    if (Test-Path $Path) {
        $UnityFound = $true
        Write-Host "  ✓ Unity found at: $Path"
        break
    }
}

if (!$UnityFound) {
    $Warnings += "Unity installation not found in standard locations. Update path in unity_workflow.ps1"
    Write-Host "  ⚠ Unity installation not found in standard locations"
}

# Test 4: Validate prompt file structure
Write-Host "`n4. Validating prompt file structure..."
$PromptFile = "docs\Visual_Prompts_Grok_Imagine.md"
if (Test-Path $PromptFile) {
    $Content = Get-Content $PromptFile -Raw

    # Check for required sections
    $RequiredSections = @(
        "## Episode 1: Welcome Packet",
        "## Episode 2: Badge & Borrow",
        "## Episode 3: Clean Room",
        "## Episode 4: Update Never Shipped",
        "## Video Generation Prompts"
    )

    foreach ($Section in $RequiredSections) {
        if ($Content.Contains($Section)) {
            if ($Verbose) { Write-Host "  ✓ $Section" }
        } else {
            $Issues += "Missing section in prompt file: $Section"
            Write-Host "  ✗ $Section"
        }
    }

    # Count prompts (rough estimate)
    $PromptCount = [regex]::Matches($Content, '\*\*.*:\*\*').Count
    Write-Host "  ✓ Found approximately $PromptCount prompt entries"
}

# Test 5: Check script syntax
Write-Host "`n5. Validating script syntax..."
$Scripts = @(
    "scripts\batch_generate_grok_assets.ps1",
    "scripts\unity_workflow.ps1"
)

foreach ($Script in $Scripts) {
    if (Test-Path $Script) {
        try {
            $null = [System.Management.Automation.PSParser]::Tokenize((Get-Content $Script -Raw), [ref]$null)
            if ($Verbose) { Write-Host "  ✓ $Script syntax OK" }
        } catch {
            $Issues += "Syntax error in $Script`: $($_.Exception.Message)"
            Write-Host "  ✗ $Script syntax error"
        }
    }
}

# Test 6: Check output directories
Write-Host "`n6. Checking output directories..."
$Directories = @(
    "GeneratedAssets",
    "Assets",
    "Assets\Sprites",
    "Assets\Editor"
)

foreach ($Dir in $Directories) {
    if (Test-Path $Dir) {
        if ($Verbose) { Write-Host "  ✓ $Dir" }
    } else {
        Write-Host "  ⚠ $Dir (will be created during workflow)"
    }
}

# Summary
Write-Host "`n=== Validation Summary ==="

if ($Issues.Count -eq 0) {
    Write-Host "✓ All critical checks passed!"
} else {
    Write-Host "✗ $($Issues.Count) critical issues found:"
    foreach ($Issue in $Issues) {
        Write-Host "  - $Issue"
    }
}

if ($Warnings.Count -gt 0) {
    Write-Host "`n⚠ $($Warnings.Count) warnings:"
    foreach ($Warning in $Warnings) {
        Write-Host "  - $Warning"
    }
}

# Recommendations
Write-Host "`n=== Recommendations ==="

if ($Issues.Count -gt 0 -and $FixIssues) {
    Write-Host "Attempting to fix issues..."

    # Fix execution policy
    if ($ExecutionPolicy -eq "Restricted") {
        try {
            Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser -Force
            Write-Host "✓ Fixed execution policy"
        } catch {
            Write-Host "✗ Could not fix execution policy: $($_.Exception.Message)"
        }
    }

    # Create missing directories
    foreach ($Dir in $Directories) {
        if (!(Test-Path $Dir)) {
            try {
                New-Item -ItemType Directory -Path $Dir -Force | Out-Null
                Write-Host "✓ Created directory: $Dir"
            } catch {
                Write-Host "✗ Could not create directory $Dir`: $($_.Exception.Message)"
            }
        }
    }
} else {
    Write-Host "1. Run: .\scripts\batch_generate_grok_assets.ps1 -DryRun"
    Write-Host "2. Run: .\scripts\unity_workflow.ps1 -DryRun"
    Write-Host "3. Visit: https://grok.com/imagine to test access"
    Write-Host "4. Read: Grok_Imagine_Integration_README.md for detailed instructions"
}

Write-Host "`nValidation complete."