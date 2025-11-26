using System;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRender;
    [SerializeField] private float _jumpDelay = 3;
    [SerializeField] private Vector2 _jumpForce;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRender = GetComponent<SpriteRenderer>();
        InvokeRepeating("Jump", _jumpDelay, _jumpDelay);
    }

    private void Jump()
    {
        _rb.AddForce(_jumpForce);
    }
}
