using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image[] _hearts;
    private Player _player;

    public void Bind(Player player)
    {
        _player = player;
        _player.CoinsChanged += UpdateCoins;
        _player.HealthChanged += UpdateHealth;
        UpdateCoins();
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
            Image heart = _hearts[i];
            heart.enabled = i < _player.Health;
        }
    }

    private void UpdateCoins()
    {
        _scoreText.SetText(_player.Coins.ToString());
    }
}
