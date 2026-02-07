using System;

namespace CrimsonCompass.Runtime
{
    public enum WarrantConfidence { Press, Hold }
    public enum TransferModality { Vault, Vessel, Surrogate }
    public enum ApproachMode { QuietIntercept, RapidContainment, EvidenceFirst }

    [Serializable]
    public struct WarrantSelection
    {
        public string Who;
        public TransferModality Where;
        public ApproachMode How;
        public WarrantConfidence Confidence;
    }

    public static class WarrantRules
    {
        public static bool CanOpenWarrant(GameState state)
            => state.WarrantPressure == WarrantPressure.Partial || state.WarrantPressure == WarrantPressure.Full;

        public static bool CanPress(GameState state)
            => state.WarrantPressure == WarrantPressure.Full;

        public static int HoldTimeCostSegments => 1;
    }
}
