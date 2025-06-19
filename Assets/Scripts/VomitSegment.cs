using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VomitSegment : MonoBehaviour
{
    [Header("Debuff Settings")]
    [SerializeField] float tickInterval = 1f;
    [SerializeField] float slowAmount = 2f;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 8f;

    [Header("Auto-Despawn")]
    [SerializeField] float lifetime = 8f;

    private AK.Wwise.Event impactSound;
    private ScavengerNPC scavenger;

    private float age = 0f;
    private float timer = 0f;
    private bool playerInside = false;

    private LevelGenerator levelGenerator;

    void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

        if (playerInside)
        {
            timer += Time.deltaTime;

            if (timer >= tickInterval)
            {
                timer = 0f;

                // Ask scavenger if we're allowed to tick
                if (scavenger != null && scavenger.CanApplyTick())
                {
                    if (levelGenerator == null)
                        levelGenerator = FindObjectOfType<LevelGenerator>();

                    if (levelGenerator != null)
                    {
                        levelGenerator.ChangeChunkMoveSpeed(-slowAmount);
                        Debug.Log(">> Vomit tick: slowed by " + slowAmount);
                    }

                    impactSound?.Post(gameObject);
                    scavenger.PlayLaugh();
                }
                else
                {
                    // Disable ticking if scavenger budget exhausted
                    playerInside = false;
                }
            }
        }

        age += Time.deltaTime;
        if (age >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            playerInside = true;
            timer = tickInterval; // trigger first tick instantly
            Debug.Log(">> Player entered vomit zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            playerInside = false;
            timer = 0f;
            Debug.Log(">> Player exited vomit zone.");
        }
    }

    public void SetImpactSound(AK.Wwise.Event soundEvent)
    {
        impactSound = soundEvent;
    }

    public void SetScavengerReference(ScavengerNPC scavengerNPC)
    {
        scavenger = scavengerNPC;
    }
}
