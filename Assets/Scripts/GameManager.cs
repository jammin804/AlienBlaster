using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] List<PlayerData> _playerData = new List<PlayerData>();
    
    private PlayerInputManager _playerInputManager;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += HandlePlayerJoined;

        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "Menu")
        {
            _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        }
        else
        {
            _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        }
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

}
