using UnityEngine;

public class Player : MonoBehaviour
{
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
        rb.linearVelocity = new Vector2(horizontal, rb.linearVelocity.y);
    }
}
