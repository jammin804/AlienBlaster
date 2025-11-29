using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Frog : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRender;
    private Sprite _defaultSprite;
    private int _jumpsRemaining;
    
    [SerializeField] private int _jumps = 2;
    [SerializeField] private float _jumpDelay = 3;
    [SerializeField] private Vector2 _jumpForce;
    [SerializeField] private Sprite _jumpSprite;
    private AudioSource _audioSource;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRender = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _defaultSprite = _spriteRender.sprite;
        _jumpsRemaining = _jumps;
        InvokeRepeating("Jump", _jumpDelay, _jumpDelay);
        
    }

    private void Jump()
    {
        if (_jumpsRemaining <= 0)
        {
            _jumpForce *= new Vector2(-1, 1);
            _jumpsRemaining = _jumps;
        }
        _jumpsRemaining--;
        
        _rb.AddForce(_jumpForce);

        _spriteRender.flipX = _jumpForce.x > 0;
        _spriteRender.sprite = _jumpSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _spriteRender.sprite = _defaultSprite;
            _audioSource.Play();
        }
    }
}
