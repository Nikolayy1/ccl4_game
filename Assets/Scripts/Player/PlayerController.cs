using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float xClamp = 3f;
    [SerializeField] float zClamp = 2f;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 6f;
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Animator animator;

    Vector2 movement;
    Rigidbody rigidBody;
    LevelGenerator levelGenerator;

    private bool isTrapped = false;
    private bool isGrounded = false;

    const string jumpBool = "Jump";

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    void FixedUpdate()
    {
        CheckGrounded();
        HandleMovement();
    }

    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded && animator != null)
        {
            animator.SetBool(jumpBool, false);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isTrapped)
        {
            movement = Vector2.zero;
            return;
        }

        movement = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded && !isTrapped)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (animator != null)
                animator.SetBool(jumpBool, true);
        }
    }

    void HandleMovement()
    {
        if (isTrapped) return;

        Vector3 currentPosition = rigidBody.position;
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y);

        float speedMultiplier = 1f;

        if (levelGenerator != null)
        {
            float baseSpeed = 8f;
            speedMultiplier = levelGenerator.GetMoveSpeed() / baseSpeed;
        }

        Vector3 newPosition = currentPosition + moveDirection * (moveSpeed * speedMultiplier * Time.fixedDeltaTime);
        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
        newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp);

        rigidBody.MovePosition(newPosition);
    }

    public void LockMovement(float duration)
    {
        Debug.Log(">> LockMovement called by: " + duration + "s");
        StartCoroutine(TrapLockCoroutine(duration));
    }

    private IEnumerator TrapLockCoroutine(float duration)
    {
        Debug.Log(">> Player movement LOCKED");
        isTrapped = true;
        movement = Vector2.zero;
        yield return new WaitForSeconds(duration);
        isTrapped = false;
        Debug.Log(">> Player movement UNLOCKED");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
