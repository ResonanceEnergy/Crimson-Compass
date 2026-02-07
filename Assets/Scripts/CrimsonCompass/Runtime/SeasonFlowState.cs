namespace CrimsonCompass.Runtime
{
    public enum SeasonFlowState
    {
        Boot,
        LoadingEpisode,
        SceneActive,
        ChoiceResolving,
        SnapHandling,
        CrossOff,
        WarrantRitual,
        EpisodeEnd,
        HardFail,
        SeasonEnd
    }

    public enum HardFailReason
    {
        TimeOut,
        WrongWarrant_WHO,
        WrongWarrant_WHERE,
        WrongWarrant_HOW
    }
}
