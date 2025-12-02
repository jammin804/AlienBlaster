using System;
using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    
    private Player _player;

    public void Bind(Player player)
    {
        _player = player;
    }

    private void Update()
    {
        if (_player)
        {
            _scoreText.SetText(_player.Coins.ToString());
        }
 
    }
}
