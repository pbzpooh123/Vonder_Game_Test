using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGrounded();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_moveInput.x * moveSpeed, _rb.velocity.y);
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (!_isGrounded) return;
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(
            groundCheck.position, 
            groundCheckRadius, 
            groundLayer
        );
    }

    private void FlipSprite()
    {
        if (_moveInput.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (_moveInput.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}