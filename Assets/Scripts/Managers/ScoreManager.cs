using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text scoreText;

    int score = 0;

    public int CurrentScore => score;

    public void IncreaseScore(int amount)
    {
        if (gameManager.GameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    // Subtract score (used by RobberNPC)
    public void ModifyScore(int amount)
    {
        score += amount;
        if (score < 0) score = 0;
        UpdateScoreUI();
    }

    // Used by RobberNPC to check current value
    public int GetScore()
    {
        return score;
    }

    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }
}

