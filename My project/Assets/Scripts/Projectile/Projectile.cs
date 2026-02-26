using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    private int _damage;
    private float _lifetime = 3f;

    public void Initialize(Vector2 direction, float speed, int damageMin, int damageMax)
    {
        _rb = GetComponent<Rigidbody2D>();
        _damage = Random.Range(damageMin, damageMax + 1);
        _rb.velocity = direction * speed;

        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;

        if (other.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(_damage);

        Destroy(gameObject);
    }
}