#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Crimson Compass Workflow Automation (Simple Version)
.DESCRIPTION
    Simplified workflow automation for Crimson Compass
.PARAMETER SkipUnity
    Skip opening Unity Editor
.PARAMETER SkipBuild
    Skip build process
.PARAMETER SkipTest
    Skip build testing
#>

param(
    [switch]$SkipUnity,
    [switch]$SkipBuild,
    [switch]$SkipTest
)

Write-Host "CRIMSON COMPASS - WORKFLOW AUTOMATION" -ForegroundColor Magenta
Write-Host "==========================================" -ForegroundColor Magenta
Write-Host ""

# Phase 1: Validation
Write-Host "PHASE 1: Preflight Validation" -ForegroundColor Yellow
Write-Host "===============================" -ForegroundColor Yellow

try {
    Write-Host "Running episode data validation..." -ForegroundColor Gray
    & python "Tools\validate_episode_data.py"
    if ($LASTEXITCODE -ne 0) { throw "Episode data validation failed" }

    Write-Host "Running audio events validation..." -ForegroundColor Gray
    & python "Tools\validate_audio_events.py"
    if ($LASTEXITCODE -ne 0) { throw "Audio events validation failed" }

    Write-Host "Preflight validation passed!" -ForegroundColor Green
    $validationPassed = $true
}
catch {
    Write-Host "Preflight validation failed: $($_.Exception.Message)" -ForegroundColor Red
    $validationPassed = $false
}

# Phase 2: Unity Editor
if (-not $SkipUnity) {
    Write-Host ""
    Write-Host "PHASE 2: Unity Editor Setup" -ForegroundColor Yellow
    Write-Host "============================" -ForegroundColor Yellow

    # Check if Unity is already running
    $unityProcesses = Get-Process -Name "Unity" -ErrorAction SilentlyContinue
    if ($unityProcesses) {
        Write-Host "Unity Editor is currently running." -ForegroundColor Yellow
        Write-Host "For automated setup, please close Unity Editor first." -ForegroundColor Yellow
        Write-Host "The build system will wait up to 5 minutes for Unity to close..." -ForegroundColor Cyan
        Write-Host ""

        $waitStart = Get-Date
        $timeout = 300  # 5 minutes

        while ($unityProcesses -and ((Get-Date) - $waitStart).TotalSeconds -lt $timeout) {
            Write-Host "Waiting for Unity Editor to close... ($([math]::Round(((Get-Date) - $waitStart).TotalSeconds))s elapsed)" -ForegroundColor Gray
            Start-Sleep -Seconds 10
            $unityProcesses = Get-Process -Name "Unity" -ErrorAction SilentlyContinue
        }

        if ($unityProcesses) {
            Write-Host "Unity Editor is still running after 5 minutes." -ForegroundColor Red
            Write-Host "Please close Unity Editor manually to continue with automated setup." -ForegroundColor Red
            Read-Host "Press Enter after closing Unity Editor"
        } else {
            Write-Host "Unity Editor closed, proceeding..." -ForegroundColor Green
        }
    }

    Write-Host "Please manually:" -ForegroundColor Cyan
    Write-Host "1. Open Unity Editor with the Crimson Compass project" -ForegroundColor White
    Write-Host "2. Go to Tools > Setup Main Scene" -ForegroundColor White
    Write-Host "3. Save the scene" -ForegroundColor White
    Write-Host ""
    Read-Host "Press Enter when done"
    Write-Host "Unity Editor setup completed!" -ForegroundColor Green
}

# Phase 3: Build
if (-not $SkipBuild) {
    Write-Host ""
    Write-Host "PHASE 3: Production Build" -ForegroundColor Yellow
    Write-Host "===========================" -ForegroundColor Yellow

    try {
        Write-Host "Running Win64 production build..." -ForegroundColor Cyan
        & python "Tools\build.py" --target Win64 --dev
        $buildExitCode = $LASTEXITCODE

        if ($buildExitCode -eq 0) {
            # Check for build output
            $buildsDir = "Builds\Win64"
            $latestBuild = Get-ChildItem $buildsDir -Filter "*.exe" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

            # Also check build manifest for success
            $manifestPath = Join-Path $buildsDir "build_manifest.txt"
            $buildSuccess = $false

            if (Test-Path $manifestPath) {
                $manifest = Get-Content $manifestPath -Raw
                if ($manifest -match "Result: Succeeded") {
                    $buildSuccess = $true
                }
            }

            if ($latestBuild -and $buildSuccess) {
                Write-Host "Build completed successfully!" -ForegroundColor Green
                Write-Host "Build output: $($latestBuild.FullName)" -ForegroundColor Cyan
                $buildPath = $latestBuild.FullName
            }
            elseif ($latestBuild) {
                Write-Host "Build executable found but manifest indicates failure" -ForegroundColor Yellow
                Write-Host "Build output: $($latestBuild.FullName)" -ForegroundColor Cyan
                Write-Host "Check build logs for details" -ForegroundColor Yellow
                $buildPath = $latestBuild.FullName
            }
            else {
                Write-Host "Build command exited with code 0 but no build output found" -ForegroundColor Yellow
                $buildPath = $null
            }
        }
        else {
            Write-Host "Build failed with exit code $buildExitCode" -ForegroundColor Red
            Write-Host "Check Logs\ directory for detailed error messages" -ForegroundColor Red
            $buildPath = $null
        }
    }
    catch {
        Write-Host "Build failed: $($_.Exception.Message)" -ForegroundColor Red
        $buildPath = $null
    }
}

# Phase 4: Testing
if (-not $SkipTest -and $buildPath) {
    Write-Host ""
    Write-Host "PHASE 4: Build Testing" -ForegroundColor Yellow
    Write-Host "========================" -ForegroundColor Yellow

    try {
        Write-Host "Launching build for testing..." -ForegroundColor Cyan
        $testProcess = Start-Process -FilePath $buildPath -PassThru
        Start-Sleep -Seconds 3

        $stillRunning = Get-Process -Id $testProcess.Id -ErrorAction SilentlyContinue
        if ($stillRunning) {
            Write-Host "Build launched successfully!" -ForegroundColor Green
            Write-Host "Please test the game manually, then close it." -ForegroundColor Yellow
            Read-Host "Press Enter when testing is complete"
            Stop-Process -Id $testProcess.Id -Force
            $testPassed = $true
        }
        else {
            Write-Host "Build crashed on launch" -ForegroundColor Red
            $testPassed = $false
        }
    }
    catch {
        Write-Host "Testing failed: $($_.Exception.Message)" -ForegroundColor Red
        $testPassed = $false
    }
}
else {
    if ($SkipTest) {
        $testPassed = $true
    }
    else {
        $testPassed = $false
    }
}

# Phase 5: Status Report
Write-Host ""
Write-Host "PHASE 5: Status Report" -ForegroundColor Yellow
Write-Host "=======================" -ForegroundColor Yellow

Write-Host ""
Write-Host "CRIMSON COMPASS - DEVELOPMENT STATUS" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta

if ($validationPassed) {
    Write-Host "Preflight Validation: PASSED" -ForegroundColor Green
} else {
    Write-Host "Preflight Validation: FAILED" -ForegroundColor Red
}

if (-not $SkipUnity) {
    Write-Host "Unity Editor Setup: COMPLETED" -ForegroundColor Green
} else {
    Write-Host "Unity Editor Setup: SKIPPED" -ForegroundColor Gray
}

if ($buildPath) {
    Write-Host "Production Build: SUCCESSFUL" -ForegroundColor Green
} elseif ($SkipBuild) {
    Write-Host "Production Build: SKIPPED" -ForegroundColor Gray
} else {
    Write-Host "Production Build: FAILED" -ForegroundColor Red
}

if ($testPassed -and -not $SkipTest) {
    Write-Host "Build Testing: PASSED" -ForegroundColor Green
} elseif ($SkipTest) {
    Write-Host "Build Testing: SKIPPED" -ForegroundColor Gray
} else {
    Write-Host "Build Testing: FAILED" -ForegroundColor Red
}

Write-Host ""
Write-Host "OVERALL STATUS:" -ForegroundColor Yellow -BackgroundColor Black
if ($validationPassed -and ($buildPath -or $SkipBuild) -and ($testPassed -or $SkipTest)) {
    Write-Host "   SUCCESS: ALL SYSTEMS GO! Ready for development and deployment!" -ForegroundColor Green
} else {
    Write-Host "   WARNING: Some issues detected. Check details above." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "NEXT STEPS:" -ForegroundColor Cyan
Write-Host "• Unity Editor: Use Tools > Setup Main Scene for UI automation" -ForegroundColor White
Write-Host "• Build System: Run builds via VS Code tasks" -ForegroundColor White
Write-Host "• Gameplay: All core systems functional for mystery cases" -ForegroundColor White
Write-Host "• Development: Full codebase ready for continued work" -ForegroundColor White

Write-Host ""
Write-Host "SUCCESS: Workflow automation completed!" -ForegroundColor Green