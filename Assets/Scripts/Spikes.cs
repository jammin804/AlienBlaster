using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var player =  collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }
}
