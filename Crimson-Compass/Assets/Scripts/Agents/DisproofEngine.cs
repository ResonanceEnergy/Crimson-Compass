using System;

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

    public class DisproofEngine
    {
        // TODO: Use mission truth triad and intel sources to return exactly ONE disproof.
        public Disproof Disprove(Hypothesis h, string source)
        {
            // placeholder; real version ensures fairness + solvability
            return new Disproof { axis = TriadAxis.WHO, disprovedId = h.whoId, source = source };
        }
    }
}
