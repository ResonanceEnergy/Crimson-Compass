# Crimson Compass App Store Deployment
# Handles automated submission to Google Play Store and Apple App Store

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("Android", "iOS", "Both")]
    [string]$Target,

    [Parameter(Mandatory=$true)]
    [ValidateSet("staging", "production")]
    [string]$Environment,

    [Parameter(Mandatory=$false)]
    [string]$ReleaseNotes,

    [switch]$SkipValidation
)

$ErrorActionPreference = "Stop"

Write-Host "🚀 Crimson Compass App Store Deployment" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green

$ProjectRoot = Get-Location
$BuildsDir = Join-Path $ProjectRoot "Builds"

# Function to validate build artifacts
function Test-BuildArtifacts {
    param([string]$Platform)

    Write-Host "`n📋 Validating $Platform build artifacts..." -ForegroundColor Yellow

    $platformDir = Join-Path $BuildsDir $Platform
    if (-not (Test-Path $platformDir)) {
        throw "$Platform build directory not found: $platformDir"
    }

    $buildFiles = Get-ChildItem $platformDir -File -Recurse
    if ($buildFiles.Count -eq 0) {
        throw "No build artifacts found in $platformDir"
    }

    Write-Host "✅ Found $($buildFiles.Count) build files" -ForegroundColor Green

    # Platform-specific validation
    switch ($Platform) {
        "Android" {
            $apkFiles = $buildFiles | Where-Object { $_.Extension -eq ".apk" }
            if ($apkFiles.Count -eq 0) {
                throw "No APK files found in Android build"
            }
            Write-Host "📱 APK file: $($apkFiles[0].Name)" -ForegroundColor White
        }
        "iOS" {
            $ipaFiles = $buildFiles | Where-Object { $_.Extension -eq ".ipa" }
            if ($ipaFiles.Count -eq 0) {
                throw "No IPA files found in iOS build"
            }
            Write-Host "🍎 IPA file: $($ipaFiles[0].Name)" -ForegroundColor White
        }
    }
}

# Function to submit to Google Play Store
function Submit-ToGooglePlay {
    param([string]$ApkPath, [string]$Track = "internal")

    Write-Host "`n📱 Submitting to Google Play Store ($Track track)..." -ForegroundColor Cyan

    # Check if fastlane is available
    if (-not (Get-Command fastlane -ErrorAction SilentlyContinue)) {
        Write-Host "⚠️  Fastlane not found. Installing..." -ForegroundColor Yellow
        & gem install fastlane
    }

    # Create Fastfile if it doesn't exist
    $fastlaneDir = Join-Path $ProjectRoot "fastlane"
    if (-not (Test-Path $fastlaneDir)) {
        New-Item -ItemType Directory -Path $fastlaneDir | Out-Null
    }

    $fastfilePath = Join-Path $fastlaneDir "Fastfile"
    if (-not (Test-Path $fastfilePath)) {
        $fastfileContent = @'
# Crimson Compass Fastfile
default_platform(:android)

platform :android do
  desc "Deploy to Google Play Store"
  lane :deploy do |options|
    upload_to_play_store(
      track: options[:track] || "internal",
      apk: options[:apk_path],
      json_key: ENV["GOOGLE_PLAY_JSON_KEY"],
      skip_upload_metadata: true,
      skip_upload_images: true,
      skip_upload_screenshots: true
    )
  end
end
'@
        $fastfileContent | Out-File $fastfilePath -Encoding UTF8
    }

    # Set environment variables
    $env:GOOGLE_PLAY_JSON_KEY = $env:GOOGLE_PLAY_JSON_KEY_PATH
    if (-not $env:GOOGLE_PLAY_JSON_KEY) {
        throw "GOOGLE_PLAY_JSON_KEY_PATH environment variable not set"
    }

    # Run fastlane
    Push-Location $ProjectRoot
    try {
        & fastlane android deploy track:$Track apk_path:$ApkPath
        Write-Host "✅ Successfully submitted to Google Play Store" -ForegroundColor Green
    }
    finally {
        Pop-Location
    }
}

# Function to submit to Apple App Store
function Submit-ToAppStore {
    param([string]$IpaPath, [string]$Environment)

    Write-Host "`n🍎 Submitting to Apple App Store..." -ForegroundColor Cyan

    # Check if fastlane is available
    if (-not (Get-Command fastlane -ErrorAction SilentlyContinue)) {
        Write-Host "⚠️  Fastlane not found. Installing..." -ForegroundColor Yellow
        & gem install fastlane
    }

    # Create Fastfile if it doesn't exist
    $fastlaneDir = Join-Path $ProjectRoot "fastlane"
    if (-not (Test-Path $fastlaneDir)) {
        New-Item -ItemType Directory -Path $fastlaneDir | Out-Null
    }

    $fastfilePath = Join-Path $fastlaneDir "Fastfile"
    $fastfileContent = Get-Content $fastfilePath -Raw -ErrorAction SilentlyContinue

    if (-not $fastfileContent -or -not ($fastfileContent -match "platform :ios")) {
        $iosFastfileContent = @'

platform :ios do
  desc "Deploy to TestFlight"
  lane :beta do |options|
    upload_to_testflight(
      ipa: options[:ipa_path],
      api_key_path: ENV["APP_STORE_CONNECT_API_KEY"],
      skip_waiting_for_build_processing: true
    )
  end

  desc "Deploy to App Store"
  lane :release do |options|
    deliver(
      ipa: options[:ipa_path],
      api_key_path: ENV["APP_STORE_CONNECT_API_KEY"],
      skip_metadata: true,
      skip_screenshots: true,
      submit_for_review: true,
      automatic_release: true
    )
  end
end
'@
        if ($fastfileContent) {
            $fastfileContent += $iosFastfileContent
        } else {
            $fastfileContent = "# Crimson Compass Fastfile`n" + $iosFastfileContent
        }
        $fastfileContent | Out-File $fastfilePath -Encoding UTF8
    }

    # Set environment variables
    if (-not $env:APP_STORE_CONNECT_API_KEY) {
        throw "APP_STORE_CONNECT_API_KEY environment variable not set"
    }

    # Choose lane based on environment
    $lane = if ($Environment -eq "production") { "release" } else { "beta" }

    # Run fastlane
    Push-Location $ProjectRoot
    try {
        & fastlane ios $lane ipa_path:$IpaPath
        $storeName = if ($Environment -eq "production") { "App Store" } else { "TestFlight" }
        Write-Host "✅ Successfully submitted to $storeName" -ForegroundColor Green
    }
    finally {
        Pop-Location
    }
}

# Function to generate deployment report
function New-DeploymentReport {
    param([string]$Platform, [string]$Status, [string]$SubmissionId = "")

    $report = @{
        timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
        platform = $Platform
        environment = $Environment
        status = $Status
        submission_id = $SubmissionId
        version = (Get-Content "version.json" | ConvertFrom-Json).version
        build = (Get-Content "version.json" | ConvertFrom-Json).build
    }

    $reportPath = Join-Path $BuildsDir "deployment_report_$(Get-Date -Format 'yyyyMMdd_HHmmss').json"
    $report | ConvertTo-Json -Depth 10 | Out-File $reportPath -Encoding UTF8

    Write-Host "📋 Deployment report saved: $reportPath" -ForegroundColor White
}

# Main execution
try {
    # Validate builds if not skipped
    if (-not $SkipValidation) {
        if ($Target -eq "Both" -or $Target -eq "Android") {
            Test-BuildArtifacts -Platform "Android"
        }

        if ($Target -eq "Both" -or $Target -eq "iOS") {
            Test-BuildArtifacts -Platform "iOS"
        }
    }

    # Determine track based on environment
    $track = if ($Environment -eq "production") { "production" } else { "beta" }

    # Submit to Android if requested
    if ($Target -eq "Both" -or $Target -eq "Android") {
        $apkPath = Get-ChildItem (Join-Path $BuildsDir "Android") -Filter "*.apk" -Recurse | Select-Object -First 1
        if ($apkPath) {
            Submit-ToGooglePlay -ApkPath $apkPath.FullName -Track $track
            New-DeploymentReport -Platform "Android" -Status "success"
        } else {
            throw "No APK file found for Android deployment"
        }
    }

    # Submit to iOS if requested
    if ($Target -eq "Both" -or $Target -eq "iOS") {
        $ipaPath = Get-ChildItem (Join-Path $BuildsDir "iOS") -Filter "*.ipa" -Recurse | Select-Object -First 1
        if ($ipaPath) {
            Submit-ToAppStore -IpaPath $ipaPath.FullName -Environment $Environment
            New-DeploymentReport -Platform "iOS" -Status "success"
        } else {
            throw "No IPA file found for iOS deployment"
        }
    }

    Write-Host "`n🎉 App store deployment completed!" -ForegroundColor Green
    Write-Host "Environment: $Environment" -ForegroundColor White
    Write-Host "Target: $Target" -ForegroundColor White

}
catch {
    Write-Host "`n❌ Deployment failed: $($_.Exception.Message)" -ForegroundColor Red
    New-DeploymentReport -Platform $Target -Status "failed"
    exit 1
}</content>
<parameter name="filePath">c:\Users\gripa\OneDrive\Desktop\CrimsonCompass1\CrimsonCompass\deploy-to-stores.ps1