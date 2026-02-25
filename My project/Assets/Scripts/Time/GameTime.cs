public readonly struct GameTime
{
    public readonly int TotalDays;
    public readonly int PeriodIndex;

    private readonly string[] _periodNames;
    private readonly string[] _dayNames;

    public GameTime(int totalDays, int periodIndex, string[] periodNames, string[] dayNames)
    {
        TotalDays   = totalDays;
        PeriodIndex = periodIndex;
        _periodNames = periodNames;
        _dayNames    = dayNames;
    }

    public string PeriodName => _periodNames[PeriodIndex];
    public string DayName    => _dayNames[TotalDays % _dayNames.Length];
}