using System;
using System.Collections.Generic;

namespace CrimsonCompass.Agents
{
    public enum TriadAxis { WHO, HOW, WHERE }

    [Serializable]
    public class Hypothesis
    {
        public string whoId;
        public string howId;
        public string whereId;
    }

    [Serializable]
    public class Disproof
    {
        public TriadAxis axis;
        public string disprovedId;
        public string source;
    }

    [Serializable]
    public class IntelSource
    {
        public string id;
        public TriadAxis axis;
        public string value; // The correct value this source reveals
        public string description;
    }

    public class DisproofEngine
    {
        private Hypothesis _truth;
        private List<IntelSource> _intelSources;

        public void SetMissionTruth(Hypothesis truth)
        {
            _truth = truth;
        }

        public void SetIntelSources(List<IntelSource> sources)
        {
            _intelSources = sources;
        }

        public Hypothesis GetTruth()
        {
            return _truth;
        }

        public List<IntelSource> GetIntelSources()
        {
            return _intelSources;
        }

        // Returns exactly ONE disproof based on available intel
        public Disproof Disprove(Hypothesis h, string sourceId)
        {
            if (_truth == null || _intelSources == null) return null;

            // Find the intel source
            var source = _intelSources.Find(s => s.id == sourceId);
            if (source == null) return null;

            // Get the hypothesis value for this axis
            string hypothesisValue = GetHypothesisValue(h, source.axis);
            
            // If the hypothesis value doesn't match the truth, but this intel source reveals the truth,
            // then the hypothesis is wrong and should be disproven
            if (hypothesisValue != _truth.GetValue(source.axis) && source.value == _truth.GetValue(source.axis))
            {
                return new Disproof { axis = source.axis, disprovedId = hypothesisValue, source = sourceId };
            }

            return null; // No disproof - either hypothesis is correct or intel doesn't contradict it
        }

        private string GetHypothesisValue(Hypothesis h, TriadAxis axis)
        {
            switch (axis)
            {
                case TriadAxis.WHO: return h.whoId;
                case TriadAxis.HOW: return h.howId;
                case TriadAxis.WHERE: return h.whereId;
            }
            return null;
        }
    }

    public static class HypothesisExtensions
    {
        public static string GetValue(this Hypothesis h, TriadAxis axis)
        {
            switch (axis)
            {
                case TriadAxis.WHO: return h.whoId;
                case TriadAxis.HOW: return h.howId;
                case TriadAxis.WHERE: return h.whereId;
            }
            return null;
        }
    }
}
