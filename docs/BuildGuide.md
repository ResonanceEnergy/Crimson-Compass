# Build Guide

## Android Build Setup
1. In Unity: File > Build Settings.
2. Select Android platform, click Switch Platform.
3. Set Player Settings:
   - Company Name: Resonance Energy
   - Product Name: Crimson Compass
   - Version: 0.1
   - Bundle Identifier: com.resonanceenergy.crimsoncompass
4. Enable ARM64, ARMv7 architectures.
5. Set Minimum API Level: 21 (Android 5.0).
6. Add MainScene to Scenes in Build.
7. Click Build, select output folder (e.g., Builds/Android).
8. Unity generates APK.

## Optimization
- Use Unity's Addressables for assets.
- Enable IL2CPP scripting backend.
- Set compression to LZ4HC.
- Test on device via ADB: `adb install -r CrimsonCompass.apk`

## Release
- Sign APK with keystore.
- Upload to Google Play Console.
- Set up internal testing track.