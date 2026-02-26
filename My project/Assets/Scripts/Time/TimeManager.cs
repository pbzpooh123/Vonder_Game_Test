using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private TimeConfig config;

    public GameTime CurrentTime { get; private set; }

    public event System.Action<GameTime> OnTimeChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        CurrentTime = new GameTime(100, 0, config.PeriodNames, config.DayNames);
    }

    public void AdvanceTime()
    {
        int nextPeriod = CurrentTime.PeriodIndex + 1;
        bool dayHasEnded = nextPeriod >= config.PeriodNames.Length;

        int nextDay    = dayHasEnded ? CurrentTime.TotalDays + 1 : CurrentTime.TotalDays;
        int wrappedPeriod = dayHasEnded ? 0 : nextPeriod;

        CurrentTime = new GameTime(nextDay, wrappedPeriod, config.PeriodNames, config.DayNames);
        OnTimeChanged?.Invoke(CurrentTime);
    }
}