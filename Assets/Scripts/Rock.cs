using UnityEngine;
using Cinemachine;

public class Rock : MonoBehaviour
{
    [Header("FX")]
    [SerializeField] float shakeModifier = 10f;
    [SerializeField] ParticleSystem collisionParticleSystem;
    [SerializeField] AK.Wwise.Event collisionSound;

    [Header("Gameplay")]
    [SerializeField] public float speedPenalty = -5f;

    CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] float collisionTimer = 1f;

    void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        collisionTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (collisionTimer < 0.1f) return;

        FireImpulse(other);
        CollisionFX(other);
        collisionTimer = 0f;

        if (other.gameObject.CompareTag("Player"))
        {
            LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
            if (levelGenerator != null)
            {
                levelGenerator.ChangeChunkMoveSpeed(speedPenalty);
                Debug.Log($"Rock hit: {speedPenalty} speed penalty applied.");
            }
        }
    }

    void FireImpulse(Collision other)
    {
        float distance = Vector3.Distance(other.transform.position, Camera.main.transform.position);
        float shakeIntensity = Mathf.Min((1f / distance) * shakeModifier, 1f);
        cinemachineImpulseSource.GenerateImpulse(shakeIntensity);
    }

    void CollisionFX(Collision other)
    {
        ContactPoint contactPoint = other.GetContact(0);
        collisionParticleSystem.transform.position = contactPoint.point;
        collisionParticleSystem.Play();
        collisionSound?.Post(gameObject);
    }
}
