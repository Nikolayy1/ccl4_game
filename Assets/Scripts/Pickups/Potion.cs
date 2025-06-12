using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Pickup
{
    [SerializeField] float manipulateMoveSpeedAmount = 3f;

    LevelGenerator levelGenerator;

    // dependency injection, faster than FindObjectOfType<LevelGenerator>()
    public void Init(LevelGenerator levelGenerator)
    {
        this.levelGenerator = levelGenerator;
    }

    protected override void OnPickup()
    {
        levelGenerator.ChangeChunkMoveSpeed(manipulateMoveSpeedAmount);
    }
}
