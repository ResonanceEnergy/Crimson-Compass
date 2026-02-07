using System;

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
        public int TimeRemaining; // segments
        public int Heat;          // 0–100

        public LeadIntegrity LeadIntegrity;
        public GasketState Gasket;
        public FlagState Flag;

        public WarrantPressure WarrantPressure;

        public HeatBand GetHeatBand()
        {
            if (Heat <= 33) return HeatBand.Low;
            if (Heat <= 66) return HeatBand.Med;
            return HeatBand.High;
        }

        public TimeBand GetTimeBand()
        {
            if (TimeRemaining >= 5) return TimeBand.High;
            if (TimeRemaining >= 3) return TimeBand.Med;
            return TimeBand.Low;
        }

        public bool IsTimeOut() => TimeRemaining <= 0;
    }
}
