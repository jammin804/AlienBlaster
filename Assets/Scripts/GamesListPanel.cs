using System;
using UnityEngine;

public class GamesListPanel : MonoBehaviour
{
    [SerializeField] private LoadGameButton _buttonPrefab;

    private void Start()
    {
        foreach (var gameName in GameManager.Instance.AllGameNames)
        {
            var button =  Instantiate(_buttonPrefab, transform); 
            button.SetGameName(gameName);
        }
    }
}
