#!/usr/bin/env python3
"""Crimson Compass Repo Autogen

One Python entrypoint that CREATES and UPDATES repo scaffolding files deterministically.
Idempotent: safe to run repeatedly.
Supports autogen blocks inside files so you can keep manual edits.

Usage
  python Tools/cc_autogen.py init --unity-version 2022.3.62f3
  python Tools/cc_autogen.py audio
  python Tools/cc_autogen.py all

Autogen blocks
  // BEGIN CC_AUTOGEN:<name>
  // END CC_AUTOGEN:<name>

For MD/HTML
  <!-- BEGIN CC_AUTOGEN:<name> -->
  <!-- END CC_AUTOGEN:<name> -->

The generator replaces only what's inside blocks.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import os
import re
from dataclasses import dataclass
from pathlib import Path

AUTOGEN_TAG = "CC_AUTOGEN"


@dataclass
class WriteResult:
    path: Path
    changed: bool
    reason: str


def sha256_text(s: str) -> str:
    return hashlib.sha256(s.encode("utf-8")).hexdigest()


def ensure_dir(p: Path) -> None:
    p.mkdir(parents=True, exist_ok=True)


def write_text_if_changed(path: Path, content: str) -> WriteResult:
    ensure_dir(path.parent)
    if path.exists():
        old = path.read_text(encoding="utf-8")
        if sha256_text(old) == sha256_text(content):
            return WriteResult(path, False, "unchanged")
    path.write_text(content, encoding="utf-8")
    return WriteResult(path, True, "written")


def replace_autogen_block(existing: str, block_name: str, new_block_body: str) -> tuple[str, bool]:
    """Replace the contents inside an autogen block. Returns (new_text, changed)."""

    patterns = [
        rf"(^\s*//\s*BEGIN\s+{AUTOGEN_TAG}:{re.escape(block_name)}\s*$)(.*?)(^\s*//\s*END\s+{AUTOGEN_TAG}:{re.escape(block_name)}\s*$)",
        rf"(^\s*<!--\s*BEGIN\s+{AUTOGEN_TAG}:{re.escape(block_name)}\s*-->\s*$)(.*?)(^\s*<!--\s*END\s+{AUTOGEN_TAG}:{re.escape(block_name)}\s*-->\s*$)",
    ]

    for pat in patterns:
        m = re.search(pat, existing, flags=re.MULTILINE | re.DOTALL)
        if m:
            body = "\n" + new_block_body.rstrip() + "\n"
            replaced = existing[: m.start(2)] + body + existing[m.end(2) :]
            return replaced, (sha256_text(existing) != sha256_text(replaced))

    # Block not found -> append a new one at end
    if existing and not existing.endswith("\n"):
        existing += "\n"
    if new_block_body and not new_block_body.endswith("\n"):
        new_block_body += "\n"

    appended = (
        existing
        + f"// BEGIN {AUTOGEN_TAG}:{block_name}\n"
        + new_block_body
        + f"// END {AUTOGEN_TAG}:{block_name}\n"
    )
    return appended, True


def upsert_file_with_block(path: Path, block_name: str, new_block_body: str, *, header: str = "") -> WriteResult:
    ensure_dir(path.parent)
    if path.exists():
        existing = path.read_text(encoding="utf-8")
        updated, changed = replace_autogen_block(existing, block_name, new_block_body)
        if not changed:
            return WriteResult(path, False, "unchanged")
        path.write_text(updated, encoding="utf-8")
        return WriteResult(path, True, "block-updated")

    base = header
    if base and not base.endswith("\n"):
        base += "\n"
    created = (
        base
        + f"// BEGIN {AUTOGEN_TAG}:{block_name}\n"
        + new_block_body.rstrip()
        + "\n"
        + f"// END {AUTOGEN_TAG}:{block_name}\n"
    )
    path.write_text(created, encoding="utf-8")
    return WriteResult(path, True, "created")


# -------------------------------
# Templates
# -------------------------------

def tpl_buildplayer_cli_cs() -> str:
    return """using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>
/// Command-line build entrypoint.
/// Invoke with:
///   unity.exe -batchmode -nographics -quit -projectPath <path> -executeMethod BuildPlayerCLI.Build
///   -buildTarget Win64 -buildPath <output> -devBuild true
/// </summary>
public static class BuildPlayerCLI
{
    public static void Build()
    {
        try
        {
            var args = Environment.GetCommandLineArgs();

            string targetArg = GetArg(args, "-buildTarget", "Win64");
            string buildPath = GetArg(args, "-buildPath", "Builds/Win64/CrimsonCompass.exe");
            bool devBuild = bool.Parse(GetArg(args, "-devBuild", "false"));

            var target = ParseBuildTarget(targetArg);

            var scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if (scenes.Length == 0)
                throw new Exception("No enabled scenes found in Build Settings (File > Build Settings...).");

            Directory.CreateDirectory(Path.GetDirectoryName(buildPath));

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildPath,
                target = target,
                options = devBuild ? BuildOptions.Development : BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;

            WriteManifest(buildPath, summary);

            if (summary.result != BuildResult.Succeeded)
                throw new Exception($"Build failed: {summary.result} (Errors: {summary.totalErrors})");

            Debug.Log("Build succeeded.");
            EditorApplication.Exit(0);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            EditorApplication.Exit(1);
        }
    }

    private static void WriteManifest(string buildPath, BuildSummary summary)
    {
        var manifestPath = Path.Combine(Path.GetDirectoryName(buildPath), "build_manifest.txt");
        File.WriteAllText(manifestPath,
            $"Result: {summary.result}\n" +
            $"Platform: {summary.platform}\n" +
            $"Output: {buildPath}\n" +
            $"Size: {summary.totalSize} bytes\n" +
            $"Time: {summary.totalTime}\n" +
            $"Errors: {summary.totalErrors}\n" +
            $"Warnings: {summary.totalWarnings}\n" +
            $"UTC: {DateTime.UtcNow:O}\n");
    }

    private static string GetArg(string[] args, string name, string defaultValue)
    {
        int index = Array.IndexOf(args, name);
        if (index >= 0 && index < args.Length - 1) return args[index + 1];
        return defaultValue;
    }

    private static BuildTarget ParseBuildTarget(string arg)
    {
        return arg switch
        {
            "Win64" => BuildTarget.StandaloneWindows64,
            "Mac" => BuildTarget.StandaloneOSX,
            "Linux64" => BuildTarget.StandaloneLinux64,
            "Android" => BuildTarget.Android,
            "iOS" => BuildTarget.iOS,
            _ => BuildTarget.StandaloneWindows64
        };
    }
}
"""


def tpl_build_ps1(unity_version: str) -> str:
    # Construct without f-strings to avoid brace escaping pain.
    return (
        "param(\n"
        f"  [string]$UnityPath = \"C:\\Program Files\\Unity\\Hub\\Editor\\{unity_version}\\Editor\\Unity.exe\",\n"
        "  [string]$ProjectPath = (Resolve-Path \".\").Path,\n"  # Adjusted for root-level project
        "  [ValidateSet(\"Win64\",\"Mac\",\"Linux64\",\"Android\",\"iOS\")]\n"
        "  [string]$Target = \"Win64\",\n"
        "  [string]$OutDir = (Resolve-Path \"..\\Builds\").Path,\n"
        "  [switch]$DevBuild\n"
        ")\n"
        "\n"
        "$ErrorActionPreference = \"Stop\"\n"
        "\n"
        "$timestamp = (Get-Date).ToString(\"yyyyMMdd_HHmmss\")\n"
        "$targetDir = Join-Path $OutDir $Target\n"
        "New-Item -ItemType Directory -Force -Path $targetDir | Out-Null\n"
        "\n"
        "$buildPath = switch ($Target) {\n"
        "  \"Win64\"   { Join-Path $targetDir \"CrimsonCompass_$timestamp.exe\" }\n"
        "  \"Mac\"     { Join-Path $targetDir \"CrimsonCompass_$timestamp.app\" }\n"
        "  \"Linux64\" { Join-Path $targetDir \"CrimsonCompass_$timestamp.x86_64\" }\n"
        "  \"Android\" { Join-Path $targetDir \"CrimsonCompass_$timestamp.apk\" }\n"
        "  \"iOS\"     { Join-Path $targetDir \"iOSBuild_$timestamp\" }\n"
        "}\n"
        "\n"
        "$logDir = (Resolve-Path \"..\\Logs\").Path\n"
        "New-Item -ItemType Directory -Force -Path $logDir | Out-Null\n"
        "$logFile = Join-Path $logDir \"unity_build_$Target_$timestamp.log\"\n"
        "\n"
        "$dev = $DevBuild.IsPresent.ToString().ToLower()\n"
        "\n"
        "Write-Host \"Building: $Target\"\n"
        "Write-Host \"Unity   : $UnityPath\"\n"
        "Write-Host \"Project : $ProjectPath\"\n"
        "Write-Host \"Output  : $buildPath\"\n"
        "Write-Host \"Log     : $logFile\"\n"
        "\n"
        "if (-not (Test-Path $UnityPath)) {\n"
        f"  throw \"Unity executable not found at: $UnityPath. Update -UnityPath or install Unity {unity_version} in Unity Hub.\"\n"
        "}\n"
        "\n"
        "& $UnityPath `\n"
        "  -batchmode -nographics -quit `\n"
        "  -projectPath \"$ProjectPath\" `\n"
        "  -logFile \"$logFile\" `\n"
        "  -executeMethod BuildPlayerCLI.Build `\n"
        "  -buildTarget $Target `\n"
        "  -buildPath \"$buildPath\" `\n"
        "  -devBuild $dev\n"
        "\n"
        "$exitCode = $LASTEXITCODE\n"
        "if ($exitCode -ne 0) {\n"
        "  Write-Error \"Unity build failed with exit code $exitCode. Check log: $logFile\"\n"
        "  exit $exitCode\n"
        "}\n"
        "\n"
        "Write-Host \"Build OK: $buildPath\"\n"
    )


def tpl_build_py(unity_version: str) -> str:
    return f"""import argparse
import subprocess
import sys
from pathlib import Path


def run(cmd, cwd=None):
    print(">", " \".join(cmd))
    result = subprocess.run(cmd, cwd=cwd)
    if result.returncode != 0:
        sys.exit(result.returncode)


def main():
    p = argparse.ArgumentParser(description="Crimson Compass build orchestrator")
    p.add_argument("--target", default="Win64", choices=["Win64","Mac","Linux64","Android","iOS"])
    p.add_argument("--dev", action="store_true")
    p.add_argument("--unity", default=r"C:\\Program Files\\Unity\\Hub\\Editor\\{unity_version}\\Editor\\Unity.exe")
    p.add_argument("--preflight", action="store_true", help="Run preflight validators before building")
    args = p.parse_args()

    root = Path(__file__).resolve().parent.parent  # Adjusted for root-level project
    tools = root / "Tools"

    if args.preflight:
        run([sys.executable, str(tools / "validate_episode_data.py")])
        run([sys.executable, str(tools / "validate_audio_events.py")])

    ps1 = tools / "build.ps1"
    cmd = [
        "powershell", "-ExecutionPolicy", "Bypass",
        "-File", str(ps1),
        "-UnityPath", args.unity,
        "-Target", args.target
    ]
    if args.dev:
        cmd.append("-DevBuild")

    run(cmd)


if __name__ == "__main__":
    main()
"""


def tpl_vscode_tasks() -> str:
    tasks = {
        "version": "2.0.0",
        "tasks": [
            {
                "label": "Preflight (Validators)",
                "type": "shell",
                "command": "python",
                "args": ["Tools/build.py", "--preflight"],
                "problemMatcher": [],
                "group": "build",
            },
            {
                "label": "Build (Win64 Dev)",
                "type": "shell",
                "command": "python",
                "args": ["Tools/build.py", "--target", "Win64", "--dev"],
                "problemMatcher": [],
                "group": "build",
            },
            {
                "label": "Preflight + Build (Win64 Dev)",
                "type": "shell",
                "command": "python",
                "args": ["Tools/build.py", "--target", "Win64", "--dev", "--preflight"],
                "problemMatcher": [],
                "group": "build",
            },
        ],
    }
    return json.dumps(tasks, indent=2) + "\n"


def tpl_validate_episode_stub() -> str:
    return """\"\"\"Episode data validator (stub).

Later we will validate Season/Episode JSON scaffolds.

Exit code:
- 0 = OK
- 1 = validation failed
\"\"\"

import sys


def main():
    print("[validate_episode_data] OK (stub) — no episode JSON wired yet")
    return 0


if __name__ == "__main__":
    sys.exit(main())
"""


def tpl_validate_audio_stub() -> str:
    return """\"\"\"Audio event validator (stub).

Later we will validate that audio event IDs referenced by episode JSON exist in the AudioCatalog.

Exit code:
- 0 = OK
- 1 = validation failed
\"\"\"

import sys


def main():
    print("[validate_audio_events] OK (stub) — audio registry not wired yet")
    return 0


if __name__ == "__main__":
    sys.exit(main())
"""


def tpl_audio_event_cs() -> str:
    return """using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Audio Event")]
public class AudioEvent : ScriptableObject
{
    [Header("Identity")]
    public string eventId;

    [Header("Clips")]
    public AudioClip[] clips;

    [Header("Playback")]
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitchRandom = 0f;
    public bool loop;

    [Header("Routing")]
    public AudioMixerGroup outputGroup;

    [Header("Snapshot")]
    public AudioMixerSnapshot snapshot;
    [Range(0f, 2f)] public float snapshotBlendTime = 0.25f;

    [Header("Concurrency")]
    public bool singleInstance;
}
"""


def tpl_audio_catalog_cs() -> str:
    return """using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Catalog")]
public class AudioCatalog : ScriptableObject
{
    public List<AudioEvent> events = new();

    private Dictionary<string, AudioEvent> _map;

    public void Init()
    {
        _map = new Dictionary<string, AudioEvent>();
        foreach (var e in events)
            if (!string.IsNullOrEmpty(e.eventId))
                _map[e.eventId] = e;
    }

    public AudioEvent Get(string id)
    {
        if (_map == null) Init();
        return _map.TryGetValue(id, out var e) ? e : null;
    }
}
"""


def tpl_audio_service_cs() -> str:
    return """using UnityEngine;

public class AudioService : MonoBehaviour
{
    public static AudioService I;
    public AudioCatalog catalog;

    private readonly System.Collections.Generic.HashSet<string> _playingSingle = new();

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        if (catalog != null) catalog.Init();
    }

    public void Play(string eventId)
    {
        if (catalog == null) { Debug.LogWarning("AudioService: catalog not assigned."); return; }
        var ev = catalog.Get(eventId);
        if (ev == null) { Debug.LogWarning($"AudioEvent not found: {eventId}"); return; }

        if (ev.singleInstance && _playingSingle.Contains(eventId))
            return;

        var go = new GameObject($"Audio_{eventId}");
        var src = go.AddComponent<AudioSource>();

        var clips = ev.clips;
        if (clips == null || clips.Length == 0) { Destroy(go); Debug.LogWarning($"AudioEvent has no clips: {eventId}"); return; }

        src.clip = clips[Random.Range(0, clips.Length)];
        src.volume = ev.volume;
        src.pitch = 1f + Random.Range(-ev.pitchRandom, ev.pitchRandom);
        src.outputAudioMixerGroup = ev.outputGroup;
        src.loop = ev.loop;

        if (ev.snapshot != null)
            ev.snapshot.TransitionTo(ev.snapshotBlendTime);

        if (ev.singleInstance) _playingSingle.Add(eventId);

        src.Play();

        if (!ev.loop)
            Destroy(go, src.clip.length + 0.1f);
        else
            DontDestroyOnLoad(go);

        if (ev.singleInstance && !ev.loop)
            StartCoroutine(ClearSingleAfter(eventId, src.clip.length));
    }

    private System.Collections.IEnumerator ClearSingleAfter(string id, float t)
    {
        yield return new WaitForSeconds(t + 0.1f);
        _playingSingle.Remove(id);
    }
}
"""


# -------------------------------
# Actions
# -------------------------------

def action_init(repo: Path, unity_version: str) -> list[WriteResult]:
    out: list[WriteResult] = []

    ensure_dir(repo / "Tools")
    ensure_dir(repo / ".vscode")
    ensure_dir(repo / "Assets" / "Editor" / "Build")  # Adjusted for root-level project
    ensure_dir(repo / "Builds")
    ensure_dir(repo / "Logs")

    out.append(write_text_if_changed(repo / "Assets" / "Editor" / "Build" / "BuildPlayerCLI.cs", tpl_buildplayer_cli_cs()))  # Adjusted path
    out.append(write_text_if_changed(repo / "Tools" / "build.ps1", tpl_build_ps1(unity_version)))
    out.append(write_text_if_changed(repo / "Tools" / "build.py", tpl_build_py(unity_version)))
    out.append(write_text_if_changed(repo / "Tools" / "validate_episode_data.py", tpl_validate_episode_stub()))
    out.append(write_text_if_changed(repo / "Tools" / "validate_audio_events.py", tpl_validate_audio_stub()))
    out.append(write_text_if_changed(repo / ".vscode" / "tasks.json", tpl_vscode_tasks()))

    settings = json.dumps({"files.eol": "\n", "python.defaultInterpreterPath": "python"}, indent=2) + "\n"
    out.append(write_text_if_changed(repo / ".vscode" / "settings.json", settings))

    return out


def action_audio(repo: Path) -> list[WriteResult]:
    out: list[WriteResult] = []

    ensure_dir(repo / "Assets" / "Audio" / "Scripts")  # Adjusted for root-level project

    out.append(write_text_if_changed(repo / "Assets" / "Audio" / "Scripts" / "AudioEvent.cs", tpl_audio_event_cs()))  # Adjusted path
    out.append(write_text_if_changed(repo / "Assets" / "Audio" / "Scripts" / "AudioCatalog.cs", tpl_audio_catalog_cs()))  # Adjusted path
    out.append(write_text_if_changed(repo / "Assets" / "Audio" / "Scripts" / "AudioService.cs", tpl_audio_service_cs()))  # Adjusted path

    audio_readme = repo / "Assets" / "Audio" / "AUDIO_README.md"  # Adjusted path
    block = """## Mixer Layout (recommended)

- MASTER
  - MUS
  - SFX
  - UI
  - VO
  - AMB
  - GASKET_FX

## Snapshots (recommended)

- Normal
- Investigation
- Extraction
- UneaseTail

## Event IDs to create first

- UI_CASE_CLOSED
- UI_FILE_STAMP
- UNEASE_TAIL_A
- UNEASE_TAIL_B
- UNEASE_TAIL_C
- GF_S01_01
- GF_S01_02
"""
    out.append(upsert_file_with_block(audio_readme, "audio-guidance", block, header="# Crimson Compass Audio\n"))

    return out


def summarize(results: list[WriteResult]) -> None:
    changed = [r for r in results if r.changed]
    unchanged = [r for r in results if not r.changed]

    print("\n=== CC_AUTOGEN SUMMARY ===")
    for r in changed:
        print(f"[CHANGED] {r.path.as_posix()} ({r.reason})")
    for r in unchanged:
        print(f"[OK]      {r.path.as_posix()} ({r.reason})")
    print(f"\nTotal: {len(results)} | Changed: {len(changed)} | Unchanged: {len(unchanged)}")


def main() -> int:
    p = argparse.ArgumentParser()
    sub = p.add_subparsers(dest="cmd", required=True)

    p_init = sub.add_parser("init", help="Generate/Update build + VS Code scaffolding")
    p_init.add_argument("--repo", default=".")
    p_init.add_argument("--unity-version", default=os.environ.get("UNITY_VERSION", "2022.3.62f3"))

    p_audio = sub.add_parser("audio", help="Generate/Update audio scaffolding scripts")
    p_audio.add_argument("--repo", default=".")

    p_all = sub.add_parser("all", help="Run init + audio")
    p_all.add_argument("--repo", default=".")
    p_all.add_argument("--unity-version", default=os.environ.get("UNITY_VERSION", "2022.3.62f3"))

    args = p.parse_args()
    repo = Path(args.repo).resolve()

    results: list[WriteResult] = []

    if args.cmd == "init":
        results += action_init(repo, args.unity_version)
    elif args.cmd == "audio":
        results += action_audio(repo)
    elif args.cmd == "all":
        results += action_init(repo, args.unity_version)
        results += action_audio(repo)

    summarize(results)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
