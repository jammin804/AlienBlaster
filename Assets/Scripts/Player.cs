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
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _footOffset = 0.35f;
    public bool IsGrounded;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private float _horizontal;



    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;
        
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.sprite.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
        
        //Draw Left Foot
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - spriteRenderer.sprite.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
        
        //Draw Right Foot
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - spriteRenderer.sprite.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateGrounding();

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

    private void UpdateGrounding()
    {
        IsGrounded = false;
        
        //Check center
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
            IsGrounded = true;
        
        //Check left
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
            IsGrounded = true;
        
        //Check right
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.sprite.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
            IsGrounded = true;
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