using System;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRender;
    private bool IsGrounded;
    private Sprite _defaultSprite;

    [SerializeField] private float _jumpDelay = 3;
    [SerializeField] private Vector2 _jumpForce;
    [SerializeField] private Sprite _jumpSprite;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRender = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRender.sprite;
        InvokeRepeating("Jump", _jumpDelay, _jumpDelay);
        
    }

    private void Jump()
    {
        // IsGrounded = true;
        _rb.AddForce(_jumpForce);
        _jumpForce *= new Vector2(-1, 1);
        _spriteRender.flipX = !_spriteRender.flipX;
        _spriteRender.sprite = _jumpSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _spriteRender.sprite = _defaultSprite;
        }
    }
}
