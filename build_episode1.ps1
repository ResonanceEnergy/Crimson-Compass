# Build and Test Episode 1

Write-Host "Building Crimson Compass Episode 1..." -ForegroundColor Green

# Check if Unity is available
$unityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.20f1\Editor\Unity.exe"
if (!(Test-Path $unityPath)) {
    Write-Host "Unity not found at expected path. Please update the path in this script." -ForegroundColor Red
    exit 1
}

# Build the project
Write-Host "Compiling project..." -ForegroundColor Yellow
& $unityPath -batchmode -nographics -projectPath "$PSScriptRoot\.." -executeMethod CrimsonCompass.Editor.BuildProcessor.BuildEpisode1 -quit

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful!" -ForegroundColor Green
    Write-Host "Episode 1 is ready for testing." -ForegroundColor Green
    Write-Host "Run the game and select Episode 1 from the main menu." -ForegroundColor Cyan
} else {
    Write-Host "Build failed. Check the Unity console for errors." -ForegroundColor Red
}