using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Coin : Pickup
{
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] int scorePerCoinValue = 10;

    public void Init(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    protected override void OnPickup()
    {
        scoreManager.IncreaseScore(scorePerCoinValue);
    }
}