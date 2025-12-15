using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [FormerlySerializedAs("_acceleration")] [SerializeField] private float _groundAcceleration = 10.0f;
    [SerializeField] private float _snowAcceleration = 1.0f;
    [SerializeField] private AudioClip _coinSFX;
    
    public bool IsGrounded;
    [FormerlySerializedAs("OnSnow")] public bool IsOnSnow;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    private Rigidbody2D _rb;
    private PlayerInput _playerInput;
    
    private float _horizontal;
    private int _jumpsRemaining;
    private float _jumpEndTime;
    
    private PlayerData _playerData = new PlayerData();
    
    public int Coins { get => _playerData.Coins; private set => _playerData.Coins = value; }
    

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        
        FindFirstObjectByType<PlayerCanvas>().Bind(this);
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
        
        var horizontalInput = _playerInput.actions["Move"].ReadValue<Vector2>().x;
        
        var vertical = _rb.linearVelocity.y;

        if (_playerInput.actions["Jump"].WasPerformedThisFrame() && _jumpsRemaining > 0)
        {
            _jumpEndTime = Time.time + _jumpDuration;
            _jumpsRemaining--;

            _audioSource.pitch = (_jumpsRemaining > 0) ? 1f : 1.2f;
            _audioSource.Play();
        }

        if (_playerInput.actions["Jump"].ReadValue<float>() > 0 && _jumpEndTime > Time.time) vertical = _jumpVelocity;

        var desiredHorizontal = horizontalInput * _maxHorizontalSpeed;
        var acceleration = IsOnSnow ? _snowAcceleration : _groundAcceleration;
        _horizontal = Mathf.Lerp(_horizontal, desiredHorizontal, Time.deltaTime * acceleration);
        _rb.linearVelocity = new Vector2(_horizontal, vertical);
        UpdateSprite();
    }

    private void UpdateGrounding()
    {
        IsGrounded = false;
        IsOnSnow = false;

        //Check center
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
        {
            IsGrounded = true;
            IsOnSnow = hit.collider.CompareTag("Snow");
        }


        //Check left
        origin = new Vector2(transform.position.x - _footOffset,
            transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
        {
            IsGrounded = true;
            IsOnSnow = hit.collider.CompareTag("Snow");
        }

        //Check right
        origin = new Vector2(transform.position.x + _footOffset,
            transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
        {
            IsGrounded = true;
            IsOnSnow = hit.collider.CompareTag("Snow");
        }

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

    public void AddPoint()
    {
        Coins++;
        _audioSource.PlayOneShot(_coinSFX);
    }

    public void Bind(PlayerData playerData)
    {
        _playerData = playerData;
    }
}