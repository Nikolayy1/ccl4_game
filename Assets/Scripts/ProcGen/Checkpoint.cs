using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] float CheckpointTimeExterntion = 5f;
    [SerializeField] float ObstacleDecreaseTimeAmount = 0.2f; // with every checkpoint, obstacles spwn faster

    GameManager gameManager;
    ObstacleSpawner obstacleSpawner;

    const string PlayerString = "Player";

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerString))
        {
            gameManager.IncreaseTime(CheckpointTimeExterntion);
            obstacleSpawner.DecreaseObstacleSpawnTime(ObstacleDecreaseTimeAmount);
        }
    }
}
