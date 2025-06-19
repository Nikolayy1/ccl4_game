using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    protected GameObject triggeringObject;

    const string playerString = "Player";

    void Update()
    {
        // Rotate the pickup item around its Y-axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerString))
        {
            triggeringObject = other.gameObject; // 👈 this is the key line
            OnPickup();
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickup();
}
