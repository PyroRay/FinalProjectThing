using UnityEngine;
using UnityEngine.SceneManagement;
public class ClickToStart : MonoBehaviour
{
    public bool clicked = false;

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && clicked == false)
        {
            clicked = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
