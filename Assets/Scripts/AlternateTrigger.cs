using UnityEngine;

public class AlternateTrigger : MonoBehaviour
{
    public int levelnum;
    public GameManager gameManager;

    private void OnTriggerEnter()
    {
        gameManager.GoToLevel(levelnum);
    }
}
