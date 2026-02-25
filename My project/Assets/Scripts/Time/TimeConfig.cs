using UnityEngine;

[CreateAssetMenu(fileName = "TimeConfig", menuName = "Game/Time Config")]
public class TimeConfig : ScriptableObject
{
    [Header("Periods")]
    public string[] PeriodNames = { "Morning", "Afternoon", "Evening" };

    [Header("Week")]
    public string[] DayNames =
    {
        "Monday", "Tuesday", "Wednesday",
        "Thursday", "Friday", "Saturday", "Sunday"
    };
}