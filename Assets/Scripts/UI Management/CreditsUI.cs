using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OpenGitHubRepo()
    {
        Application.OpenURL("https://github.com/Nikolayy1/ccl4_game");
    }
}
