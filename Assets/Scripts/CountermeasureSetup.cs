using UnityEngine;
using CrimsonCompass.Runtime;

public class CountermeasureSetup : MonoBehaviour
{
    public static CountermeasureSetup Instance { get; private set; }

    private CountermeasureDeck deck;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        deck = new CountermeasureDeck();
        SetupDefaultCountermeasures();
    }

    private void SetupDefaultCountermeasures()
    {
        // Heat-based countermeasures
        deck.Add(new CountermeasureCard
        {
            Id = "heat_spike_warning",
            Name = "Heat Spike Warning",
            SnapType = SnapType.CountermeasureActivation,
            Trigger = (state, ctx) => state.Heat >= 75,
            TimeDelta = -1,
            HeatDelta = -10,
            UiToast = "Countermeasure activated: Heat dissipation protocols engaged",
            LogLine = "Heat spike countermeasure triggered"
        });

        // Time pressure countermeasures
        deck.Add(new CountermeasureCard
        {
            Id = "time_extension_protocol",
            Name = "Time Extension Protocol",
            SnapType = SnapType.CountermeasureActivation,
            Trigger = (state, ctx) => state.TimeRemaining <= 2 && state.Heat >= 50,
            TimeDelta = 2,
            HeatDelta = 5,
            UiToast = "Emergency time extension granted",
            LogLine = "Time extension protocol activated"
        });

        // Flag-based countermeasures
        deck.Add(new CountermeasureCard
        {
            Id = "route_reconstruction",
            Name = "Route Reconstruction",
            SnapType = SnapType.MajorIntrusion,
            Trigger = (state, ctx) => state.Flag == FlagState.RouteCollapsed,
            TimeDelta = -3,
            HeatDelta = -20,
            Flag = FlagState.None,
            UiToast = "Route reconstruction in progress...",
            LogLine = "Major intrusion: Route reconstruction initiated"
        });

        // Gasket countermeasures
        deck.Add(new CountermeasureCard
        {
            Id = "gasket_containment",
            Name = "Gasket Containment Protocol",
            SnapType = SnapType.GasketBoilover,
            Trigger = (state, ctx) => state.Gasket == GasketState.Uncontained,
            TimeDelta = -2,
            HeatDelta = 15,
            Gasket = GasketState.Contained,
            UiToast = "Gasket containment protocols activated",
            LogLine = "Gasket boilover contained"
        });
    }

    public CountermeasureDeck GetDeck()
    {
        return deck;
    }
}
