using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void RetryGame()
    {
        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
