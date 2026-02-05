#!/usr/bin/env bash
set -euo pipefail
if [ $# -lt 1 ]; then echo "Usage: $0 <REMOTE_URL>"; exit 1; fi
REMOTE="$1"
ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT_DIR/CrimsonCompass"
git init
# git lfs install  # later if needed

git add .
git commit -m "Initial commit: Crimson Compass one-drop" || true

git branch -M main

git remote remove origin >/dev/null 2>&1 || true

git remote add origin "$REMOTE"

git push -u origin main
