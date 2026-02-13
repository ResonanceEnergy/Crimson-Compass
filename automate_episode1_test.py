#!/usr/bin/env python3
"""
Crimson Compass Episode 1 Automation Script
Automates Unity Editor operations for testing Episode 1 implementation
"""

import os
import sys
import subprocess
import time
import platform

class UnityEpisode1Automator:
    def __init__(self):
        self.project_path = self._get_project_path()
        self.unity_path = self._find_unity_executable()
        self.scene_path = "Assets/Scenes/S01E01_AgencyBriefingRoom.unity"

    def _get_project_path(self):
        """Get the Crimson Compass project path"""
        script_dir = os.path.dirname(os.path.abspath(__file__))
        project_path = os.path.join(script_dir, "..")
        return os.path.abspath(project_path)

    def _find_unity_executable(self):
        """Find Unity executable path"""
        possible_paths = [
            "C:\\Program Files\\Unity\\Hub\\Editor\\2022.3.62f3\\Editor\\Unity.exe",
            "C:\\Program Files\\Unity\\Hub\\Editor\\2022.3.20f1\\Editor\\Unity.exe",
            "C:\\Program Files\\Unity\\Hub\\Editor\\2022.3.19f1\\Editor\\Unity.exe",
            "C:\\Program Files\\Unity\\Hub\\Editor\\2022.3.18f1\\Editor\\Unity.exe",
            "C:\\Program Files\\Unity\\Hub\\Editor\\6000.3.6f1\\Editor\\Unity.exe",
            "/Applications/Unity/Hub/Editor/2022.3.20f1/Unity.app/Contents/MacOS/Unity",
            "/Applications/Unity/Hub/Editor/2022.3.19f1/Unity.app/Contents/MacOS/Unity",
        ]

        for path in possible_paths:
            if os.path.exists(path):
                return path

        # Try to find Unity in PATH
        try:
            result = subprocess.run(["which", "unity"], capture_output=True, text=True)
            if result.returncode == 0:
                return result.stdout.strip()
        except:
            pass

        raise FileNotFoundError("Unity executable not found. Please install Unity 2022.3.x or update the path in this script.")

    def create_unity_test_script(self):
        """Create a Unity editor script to automate the setup"""
        script_content = '''
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace CrimsonCompass.Editor
{
    public class Episode1TestAutomation
    {
        [MenuItem("CrimsonCompass/Automate Episode 1 Test")]
        static void AutomateEpisode1Test()
        {
            Debug.Log("Starting Episode 1 automation...");

            // Load the scene
            string scenePath = "Assets/Scenes/S01E01_AgencyBriefingRoom.unity";
            if (!System.IO.File.Exists(scenePath))
            {
                Debug.LogError($"Scene not found: {scenePath}");
                return;
            }

            EditorSceneManager.OpenScene(scenePath);
            Debug.Log("Scene loaded successfully");

            // Find or create a setup GameObject
            GameObject setupObject = GameObject.Find("Episode1TestSetup");
            if (setupObject == null)
            {
                setupObject = new GameObject("Episode1TestSetup");
                Debug.Log("Created Episode1TestSetup GameObject");
            }

            // Add the test setup component
            Episode1SceneTestSetup testSetup = setupObject.GetComponent<Episode1SceneTestSetup>();
            if (testSetup == null)
            {
                testSetup = setupObject.AddComponent<Episode1SceneTestSetup>();
                Debug.Log("Added Episode1SceneTestSetup component");
            }

            // Add the test runner component
            Episode1TestRunner testRunner = setupObject.GetComponent<Episode1TestRunner>();
            if (testRunner == null)
            {
                testRunner = setupObject.AddComponent<Episode1TestRunner>();
                Debug.Log("Added Episode1TestRunner component");
            }

            // Save the scene
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            Debug.Log("Scene saved with test components");

            // Start play mode
            Debug.Log("Starting Play Mode...");
            EditorApplication.EnterPlaymode();

            Debug.Log("Episode 1 automation complete! The game should now be running in Play Mode.");
            Debug.Log("Controls:");
            Debug.Log("- Click objects to interact based on selected verb");
            Debug.Log("- Use bottom verb bar to change interaction modes");
            Debug.Log("- KIT verb opens inventory");
            Debug.Log("- Click NPCs to start dialogue");
        }

        [MenuItem("CrimsonCompass/Stop Play Mode")]
        static void StopPlayMode()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.ExitPlaymode();
                Debug.Log("Exited Play Mode");
            }
            else
            {
                Debug.Log("Not in Play Mode");
            }
        }
    }
}
'''

        script_path = os.path.join(self.project_path, "Assets", "Scripts", "Editor", "Episode1TestAutomation.cs")

        # Create Editor directory if it doesn't exist
        os.makedirs(os.path.dirname(script_path), exist_ok=True)

        with open(script_path, 'w') as f:
            f.write(script_content)

        print(f"Created Unity automation script: {script_path}")
        return script_path

    def run_unity_automation(self):
        """Run Unity with the automation script"""
        print("Starting Unity automation...")

        # Create the automation script first
        self.create_unity_test_script()

        # Unity command line arguments
        args = [
            self.unity_path,
            "-projectPath", self.project_path,
            "-executeMethod", "CrimsonCompass.Editor.Episode1TestAutomation.AutomateEpisode1Test",
            "-batchmode",
            "-nographics",
            "-quit"
        ]

        print(f"Running Unity command: {' '.join(args)}")

        try:
            result = subprocess.run(args, capture_output=True, text=True, timeout=300)

            print("Unity automation completed!")
            print("STDOUT:", result.stdout)
            if result.stderr:
                print("STDERR:", result.stderr)

            if result.returncode == 0:
                print("✅ Episode 1 test automation successful!")
                print("\nNext steps:")
                print("1. Open Unity Editor manually")
                print("2. Load the S01E01_AgencyBriefingRoom scene")
                print("3. The scene should now have the test components attached")
                print("4. Press Play to test the implementation")
            else:
                print(f"❌ Unity automation failed with return code: {result.returncode}")

        except subprocess.TimeoutExpired:
            print("❌ Unity automation timed out after 5 minutes")
        except Exception as e:
            print(f"❌ Error running Unity automation: {e}")

    def create_batch_file(self):
        """Create a Windows batch file for easy execution"""
        batch_content = f'''@echo off
echo Crimson Compass Episode 1 Test Automation
echo ========================================
echo.

python "{os.path.abspath(__file__)}"

echo.
echo Press any key to exit...
pause > nul
'''

        batch_path = os.path.join(self.project_path, "run_episode1_test.bat")
        with open(batch_path, 'w') as f:
            f.write(batch_content)

        print(f"Created batch file: {batch_path}")
        return batch_path

def main():
    print("Crimson Compass Episode 1 Automation Script")
    print("=" * 50)

    try:
        automator = UnityEpisode1Automator()

        print(f"Project Path: {automator.project_path}")
        print(f"Unity Path: {automator.unity_path}")
        print()

        # Create batch file for easy execution
        automator.create_batch_file()

        # Run the automation
        automator.run_unity_automation()

    except Exception as e:
        print(f"❌ Error: {e}")
        sys.exit(1)

if __name__ == "__main__":
    main()