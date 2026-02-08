"""Audio event validator for Crimson Compass audio system.

Validates AudioEvent assets and AudioCatalog configuration.

Exit code:
- 0 = OK
- 1 = validation failed
"""

import json
import os
import sys
from pathlib import Path
from typing import Dict, List, Any


class AudioEventValidator:
    """Validator for audio event assets and catalog."""

    def __init__(self, project_root: str):
        self.project_root = Path(project_root)
        self.errors: List[str] = []
        self.warnings: List[str] = []

    def validate_audio_system(self) -> bool:
        """Validate the complete audio system."""
        audio_dir = self.project_root / "Assets" / "Audio"
        if not audio_dir.exists():
            self.errors.append("Assets/Audio directory not found")
            return False

        # Check for AudioCatalog
        catalog_path = audio_dir / "CrimsonCompass" / "CATALOG" / "CrimsonCompassAudioCatalog.asset"
        if not catalog_path.exists():
            self.warnings.append("AudioCatalog not found - run AudioSetup from Unity Editor")
            return True  # Not a hard failure, just a warning

        # Check for AudioMixer
        mixer_path = audio_dir / "CrimsonCompass" / "MIXER" / "CrimsonCompassAudioMixer.mixer"
        if not mixer_path.exists():
            self.errors.append("CrimsonCompassAudioMixer.mixer not found")
            return False

        # Check for recommended event IDs
        events_dir = audio_dir / "CrimsonCompass" / "EVENTS"
        if not events_dir.exists():
            self.warnings.append("EVENTS directory not found - run AudioSetup from Unity Editor")
            return True

        # Recommended event IDs from AUDIO_README.md
        recommended_events = [
            "UI_CASE_CLOSED",
            "UI_FILE_STAMP",
            "UNEASE_TAIL_A",
            "UNEASE_TAIL_B",
            "UNEASE_TAIL_C",
            "GF_S01_01",
            "GF_S01_02"
        ]

        found_events = []
        event_files = list(events_dir.glob("*.asset"))
        for event_file in event_files:
            if self._validate_event_asset(event_file):
                # Extract event ID from filename (remove .asset extension)
                event_id = event_file.stem
                found_events.append(event_id)

        # Check for missing recommended events
        missing_events = set(recommended_events) - set(found_events)
        if missing_events:
            self.warnings.append(f"Missing recommended AudioEvent assets: {', '.join(sorted(missing_events))}")

        # Check for extra events (not necessarily an error)
        extra_events = set(found_events) - set(recommended_events)
        if extra_events:
            self.warnings.append(f"Found additional AudioEvent assets: {', '.join(sorted(extra_events))}")

        return len(self.errors) == 0

    def _validate_event_asset(self, event_file: Path) -> bool:
        """Validate a single AudioEvent asset file."""
        try:
            with open(event_file, 'r', encoding='utf-8') as f:
                content = f.read()

            # Basic validation - check for required fields in YAML-like Unity asset format
            if 'eventId:' not in content:
                self.errors.append(f"{event_file.name}: missing eventId field")
                return False

            # Extract eventId (basic parsing)
            lines = content.split('\n')
            event_id = None
            for line in lines:
                if line.strip().startswith('eventId:'):
                    event_id = line.split(':', 1)[1].strip()
                    break

            if not event_id:
                self.errors.append(f"{event_file.name}: could not parse eventId")
                return False

            # Validate eventId format (should be uppercase with underscores)
            if not event_id.replace('_', '').isupper():
                self.warnings.append(f"{event_file.name}: eventId '{event_id}' should be uppercase with underscores")

            return True

        except Exception as e:
            self.errors.append(f"{event_file.name}: failed to validate - {e}")
            return False


def main():
    """Main validation entry point."""
    # Get the project root
    script_dir = Path(__file__).parent
    project_root = script_dir.parent

    validator = AudioEventValidator(str(project_root))

    if validator.validate_audio_system():
        print("[validate_audio_events] Audio system validation passed ✓")
        if validator.warnings:
            print("Warnings:")
            for warning in validator.warnings:
                print(f"  WARNING: {warning}")
        return 0
    else:
        print("[validate_audio_events] Validation failed:")
        for error in validator.errors:
            print(f"  ERROR: {error}")
        if validator.warnings:
            print("Warnings:")
            for warning in validator.warnings:
                print(f"  WARNING: {warning}")
        return 1


if __name__ == "__main__":
    sys.exit(main())
