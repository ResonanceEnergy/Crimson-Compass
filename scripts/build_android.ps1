# Crimson Compass Build Script
# Automates Unity Android build with enhanced error handling

param(
    [string]$UnityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe",
    [string]$ProjectPath = "C:\Users\gripa\OneDrive\Desktop\CrimsonCompass\Crimson-Compass",
    [string]$BuildPath = "C:\Users\gripa\OneDrive\Desktop\CrimsonCompass\Builds\Android",
    [string]$LogFile = "C:\Users\gripa\OneDrive\Desktop\CrimsonCompass\Builds\build_log.txt"
)

# Create build directory if not exists
if (!(Test-Path $BuildPath)) {
    New-Item -ItemType Directory -Path $BuildPath -Force
}

# Start logging
Start-Transcript -Path $LogFile -Append

Write-Host "Starting Unity Android build..."
Write-Host "Unity Path: $UnityPath"
Write-Host "Project Path: $ProjectPath"
Write-Host "Build Path: $BuildPath"

# Check if Unity exists
if (!(Test-Path $UnityPath)) {
    Write-Error "Unity executable not found at $UnityPath"
    Stop-Transcript
    exit 1
}

# Run Unity build with error handling
try {
    $process = Start-Process -FilePath $UnityPath -ArgumentList "-batchmode -nographics -projectPath `"$ProjectPath`" -buildTarget Android -buildPath `"$BuildPath`"" -NoNewWindow -Wait -PassThru

    if ($process.ExitCode -eq 0) {
        Write-Host "Build completed successfully. Check $BuildPath for APK."
    } else {
        Write-Error "Build failed with exit code $($process.ExitCode)"
        Stop-Transcript
        exit $process.ExitCode
    }
} catch {
    Write-Error "Build process failed: $($_.Exception.Message)"
    Stop-Transcript
    exit 1
}

Stop-Transcript