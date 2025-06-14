using UnityEngine;
using Cinemachine; // optional depending on version

public class Rock : MonoBehaviour
{
    [SerializeField] float shakeModifier = 10f;

    CinemachineImpulseSource cinemachineImpulseSource;

    void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        // Distance between the rock and the camera, to calculate the impulse strength
        float distance = Vector3.Distance(other.transform.position, Camera.main.transform.position);
        float shakeIntensity = (1f / distance) * shakeModifier; // Adjust the intensity based on distance
        shakeIntensity = Mathf.Min(shakeIntensity, 1f); // Cap the intensity to a maximum value
        cinemachineImpulseSource.GenerateImpulse(shakeIntensity); // Generate the impulse with the calculated intensity
    }
}
