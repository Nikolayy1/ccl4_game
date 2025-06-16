using System.Collections;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float trapDuration = 3f;

    private const string PlayerTag = "Player";
    private bool hasSnapped = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasSnapped) return;

        if (other.CompareTag(PlayerTag))
        {
            hasSnapped = true;
            animator.SetTrigger("Snap"); // Trigger your snap animation

            // Freeze player movement
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.LockMovement(trapDuration);
            }

            StartCoroutine(ReleaseTrap());
        }
    }

    IEnumerator ReleaseTrap()
    {
        yield return new WaitForSeconds(trapDuration);
        // You can do something like play a "reset" anim here if needed
    }
}
