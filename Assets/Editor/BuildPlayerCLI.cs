using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UnityEditor
{
    /// <summary>
    /// Command-line build entrypoint.
    /// </summary>
    public class BuildPlayerCLI : EditorWindow
    {
        [MenuItem("Tools/Build Game")]
        public static void PerformBuild()
        {
            try
            {
                Debug.Log("EXECUTE METHOD CALLED: BuildPlayerCLI.PerformBuild()");

                // Get command line arguments
                string[] args = System.Environment.GetCommandLineArgs();
                Debug.Log($"Command line args: {string.Join(" ", args)}");

                // Parse arguments
                string buildPath = GetArg(args, "-buildPath", "Builds/Win64/CrimsonCompass.exe");
                string buildTargetStr = GetArg(args, "-buildTarget", "Win64");
                bool devBuild = GetArg(args, "-devBuild", "false").ToLower() == "true";

                Debug.Log($"Build path: {buildPath}");
                Debug.Log($"Build target: {buildTargetStr}");
                Debug.Log($"Dev build: {devBuild}");

                BuildTarget buildTarget = ParseBuildTarget(buildTargetStr);

                // Recreate the MainScene to ensure it's properly set up
                SceneSetup.SetupMainScene();

                // Hardcode the MainScene for now
                var scenes = new[] { "Assets/Scenes/MainScene.unity" };

                Debug.Log($"Building with {scenes.Length} scenes: {string.Join(", ", scenes)}");

                // Set up build settings
                var buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = scenes,
                    locationPathName = buildPath,
                    target = buildTarget,
                    options = devBuild ? BuildOptions.Development : BuildOptions.None
                };

                Debug.Log("Starting build...");
                BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
                BuildSummary summary = report.summary;

                Debug.Log($"Build result: {summary.result}");
                Debug.Log($"Build size: {summary.totalSize} bytes");
                Debug.Log($"Build time: {summary.totalTime}");
                Debug.Log($"Errors: {summary.totalErrors}, Warnings: {summary.totalWarnings}");

                // Write manifest
                WriteManifest(buildPath, summary);

                // Exit with appropriate code
                int exitCode = summary.result == BuildResult.Succeeded ? 0 : 1;
                Debug.Log($"Exiting with code: {exitCode}");
                if (summary.result != BuildResult.Succeeded)
                {
                    EditorApplication.Exit(exitCode);
                }
                // If build succeeded, let Unity exit naturally in batch mode
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Exception in PerformBuild: {ex.Message}");
                Debug.LogError($"Stack trace: {ex.StackTrace}");
                EditorApplication.Exit(1);
            }
        }

        private static void WriteManifest(string buildPath, BuildSummary summary)
        {
            var manifestPath = Path.Combine(Path.GetDirectoryName(buildPath), "build_manifest.txt");
            string manifest = string.Format(
                "Result: {0}\nPlatform: {1}\nOutput: {2}\nSize: {3} bytes\nTime: {4}\nErrors: {5}\nWarnings: {6}\nUTC: {7}",
                summary.result,
                summary.platform,
                buildPath,
                summary.totalSize,
                summary.totalTime,
                summary.totalErrors,
                summary.totalWarnings,
                DateTime.UtcNow.ToString("O")
            );
            File.WriteAllText(manifestPath, manifest);
        }

        private static string GetArg(string[] args, string name, string defaultValue)
        {
            int index = Array.IndexOf(args, name);
            if (index >= 0 && index < args.Length - 1) return args[index + 1];
            return defaultValue;
        }

        private static BuildTarget ParseBuildTarget(string arg)
        {
            switch (arg)
            {
                case "Win64":
                    return BuildTarget.StandaloneWindows64;
                case "Mac":
                    return BuildTarget.StandaloneOSX;
                case "Linux64":
                    return BuildTarget.StandaloneLinux64;
                case "Android":
                    return BuildTarget.Android;
                case "iOS":
                    return BuildTarget.iOS;
                default:
                    return BuildTarget.StandaloneWindows64;
            }
        }
    }
}
