using UnityEngine;

public class Player : MonoBehaviour
{
    private float _jumpEndTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        Debug.Log(horizontal);
        var rb = GetComponent<Rigidbody2D>();
        var vertical = rb.linearVelocity.y;
        
        if (Input.GetButtonDown("Fire1"))
            _jumpEndTime = Time.time + 0.5f;
        
        if (Input.GetButton("Fire1") && _jumpEndTime > Time.time)
        {
            vertical = 5;
        }

        rb.linearVelocity = new Vector2(horizontal, vertical);
    }
}
