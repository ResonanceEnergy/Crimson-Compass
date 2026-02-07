# Crimson Compass Build Script for iOS
# Automates Unity iOS build (creates Xcode project)

$unityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"
$projectPath = "C:\Users\gripa\OneDrive\Desktop\CrimsonCompass\Crimson-Compass"
$buildPath = "C:\Users\gripa\OneDrive\Desktop\CrimsonCompass\Builds\iOS"

# Create build directory if not exists
if (!(Test-Path $buildPath)) {
    New-Item -ItemType Directory -Path $buildPath
}

# Run Unity build
& $unityPath -batchmode -nographics -projectPath $projectPath -buildTarget iOS -buildPath $buildPath

Write-Host "iOS build completed. Check $buildPath for Xcode project."


