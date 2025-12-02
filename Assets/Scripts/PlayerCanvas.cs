using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private PlayerPanel[] _playerPanels;

    public void Bind(Player player)
    {
        var playerInput = player.GetComponent<PlayerInput>();
        _playerPanels[playerInput.playerIndex].Bind(player);
    }
}
