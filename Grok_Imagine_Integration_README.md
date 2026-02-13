# Crimson Compass Grok Imagine Integration Guide

## Overview

This guide covers the integration of **Grok Imagine** (xAI's AI image and video generation tool) into the Crimson Compass visual production pipeline. Grok Imagine provides faster generation, better style consistency, and video capabilities compared to traditional tools.

## What is Grok Imagine?

**Grok Imagine** is xAI's cutting-edge AI image and video generator powered by Grok 4 and Flux models. Key features:

- **Text-to-Image**: Generate photorealistic images and surreal art
- **Image-to-Video**: Create short videos (6-15 seconds) with natural motion
- **Image Editing**: Transform and edit existing images
- **Audio Generation**: Add audio to videos
- **Style Consistency**: Maintains visual coherence across generations

**Access**: Available at [grok.com/imagine](https://grok.com/imagine)

## Converted Prompts

All original Midjourney prompts have been converted to Grok Imagine format in `docs/Visual_Prompts_Grok_Imagine.md`. The conversion includes:

- **5-Pillar Framework**: Subject & Character, Action & Motion, Environment & Setting, Cinematic Framing & Camera, Aesthetic & Style
- **Natural Language**: Aspect ratios described naturally ("9:16 vertical composition")
- **Video Prompts**: Added dynamic scenes for character movements and gadget demonstrations
- **Mobile Optimization**: All prompts include mobile-first specifications

## Quick Start

### 1. Manual Generation (Recommended for Quality Control)

```bash
# Open Grok Imagine in your browser
start https://grok.com/imagine

# Copy prompts from docs/Visual_Prompts_Grok_Imagine.md
# Generate assets one by one for best quality control
```

### 2. Batch Generation Script

```powershell
# Run the batch generation script
.\scripts\batch_generate_grok_assets.ps1 -DryRun

# For actual generation (opens browser tabs)
.\scripts\batch_generate_grok_assets.ps1
```

### 3. Full Unity Workflow

```powershell
# Complete pipeline: generation -> import -> optimization
.\scripts\unity_workflow.ps1

# Dry run to see what would happen
.\scripts\unity_workflow.ps1 -DryRun
```

### 4. API Integration (Programmatic Generation)

For automated generation within Unity, use the integrated API service:

#### Setup
1. **Get API Key**: Sign up at [x.ai](https://x.ai) and get your API key
2. **Configure Service**: Add the `GrokImagineService` component to a GameObject in your scene
3. **Set API Key**: Enter your API key in the service component (or set via code)

#### Usage in Unity
```csharp
using CrimsonCompass;

// Get the service
var grokService = FindObjectOfType<GrokImagineService>();

// Generate an image
grokService.GenerateImage(
    "Futurama Detective Noir + Uncharted Action-Adventure fusion, rainy city street at night",
    "16:9",
    (url) => Debug.Log($"Image ready: {url}"),
    (error) => Debug.LogError($"Generation failed: {error}")
);

// Generate multiple images
grokService.GenerateImage(
    "Detective character portrait",
    "1:1",
    4, // Generate 4 variations
    "url", // Return URLs (or "base64" for embedded data)
    (url) => Debug.Log($"Image ready: {url}"),
    (error) => Debug.LogError($"Generation failed: {error}")
);

// Edit an existing image
grokService.EditImage(
    "Add a mysterious glowing artifact in the detective's hand",
    "https://example.com/existing-image.jpg",
    "16:9",
    (url) => Debug.Log($"Edited image ready: {url}"),
    (error) => Debug.LogError($"Edit failed: {error}")
);

// Generate a video
grokService.GenerateVideo(
    "Dynamic scene of detective examining evidence, handheld camera, slow deliberate movement",
    8,
    "9:16",
    (url) => Debug.Log($"Video ready: {url}"),
    (error) => Debug.LogError($"Generation failed: {error}")
);
```

#### Manager Class
Use `GrokImagineManager` for high-level asset generation:

```csharp
var manager = FindObjectOfType<GrokImagineManager>();

// Generate background assets
manager.GenerateBackgroundAssets();

// Generate character portraits
manager.GenerateCharacterAssets();

// Generate UI elements
manager.GenerateUIAssets();
```

#### API Features
- **Image Generation**: Text-to-image with custom aspect ratios
- **Video Generation**: Text-to-video with duration and resolution control
- **Image Editing**: Modify existing images with natural language
- **Batch Processing**: Generate multiple assets concurrently
- **Base64 Output**: Get images as embedded data instead of URLs
- **Automatic Download**: Assets are saved to `Assets/GeneratedAssets/GrokImagine/`

#### API Parameters

**GenerateImage Parameters:**
- `prompt` (string): Text description of the image to generate
- `aspectRatio` (string, optional): Aspect ratio (e.g., "16:9", "1:1", "9:16"). Default: "16:9"
- `n` (int, optional): Number of images to generate (1-10). Default: 1
- `imageFormat` (string, optional): "url" for temporary URLs or "base64" for embedded data. Default: "url"
- `onComplete` (Action<string>): Callback with generated image URL or base64 data
- `onError` (Action<string>): Callback with error message

**EditImage Parameters:**
- `prompt` (string): Description of the edits to apply
- `imageUrl` (string): URL of the source image to edit
- `aspectRatio` (string, optional): Aspect ratio. Default: "16:9"
- `n` (int, optional): Number of edited versions. Default: 1
- `imageFormat` (string, optional): "url" or "base64". Default: "url"
- `onComplete` (Action<string>): Callback with edited image URL or data
- `onError` (Action<string>): Callback with error message

**Supported Aspect Ratios:**
- "1:1" - Square (social media, thumbnails)
- "16:9" - Widescreen (presentations, banners)
- "9:16" - Vertical (mobile, stories)
- "4:3" - Classic (photography)
- "3:2" - Photography
- "2:1" - Ultra-wide
- "auto" - Model auto-selects best ratio

#### Editor Testing Tool
Use the Unity Editor tool to test the API integration:

1. **Open the Editor Tool**:
   - Go to `Crimson Compass > Grok API Tester` in the Unity menu
2. **Enter your API key** (if not already set)
3. **Test image/video generation**:
   - Switch to "Image/Video Generation" tab
   - Try the preset buttons or enter custom prompts
   - Click "Generate Image" or "Generate Video" to test
4. **Test chat completions**:
   - Switch to "Chat Completions" tab
   - Enter system and user messages
   - Click "Send Chat Message" to test
5. **Use quick test buttons** for common asset types and chat examples

## Grok Chat API Integration

In addition to image/video generation, the project now includes integration with Grok's chat completions API for interactive AI assistance.

### Chat API Features
- **Conversational AI**: Multi-turn conversations with context retention
- **System Prompts**: Customizable AI behavior and personality
- **Game Integration**: Specialized prompts for Crimson Compass gameplay
- **Clue Assistance**: AI help with detective puzzles and story elements
- **Story Guidance**: Lore explanations and narrative support

### Chat API Setup
```csharp
using CrimsonCompass;

// Add GrokChatService to a GameObject
var chatService = gameObject.AddComponent<GrokChatService>();

// Send a simple message
chatService.SendChatMessage(
    "Hello, can you help me with a clue?",
    "You are a helpful AI assistant for the Crimson Compass detective game.",
    (response) => Debug.Log($"AI: {response}"),
    (error) => Debug.LogError($"Error: {error}")
);
```

### Chat Manager Usage
```csharp
// Use GrokChatManager for conversation management
var chatManager = FindObjectOfType<GrokChatManager>();

// Ask for clue help
chatManager.AskForClueHelp("The butler says he was in the kitchen...");

// Ask about story elements
chatManager.AskAboutStory("What's the significance of the crimson compass?");

// Get gameplay tips
chatManager.AskForGameplayTips();
```

### API Key Configuration
The API key has been integrated into the service components. For security, API keys should be set via the Unity Inspector or PlayerPrefs, not hardcoded in documentation.

## Asset Categories (58 Total)

| Category | Count | Description |
|----------|-------|-------------|
| Backgrounds | 12 | Episode environments and settings |
| Characters | 15 | Player, team members, and NPCs |
| Objects | 20 | Gadgets, evidence, relics, and props |
| UI | 16 | Interface elements and interactions |
| Effects | 3 | Atmospheric and special effects |
| Videos | 8 | Dynamic scenes and demonstrations |

## Prompt Engineering Tips

### Core Framework (5 Pillars)

1. **Subject & Character** (30% weight - first 20-30 words)
   - Define the main element and its key traits
   - Example: "A determined female detective in trench coat and fedora"

2. **Action & Motion** (For videos - explicit verbs with physics)
   - Use linear actions, explicit motion verbs, camera directions
   - Example: "slow deliberate movement, handheld camera following"

3. **Environment & Setting** (Sensory details, scale)
   - Include lighting, atmosphere, spatial relationships
   - Example: "rain-slicked surfaces reflecting neon signs"

4. **Cinematic Framing & Camera** (Perspective control)
   - Lens types, shot composition, aspect ratios
   - Example: "9:16 vertical composition, cinematic noir atmosphere"

5. **Aesthetic & Style** (Tone and polish)
   - Lighting, mood, art style, technical specifications
   - Example: "high contrast shadows, atmospheric lighting, cel-shaded illustration"

### Optimization Strategies

- **Length**: 50-200 words for balanced results
- **Consistency**: Always start with "Futurama Detective Noir + Uncharted Action-Adventure fusion"
- **Video Duration**: Maximum 6-15 seconds per clip
- **Mobile-First**: Include "mobile game art, stylized" in all prompts

## Workflow Integration

### Phase 1: Asset Generation
- Use Grok Imagine to generate all 58 assets
- Generate 3-5 variations per asset for selection
- Save with descriptive filenames: `{AssetName}_{Category}_{Timestamp}.png`

### Phase 2: Quality Control
- Review generated assets for style consistency
- Check file sizes (target <1MB for mobile)
- Ensure proper aspect ratios and resolutions

### Phase 3: Unity Import
- Run the Unity workflow script to import assets
- Assets are automatically organized into proper folders
- Compression and optimization applied automatically

### Phase 4: Testing & Iteration
- Test assets in Unity scenes
- Adjust prompts if needed
- Re-run workflow for improvements

## File Organization

```
CrimsonCompass/
├── docs/
│   ├── Visual_Prompts_Grok_Imagine.md    # Converted prompts
│   └── Season1_Visual_Production_Plan.md # Original plan
├── scripts/
│   ├── batch_generate_grok_assets.ps1    # Batch generation
│   └── unity_workflow.ps1                 # Full pipeline
├── GeneratedAssets/                       # AI-generated files
│   ├── Backgrounds/
│   ├── Characters/
│   ├── Objects/
│   ├── UI/
│   ├── Effects/
│   └── Videos/
└── Assets/
    └── Sprites/                          # Unity imports
        ├── Backgrounds/
        ├── Characters/
        └── ...
```

## Unity Integration

### Automatic Processing
The workflow includes a Unity editor script (`Assets/Editor/CrimsonCompassAssetImporter.cs`) that:

- **Texture Optimization**: Sets appropriate compression for mobile
- **Sprite Configuration**: Configures sprites for 2D game use
- **Size Limits**: Backgrounds (2048px), Characters/Objects (1024px), UI (512px)
- **Quality Settings**: Balanced compression for file size vs quality

### Manual Unity Steps
1. Open Unity project
2. Run: `Crimson Compass > Process Imported Assets`
3. Run: `Crimson Compass > Generate Asset Report`
4. Check the generated report for import status

## Troubleshooting

### Common Issues

**"Grok Imagine not accessible"**
- Ensure you have a xAI account
- Check if you're using SuperGrok for unlimited generations
- Try accessing via different browser

**"Assets too large for mobile"**
- Check the quality settings in Unity importer
- Regenerate with higher compression prompts
- Use the optimization script

**"Style inconsistency"**
- Review the 5-pillar framework in prompts
- Ensure consistent descriptors across similar assets
- Generate variations and select the best ones

**"Unity import fails"**
- Check file paths in the workflow script
- Ensure Unity editor is closed during import
- Run the workflow with -DryRun first

### Performance Tips

- **Batch Size**: Generate in small batches (5-10 assets) to maintain quality control
- **File Naming**: Use consistent naming: `{Category}_{AssetName}_{Variation}_{Timestamp}`
- **Backup**: Always backup before running Unity import
- **Testing**: Test on target device early in the process

## Advanced Features

### Video Generation
Grok Imagine supports short video clips (6-15 seconds):
- Character movement demonstrations
- Gadget functionality animations
- Atmospheric scene transitions
- UI interaction previews

### Image-to-Image Editing
- Refine existing generations
- Add details to base assets
- Create variations of approved designs

### Custom Workflows
The scripts are modular - you can:
- Run only generation phase
- Skip Unity import for manual organization
- Customize asset mapping for your project structure

## Support & Resources

- **Grok Imagine**: [grok.com/imagine](https://grok.com/imagine)
- **Documentation**: Check `docs/Visual_Prompts_Grok_Imagine.md` for all prompts
- **Scripts**: Located in `scripts/` directory
- **Logs**: Check `GeneratedAssets/Logs/` for generation reports

## Next Steps

1. **Review Prompts**: Read through `docs/Visual_Prompts_Grok_Imagine.md`
2. **Test Generation**: Try generating a few sample assets
3. **Run Workflow**: Execute the full pipeline on a small batch
4. **Iterate**: Refine prompts based on results
5. **Scale Up**: Generate all 58 assets for Season 1

The Grok Imagine integration provides a modern, efficient workflow for visual production while maintaining the unique Futurama Detective Noir + Uncharted Action-Adventure fusion style.