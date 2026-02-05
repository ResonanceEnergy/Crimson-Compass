Param(
  [Parameter(Mandatory=$true)][string]$RemoteUrl,
  [string]$ProjectPath=(Join-Path $PSScriptRoot "..\CrimsonCompass")
)
Set-Location $ProjectPath
git init
# git lfs install  # later if needed

git add .
try { git commit -m "Initial commit: Crimson Compass one-drop" } catch { }

git branch -M main
try { git remote remove origin } catch { }

git remote add origin $RemoteUrl
git push -u origin main
