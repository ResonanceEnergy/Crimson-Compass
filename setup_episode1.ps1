# Crimson Compass Episode 1 Setup Helper
Write-Host "Crimson Compass Episode 1 Manual Setup Helper" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host ""

# Check if Unity project exists
$projectPath = $PSScriptRoot
$scenePath = Join-Path $projectPath "Assets\Scenes\S01E01_AgencyBriefingRoom.unity"
$unityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"

Write-Host "Project Path: $projectPath" -ForegroundColor Yellow
Write-Host "Scene Path: $scenePath" -ForegroundColor Yellow
Write-Host ""

# Check if files exist
if (Test-Path $scenePath) {
    Write-Host "✅ Scene file found: S01E01_AgencyBriefingRoom.unity" -ForegroundColor Green
} else {
    Write-Host "❌ Scene file not found!" -ForegroundColor Red
}

if (Test-Path $unityPath) {
    Write-Host "✅ Unity Editor found: 2022.3.62f3" -ForegroundColor Green
} else {
    Write-Host "❌ Unity Editor not found at expected path" -ForegroundColor Red
}

Write-Host ""
Write-Host "Manual Setup Instructions:" -ForegroundColor Cyan
Write-Host "1. Open Unity Editor (double-click CrimsonCompass.sln)" -ForegroundColor White
Write-Host "2. In Unity: File > Open Scene > Assets/Scenes/S01E01_AgencyBriefingRoom.unity" -ForegroundColor White
Write-Host "3. In Hierarchy: Right-click > Create Empty, name it 'Episode1TestSetup'" -ForegroundColor White
Write-Host "4. Select Episode1TestSetup > Add Component > Episode1TestInitializer" -ForegroundColor White
Write-Host "5. Click Play button to test" -ForegroundColor White
Write-Host ""
Write-Host "Controls:" -ForegroundColor Green
Write-Host "- Click objects to interact based on selected verb" -ForegroundColor White
Write-Host "- Use bottom verb bar to change interaction modes" -ForegroundColor White
Write-Host "- KIT verb opens inventory" -ForegroundColor White
Write-Host "- Click NPCs to start dialogue" -ForegroundColor White

Write-Host ""
Write-Host "Opening setup guide..." -ForegroundColor Yellow
$setupGuide = Join-Path $projectPath "EPISODE1_MANUAL_SETUP.md"
if (Test-Path $setupGuide) {
    Start-Process $setupGuide
}

Write-Host ""
Read-Host "Press Enter to exit"