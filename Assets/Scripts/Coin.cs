using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.AddPoint();
            gameObject.SetActive(false);
        }
    }
}
