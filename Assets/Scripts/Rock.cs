using UnityEngine;
using Cinemachine; // optional depending on version

public class Rock : MonoBehaviour
{
    [SerializeField] float shakeModifier = 10f;
    [SerializeField] ParticleSystem collisionParticleSystem;
    [SerializeField] AudioSource collisionSound;

    CinemachineImpulseSource cinemachineImpulseSource;

    float collisionTimer = 1f;

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
        if (collisionTimer < 0.1f) return; // Prevent multiple collisions within a short time frame

        FireImpulse(other);
        CollisionFX(other);
        collisionTimer = 0f; // Reset the timer after handling the collision
    }

    void FireImpulse(Collision other)
    {
        // Distance between the rock and the camera, to calculate the impulse strength
        float distance = Vector3.Distance(other.transform.position, Camera.main.transform.position);
        float shakeIntensity = (1f / distance) * shakeModifier; // Adjust the intensity based on distance
        shakeIntensity = Mathf.Min(shakeIntensity, 1f); // Cap the intensity to a maximum value
        cinemachineImpulseSource.GenerateImpulse(shakeIntensity); // Generate the impulse with the calculated intensity
    }

    void CollisionFX(Collision other)
    {
        ContactPoint contactPoint = other.GetContact(0); // Get the first contact point of the collision;
        collisionParticleSystem.transform.position = contactPoint.point; //shifts the effect to the contact point of the collision
        collisionParticleSystem.Play(); //plays the particle system at the contact point
        collisionSound.Play(); //plays the collision sound effect
    }
}
