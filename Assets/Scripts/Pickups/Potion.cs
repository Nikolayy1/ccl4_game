using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Pickup
{
    [Header("Gameplay")]
    [SerializeField] float manipulateMoveSpeedAmount = 3f;
    [Header("FX")]
    [SerializeField] AK.Wwise.Event pickupSound;  // Wwise audio event

    LevelGenerator levelGenerator;

    // dependency injection, faster than FindObjectOfType<LevelGenerator>()
    public void Init(LevelGenerator levelGenerator)
    {
        this.levelGenerator = levelGenerator;
    }

    protected override void OnPickup()
    {
        levelGenerator.ChangeChunkMoveSpeed(manipulateMoveSpeedAmount);
        // Play Wwise sound event
        pickupSound?.Post(gameObject);
    }
}
