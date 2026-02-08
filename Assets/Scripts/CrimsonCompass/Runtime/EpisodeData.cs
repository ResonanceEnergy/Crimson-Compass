using System;
using System.Collections.Generic;

namespace CrimsonCompass.Runtime
{
    [Serializable]
    public class EpisodeDto
    {
        public string episode_id;
        public string title;
        public string arc;
        public string architect_echo;
        public string blueprint_fragment;
        public string faction_focus;
        public string primary_learning_axis;
        public string surface_crime_pillar;
        public string[] villain_traits;
        public string warrant_pressure;
        public string ship_gate;
        public string neon_snap_opportunity;
        public string end_hook;
        public string package_pdf;
        public BackgroundAbsurdityDto background_absurdity;
        public string shadow_token;
        public List<SceneDto> scenes;
    }

    [Serializable]
    public class SceneDto
    {
        public int scene_id;
        public string scene_text;
        public string scene_description;
        public List<ChoiceDto> choices;
    }

    [Serializable]
    public class ChoiceDto
    {
        public string id;
        public string advisor;
        public string label;
        public string consequence;
        public int heat_delta;
        public int time_delta;
        public string[] awards;
    }

    [Serializable]
    public class DeltaDto
    {
        public int time;
        public int heat;
    }

    [Serializable]
    public class BackgroundAbsurdityDto
    {
        public bool enabled;
        public int loop_minutes;
        public string location_type;
        public string pattern_id;
        public string theme;
    }
}