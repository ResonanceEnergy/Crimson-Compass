#!/usr/bin/env bash
set -euo pipefail
ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
PROJECT_DIR="$ROOT_DIR/CrimsonCompass"
if ! command -v code >/dev/null 2>&1; then
  echo "VS Code 'code' command not found. Install it via VS Code command palette.";
  exit 1
fi
code "$PROJECT_DIR"
