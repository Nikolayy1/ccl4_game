using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Called by the Play button
    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    // Called by the Highscore button
    public void ShowScores()
    {
        SceneManager.LoadScene("HighScoresScene");
    }

    // Called by the Credits button
    public void ShowCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    // Called by the Quit Game button
    public void ExitGame()
    {
        Debug.Log("Quitting the game..."); // Only shows in editor
        Application.Quit();
    }
}
