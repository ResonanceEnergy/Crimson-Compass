# PowerShell script to automate AudioCatalog creation

Write-Host "Launching Unity Editor for AudioCatalog setup..." -ForegroundColor Green
Start-Process "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe" -ArgumentList "-projectPath", "C:\Users\gripa\OneDrive\Desktop\CrimsonCompass\Crimson-Compass"

Write-Host "Unity Editor launched. Please manually run:" -ForegroundColor Yellow
Write-Host "  Audio -> Setup Crimson Compass Audio System" -ForegroundColor Yellow
Write-Host "Then close Unity and press Enter to continue validation..." -ForegroundColor Yellow

Read-Host "Press Enter after running the menu command and closing Unity"

Write-Host "Running validation..." -ForegroundColor Green
python Tools/validate_audio_events.py

Write-Host "Running build test..." -ForegroundColor Green
python Tools/build.py --preflight

Write-Host "Automation complete!" -ForegroundColor Green