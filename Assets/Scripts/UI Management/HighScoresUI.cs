using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoresUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreListText;

    void Start()
    {
        var scores = StateManager.Instance.highScores;
        scoreListText.text = "";

        for (int i = 0; i < scores.Count; i++)
        {
            scoreListText.text += $"#{i + 1}   {scores[i]}\n";
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
