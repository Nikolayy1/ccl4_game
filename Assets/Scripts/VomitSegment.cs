using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VomitSegment : MonoBehaviour
{
    [Header("Debuff Settings")]
    [SerializeField] float tickInterval = 0.5f;
    [SerializeField] float speedPenalty = 2f;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 8f;

    [Header("Auto-Despawn")]
    [SerializeField] float lifetime = 8f;

    private AK.Wwise.Event impactSound;
    private ScavengerNPC scavenger;
    private bool playerInside = false;
    private float timer = 0f;
    private float age = 0f;

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

                if (levelGenerator == null)
                    levelGenerator = FindObjectOfType<LevelGenerator>();

                if (levelGenerator != null)
                {
                    levelGenerator.ChangeChunkMoveSpeed(-speedPenalty);
                    Debug.Log(">> Vomit tick: slowed environment by -" + speedPenalty);
                }

                impactSound?.Post(gameObject);
                scavenger?.PlayLaugh();
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
            timer = tickInterval;
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
