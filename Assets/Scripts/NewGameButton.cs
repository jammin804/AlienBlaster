using UnityEngine;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(CreateNewGame);
    }

    public void CreateNewGame()
    {
        GameManager.Instance.NewGame();
    }
}
