# Episode 1 Manual Setup Guide

Since automated Unity batch mode has compilation issues, here's the manual setup process:

## Step 1: Open Unity Editor
1. Navigate to your Crimson Compass project folder
2. Double-click the `CrimsonCompass.sln` file or open Unity Hub and select the project

## Step 2: Load the Scene
1. In Unity Editor, go to `File > Open Scene`
2. Navigate to `Assets/Scenes/S01E01_AgencyBriefingRoom.unity`
3. Click `Open`

## Step 3: Add Test Components
1. In the Hierarchy window, right-click and select `Create Empty`
2. Name it `Episode1TestSetup`
3. With the new GameObject selected, click `Add Component` in the Inspector
4. Search for and add `Episode1TestInitializer`

## Step 4: Test the Implementation
1. Click the `Play` button at the top of the Unity Editor
2. The scene should initialize with:
   - Colored placeholder objects (Desk Terminal = Blue, Filing Cabinet = Gray, Coffee Machine = Red, Door = Green)
   - NPC characters (Helix = Cyan, Optimus = Yellow, ZTech = Magenta, Gasket = White)
   - Player character (Black square at bottom)

## Controls
- **Click objects** to interact based on selected verb
- **Verb bar** at bottom to change interaction modes (MOVE, OBSERVE, ENGAGE, KIT, PROTOCOL)
- **KIT verb** opens inventory
- **Click NPCs** to start dialogue

## Expected Behavior
- Onboarding sequence should start automatically
- Verb bar should unlock progressively
- Clicking objects should show debug messages
- NPCs should respond to dialogue interactions

## Troubleshooting
- If components don't appear, check the Console for errors
- Make sure all scripts are in the correct folders
- Verify the scene file exists and loads properly