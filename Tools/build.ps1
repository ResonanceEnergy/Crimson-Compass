param(
  [string]$UnityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe",
  [string]$ProjectPath = (Resolve-Path ".").Path,
  [ValidateSet("Win64","Mac","Linux64","Android","iOS")]
  [string]$Target = "Win64",
  [string]$OutDir,
  [switch]$DevBuild
)

$ErrorActionPreference = "Stop"

if (-not $OutDir) {
  $OutDir = Join-Path $ProjectPath "Builds"
}

$timestamp = (Get-Date).ToString("yyyyMMdd_HHmmss")
$targetDir = Join-Path $OutDir $Target
New-Item -ItemType Directory -Force -Path $targetDir | Out-Null

$buildPath = switch ($Target) {
  "Win64"   { Join-Path $targetDir "CrimsonCompass_$timestamp.exe" }
  "Mac"     { Join-Path $targetDir "CrimsonCompass_$timestamp.app" }
  "Linux64" { Join-Path $targetDir "CrimsonCompass_$timestamp.x86_64" }
  "Android" { Join-Path $targetDir "CrimsonCompass_$timestamp.apk" }
  "iOS"     { Join-Path $targetDir "iOSBuild_$timestamp" }
}

$logDir = Join-Path $ProjectPath "Logs"
New-Item -ItemType Directory -Force -Path $logDir | Out-Null
$logFile = Join-Path $logDir "unity_build_$Target_$timestamp.log"

$dev = $DevBuild.IsPresent.ToString().ToLower()

Write-Host "Building: $Target"
Write-Host "Unity   : $UnityPath"
Write-Host "Project : $ProjectPath"
Write-Host "Output  : $buildPath"
Write-Host "Log     : $logFile"

if (-not (Test-Path $UnityPath)) {
  throw "Unity executable not found at: $UnityPath. Update -UnityPath or install Unity 2022.3.62f3 in Unity Hub."
}

& $UnityPath `
  -batchmode -nographics -quit `
  -projectPath "$ProjectPath" `
  -logFile "$logFile" `
  -executeMethod BuildPlayerCLI.Build `
  -buildTarget $Target `
  -buildPath "$buildPath" `
  -devBuild $dev

$exitCode = $LASTEXITCODE
if ($exitCode -ne 0) {
  Write-Error "Unity build failed with exit code $exitCode. Check log: $logFile"
  exit $exitCode
}

Write-Host "Build OK: $buildPath"
