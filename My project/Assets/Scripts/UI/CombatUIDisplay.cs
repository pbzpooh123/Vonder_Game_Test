using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUIDisplay : MonoBehaviour
{
    [Header("HP Bar")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("AP Bar")]
    [SerializeField] private Slider apSlider;
    [SerializeField] private TextMeshProUGUI apText;

    [Header("Target")]
    [SerializeField] private PlayerCombat playerCombat;

    private void Start()
    {
        hpSlider.minValue = 0;
        hpSlider.maxValue = 100;
        hpSlider.wholeNumbers = true;

        apSlider.minValue = 0;
        apSlider.maxValue = 100;
        apSlider.wholeNumbers = true;
    }

    private void Update()
    {
        UpdateHPUI();
        UpdateAPUI();
    }

    private void UpdateHPUI()
    {
        hpSlider.value = playerCombat.CurrentHP;
        hpText.text = $"{playerCombat.CurrentHP} / {playerCombat.MaxHP}";
    }

    private void UpdateAPUI()
    {
        apSlider.value = playerCombat.CurrentAP;
        apText.text = $"{playerCombat.CurrentAP} / {playerCombat.MaxAP}";
    }
}