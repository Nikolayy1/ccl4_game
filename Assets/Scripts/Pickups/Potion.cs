using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Pickup
{
    [SerializeField] float manipulateMoveSpeedAmount = 3f;

    LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }
    protected override void OnPickup()
    {
        levelGenerator.ChangeChunkMoveSpeed(manipulateMoveSpeedAmount);
    }
}
