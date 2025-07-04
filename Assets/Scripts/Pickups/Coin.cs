using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Coin : Pickup
{
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] int scorePerCoinValue = 10;
    [SerializeField] float timeBoostPerCoin = 1f;
    [SerializeField] AK.Wwise.Event coinPickupSound;

    public void Init(ScoreManager scoreManager, GameManager gameManager)
    {
        this.scoreManager = scoreManager;
        this.gameManager = gameManager;
    }

    protected override void OnPickup()
    {
        scoreManager?.IncreaseScore(scorePerCoinValue);
        gameManager?.IncreaseTime(timeBoostPerCoin);

        coinPickupSound?.Post(triggeringObject ?? gameObject);
    }
}
