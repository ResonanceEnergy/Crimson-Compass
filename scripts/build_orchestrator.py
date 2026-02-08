#!/usr/bin/env python3
"""
Crimson Compass Build Orchestrator
Automates Unity builds for multiple platforms using PowerShell and Python
"""

import os
import sys
import subprocess
import argparse
from pathlib import Path
import logging
from datetime import datetime

class BuildOrchestrator:
    def __init__(self, project_root):
        self.project_root = Path(project_root)
        self.scripts_dir = self.project_root / "scripts"
        self.builds_dir = self.project_root / "Builds"
        self.builds_dir.mkdir(exist_ok=True)

        # Setup logging
        self.setup_logging()

    def setup_logging(self):
        log_file = self.builds_dir / f"build_orchestrator_{datetime.now().strftime('%Y%m%d_%H%M%S')}.log"
        logging.basicConfig(
            level=logging.INFO,
            format='%(asctime)s - %(levelname)s - %(message)s',
            handlers=[
                logging.FileHandler(log_file),
                logging.StreamHandler(sys.stdout)
            ]
        )
        self.logger = logging.getLogger(__name__)

    def run_powershell_script(self, script_name, *args):
        """Run a PowerShell script and return success status"""
        script_path = self.scripts_dir / script_name
        if not script_path.exists():
            self.logger.error(f"Script not found: {script_path}")
            return False

        cmd = ["powershell", "-ExecutionPolicy", "Bypass", "-File", str(script_path)] + list(args)
        self.logger.info(f"Running: {' '.join(cmd)}")

        try:
            result = subprocess.run(cmd, capture_output=True, text=True, cwd=self.project_root)
            if result.returncode == 0:
                self.logger.info(f"Script {script_name} completed successfully")
                return True
            else:
                self.logger.error(f"Script {script_name} failed with code {result.returncode}")
                self.logger.error(f"STDOUT: {result.stdout}")
                self.logger.error(f"STDERR: {result.stderr}")
                return False
        except Exception as e:
            self.logger.error(f"Failed to run script {script_name}: {e}")
            return False

    def build_android(self):
        """Build for Android platform"""
        self.logger.info("Starting Android build...")
        return self.run_powershell_script("build_android.ps1")

    def build_ios(self):
        """Build for iOS platform"""
        self.logger.info("Starting iOS build...")
        return self.run_powershell_script("build_ios.ps1")

    def build_all(self):
        """Build for all platforms"""
        self.logger.info("Starting multi-platform build...")

        results = []
        results.append(("Android", self.build_android()))
        results.append(("iOS", self.build_ios()))

        success_count = sum(1 for _, success in results if success)
        total_count = len(results)

        self.logger.info(f"Build summary: {success_count}/{total_count} platforms successful")

        for platform, success in results:
            status = "SUCCESS" if success else "FAILED"
            self.logger.info(f"{platform}: {status}")

        return success_count == total_count

    def clean_builds(self):
        """Clean build artifacts"""
        self.logger.info("Cleaning build directory...")
        import shutil
        if self.builds_dir.exists():
            shutil.rmtree(self.builds_dir)
        self.builds_dir.mkdir()
        self.logger.info("Build directory cleaned")

    def run_tests(self):
        """Run automated tests"""
        self.logger.info("Running tests...")
        # Placeholder for test execution
        # Could integrate with Unity test runner
        self.logger.info("Tests completed (placeholder)")

def main():
    parser = argparse.ArgumentParser(description="Crimson Compass Build Orchestrator")
    parser.add_argument("--project-root", default=".", help="Project root directory")
    parser.add_argument("--platform", choices=["android", "ios", "all"], default="all", help="Platform to build")
    parser.add_argument("--clean", action="store_true", help="Clean build directory first")
    parser.add_argument("--test", action="store_true", help="Run tests after build")

    args = parser.parse_args()

    # Find project root if not specified
    if args.project_root == ".":
        current = Path.cwd()
        while current.parent != current:
            if (current / "Assets").exists() and (current / "ProjectSettings").exists():
                args.project_root = str(current)
                break
            current = current.parent

    orchestrator = BuildOrchestrator(args.project_root)

    if args.clean:
        orchestrator.clean_builds()

    success = False
    if args.platform == "android":
        success = orchestrator.build_android()
    elif args.platform == "ios":
        success = orchestrator.build_ios()
    elif args.platform == "all":
        success = orchestrator.build_all()

    if args.test and success:
        orchestrator.run_tests()

    sys.exit(0 if success else 1)

if __name__ == "__main__":
    main()