using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Slime : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 20;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float attackRadius = 1f;

    [Header("Attack")]
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Split")]
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private int splitCount = 2;

    private enum State { Idle, Chase, Attack }
    private State _currentState = State.Idle;

    private Rigidbody2D _rb;
    private Transform _player;
    private int _currentHealth;
    private float _lastAttackTime;
    private bool _isSplitSlime = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;
    }

    // ── IDamageable ──────────────────────────────────────

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. HP: {_currentHealth}");

        if (_currentHealth <= 0)
            Die();
    }

    // ── Split ──────────────────────────────────────

    public void InitAsSplitSlime()
    {
        _isSplitSlime  = true;
        _currentHealth = 5;
        transform.localScale *= 0.5f;
    }

    private void Update()
    {
        if (_player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        _currentState = distanceToPlayer switch
        {
            var d when d <= attackRadius    => State.Attack,
            var d when d <= detectionRadius => State.Chase,
            _                               => State.Idle
        };

        switch (_currentState)
        {
            case State.Chase:  Chase();  break;
            case State.Attack: Attack(); break;
            case State.Idle:   Idle();   break;
        }
    }

    private void Chase()
    {
        Vector2 direction = (_player.position - transform.position).normalized;
        _rb.velocity = new Vector2(direction.x * moveSpeed, _rb.velocity.y);
    }

    private void Attack()
    {
        _rb.velocity = Vector2.zero;

        bool isOnCooldown = Time.time < _lastAttackTime + attackCooldown;
        if (isOnCooldown) return;

        if (_player.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(attackDamage);

        _lastAttackTime = Time.time;
    }

    private void Idle()
    {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    // ── Death ────────────────────────────────────────────

    private void Die()
    {
        if (!_isSplitSlime)
            SpawnSplitSlimes();

        Destroy(gameObject);
    }

    private void SpawnSplitSlimes()
    {
        for (int i = 0; i < splitCount; i++)
        {
            Vector3 offset = new Vector3(i % 2 == 0 ? 0.5f : -0.5f, 0, 0);
            GameObject obj = Instantiate(slimePrefab, transform.position + offset, Quaternion.identity);

            if (obj.TryGetComponent(out Slime splitSlime))
                splitSlime.InitAsSplitSlime();
        }
    }
}
