# Crimson Compass Build Script
# Automates Unity Android build

$unityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"
$projectPath = "C:\Users\gripa\OneDrive\Desktop\CrimsonCompass\Crimson-Compass"
$buildPath = "C:\Users\gripa\OneDrive\Desktop\CrimsonCompass\Builds\Android"

# Create build directory if not exists
if (!(Test-Path $buildPath)) {
    New-Item -ItemType Directory -Path $buildPath
}

# Run Unity build
& $unityPath -batchmode -nographics -projectPath $projectPath -buildTarget Android -buildPath $buildPath

Write-Host "Build completed. Check $buildPath for APK."