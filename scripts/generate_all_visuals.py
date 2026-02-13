#!/usr/bin/env python3
"""
Crimson Compass Grok Imagine Asset Generator
Generates all visual assets using the Grok Imagine API integration
"""

import os
import sys
import re
import json
import time
import argparse
from pathlib import Path
import logging
from datetime import datetime
import requests
from typing import Dict, List, Optional

class GrokImagineAssetGenerator:
    def __init__(self, project_root: str, api_key: str):
        self.project_root = Path(project_root)
        self.api_key = api_key
        self.generated_dir = self.project_root / "GeneratedAssets"
        self.prompts_file = self.project_root / "docs" / "Visual_Prompts_Grok_Imagine.md"

        # API endpoints
        self.base_url = "https://api.x.ai/v1"
        self.image_endpoint = f"{self.base_url}/images/generations"

        # Setup logging
        self.setup_logging()

    def setup_logging(self):
        """Setup logging configuration"""
        log_dir = self.generated_dir / "Logs"
        log_dir.mkdir(parents=True, exist_ok=True)

        log_file = log_dir / f"grok_asset_generation_{datetime.now().strftime('%Y%m%d_%H%M%S')}.log"
        logging.basicConfig(
            level=logging.INFO,
            format='%(asctime)s - %(levelname)s - %(message)s',
            handlers=[
                logging.FileHandler(log_file),
                logging.StreamHandler(sys.stdout)
            ]
        )
        self.logger = logging.getLogger(__name__)

    def parse_prompts_file(self) -> Dict[str, List[Dict]]:
        """Parse the Visual_Prompts_Grok_Imagine.md file and extract all prompts"""
        if not self.prompts_file.exists():
            raise FileNotFoundError(f"Prompts file not found: {self.prompts_file}")

        self.logger.info(f"Parsing prompts from: {self.prompts_file}")

        with open(self.prompts_file, 'r', encoding='utf-8') as f:
            content = f.read()

        assets = []

        # Find all asset entries with their prompts
        asset_pattern = r'\*\*(.+?):\*\*\s*\n(.+?)(?=\n\*\*|\n###|\n##|\Z)'
        matches = re.findall(asset_pattern, content, re.DOTALL)

        for asset_name, prompt in matches:
            asset_name = asset_name.strip()
            prompt = prompt.strip()

            # Determine category based on context or name
            if any(keyword in asset_name.lower() for keyword in ['background', 'room', 'district', 'lobby', 'office', 'stairwell']):
                category = "Backgrounds"
            elif any(keyword in asset_name.lower() for keyword in ['player', 'agent', 'detective', 'executive', 'administrator', 'manager', 'director']):
                category = "Characters"
            elif any(keyword in asset_name.lower() for keyword in ['compass', 'pen', 'keys', 'scanner', 'components', 'detector']):
                category = "Objects"
            else:
                category = "Objects"  # Default

            assets.append({
                "name": asset_name,
                "prompt": prompt,
                "category": category
            })

        self.logger.info(f"Found {len(assets)} assets to generate")
        return {"all": assets}

    def generate_image(self, prompt: str, aspect_ratio: str = "16:9") -> Optional[str]:
        """Generate a single image using Grok Imagine API"""
        headers = {
            "Authorization": f"Bearer {self.api_key}",
            "Content-Type": "application/json"
        }

        data = {
            "prompt": prompt,
            "model": "grok-imagine-image",
            "aspect_ratio": aspect_ratio,
            "n": 1,
            "image_format": "url"
        }

        self.logger.info(f"Generating image with prompt: {prompt[:100]}...")

        try:
            response = requests.post(self.image_endpoint, headers=headers, json=data, timeout=60)
            response.raise_for_status()

            result = response.json()
            self.logger.info("API response received")

            if result.get("url"):
                return result["url"]
            else:
                self.logger.error(f"No URL in response: {result}")
                return None

        except requests.exceptions.RequestException as e:
            self.logger.error(f"API request failed: {e}")
            return None

    def download_asset(self, url: str, filepath: str) -> bool:
        """Download asset from URL"""
        try:
            self.logger.info(f"Downloading: {url}")
            response = requests.get(url, timeout=30)
            response.raise_for_status()

            with open(filepath, 'wb') as f:
                f.write(response.content)

            self.logger.info(f"Saved to: {filepath}")
            return True

        except requests.exceptions.RequestException as e:
            self.logger.error(f"Download failed: {e}")
            return False

    def generate_all_assets(self, assets_data: Dict, dry_run: bool = False, max_assets: int = None) -> Dict:
        """Generate all assets"""
        results = {"generated": [], "failed": [], "skipped": []}

        assets = assets_data.get("all", [])
        total_assets = len(assets) if not max_assets else min(max_assets, len(assets))

        self.logger.info(f"Total assets to generate: {total_assets}")

        for i, asset in enumerate(assets[:total_assets]):
            asset_name = asset["name"]
            prompt = asset["prompt"]
            category = asset["category"]

            # Create category directory
            category_dir = self.generated_dir / category
            category_dir.mkdir(parents=True, exist_ok=True)

            # Create filename
            safe_name = re.sub(r'[^\w\-_\.]', '_', asset_name)
            timestamp = datetime.now().strftime('%H%M%S')
            filename = f"{safe_name}_{timestamp}.png"
            filepath = category_dir / filename

            self.logger.info(f"[{i+1}/{total_assets}] Generating: {asset_name} ({category})")

            if dry_run:
                self.logger.info(f"DRY RUN: Would generate {asset_name}")
                results["skipped"].append(asset_name)
                continue

            # Determine aspect ratio
            aspect_ratios = {
                "Backgrounds": "9:16",
                "Characters": "3:4",
                "Objects": "1:1"
            }
            aspect_ratio = aspect_ratios.get(category, "16:9")

            # Generate the asset
            url = self.generate_image(prompt, aspect_ratio)

            if url:
                success = self.download_asset(url, str(filepath))
                if success:
                    results["generated"].append({
                        "name": asset_name,
                        "category": category,
                        "filepath": str(filepath)
                    })
                    self.logger.info(f"Successfully generated: {asset_name}")
                else:
                    results["failed"].append(asset_name)
                    self.logger.error(f"Failed to download: {asset_name}")
            else:
                results["failed"].append(asset_name)
                self.logger.error(f"Failed to generate: {asset_name}")

            # Rate limiting - wait between requests
            if not dry_run:
                time.sleep(3)

        return results

def main():
    parser = argparse.ArgumentParser(description="Generate Crimson Compass visual assets using Grok Imagine API")
    parser.add_argument("--project-root", default=".", help="Project root directory")
    parser.add_argument("--api-key", required=True, help="Grok Imagine API key")
    parser.add_argument("--dry-run", action="store_true", help="Show what would be generated without actually generating")
    parser.add_argument("--max-assets", type=int, help="Maximum number of assets to generate (for testing)")

    args = parser.parse_args()

    try:
        generator = GrokImagineAssetGenerator(args.project_root, args.api_key)

        # Parse prompts
        assets_data = generator.parse_prompts_file()

        # Generate assets
        results = generator.generate_all_assets(assets_data, args.dry_run, args.max_assets)

        # Print summary
        print("\n" + "="*50)
        print("GENERATION SUMMARY")
        print("="*50)
        print(f"Generated: {len(results['generated'])}")
        print(f"Failed: {len(results['failed'])}")
        print(f"Skipped: {len(results['skipped'])}")

        if results['generated']:
            print("\nGenerated Assets:")
            for asset in results['generated']:
                print(f"  ✓ {asset['name']} ({asset['category']}) -> {asset['filepath']}")

        if results['failed']:
            print("\nFailed Assets:")
            for asset in results['failed']:
                print(f"  ✗ {asset}")

    except Exception as e:
        print(f"Error: {e}")
        import traceback
        traceback.print_exc()
        sys.exit(1)

if __name__ == "__main__":
    main()
