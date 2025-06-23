using UnityEngine;

public class RobberNPC : MonoBehaviour
{
    [Header("Animation & Effects")]
    [SerializeField] Animator animator;
    [SerializeField] AK.Wwise.Event punchSound; // ← Wwise sound to play when the punch happens

    [Header("Gameplay")]
    [SerializeField] float speedPenalty = -3f;
    [SerializeField] float holdDuration = 1.5f;
    [SerializeField] int baseStealAmount = 50;
    [SerializeField] int stealIncrement = 25;

    static int successfulRobberHits = 0;
    bool hasActivated = false;
    LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasActivated || !other.CompareTag("Player")) return;

        hasActivated = true;

        // Face the player
        Vector3 lookDirection = other.transform.position - transform.position;
        lookDirection.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, 180f, 0);

        // Trigger punch animation
        if (animator != null)
            animator.SetTrigger("Punch");

        // Play punch sound
        punchSound?.Post(gameObject);

        // Lock movement
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.LockMovement(holdDuration);
        }

        // Pause level movement
        if (levelGenerator != null)
        {
            levelGenerator.PauseLevel(holdDuration);
            levelGenerator.ChangeChunkMoveSpeed(speedPenalty);
        }

        // Steal coins
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            int totalToSteal = baseStealAmount + stealIncrement * successfulRobberHits;
            scoreManager.ModifyScore(-totalToSteal);
            Debug.Log($"Robber stole {totalToSteal} coins!");
            successfulRobberHits++;
        }
    }
}
