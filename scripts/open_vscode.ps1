Param([string]$ProjectPath=(Join-Path $PSScriptRoot "..\CrimsonCompass"))
$codeCmd = Get-Command code -ErrorAction SilentlyContinue
if (-not $codeCmd) { Write-Host "VS Code 'code' command not found."; exit 1 }
code $ProjectPath
