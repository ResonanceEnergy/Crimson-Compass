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
        public string text;
        public string primary_effect;
        public string shadow_effect;
        public DeltaDto deltas;
        public string notes;
    }

    [Serializable]
    public class DeltaDto
    {
        public int time;
        public int heat;
        public int lead_integrity;
        public int gasket;
        public string flag;
    }
}