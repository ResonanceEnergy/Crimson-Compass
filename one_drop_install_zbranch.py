"""Crimson Compass — One-Drop Content Installer (ZBRANCH Countermeasures)

Usage:
  python one_drop_install_zbranch.py /path/to/CrimsonCompass
"""

from __future__ import annotations
import shutil, sys
from pathlib import Path

FILES = {
  'CrimsonCompass_zbranch_countermeasures_0040.jsonl': 'Assets/Data/ZBRANCH/Countermeasures/',
  'CrimsonCompass_zbranch_countermeasures_set_0015.jsonl': 'Assets/Data/ZBRANCH/Countermeasures/',
  'CrimsonCompass_zbranch_countermeasures_personal_0020.jsonl': 'Assets/Data/ZBRANCH/Countermeasures/',
  'CrimsonCompass_zbranch_countermeasure_barks.json': 'Assets/Data/Agents/',
  'CrimsonCompass_zbranch_countermeasure_ui_templates_v1.json': 'Assets/Data/ZBRANCH/Countermeasures/',
  'CrimsonCompass_zbranch_countermeasure_selection_rules_v1.json': 'Assets/Data/ZBRANCH/Countermeasures/',
  'CrimsonCompass_gasket_containment_actions_0009.jsonl': 'Assets/Data/Agents/',
  'ZBRANCH_Countermeasures_Playbook_v2.md': 'Docs/',
  'README_ZBRANCH_OneDrop.md': 'Docs/'
}


def main():
    if len(sys.argv) < 2:
        raise SystemExit('Usage: python one_drop_install_zbranch.py /path/to/CrimsonCompass')
    project = Path(sys.argv[1]).expanduser().resolve()
    here = Path(__file__).parent.resolve()
    for src_name, rel in FILES.items():
        src = here / src_name
        dst_dir = project / rel
        dst_dir.mkdir(parents=True, exist_ok=True)
        shutil.copy2(src, dst_dir / src_name)
    print('ZBRANCH one-drop installed.')

if __name__ == '__main__':
    main()
