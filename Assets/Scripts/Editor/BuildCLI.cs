using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildCLI
{
    public static void PerformBuild()
    {
        try
        {
            Debug.Log("EXECUTE METHOD CALLED: BuildCLI.PerformBuild()");

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

            // Check if MainScene exists
            string scenePath = "Assets/Scenes/MainScene.unity";
            if (!File.Exists(scenePath))
            {
                throw new FileNotFoundException($"Main scene not found: {scenePath}");
            }

            // Add scene to build settings
            var scenes = new[] { new EditorBuildSettingsScene(scenePath, true) };
            EditorBuildSettings.scenes = scenes;

            // Hardcode the MainScene for now
            var buildScenes = new[] { scenePath };

            Debug.Log($"Building with {buildScenes.Length} scenes: {string.Join(", ", buildScenes)}");

            // Set up build settings
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = buildScenes,
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
        string manifest = $"Result: {summary.result}\n" +
                         $"Platform: {summary.platform}\n" +
                         $"Output: {buildPath}\n" +
                         $"Size: {summary.totalSize} bytes\n" +
                         $"Time: {summary.totalTime}\n" +
                         $"Errors: {summary.totalErrors}\n" +
                         $"Warnings: {summary.totalWarnings}\n" +
                         $"UTC: {DateTime.UtcNow.ToString("O")}";
        File.WriteAllText(manifestPath, manifest);
    }

    private static string GetArg(string[] args, string name, string defaultValue)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && i + 1 < args.Length)
            {
                return args[i + 1];
            }
        }
        return defaultValue;
    }

    private static BuildTarget ParseBuildTarget(string target)
    {
        switch (target.ToLower())
        {
            case "win64": return BuildTarget.StandaloneWindows64;
            case "mac": return BuildTarget.StandaloneOSX;
            case "linux64": return BuildTarget.StandaloneLinux64;
            case "android": return BuildTarget.Android;
            case "ios": return BuildTarget.iOS;
            default: throw new ArgumentException($"Unknown build target: {target}");
        }
    }
}