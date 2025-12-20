using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public List<string> AllGameNames = new List<string>();
    
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
        
        string commaSeparated = PlayerPrefs.GetString("AllGameNames");
        Debug.Log(commaSeparated);
        AllGameNames = commaSeparated.Split(",").ToList();
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
        
        PlayerPrefs.SetString(_gameData.GameName, text);
        
        if(AllGameNames.Contains(_gameData.GameName)==false)
            AllGameNames.Add(_gameData.GameName);
        
        string stringCommaSeperatedGameNames  = string.Join(",", AllGameNames);
        PlayerPrefs.SetString("AllGameNames", stringCommaSeperatedGameNames);
        PlayerPrefs.Save();
    }

    public void LoadGame(string gameName)
    {
        string text = PlayerPrefs.GetString(gameName);
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
        _gameData.GameName = DateTime.Now.ToString("G");
        SceneManager.LoadScene("Level 1");
    }

    public void DeleteGame(string gameName)
    {
        PlayerPrefs.DeleteKey(gameName);
        AllGameNames.Remove(gameName);
        string stringCommaSeperatedGameNames  = string.Join(",", AllGameNames);
        PlayerPrefs.SetString("AllGameNames", stringCommaSeperatedGameNames);
        PlayerPrefs.Save();
    }
}
