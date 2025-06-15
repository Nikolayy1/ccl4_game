using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TMPro.TMP_Text scoreText;

    int score = 0;

    public int CurrentScore => score;


    public void IncreaseScore(int amount)
    {
        if (gameManager.GameOver) return; // Do not increase score if the game is over

        score += amount;
        scoreText.text = score.ToString();
    }


}
