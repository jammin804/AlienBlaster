using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GetComponent<PlayerInputManager>().onPlayerJoined += HandlePlayerJoined;
    }

    private void HandlePlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("PlayerJoined " + playerInput.ToString());
        PlayerData playerData = GetPlayerData(playerInput.playerIndex);
        
        Player player = playerInput.GetComponent<Player>();
        player.Bind(playerData);
    }

    private PlayerData GetPlayerData(int playerIndex)
    {
        if (_playerData.Count <= playerIndex)
        {
            var playerData = new PlayerData();
            _playerData.Add(playerData);
        }
        return _playerData[playerIndex];
    }
    
    [SerializeField] List<PlayerData> _playerData = new List<PlayerData>();
}
