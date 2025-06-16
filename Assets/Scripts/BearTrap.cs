using System.Collections;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float trapDuration = 3f;
    [SerializeField] private float speedPenalty = -3f;

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
        animator.SetTrigger("Snap");

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
