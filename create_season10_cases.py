import json
import os
from pathlib import Path

def create_season10_cases():
    # Season 10: True Nothingness - Operations in complete void where even the concept of existence ceases

    base_case = {
        "caseId": "",
        "title": "",
        "tier": "season10",
        "hook": "",
        "timeBudget": 72,
        "hintsPerMission": 3,
        "hintCost": {
            "timeHours": 2,
            "heat": 1
        },
        "nothingnessDepth": 0,
        "existenceState": "minimal",
        "conceptualFlow": "dissolving",
        "voidLayers": [],
        "nullificationEvents": [],
        "suspects": [
            {
                "id": "S1",
                "name": "The Void Architect",
                "traits": [
                    "existence-dissolver",
                    "nothingness-engineer"
                ]
            },
            {
                "id": "S2",
                "name": "The Nullity Weaver",
                "traits": [
                    "conceptual-eraser",
                    "void-manipulator"
                ]
            },
            {
                "id": "S3",
                "name": "The Absence Incarnate",
                "traits": [
                    "non-existence-forger",
                    "nullity-crafter"
                ]
            }
        ],
        "methods": [
            {
                "id": "M1",
                "name": "Existential Dissolution",
                "signatures": [
                    "existence:erased",
                    "reality:unmade"
                ],
                "nullificationMechanics": {
                    "existenceDecay": 0.99,
                    "conceptualDissolution": 1000,
                    "nothingnessStability": 0.001
                }
            },
            {
                "id": "M2",
                "name": "Conceptual Unraveling",
                "signatures": [
                    "thoughts:unwoven",
                    "ideas:erased"
                ],
                "unravelingAnalysis": {
                    "conceptualLayers": 15,
                    "thoughtRigidity": 0.01,
                    "nullificationThreshold": 0.00001
                }
            },
            {
                "id": "M3",
                "name": "Void Convergence",
                "signatures": [
                    "presence:eliminated",
                    "being:annihilated"
                ],
                "convergenceMechanics": {
                    "voidAttraction": 0.999,
                    "existenceDensity": 0.001,
                    "nothingnessHorizon": 0.000001
                }
            }
        ],
        "locations": [
            {
                "id": "L1",
                "country": "Non-existent"
            },
            {
                "id": "L2",
                "country": "Unmade"
            }
        ],
        "truth": {
            "whoId": "S1",
            "howId": "M1",
            "whereId": "L1"
        },
        "caseToken": "True Nothingness",
        "shadowToken": "Existential Void",
        "clues": [
            {
                "id": "C1",
                "text": "Evidence suggests reality itself is being systematically erased",
                "reliability": 0.001,
                "existenceIndicator": True
            },
            {
                "id": "C2",
                "text": "Thoughts and concepts dissolve as they are perceived",
                "reliability": 0.0001,
                "conceptualDissolution": True
            },
            {
                "id": "C3",
                "text": "The very act of investigation causes further erasure",
                "reliability": 0.00001,
                "observationalDecay": True
            }
        ],
        "gadgetsOffered": [
            "EXISTENCEANCHOR",
            "CONCEPTUALSTABILIZER",
            "VOIDSHIELD",
            "NULLITYANALYZER",
            "PRESENCEAMPLIFIER",
            "THOUGHTPRESERVER",
            "REALITYREINFORCER",
            "NONEXISTENCEFILTER"
        ],
        "gadgetsSelectable": 3
    }

    # Episode titles and hooks for Season 10
    episodes = [
        {"id": "0109", "title": "Complete Erasure", "hook": "All evidence, all reality, all existence systematically erased"},
        {"id": "0110", "title": "Thought Dissolution", "hook": "Thoughts unravel as they form, leaving only emptiness"},
        {"id": "0111", "title": "Presence Elimination", "hook": "The act of being present causes further non-existence"},
        {"id": "0112", "title": "Conceptual Void", "hook": "Ideas cease to exist before they can be conceived"},
        {"id": "0113", "title": "Reality Unraveling", "hook": "The fabric of existence unravels thread by thread"},
        {"id": "0114", "title": "Existential Decay", "hook": "Being itself decays into pure nothingness"},
        {"id": "0115", "title": "Nullity Convergence", "hook": "All things converge into absolute nullity"},
        {"id": "0116", "title": "Absence Manifest", "hook": "Nothingness becomes the only presence"},
        {"id": "0117", "title": "Void Incarnation", "hook": "The void itself takes form through absence"},
        {"id": "0118", "title": "Non-Existence Engine", "hook": "A machine that generates perfect nothingness"},
        {"id": "0119", "title": "Conceptual Cessation", "hook": "All concepts cease, leaving only the inconceivable"},
        {"id": "0120", "title": "Ultimate Nullification", "hook": "The final erasure where even erasure ceases to exist"}
    ]

    cases_dir = Path('/Users/gripandripphdd/Crimson-Compass/Assets/Data/Cases')

    for i, episode in enumerate(episodes, 109):
        case = base_case.copy()
        case["caseId"] = f"CASE-{i:04d}"
        case["title"] = episode["title"]
        case["hook"] = episode["hook"]
        case["nothingnessDepth"] = (i - 108)  # Escalating nothingness levels
        case["existenceState"] = ["fading", "dissolving", "erased", "nullified"][min(3, (i - 109) // 3)]

        # Add episode-specific void layers
        case["voidLayers"] = [
            {
                "id": f"VL{(i-108)*3-2}",
                "depth": (i-108) * 0.001,
                "description": f"Void layer at depth {(i-108) * 0.001:.5f} in nothingness",
                "dissolutionRate": min(1.0, (i-108) * 0.08)
            },
            {
                "id": f"VL{(i-108)*3-1}",
                "depth": (i-108) * 0.0015,
                "description": f"Non-existence boundary at depth {(i-108) * 0.0015:.5f}",
                "dissolutionRate": min(0.999, (i-108) * 0.07)
            },
            {
                "id": f"VL{(i-108)*3}",
                "depth": (i-108) * 0.002,
                "description": f"Nullity horizon at depth {(i-108) * 0.002:.5f}",
                "dissolutionRate": min(0.99, (i-108) * 0.06)
            }
        ]

        # Add nullification events
        case["nullificationEvents"] = [
            {
                "id": f"NE{(i-108)*2-1}",
                "type": "existential-erosion",
                "description": f"Existence erodes at nothingness level {(i-108)}",
                "triggerCondition": f"existence_level <= {1.0 - (i-108) * 0.08}",
                "effect": "conceptual_decay",
                "severity": min(5, (i-108))
            },
            {
                "id": f"NE{(i-108)*2}",
                "type": "thought-dissolution",
                "description": f"Thoughts dissolve at rate {(i-108) * 0.07:.3f} per second",
                "triggerCondition": f"thought_coherence <= {1.0 - (i-108) * 0.07}",
                "effect": "mental_erosion",
                "severity": min(4, (i-108) // 2 + 1)
            }
        ]

        case_file = cases_dir / f'case_{i:04d}.json'
        with open(case_file, 'w') as f:
            json.dump(case, f, indent=2)

        print(f"Created Season 10 case: {case_file}")

if __name__ == "__main__":
    create_season10_cases()