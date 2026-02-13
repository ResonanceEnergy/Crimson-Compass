import os
import random

# Season themes
seasons = {
    1: "The Replacement",
    2: "Compass Calibration", 
    3: "Truth Vectors",
    4: "Gasket's Shadow",
    5: "Network of Deception",
    6: "Fractured Reality",
    7: "The Final Vector",
    8: "Shadows Rising",
    9: "Compass Evolution",
    10: "Global Conspiracy",
    11: "Personal Reckoning",
    12: "Eternal Unease"
}

# Character evolutions per season (brief)
character_evolutions = {
    1: "Player adapts to agency, discovers compass anomalies. Team introduces themselves.",
    2: "Player masters basic compass use. Optimus struggles with tech. Helix coordinates. ZTech innovates. Gasket watches.",
    3: "Advanced compass skills. Team bonds. Gasket provides guidance.",
    4: "Uncover Gasket's past. Compass origins revealed.",
    5: "Interconnected cases. Shadow adversary hints.",
    6: "Reality questioned. Compass illusions. Gasket's sanity tested.",
    7: "Confront adversary. Gasket's sacrifice.",
    8: "Post-sacrifice recovery. New threats from shadows.",
    9: "Compass evolves with new abilities. Team adapts.",
    10: "Global scale conspiracies. International alliances.",
    11: "Personal growth and reckonings for each character.",
    12: "Final closure, but eternal doubt remains."
}

# Sample titles, cases, etc. per season
season_titles = {
    2: ["Calibration Blues", "Vector Basics", "Compass First Steps", "Tech Troubles", "Helix's Help", "Gasket's Glare", "Anomaly Alert", "Team Tune-Up", "Puzzle Practice", "Shadow Stirrings", "Unease Builds", "Calibration Complete"],
    3: ["Vector Mastery", "Truth Revealed", "Advanced Anomalies", "Helix's Coordination", "ZTech's Gadgets", "Gasket's Guidance", "Deeper Mysteries", "Team Bonds", "Puzzle Challenges", "Shadow Hints", "Reality Shifts", "Vector Climax"],
    4: ["Gasket's Past", "Incident Investigation", "Compass Origins", "Shadow Revelations", "Team Support", "Anomaly Deep Dive", "Gasket's Trauma", "Truth Uncovered", "Puzzle of the Past", "Shadow Connections", "Climactic Flashback", "Shadow's Edge"],
    5: ["Network Webs", "Interconnected Plots", "Deception Layers", "Adversary Whispers", "Team Coordination", "Compass Overload", "Gasket's Warnings", "Conspiracy Unfolds", "Puzzle Networks", "Shadow Interference", "Reality Blurs", "Network Peak"],
    6: ["Illusion Inception", "Reality Doubts", "Compass Hallucinations", "Gasket's Breakdown", "Team Paranoia", "Fractured Truths", "Anomaly Overdrive", "Shadow Realities", "Puzzle Illusions", "Mental Strain", "Climactic Fracture", "Reality's End"],
    7: ["Final Pursuit", "Adversary Hunt", "Gasket's Sacrifice", "Compass Ultimate", "Team Unity", "Shadow Confrontation", "Truth Vectors Max", "Puzzle Finale", "Climactic Battle", "Sacrifice Made", "Victory Unease", "Final Vector"],
    8: ["Rising Shadows", "Post-Sacrifice Woes", "New Threats Emerge", "Compass Lingers", "Team Recovery", "Anomaly Returns", "Shadow Remnants", "Gadget Upgrades", "Puzzle Shadows", "Unease Persists", "Threat Builds", "Shadow Rise"],
    9: ["Evolution Begins", "Compass Upgrades", "New Abilities", "Team Adaptation", "ZTech Innovations", "Anomaly Evolution", "Shadow Evolution", "Puzzle Evolution", "Climactic Upgrade", "Power Surge", "Evolution Peak", "Compass Reborn"],
    10: ["Global Plots", "International Intrigue", "Conspiracy Worldwide", "Alliance Strains", "Team Global", "Compass Global", "Shadow Global", "Puzzle Global", "Climactic Alliance", "World Threat", "Global Climax", "Conspiracy Crushed"],
    11: ["Personal Demons", "Character Reckonings", "Inner Conflicts", "Team Reflections", "Compass Personal", "Anomaly Personal", "Shadow Personal", "Puzzle Personal", "Climactic Reckoning", "Growth Moments", "Personal Peak", "Reckoning Complete"],
    12: ["Eternal Doubt", "Final Closure", "Unease Forever", "Compass Finale", "Team Farewell", "Shadow Endgame", "Anomaly Eternal", "Puzzle Eternal", "Climactic Doubt", "Closure Achieved", "Unease Lingers", "Eternal Unease"]
}

# Template for each episode
episode_template = """# {season_name} - Episode {ep_num}: {title}

## Overview
**Case:** {case}  
**Tokens:** {tokens}  
**Theme:** {theme}

## Character Evolution for Season {season}
{char_evo}

## 3-Scene Structure

### Hook (2 minutes)
- {hook1}
- {hook2}
- {hook3}

### Investigation (2 minutes)
- {inv1}
- {inv2}
- {inv3}

### Resolution (1 minute)
- {res1}
- {res2}
- {res3}

## Key Puzzles and Mechanics
- {puzzle1}
- {puzzle2}
- {puzzle3}

## Team Dynamics and Interactions
- {team1}
- {team2}
- {team3}

## Shadow Token Integration
- {shadow1}
- {shadow2}

## Cliffhanger Elements
- {cliff1}
- {cliff2}
"""

# Function to generate episode content
def generate_episode(season, ep_num):
    season_name = seasons[season]
    title = season_titles.get(season, [f"Episode {ep_num}"])[ep_num-1] if season in season_titles and ep_num <= len(season_titles[season]) else f"Episode {ep_num}"
    case = f"A complex case involving {random.choice(['corporate espionage', 'theft', 'sabotage', 'assassination', 'data breach', 'artifact smuggling'])} with compass anomalies."
    tokens = f"Shadow tokens hinting at {random.choice(['Gasket\'s past', 'adversary connections', 'compass origins', 'team secrets', 'global conspiracies', 'personal traumas'])}."
    theme = f"Exploring {random.choice(['truth vectors', 'deception fields', 'anomaly patterns', 'shadow connections', 'reality fractures', 'final reckonings'])}."
    char_evo = character_evolutions[season]
    
    # Varied content
    hook_options = [
        "Optimus briefs the team on the mission",
        "Compass shows initial readings",
        "Helix coordinates logistics",
        "ZTech demonstrates new gadget",
        "Gasket appears with a warning",
        "Player calibrates compass"
    ]
    inv_options = [
        "Navigate truth vectors in the field",
        "Gather evidence with compass guidance",
        "Interact with environmental puzzles",
        "Coordinate with Helix via comms",
        "Use ZTech gadgets for investigation",
        "Encounter compass anomalies"
    ]
    res_options = [
        "Compass reveals 95% solution",
        "Case resolved professionally",
        "Hint of 5% unease remains",
        "Team celebrates minor victory",
        "Gasket comments cryptically",
        "Setup for shadow implications"
    ]
    puzzle_options = [
        "Compass navigation puzzle",
        "Evidence decryption",
        "Gadget-based environmental interaction",
        "Team coordination challenge",
        "Anomaly resolution puzzle",
        "Truth vector mapping"
    ]
    team_options = [
        "Optimus fumbles tech, Helix fixes it",
        "ZTech explains gadget absurdly",
        "Player bonds with team",
        "Gasket provides insight",
        "Helix mediates conflicts",
        "Quirky banter during action"
    ]
    shadow_options = [
        "Compass anomaly doesn't fit case",
        "Gasket's erratic behavior",
        "Hint of larger conspiracy",
        "Psychological toll on team",
        "Shadow token collection",
        "Unease builds subtly"
    ]
    cliff_options = [
        "Unresolved anomaly lingers",
        "Gasket disappears mysteriously",
        "New threat hinted",
        "Team questions reality",
        "Compass evolves unexpectedly",
        "Setup for next episode's hook"
    ]
    
    def pick(options, n=3):
        return random.sample(options, n)
    
    hook = pick(hook_options, 3)
    inv = pick(inv_options, 3)
    res = pick(res_options, 3)
    puzzle = pick(puzzle_options, 3)
    team = pick(team_options, 3)
    shadow = pick(shadow_options, 2)
    cliff = pick(cliff_options, 2)
    
    return episode_template.format(
        season_name=season_name, ep_num=ep_num, title=title,
        case=case, tokens=tokens, theme=theme, char_evo=char_evo,
        season=season,
        hook1=hook[0], hook2=hook[1], hook3=hook[2],
        inv1=inv[0], inv2=inv[1], inv3=inv[2],
        res1=res[0], res2=res[1], res3=res[2],
        puzzle1=puzzle[0], puzzle2=puzzle[1], puzzle3=puzzle[2],
        team1=team[0], team2=team[1], team3=team[2],
        shadow1=shadow[0], shadow2=shadow[1],
        cliff1=cliff[0], cliff2=cliff[1]
    )

# Create directories and files
base_dir = "docs/episodes"
for season in range(2, 13):  # Seasons 2 to 12
    season_dir = f"{base_dir}/Season{season}"
    os.makedirs(season_dir, exist_ok=True)
    for ep in range(1, 13):
        filename = f"S{season:02d}E{ep:02d}_{season_titles.get(season, [f'Episode_{ep}'])[ep-1].replace(' ', '_')}.md"
        filepath = os.path.join(season_dir, filename)
        content = generate_episode(season, ep)
        with open(filepath, 'w') as f:
            f.write(content)
        print(f"Created {filepath}")

print("All episode outlines generated.")