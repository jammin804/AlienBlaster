using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _jumpEndTime;
    [SerializeField] private float _jumpVelocity = 5.0f;
    [SerializeField] private float _jumpDuration = 0.5f;

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float bottomY = spriteRenderer.sprite.bounds.extents.y;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - bottomY);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
        var horizontal = Input.GetAxis("Horizontal");
        Debug.Log(horizontal);
        var rb = GetComponent<Rigidbody2D>();
        var vertical = rb.linearVelocity.y;
        
        if (Input.GetButtonDown("Fire1"))
            _jumpEndTime = Time.time + _jumpDuration;
        
        if (Input.GetButton("Fire1") && _jumpEndTime > Time.time)
        {
            vertical = _jumpVelocity;
        }

        rb.linearVelocity = new Vector2(horizontal, vertical);
    }
}
