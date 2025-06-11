using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup
{
    protected override void OnPickup()
    {
        // Logic for what happens when the coin is picked up
        Debug.Log("Coin picked up!");
    }
}
