# Crimson Compass Version Management
# Handles version bumping, build numbers, and environment configuration

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("patch", "minor", "major", "build")]
    [string]$BumpType,

    [Parameter(Mandatory=$false)]
    [ValidateSet("development", "staging", "production")]
    [string]$Environment = "development",

    [switch]$NoCommit
)

$ErrorActionPreference = "Stop"

Write-Host "🔢 Crimson Compass Version Management" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green

$VersionFile = Join-Path $PSScriptRoot "version.json"

# Read current version
try {
    $versionData = Get-Content $VersionFile -Raw | ConvertFrom-Json
}
catch {
    Write-Host "❌ Failed to parse version.json: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
$currentVersion = [version]$versionData.version

Write-Host "Current version: $($currentVersion.ToString())" -ForegroundColor White
Write-Host "Build: $($versionData.build)" -ForegroundColor White

# Bump version based on type
switch ($BumpType) {
    "patch" {
        $newVersion = [version]::new($currentVersion.Major, $currentVersion.Minor, $currentVersion.Build + 1)
    }
    "minor" {
        $newVersion = [version]::new($currentVersion.Major, $currentVersion.Minor + 1, 0)
    }
    "major" {
        $newVersion = [version]::new($currentVersion.Major + 1, 0, 0)
    }
    "build" {
        $newVersion = $currentVersion
    }
}

# Increment build number
$versionData.build = $versionData.build + 1
$versionData.version = $newVersion.ToString()

# Update platform-specific version codes
$versionData.androidVersionCode = $versionData.build
$versionData.iosBuildNumber = "$($newVersion.ToString()).$($versionData.build)"

Write-Host "New version: $($newVersion.ToString())" -ForegroundColor Green
Write-Host "New build: $($versionData.build)" -ForegroundColor Green

# Update Unity project version
$projectVersionFile = Join-Path $PSScriptRoot "ProjectSettings/ProjectVersion.txt"
if (Test-Path $projectVersionFile) {
    $versionContent = @"
m_EditorVersion: 6000.3.6f1
m_EditorVersionWithRevision: 6000.3.6f1 (1234567890abcdef)
"@
    $versionContent | Out-File $projectVersionFile -Encoding UTF8
    Write-Host "✅ Updated Unity ProjectVersion.txt" -ForegroundColor Green
}

# Update Android version in manifest
$androidManifest = Join-Path $PSScriptRoot "Assets/Plugins/Android/AndroidManifest.xml"
if (Test-Path $androidManifest) {
    $manifestContent = Get-Content $androidManifest -Raw
    $manifestContent = $manifestContent -replace 'android:versionCode="\d+"', "android:versionCode=""$($versionData.androidVersionCode)"""
    $manifestContent = $manifestContent -replace 'android:versionName="[^"]*"', "android:versionName=""$($newVersion.ToString())"""
    $manifestContent | Out-File $androidManifest -Encoding UTF8
    Write-Host "✅ Updated Android manifest version" -ForegroundColor Green
}

# Update iOS version in plist
$iosPlist = Join-Path $PSScriptRoot "Assets/Plugins/iOS/Info.plist"
if (Test-Path $iosPlist) {
    $plistContent = Get-Content $iosPlist -Raw
    $plistContent = $plistContent -replace '<key>CFBundleVersion</key>\s*<string>[^<]*</string>', "<key>CFBundleVersion</key>`n`t<string>$($versionData.iosBuildNumber)</string>"
    $plistContent = $plistContent -replace '<key>CFBundleShortVersionString</key>\s*<string>[^<]*</string>', "<key>CFBundleShortVersionString</key>`n`t<string>$($newVersion.ToString())</string>"
    $plistContent | Out-File $iosPlist -Encoding UTF8
    Write-Host "✅ Updated iOS plist version" -ForegroundColor Green
}

# Save updated version file
$versionData | ConvertTo-Json -Depth 10 | Out-File $VersionFile -Encoding UTF8
Write-Host "✅ Updated version.json" -ForegroundColor Green

# Generate changelog entry
$changelogEntry = @"

## [$($newVersion.ToString())] - $(Get-Date -Format "yyyy-MM-dd")

### Changes
- Version bump: $BumpType
- Build number: $($versionData.build)
- Environment: $Environment

"@

$changelogFile = Join-Path $PSScriptRoot "CHANGELOG.md"
if (Test-Path $changelogFile) {
    $existingContent = Get-Content $changelogFile -Raw
    $newContent = $changelogEntry + $existingContent
    $newContent | Out-File $changelogFile -Encoding UTF8
} else {
    $changelogEntry | Out-File $changelogFile -Encoding UTF8
}
Write-Host "✅ Updated CHANGELOG.md" -ForegroundColor Green

# Git commit if requested
if (-not $NoCommit) {
    & git add version.json CHANGELOG.md
    if (Test-Path $projectVersionFile) { & git add "ProjectSettings/ProjectVersion.txt" }
    if (Test-Path $androidManifest) { & git add "Assets/Plugins/Android/AndroidManifest.xml" }
    if (Test-Path $iosPlist) { & git add "Assets/Plugins/iOS/Info.plist" }

    $commitMessage = "Bump version to $($newVersion.ToString()) ($BumpType) - Build $($versionData.build)"
    & git commit -m $commitMessage

    Write-Host "✅ Committed version changes" -ForegroundColor Green
}

Write-Host "`n🎉 Version bump completed!" -ForegroundColor Green
Write-Host "Version: $($newVersion.ToString())" -ForegroundColor White
Write-Host "Build: $($versionData.build)" -ForegroundColor White
Write-Host "Android Version Code: $($versionData.androidVersionCode)" -ForegroundColor White
