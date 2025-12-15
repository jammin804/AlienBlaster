using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] GameData _gameData;
    
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
            SaveGame();
        }


    }

    private void SaveGame()
    {
        string text = JsonUtility.ToJson(_gameData);
        Debug.Log(text);
        PlayerPrefs.SetString("Game1", text);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        string text = PlayerPrefs.GetString("Game1");
        _gameData = JsonUtility.FromJson<GameData>(text);
        SceneManager.LoadScene("Level 1");
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
        if (_gameData.PlayerDatas.Count <= playerIndex)
        {
            var playerData = new PlayerData();
            _gameData.PlayerDatas.Add(playerData);
        }
        return _gameData.PlayerDatas[playerIndex];
    }

    public void NewGame()
    {
        Debug.Log("New Game Called");
        _gameData = new GameData();
        SceneManager.LoadScene("Level 1");
    }
}
