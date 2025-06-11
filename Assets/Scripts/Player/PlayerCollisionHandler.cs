using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float collisionCooldown = 1f;
    [SerializeField] float manipulateMoveSpeedAmount = -2f;

    const string hitString = "Hit";

    float cooldownTimer = 0f;

    LevelGenerator levelGenerator; // Reference to LevelGenerator script

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>(); // Find the LevelGenerator in the scene
    }
    void Update()
    {

        cooldownTimer += Time.deltaTime; // Increment cooldown timer, seconds

    }

    void OnCollisionEnter(Collision other)
    {
        if (cooldownTimer < collisionCooldown) return; // blocks the hit animation if on cooldown
        levelGenerator.ChangeChunkMoveSpeed(manipulateMoveSpeedAmount);
        animator.SetTrigger(hitString);
        cooldownTimer = 0f; // Reset cooldown timer, so no multiple hits in a short time
    }
}
