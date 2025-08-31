using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Componentes
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    //// Hashes de Animator (no quedan variables editables en el Inspector)
    //
    //private static readonly int AnimIsGrounded = Animator.StringToHash("IsGrounded");

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private bool flipOnMove = true;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private bool enableDoubleJump = false;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [Header("Detección de suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private float _inputX;
    private bool _isGrounded;
    private bool _jumpPressed;
    private bool _canDoubleJump;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // 1) Leer input (en Update)
        _inputX = Input.GetAxisRaw("Horizontal");  // -1, 0, 1

        // Registrar intento de salto (se consume en FixedUpdate)
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }

        // Jump cut: si se suelta el botón y aún vamos hacia arriba, recorta salto
        if (Input.GetButtonUp("Jump") && _rigidbody2D.linearVelocity.y > 0f)
        {
            _rigidbody2D.linearVelocity = new Vector2(
                _rigidbody2D.linearVelocity.x,
                _rigidbody2D.linearVelocity.y * jumpCutMultiplier
            );
        }

        // Voltear sprite si se pide
        if (flipOnMove && Mathf.Abs(_inputX) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(_inputX) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void FixedUpdate()
    {
        // 2) Chequeo de suelo
        if (groundCheck != null)
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // Si tocamos suelo, restaurar doble salto
        if (_isGrounded)
        {
            _canDoubleJump = enableDoubleJump;
        }

        // 3) Movimiento horizontal
        _rigidbody2D.linearVelocity = new Vector2(_inputX * moveSpeed, _rigidbody2D.linearVelocity.y);

        // 4) Salto
        if (_jumpPressed)
        {
            bool puedeSaltar = _isGrounded || (_canDoubleJump && enableDoubleJump);

            if (puedeSaltar)
            {
                _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, jumpForce);

                if (!_isGrounded && enableDoubleJump)
                {
                    _canDoubleJump = false;
                }
            }

            _jumpPressed = false; // Consumir el intento de salto
        }

        // 5) Animación: Speed (0..1) e IsGrounded
        if (_animator != null)
        {
            float speed01 = Mathf.Clamp01(Mathf.Abs(_rigidbody2D.linearVelocity.x) / Mathf.Max(0.0001f, moveSpeed));
            _animator.SetFloat("IsRuning", speed01);
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
