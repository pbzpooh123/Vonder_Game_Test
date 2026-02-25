using UnityEngine;
using TMPro;

public class TimeUIDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeLabel;

    private void Start()
    {
        if (TimeManager.Instance == null)
        {
            Debug.LogError("TimeManager not found in scene!");
            return;
        }

        TimeManager.Instance.OnTimeChanged += UpdateDisplay;
        UpdateDisplay(TimeManager.Instance.CurrentTime);
    }

    private void OnDisable()
    {
        if (TimeManager.Instance == null) return;
        TimeManager.Instance.OnTimeChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(GameTime time)
    {
        timeLabel.text = $"Day {time.TotalDays} — {time.DayName} — {time.PeriodName}";
    }
}