# Crimson Compass Visual Assets

## Folder Structure
- **Backgrounds/**: Scene backgrounds and environments
- **Characters/**: Character portraits and sprites
- **UI/**: User interface elements
- **Props/**: Evidence items, interactive objects, and decorations

## Naming Convention
`s[season]e[episode]_[asset_type]_[variant]_[state].png`

Examples:
- `s01e01_background_rooftop_dusk.png`
- `s01e01_character_player_neutral.png`
- `s01e01_ui_dialogue_frame.png`
- `s01e01_prop_access_ring.png`

## Technical Specifications
- **Resolution**: 1920x1080 (9:16) for backgrounds, 1080x1350 (3:4) for characters
- **Format**: PNG with transparency where needed
- **Color Depth**: 32-bit RGBA
- **File Size**: Optimize for <1MB per asset
- **Style**: Futurama Detective Noir - thick black outlines, bright saturated colors, exaggerated features, retro-futuristic absurdity

## Generation Workflow
1. Use prompts from `Assets/Art_Prompts/`
2. Generate with Midjourney/DALL-E
3. Post-process in Photoshop/GIMP
4. Save to appropriate folder with correct naming
5. Import to Unity and test

## Current Focus: S01E01
See `Assets/Art_Prompts/S01E01_Visual_Plan.md` for episode-specific requirements.</content>
<parameter name="filePath">c:\Users\gripa\OneDrive\Desktop\CrimsonCompass1\CrimsonCompass\Assets\Visuals\README.md