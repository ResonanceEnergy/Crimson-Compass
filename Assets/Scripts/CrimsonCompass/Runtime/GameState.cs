using System;
using System.Collections.Generic;

namespace CrimsonCompass.Runtime
{
    public enum LeadIntegrity { Clean, Tainted, Burned, NoChange }
    public enum GasketState { Contained, Uncontained, NoChange }
    public enum FlagState { None, Tailed, StickyHeat, RouteCollapsed, NoChange }
    public enum WarrantPressure { None, Preview, Partial, Full }

    public enum HeatBand { Low, Med, High }
    public enum TimeBand { High, Med, Low }

    [Serializable]
    public struct GameState
    {
        public int timeBudget; // segments
        public int heat;          // 0–100

        public LeadIntegrity leadIntegrity;
        public GasketState gasket;
        public FlagState flag;

        public WarrantPressure warrantPressure;

        public List<string> tokens; // awarded tokens

        // Aliases for compatibility
        public int TimeRemaining { get => timeBudget; set => timeBudget = value; }
        public int Heat { get => heat; set => heat = value; }
        public LeadIntegrity LeadIntegrity { get => leadIntegrity; set => leadIntegrity = value; }
        public GasketState Gasket { get => gasket; set => gasket = value; }
        public FlagState Flag { get => flag; set => flag = value; }
        public WarrantPressure WarrantPressure { get => warrantPressure; set => warrantPressure = value; }

        public HeatBand GetHeatBand()
        {
            if (heat <= 33) return HeatBand.Low;
            if (heat <= 66) return HeatBand.Med;
            return HeatBand.High;
        }

        public TimeBand GetTimeBand()
        {
            if (timeBudget >= 5) return TimeBand.High;
            if (timeBudget >= 3) return TimeBand.Med;
            return TimeBand.Low;
        }

        public bool IsTimeOut() => timeBudget <= 0;

        public void AddToken(string token)
        {
            if (tokens == null) tokens = new List<string>();
            if (!tokens.Contains(token))
            {
                tokens.Add(token);
            }
        }

        public bool HasToken(string token)
        {
            return tokens != null && tokens.Contains(token);
        }
    }
}
