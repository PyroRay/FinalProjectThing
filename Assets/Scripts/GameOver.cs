using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter()
    {
        gameManager.Restart();
    }
}
