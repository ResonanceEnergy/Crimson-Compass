using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonCompass.Data
{
    [CreateAssetMenu(menuName = "CrimsonCompass/Episode Asset", fileName = "EpisodeAsset")]
    public class EpisodeAsset : ScriptableObject
    {
        [Header("Metadata")]
        public string episodeId;
        public string title;
        public string arc;
        public string blueprintFragment;
        public string architectEcho;
        public string surfaceCrimePillar;
        public string factionFocus;
        public List<string> villainTraits = new();
        public string primaryLearningAxis;
        public string warrantPressure;
        public string neonSnapOpportunity;
        public string endHook;
        public string shipGate;
        public string packagePdf;

        [Header("Scenes")]
        public List<SceneData> scenes = new();

        [NonSerialized] public Dictionary<int, SceneData> sceneById;
        [NonSerialized] public Dictionary<(int sceneId, string choiceId), ChoiceData> choiceByKey;

        public void BuildIndexes()
        {
            sceneById = new Dictionary<int, SceneData>();
            choiceByKey = new Dictionary<(int, string), ChoiceData>();
            if (scenes == null) return;
            foreach (var s in scenes)
            {
                sceneById[s.sceneId] = s;
                if (s.choices == null) continue;
                foreach (var c in s.choices)
                    choiceByKey[(s.sceneId, c.id)] = c;
            }
        }

        [Serializable]
        public class SceneData
        {
            public int sceneId;
            public List<ChoiceData> choices = new();
        }

        [Serializable]
        public class ChoiceData
        {
            public string id;
            public string text;
            public string primaryEffect;
            public string shadowEffect;
            public DeltaData deltas;
            public string notes;
        }

        [Serializable]
        public class DeltaData
        {
            public int time;
            public int heat;
            public string leadIntegrity;
            public string gasket;
            public string flag;
        }
    }
}
