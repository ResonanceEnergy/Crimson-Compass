#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Crimson Compass Workflow Automation (Test Version)
.DESCRIPTION
    Simplified test version of the workflow automation
#>

param(
    [switch]$SkipUnity,
    [switch]$SkipBuild,
    [switch]$SkipTest
)

Write-Host "Testing workflow automation..." -ForegroundColor Green

# Test validation
Write-Host "Phase 1: Validation" -ForegroundColor Yellow
try {
    & python "Tools\validate_episode_data.py"
    if ($LASTEXITCODE -ne 0) { throw "Validation failed" }
    Write-Host "Validation passed!" -ForegroundColor Green
} catch {
    Write-Host "Validation failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "Test completed!" -ForegroundColor Green