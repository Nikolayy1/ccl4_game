using UnityEngine;

// The Pickup class is a base class for all pickup items in the game. Potion and Coin classes inherit from it.
// It handles the basic functionality of detecting when a player collides with a pickup item.
public abstract class Pickup : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;

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
            OnPickup();
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickup();
}
