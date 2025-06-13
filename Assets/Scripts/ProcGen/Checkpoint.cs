using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] float CheckpointTimeExterntion = 5f;

    GameManager gameManager;

    const string PlayerString = "Player";

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerString))
        {
            gameManager.IncreaseTime(CheckpointTimeExterntion);
        }
    }
}
