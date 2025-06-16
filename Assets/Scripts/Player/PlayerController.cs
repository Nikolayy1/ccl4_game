using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float xClamp = 3f; // limit movement on the X-axis (side-to-side)
    [SerializeField] float zClamp = 2f; // limit movement on the z-axis (front-to-back)

    Vector2 movement;
    Rigidbody rigidBody;

    private bool isTrapped = false;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isTrapped)
        {
            movement = Vector2.zero; // Ignore movement when trapped
            return;
        }

        movement = context.ReadValue<Vector2>();
        Debug.Log($"Movement input received: {movement}");
    }

    void HandleMovement()
    {
        if (isTrapped) return;

        Vector3 currentPosition = rigidBody.position;
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y); // current postion
        Vector3 newPosition = currentPosition + moveDirection * (moveSpeed * Time.fixedDeltaTime);

        // Clamp (restrict) the player movement within the specified bounds
        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
        newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp);

        rigidBody.MovePosition(newPosition);
    }

    public void LockMovement(float duration)
    {
        StartCoroutine(TrapLockCoroutine(duration));
    }

    private IEnumerator TrapLockCoroutine(float duration)
    {
        Debug.Log(">> Player movement LOCKED");

        isTrapped = true;
        movement = Vector2.zero; // Immediately stop moving

        yield return new WaitForSeconds(duration);

        isTrapped = false;
        Debug.Log(">> Player movement UNLOCKED");
    }
}
