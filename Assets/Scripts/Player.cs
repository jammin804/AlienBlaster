using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly int Grounded = Animator.StringToHash("IsGrounded");
    private float _jumpEndTime;
    [SerializeField] private float _horizontalVelocity = 3.0f;
    [SerializeField] private float _jumpVelocity = 5.0f;
    [SerializeField] private float _jumpDuration = 0.5f;
    [SerializeField] private Sprite _jumpSprite;
    private Sprite _defaultSprite;
    public bool IsGrounded;
    private SpriteRenderer _spriteRenderer;
    private float _horizontal;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
    }

    private void OnDrawGizmos()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.sprite.bounds.extents.y);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per frame
    private void Update()
    {
        var origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider != null)
            IsGrounded = true;
        else
            IsGrounded = false;

        _horizontal = Input.GetAxis("Horizontal");
        Debug.Log(_horizontal);
        var rb = GetComponent<Rigidbody2D>();
        var vertical = rb.linearVelocity.y;

        if (Input.GetButtonDown("Fire1") && IsGrounded)
            _jumpEndTime = Time.time + _jumpDuration;

        if (Input.GetButton("Fire1") && _jumpEndTime > Time.time) vertical = _jumpVelocity;

        _horizontal *= _horizontalVelocity;
        rb.linearVelocity = new Vector2(_horizontal, vertical);
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        GetComponent<Animator>().SetBool(Grounded, IsGrounded);
        GetComponent<Animator>().SetFloat("HorizontalSpeed", Math.Abs(_horizontal));

        if (_horizontal > 0)
            _spriteRenderer.flipX = false;
        else if (_horizontal < 0)
            _spriteRenderer.flipX = true;
    }
}