using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAreaTrigger : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent(out PlayerCombat playerCombat))
            playerCombat.ResetAP();
    }
}
