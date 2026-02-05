using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Agents;
using CrimsonCompass.Core;

namespace CrimsonCompass
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Data")]
        public TextAsset caseJson;
        public TextAsset agentsJson;
        public TextAsset insightsJsonl;

        [Header("UI")]
        public NotepadUI notepadUI;
        public HypothesisInput hypothesisInput;

        private CaseData currentCase;
        private DisproofEngine disproofEngine;
        private EventBus eventBus;

        void Awake()
        {
            Instance = this;
            eventBus = new EventBus();
            disproofEngine = new DisproofEngine();
        }

        void Start()
        {
            LoadCase();
            eventBus.Subscribe(GameEventType.HYPOTHESIS_SUBMITTED, OnHypothesisSubmitted);
        }

        void LoadCase()
        {
            currentCase = JsonUtility.FromJson<CaseData>(caseJson.text);
            // Set truth for disproof engine
            var truth = new Hypothesis
            {
                whoId = currentCase.truth.whoId,
                howId = currentCase.truth.howId,
                whereId = currentCase.truth.whereId
            };
            disproofEngine.SetMissionTruth(truth);

            // Set intel sources (simplified: each suspect/method/location has an intel source)
            var sources = new List<IntelSource>();
            foreach (var s in currentCase.suspects)
            {
                sources.Add(new IntelSource { id = s.id, axis = TriadAxis.WHO, value = s.id, description = $"Intel on {s.name}" });
            }
            foreach (var m in currentCase.methods)
            {
                sources.Add(new IntelSource { id = m.id, axis = TriadAxis.HOW, value = m.id, description = $"Intel on {m.name}" });
            }
            foreach (var l in currentCase.locations)
            {
                sources.Add(new IntelSource { id = l.id, axis = TriadAxis.WHERE, value = l.id, description = $"Intel on {l.id}" });
            }
            disproofEngine.SetIntelSources(sources);

            eventBus.Publish(GameEventType.SESSION_OPEN);
        }

        void OnHypothesisSubmitted(object payload)
        {
            var hypothesis = (Hypothesis)payload;
            // For now, use first intel source
            var disproof = disproofEngine.Disprove(hypothesis, currentCase.suspects[0].id);
            if (disproof != null)
            {
                notepadUI.MarkDisproved(disproof.axis.ToString(), disproof.disprovedId);
                eventBus.Publish(GameEventType.DISPROOF_RETURNED, disproof);
            }
        }
    }

    [System.Serializable]
    public class CaseData
    {
        public string caseId;
        public string title;
        public Suspect[] suspects;
        public Method[] methods;
        public Location[] locations;
        public Truth truth;
    }

    [System.Serializable]
    public class Suspect { public string id; public string name; }
    [System.Serializable]
    public class Method { public string id; public string name; }
    [System.Serializable]
    public class Location { public string id; public string country; }
    [System.Serializable]
    public class Truth { public string whoId; public string howId; public string whereId; }
}