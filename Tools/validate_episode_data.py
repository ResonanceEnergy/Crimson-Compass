"""Episode data validator for Season 1 JSON files.

Validates the structure and integrity of episode JSON files in Assets/Data/Cases/.

Exit code:
- 0 = OK
- 1 = validation failed
"""

import json
import os
import sys
from pathlib import Path
from typing import Dict, List, Any


class EpisodeDataValidator:
    """Validator for episode JSON data files."""

    def __init__(self, cases_dir: str):
        self.cases_dir = Path(cases_dir)
        self.errors: List[str] = []

    def validate_all_cases(self) -> bool:
        """Validate all case JSON files in the cases directory."""
        if not self.cases_dir.exists():
            self.errors.append(f"Cases directory does not exist: {self.cases_dir}")
            return False

        case_files = list(self.cases_dir.glob("case_*.json"))
        if not case_files:
            self.errors.append(f"No case JSON files found in {self.cases_dir}")
            return False

        print(f"[validate_episode_data] Found {len(case_files)} case files")

        all_valid = True
        for case_file in sorted(case_files):
            if not self.validate_case_file(case_file):
                all_valid = False

        return all_valid

    def validate_case_file(self, case_file: Path) -> bool:
        """Validate a single case JSON file."""
        try:
            with open(case_file, 'r', encoding='utf-8') as f:
                data = json.load(f)
        except json.JSONDecodeError as e:
            self.errors.append(f"{case_file.name}: Invalid JSON - {e}")
            return False
        except Exception as e:
            self.errors.append(f"{case_file.name}: Failed to read file - {e}")
            return False

        # Validate filename matches caseId (convert CASE-XXXX to case_XXXX)
        expected_case_id = f"case_{case_file.stem.split('_')[1]}"  # e.g., "case_0001"
        actual_case_id = data.get('caseId')
        if actual_case_id and not actual_case_id.upper().endswith(expected_case_id.upper()):
            # Allow CASE-0001 to match case_0001
            expected_upper = f"CASE-{case_file.stem.split('_')[1]}"
            if actual_case_id != expected_upper:
                self.errors.append(f"{case_file.name}: caseId '{actual_case_id}' does not match expected '{expected_upper}'")

        # Validate required fields
        required_fields = [
            'caseId', 'title', 'tier', 'timeBudget',
            'suspects', 'methods', 'locations', 'truth',
            'caseToken', 'shadowToken'
        ]

        for field in required_fields:
            if field not in data:
                self.errors.append(f"{case_file.name}: Missing required field '{field}'")
                continue

            # Type validation
            if field in ['suspects', 'methods', 'locations']:
                if not isinstance(data[field], list):
                    self.errors.append(f"{case_file.name}: Field '{field}' must be an array")
            elif field == 'timeBudget':
                if not isinstance(data[field], (int, float)):
                    self.errors.append(f"{case_file.name}: Field '{field}' must be a number")
            elif field == 'truth':
                if not isinstance(data[field], dict):
                    self.errors.append(f"{case_file.name}: Field '{field}' must be an object")
            else:
                if not isinstance(data[field], str):
                    self.errors.append(f"{case_file.name}: Field '{field}' must be a string")

        # Validate suspects array
        if 'suspects' in data and isinstance(data['suspects'], list):
            suspect_ids = set()
            for i, suspect in enumerate(data['suspects']):
                if not self._validate_suspect(suspect, f"suspects[{i}]", case_file.name):
                    continue
                if suspect['id'] in suspect_ids:
                    self.errors.append(f"{case_file.name}: Duplicate suspect ID '{suspect['id']}'")
                suspect_ids.add(suspect['id'])

        # Validate methods array
        if 'methods' in data and isinstance(data['methods'], list):
            method_ids = set()
            for i, method in enumerate(data['methods']):
                if not self._validate_method(method, f"methods[{i}]", case_file.name):
                    continue
                if method['id'] in method_ids:
                    self.errors.append(f"{case_file.name}: Duplicate method ID '{method['id']}'")
                method_ids.add(method['id'])

        # Validate locations array
        if 'locations' in data and isinstance(data['locations'], list):
            location_ids = set()
            for i, location in enumerate(data['locations']):
                if not isinstance(location, dict):
                    self.errors.append(f"{case_file.name}: locations[{i}] must be an object")
                    continue
                if 'id' not in location or 'country' not in location:
                    self.errors.append(f"{case_file.name}: locations[{i}] missing required fields 'id' and/or 'country'")
                    continue
                if not isinstance(location['id'], str) or not isinstance(location['country'], str):
                    self.errors.append(f"{case_file.name}: locations[{i}] 'id' and 'country' must be strings")
                    continue
                if location['id'] in location_ids:
                    self.errors.append(f"{case_file.name}: Duplicate location ID '{location['id']}'")
                location_ids.add(location['id'])

        # Validate truth object
        if 'truth' in data and isinstance(data['truth'], dict):
            truth = data['truth']
            required_truth_fields = ['whoId', 'howId', 'whereId']
            for field in required_truth_fields:
                if field not in truth:
                    self.errors.append(f"{case_file.name}: truth object missing required field '{field}'")
                elif not isinstance(truth[field], str):
                    self.errors.append(f"{case_file.name}: truth.{field} must be a string")

            # Validate truth references exist
            if 'suspects' in data and truth.get('whoId'):
                suspect_ids = {s['id'] for s in data['suspects'] if isinstance(s, dict) and 'id' in s}
                if truth['whoId'] not in suspect_ids:
                    self.errors.append(f"{case_file.name}: truth.whoId '{truth['whoId']}' not found in suspects")

            if 'methods' in data and truth.get('howId'):
                method_ids = {m['id'] for m in data['methods'] if isinstance(m, dict) and 'id' in m}
                if truth['howId'] not in method_ids:
                    self.errors.append(f"{case_file.name}: truth.howId '{truth['howId']}' not found in methods")

            if 'locations' in data and truth.get('whereId'):
                location_ids = {l['id'] for l in data['locations'] if isinstance(l, dict) and 'id' in l}
                if truth['whereId'] not in location_ids:
                    self.errors.append(f"{case_file.name}: truth.whereId '{truth['whereId']}' not found in locations")

        return len([e for e in self.errors if case_file.name in e]) == 0

    def _validate_suspect(self, suspect: Dict[str, Any], path: str, filename: str) -> bool:
        """Validate a suspect entity."""
        if not isinstance(suspect, dict):
            self.errors.append(f"{filename}: {path} must be an object")
            return False

        if 'id' not in suspect or 'name' not in suspect or 'traits' not in suspect:
            self.errors.append(f"{filename}: {path} missing required fields 'id', 'name', and/or 'traits'")
            return False

        if not isinstance(suspect['id'], str) or not isinstance(suspect['name'], str):
            self.errors.append(f"{filename}: {path} 'id' and 'name' must be strings")
            return False

        if not isinstance(suspect['traits'], list):
            self.errors.append(f"{filename}: {path}.traits must be an array")
            return False

        for j, trait in enumerate(suspect['traits']):
            if not isinstance(trait, str):
                self.errors.append(f"{filename}: {path}.traits[{j}] must be a string")

        return True

    def _validate_method(self, method: Dict[str, Any], path: str, filename: str) -> bool:
        """Validate a method entity."""
        if not isinstance(method, dict):
            self.errors.append(f"{filename}: {path} must be an object")
            return False

        if 'id' not in method or 'name' not in method or 'signatures' not in method:
            self.errors.append(f"{filename}: {path} missing required fields 'id', 'name', and/or 'signatures'")
            return False

        if not isinstance(method['id'], str) or not isinstance(method['name'], str):
            self.errors.append(f"{filename}: {path} 'id' and 'name' must be strings")
            return False

        if not isinstance(method['signatures'], list):
            self.errors.append(f"{filename}: {path}.signatures must be an array")
            return False

        for j, sig in enumerate(method['signatures']):
            if not isinstance(sig, str):
                self.errors.append(f"{filename}: {path}.signatures[{j}] must be a string")

        return True


def main():
    """Main validation entry point."""
    # Get the project root (assuming script is in Tools/)
    script_dir = Path(__file__).parent
    project_root = script_dir.parent
    cases_dir = project_root / "Assets" / "Data" / "Cases"

    validator = EpisodeDataValidator(str(cases_dir))

    if validator.validate_all_cases():
        print("[validate_episode_data] All episode data validation passed ✓")
        return 0
    else:
        print("[validate_episode_data] Validation failed:")
        for error in validator.errors:
            print(f"  ERROR: {error}")
        return 1


if __name__ == "__main__":
    sys.exit(main())
