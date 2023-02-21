using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayScene : MonoBehaviour
{
    public static void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
