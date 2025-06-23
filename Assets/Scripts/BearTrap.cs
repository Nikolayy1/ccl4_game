using System.Collections;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float trapDuration = 3f;
    [SerializeField] private float speedPenalty = -3f;

    [Header("Audio")]
    [SerializeField] private AK.Wwise.Event snapSound; // ← Wwise audio for trap snap

    private const string PlayerTag = "Player";
    private bool hasSnapped = false;
    private LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bear trap triggered by: " + other.name);

        if (hasSnapped || !other.CompareTag(PlayerTag)) return;

        hasSnapped = true;

        // Play snap animation
        animator.SetTrigger("Snap");

        // Play Wwise snap sound
        snapSound?.Post(gameObject);

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.LockMovement(trapDuration);
            levelGenerator.PauseLevel(trapDuration);
            levelGenerator.ChangeChunkMoveSpeed(speedPenalty);
        }

        StartCoroutine(StaySnapped());
    }

    IEnumerator StaySnapped()
    {
        yield return new WaitForSeconds(trapDuration);
        animator.SetTrigger("Reset");
    }
}
