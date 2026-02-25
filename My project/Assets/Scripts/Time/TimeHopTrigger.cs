using UnityEngine;

public class TimeHopTrigger : MonoBehaviour
{
    [SerializeField] private float cooldownSeconds = 1f;

    private bool _onCooldown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_onCooldown) return;
        if (!other.CompareTag("Player")) return;

        TimeManager.Instance.AdvanceTime();
        StartCoroutine(StartCooldown());
    }

    private System.Collections.IEnumerator StartCooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(cooldownSeconds);
        _onCooldown = false;
    }
}