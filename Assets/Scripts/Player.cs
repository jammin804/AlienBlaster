using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private static readonly int Grounded = Animator.StringToHash("IsGrounded");
    [FormerlySerializedAs("_horizontalVelocity")] [SerializeField] private float _maxHorizontalSpeed = 5.0f;
    [SerializeField] private float _jumpVelocity = 5.0f;
    [SerializeField] private float _jumpDuration = 0.5f;
    [SerializeField] private Sprite _jumpSprite;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _footOffset = 0.35f;
    [SerializeField] private float _acceleration = 10.0f;
    
    public bool IsGrounded;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    
    private float _horizontal;
    private int _jumpsRemaining;
    private float _jumpEndTime;
    private Rigidbody2D _rb;



    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.sprite.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        //Draw Left Foot
        origin = new Vector2(transform.position.x - _footOffset,
            transform.position.y - spriteRenderer.sprite.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        //Draw Right Foot
        origin = new Vector2(transform.position.x + _footOffset,
            transform.position.y - spriteRenderer.sprite.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateGrounding();
        
        var horizontalInput = Input.GetAxis("Horizontal");
        
        var vertical = _rb.linearVelocity.y;

        if (Input.GetButtonDown("Fire1") && _jumpsRemaining > 0)
        {
            _jumpEndTime = Time.time + _jumpDuration;
            _jumpsRemaining--;

            _audioSource.pitch = (_jumpsRemaining > 0) ? 1f : 1.2f;
            _audioSource.Play();
        }

        if (Input.GetButton("Fire1") && _jumpEndTime > Time.time) vertical = _jumpVelocity;

        var desiredHorizontal = horizontalInput * _maxHorizontalSpeed;
        _horizontal = Mathf.Lerp(_horizontal, desiredHorizontal, Time.deltaTime * _acceleration);
        _rb.linearVelocity = new Vector2(_horizontal, vertical);
        UpdateSprite();
    }

    private void UpdateGrounding()
    {
        IsGrounded = false;

        //Check center
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
            IsGrounded = true;

        //Check left
        origin = new Vector2(transform.position.x - _footOffset,
            transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
            IsGrounded = true;

        //Check right
        origin = new Vector2(transform.position.x + _footOffset,
            transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
            IsGrounded = true;

        if (IsGrounded && GetComponent<Rigidbody2D>().linearVelocity.y <= 0) _jumpsRemaining = 2;
    }

    private void UpdateSprite()
    {
        _animator.SetBool(Grounded, IsGrounded);
        _animator.SetFloat("HorizontalSpeed", Math.Abs(_horizontal));

        if (_horizontal > 0)
            _spriteRenderer.flipX = false;
        else if (_horizontal < 0)
            _spriteRenderer.flipX = true;
    }
}