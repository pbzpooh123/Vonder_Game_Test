using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCombat : MonoBehaviour, IDamageable
{
    [Header("Wand")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 0.3f;

    [Header("Arcane Power")]
    [SerializeField] private float maxAP = 100f;
    [SerializeField] private float apCostPerShot = 10f;

    [Header("Damage")]
    [SerializeField] private int damageMin = 3;
    [SerializeField] private int damageMax = 5;

    [Header("Projectile")]
    [SerializeField] private float projectileSpeed = 15f;

    [Header("Health")]
    [SerializeField] private float maxHP = 100f;

    public float CurrentAP { get; private set; }
    public float MaxAP     => maxAP;
    public float CurrentHP { get; private set; }
    public float MaxHP     => maxHP;

    private float _lastFireTime;
    private Vector2 _aimDirection = Vector2.right;

    private void Start()
    {
        CurrentAP = maxAP;
        CurrentHP = maxHP;

        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeChanged += OnTimeChanged;
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeChanged -= OnTimeChanged;
    }

    // ── Input ────────────────────────────────────────────

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input.x != 0)
            _aimDirection = new Vector2(Mathf.Sign(input.x), 0);
    }

    public void OnFire(InputValue value)
    {
        if (!value.isPressed) return;
        TryShoot();
    }

    // ── Combat ───────────────────────────────────────────

    private void TryShoot()
    {
        bool isOnCooldown = Time.time < _lastFireTime + attackCooldown;
        bool hasEnoughAP  = CurrentAP >= apCostPerShot;

        if (isOnCooldown || !hasEnoughAP) return;

        SpawnProjectile();
        CurrentAP    -= apCostPerShot;
        _lastFireTime = Time.time;
    }

    private void SpawnProjectile()
    {
        GameObject obj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        if (obj.TryGetComponent(out Projectile projectile))
            projectile.Initialize(_aimDirection, projectileSpeed, damageMin, damageMax);
    }

    public void TakeDamage(int amount)
    {
        CurrentHP = Mathf.Max(CurrentHP - amount, 0);
        Debug.Log($"Player took {amount} damage. HP: {CurrentHP}");

        if (CurrentHP <= 0)
            Die();
    }

    private void Die()
    {
        CurrentHP = maxHP;
        CurrentAP = maxAP;
        Debug.Log("Player died — HP & AP reset");
    }

    // ── Reset ────────────────────────────────────────────

    public void ResetAP()
    {
        CurrentAP = maxAP;
    }

    private void OnTimeChanged(GameTime time)
    {
        ResetAP();
    }
}