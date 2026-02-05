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

        // Returns exactly ONE disproof based on available intel
        public Disproof Disprove(Hypothesis h, string sourceId)
        {
            if (_truth == null || _intelSources == null) return null;

            // Find the intel source
            var source = _intelSources.Find(s => s.id == sourceId);
            if (source == null) return null;

            // Check if hypothesis matches truth on this axis
            string hypothesisValue = GetHypothesisValue(h, source.axis);
            if (hypothesisValue == _truth.GetValue(source.axis))
            {
                // Hypothesis is correct on this axis, but source reveals the truth, so disprove wrong ones
                // Actually, intel sources reveal correct values, so if hypothesis has wrong value, disprove it
                if (hypothesisValue != source.value)
                {
                    return new Disproof { axis = source.axis, disprovedId = hypothesisValue, source = sourceId };
                }
            }
            else
            {
                // Hypothesis is wrong, but source might confirm or disprove
                // For simplicity, if source reveals the correct value, and hypothesis is wrong, disprove hypothesis
                if (source.value == _truth.GetValue(source.axis))
                {
                    return new Disproof { axis = source.axis, disprovedId = hypothesisValue, source = sourceId };
                }
            }

            return null; // No disproof
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
