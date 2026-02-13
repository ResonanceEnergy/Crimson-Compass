# Crimson Compass Nightly Build Setup
# Run this script as Administrator to set up automated nightly builds

param(
    [string]$ProjectPath = "C:\Users\gripa\OneDrive\Desktop\CrimsonCompass\Crimson-Compass"
)

Write-Host "=== Setting up Crimson Compass Nightly Builds ===" -ForegroundColor Green
Write-Host "Project path: $ProjectPath"

$buildScript = Join-Path $ProjectPath "Tools\build.py"
$pythonExe = "python"  # Assumes python is in PATH

if (!(Test-Path $buildScript)) {
    Write-Host "ERROR: Build script not found: $buildScript" -ForegroundColor Red
    exit 1
}

$taskName = "CrimsonCompass_NightlyBuild"
$buildCommand = "`"$pythonExe`" `"$buildScript`" --preflight --test"

Write-Host "Build command: $buildCommand" -ForegroundColor Yellow
Write-Host "Schedule: Daily at 2:00 AM" -ForegroundColor Yellow

# Remove existing task if it exists
Write-Host "`nRemoving existing task '$taskName' if it exists..." -ForegroundColor Cyan
schtasks /delete /tn $taskName /f 2>$null

# Create new scheduled task
Write-Host "Creating nightly build task..." -ForegroundColor Cyan

$createResult = schtasks /create /tn $taskName /tr $buildCommand /sc daily /st 02:00 /ru $env:USERNAME /rl HIGHEST /f

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n🎉 Nightly build setup complete!" -ForegroundColor Green
    Write-Host "Task name: $taskName"
    Write-Host "The build will run every night at 2:00 AM"
    Write-Host "Check the Logs directory for build output"
    Write-Host "`nTo view the task: schtasks /query /tn $taskName" -ForegroundColor Gray
    Write-Host "To delete the task: schtasks /delete /tn $taskName" -ForegroundColor Gray
} else {
    Write-Host "`n❌ Failed to set up nightly build" -ForegroundColor Red
    Write-Host "You may need to run this script as Administrator" -ForegroundColor Yellow
    Write-Host "Alternatively, create the task manually in Task Scheduler:" -ForegroundColor Yellow
    Write-Host "1. Open Task Scheduler" -ForegroundColor Gray
    Write-Host "2. Create a new task" -ForegroundColor Gray
    Write-Host "3. Set the trigger to daily at 2:00 AM" -ForegroundColor Gray
    Write-Host "4. Set the action to run: $buildCommand" -ForegroundColor Gray
    Write-Host "5. Set 'Run with highest privileges'" -ForegroundColor Gray
}