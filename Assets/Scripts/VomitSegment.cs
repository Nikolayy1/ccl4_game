using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VomitSegment : MonoBehaviour
{
    [Header("Debuff Settings")]
    [SerializeField] float speedPenalty = -2f;
    [SerializeField] float tickInterval = 0.5f;

    [Header("Movement Settings")]
    [Tooltip("Speed at which this segment moves toward the player (units/sec)")]
    [SerializeField] float moveSpeed = 8f;

    [Header("Auto-Despawn")]
    [Tooltip("Time in seconds before this segment is automatically destroyed")]
    [SerializeField] float lifetime = 8f;

    private bool playerInside = false;
    private float timer = 0f;
    private PlayerController player;
    private float age = 0f;

    void Awake()
    {
        // Ensure the Rigidbody is kinematic to avoid unwanted physics
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Update()
    {
        // Move the segment toward the player
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

        // Debuff ticking
        if (playerInside && player != null)
        {
            timer += Time.deltaTime;
            if (timer >= tickInterval)
            {
                timer = 0f;
                player.ApplySpeedDebuff(speedPenalty);
                Debug.Log(">> [Segment] Debuff applied (" + speedPenalty + ")");
            }
        }

        // Auto-destroy after lifetime
        age += Time.deltaTime;
        if (age >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var candidate = other.GetComponentInParent<PlayerController>();
        if (candidate != null)
        {
            player = candidate;
            playerInside = true;
            timer = tickInterval; // trigger immediately
            Debug.Log(">> Player entered vomit zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        var candidate = other.GetComponentInParent<PlayerController>();
        if (candidate != null && candidate == player)
        {
            playerInside = false;
            player = null;
            timer = 0f;
            Debug.Log(">> Player exited vomit zone.");
        }
    }
}
