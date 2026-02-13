import argparse
import subprocess
import sys
from pathlib import Path


def run(cmd, cwd=None):
    print(">", " ".join(cmd))
    result = subprocess.run(cmd, cwd=cwd)
    if result.returncode != 0:
        sys.exit(result.returncode)


def main():
    p = argparse.ArgumentParser(description="Crimson Compass build orchestrator")
    p.add_argument("--target", default="Win64", choices=["Win64","Mac","Linux64","Android","iOS"])
    p.add_argument("--dev", action="store_true")
    p.add_argument("--unity", default=r"C:\Program Files\Unity\Hub\Editor\6000.3.6f1\Editor\Unity.exe")
    p.add_argument("--preflight", action="store_true", help="Run preflight validators before building")
    p.add_argument("--validate-only", action="store_true", help="Run only preflight validators, no build")
    args = p.parse_args()

    root = Path(__file__).resolve().parent.parent / "Crimson-Compass"  # Adjusted for project in subfolder
    tools = Path(__file__).resolve().parent

    if args.preflight or args.validate_only:
        run([sys.executable, str(tools / "validate_episode_data.py")])
        run([sys.executable, str(tools / "validate_audio_events.py")])

    if not args.validate_only:
        ps1 = tools / "build.ps1"
        cmd = [
            "powershell", "-ExecutionPolicy", "Bypass",
            "-File", str(ps1),
            "-ProjectPath", str(root),
            "-UnityPath", args.unity,
            "-Target", args.target
        ]
        if args.dev:
            cmd.append("-DevBuild")

        run(cmd)


if __name__ == "__main__":
    main()
