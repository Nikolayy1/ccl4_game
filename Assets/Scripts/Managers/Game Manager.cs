using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TMPro.TMP_Text timeText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] float startTime = 15f; // Starting time in seconds

    float timeLeft;
    bool gameOver = false;

    //public bool GameOver { get { return gameOver; } } // Property to check if the game is over
    public bool GameOver => gameOver; // same as above, but stimpler. We just need to return the value of gameOver

    void Start()
    {
        timeLeft = startTime;
    }

    void Update()
    {
        DecreaseTime();
    }

    private void DecreaseTime()
    {
        if (gameOver) return;

        timeLeft -= Time.deltaTime;
        timeText.text = timeLeft.ToString("F1"); // overload, time formatted to 1 decimal place

        if (timeLeft <= 0f)
        {
            PLayerGameOver();
        }
    }

    void PLayerGameOver()
    {
        gameOver = true;
        playerController.enabled = false; // Disables player controls
        gameOverText.SetActive(true); //toggles the game over text visibility to active
        Time.timeScale = .1f; // Pause the game
        // Additional game over logic can be added here, such as showing final score or restarting options.
    }

}
