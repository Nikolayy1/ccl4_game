using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroToMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
