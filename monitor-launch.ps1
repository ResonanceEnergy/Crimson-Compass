# Crimson Compass Launch Monitoring
# Monitors post-launch metrics and alerts on issues

param(
    [Parameter(Mandatory=$false)]
    [int]$TimeWindowHours = 24,

    [Parameter(Mandatory=$false)]
    [string]$SlackWebhookUrl = $env:SLACK_WEBHOOK_URL,

    [switch]$SendAlerts
)

$ErrorActionPreference = "Stop"

Write-Host "📊 Crimson Compass Launch Monitoring" -ForegroundColor Green
Write-Host "====================================" -ForegroundColor Green

# Configuration
$CriticalThresholds = @{
    CrashRate = 0.05  # 5%
    ResponseTime = 5000  # 5 seconds
    ErrorRate = 0.10  # 10%
}

$WarningThresholds = @{
    CrashRate = 0.02  # 2%
    ResponseTime = 2000  # 2 seconds
    ErrorRate = 0.05  # 5%
}

# Function to get crash rate from crash reporting service
function Get-CrashRate {
    param([int]$Hours)

    Write-Host "🔍 Checking crash rates..." -ForegroundColor Yellow

    # This would integrate with your crash reporting service (Firebase Crashlytics, etc.)
    # For now, return mock data
    $mockCrashData = @{
        total_crashes = 15
        total_sessions = 10000
        crash_rate = 0.0015  # 0.15%
    }

    Write-Host "📱 Total crashes: $($mockCrashData.total_crashes)" -ForegroundColor White
    Write-Host "👥 Total sessions: $($mockCrashData.total_sessions)" -ForegroundColor White
    Write-Host "💥 Crash rate: $([math]::Round($mockCrashData.crash_rate * 100, 2))%" -ForegroundColor White

    return $mockCrashData.crash_rate
}

# Function to get API response times
function Get-APIResponseTime {
    param([int]$Hours)

    Write-Host "⚡ Checking API performance..." -ForegroundColor Yellow

    # This would integrate with your API monitoring (DataDog, New Relic, etc.)
    # For now, return mock data
    $mockAPIData = @{
        average_response_time = 850  # milliseconds
        p95_response_time = 2100
        error_rate = 0.02  # 2%
    }

    Write-Host "⏱️  Average response time: $($mockAPIData.average_response_time)ms" -ForegroundColor White
    Write-Host "📊 P95 response time: $($mockAPIData.p95_response_time)ms" -ForegroundColor White
    Write-Host "❌ Error rate: $([math]::Round($mockAPIData.error_rate * 100, 2))%" -ForegroundColor White

    return $mockAPIData
}

# Function to get user engagement metrics
function Get-UserEngagement {
    param([int]$Hours)

    Write-Host "👥 Checking user engagement..." -ForegroundColor Yellow

    # This would integrate with your analytics service (Firebase Analytics, etc.)
    # For now, return mock data
    $mockEngagementData = @{
        daily_active_users = 5000
        average_session_duration = 900  # seconds
        retention_day1 = 0.75  # 75%
        retention_day7 = 0.45  # 45%
        retention_day30 = 0.25  # 25%
    }

    Write-Host "📈 Daily active users: $($mockEngagementData.daily_active_users)" -ForegroundColor White
    Write-Host "⏰ Average session: $([math]::Round($mockEngagementData.average_session_duration / 60, 1)) minutes" -ForegroundColor White
    Write-Host "🔄 Day 1 retention: $([math]::Round($mockEngagementData.retention_day1 * 100, 1))%" -ForegroundColor White
    Write-Host "🔄 Day 7 retention: $([math]::Round($mockEngagementData.retention_day7 * 100, 1))%" -ForegroundColor White
    Write-Host "🔄 Day 30 retention: $([math]::Round($mockEngagementData.retention_day30 * 100, 1))%" -ForegroundColor White

    return $mockEngagementData
}

# Function to get app store ratings
function Get-AppStoreRatings {

    Write-Host "⭐ Checking app store ratings..." -ForegroundColor Yellow

    # This would integrate with app store APIs
    # For now, return mock data
    $mockRatingData = @{
        android_rating = 4.2
        android_reviews = 1250
        ios_rating = 4.4
        ios_reviews = 890
    }

    Write-Host "🤖 Google Play: $($mockRatingData.android_rating)⭐ ($($mockRatingData.android_reviews) reviews)" -ForegroundColor White
    Write-Host "🍎 App Store: $($mockRatingData.ios_rating)⭐ ($($mockRatingData.ios_reviews) reviews)" -ForegroundColor White

    return $mockRatingData
}

# Function to send Slack alert
function Send-SlackAlert {
    param([string]$Message, [string]$Color = "danger")

    if (-not $SlackWebhookUrl) {
        Write-Host "⚠️  Slack webhook not configured" -ForegroundColor Yellow
        return
    }

    $payload = @{
        text = "🚨 Crimson Compass Alert"
        attachments = @(
            @{
                color = $Color
                text = $Message
                footer = "Crimson Compass Monitoring"
                ts = [int][double]::Parse((Get-Date -UFormat %s))
            }
        )
    }

    try {
        Invoke-RestMethod -Uri $SlackWebhookUrl -Method Post -Body ($payload | ConvertTo-Json) -ContentType "application/json"
        Write-Host "📢 Alert sent to Slack" -ForegroundColor Green
    }
    catch {
        Write-Host "❌ Failed to send Slack alert: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Function to check thresholds and alert
function Test-Thresholds {
    param([string]$MetricName, [double]$Value, [double]$CriticalThreshold, [double]$WarningThreshold)

    if ($Value -ge $CriticalThreshold) {
        $message = "🚨 CRITICAL: $MetricName is at $([math]::Round($Value * 100, 2))% (threshold: $([math]::Round($CriticalThreshold * 100, 2))%)"
        Write-Host $message -ForegroundColor Red
        if ($SendAlerts) {
            Send-SlackAlert -Message $message -Color "danger"
        }
        return $true
    }
    elseif ($Value -ge $WarningThreshold) {
        $message = "⚠️  WARNING: $MetricName is at $([math]::Round($Value * 100, 2))% (threshold: $([math]::Round($WarningThreshold * 100, 2))%)"
        Write-Host $message -ForegroundColor Yellow
        if ($SendAlerts) {
            Send-SlackAlert -Message $message -Color "warning"
        }
        return $true
    }

    return $false
}

# Main monitoring execution
try {
    Write-Host "Monitoring last $TimeWindowHours hours..." -ForegroundColor Cyan
    Write-Host "Alerts: $(if ($SendAlerts) { "Enabled" } else { "Disabled" })" -ForegroundColor White
    Write-Host "" -ForegroundColor White

    $alertsTriggered = 0

    # Check crash rates
    $crashRate = Get-CrashRate -Hours $TimeWindowHours
    if (Test-Thresholds -MetricName "Crash Rate" -Value $crashRate -CriticalThreshold $CriticalThresholds.CrashRate -WarningThreshold $WarningThresholds.CrashRate) {
        $alertsTriggered++
    }

    Write-Host ""

    # Check API performance
    $apiData = Get-APIResponseTime -Hours $TimeWindowHours
    if ($apiData.error_rate -ge $CriticalThresholds.ErrorRate) {
        $message = "🚨 CRITICAL: API Error Rate is at $([math]::Round($apiData.error_rate * 100, 2))% (threshold: $([math]::Round($CriticalThresholds.ErrorRate * 100, 2))%)"
        Write-Host $message -ForegroundColor Red
        if ($SendAlerts) { Send-SlackAlert -Message $message -Color "danger" }
        $alertsTriggered++
    }
    elseif ($apiData.error_rate -ge $WarningThresholds.ErrorRate) {
        $message = "⚠️  WARNING: API Error Rate is at $([math]::Round($apiData.error_rate * 100, 2))% (threshold: $([math]::Round($WarningThresholds.ErrorRate * 100, 2))%)"
        Write-Host $message -ForegroundColor Yellow
        if ($SendAlerts) { Send-SlackAlert -Message $message -Color "warning" }
        $alertsTriggered++
    }

    Write-Host ""

    # Check user engagement
    $engagementData = Get-UserEngagement -Hours $TimeWindowHours

    Write-Host ""

    # Check app store ratings
    $ratingData = Get-AppStoreRatings

    Write-Host ""

    # Summary
    Write-Host "📋 Monitoring Summary" -ForegroundColor Green
    Write-Host "===================" -ForegroundColor Green

    if ($alertsTriggered -eq 0) {
        Write-Host "✅ All metrics within acceptable ranges" -ForegroundColor Green
        $summaryMessage = "✅ Crimson Compass monitoring check passed - all metrics healthy"
        if ($SendAlerts) {
            Send-SlackAlert -Message $summaryMessage -Color "good"
        }
    } else {
        Write-Host "⚠️  $alertsTriggered alert(s) triggered" -ForegroundColor Yellow
    }

    Write-Host "⏰ Next check in 1 hour" -ForegroundColor White

}
catch {
    Write-Host "`n❌ Monitoring failed: $($_.Exception.Message)" -ForegroundColor Red
    if ($SendAlerts) {
        Send-SlackAlert -Message "❌ Monitoring system failure: $($_.Exception.Message)" -Color "danger"
    }
    exit 1
}</content>
<parameter name="filePath">c:\Users\gripa\OneDrive\Desktop\CrimsonCompass1\CrimsonCompass\monitor-launch.ps1