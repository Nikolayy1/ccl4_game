using UnityEngine;

public class PotionTracker : MonoBehaviour
{
    public static PotionTracker Instance { get; private set; }

    [SerializeField] int potionsPerScavenger = 5;

    public int PotionsPickedUp { get; private set; } = 0;
    public bool ScavengerActive { get; private set; } = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }

    public void RegisterPotionPickup()
    {
        PotionsPickedUp++;
    }

    public bool ShouldSpawnScavenger()
    {
        return !ScavengerActive && PotionsPickedUp >= potionsPerScavenger;
    }

    public void ResetPotionCounter()
    {
        PotionsPickedUp = 0;
    }

    public void SetScavengerActive(bool active)
    {
        ScavengerActive = active;
        Debug.Log(">> ScavengerActive set to: " + active);
    }
}
