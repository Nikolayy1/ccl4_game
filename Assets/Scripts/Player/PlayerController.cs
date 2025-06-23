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
    private bool isJumping = false;
    bool groundCheckEnabled = true;

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
        if (!groundCheckEnabled)
            return;

        isGrounded = false;

        Vector3 rayOrigin = transform.position + Vector3.down * 0.1f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
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
        if (context.started && isGrounded && !isJumping && !isTrapped)
        {
            StartCoroutine(JumpFlow());
        }
    }

    IEnumerator JumpFlow()
    {
        isJumping = true;
        groundCheckEnabled = false;

        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        if (animator != null)
            animator.SetBool("Jump", true); // ← bool again

        // Delay rechecking ground to avoid false positive
        yield return new WaitForSeconds(0.2f);
        groundCheckEnabled = true;

        // Wait until actually landed
        yield return new WaitUntil(() => isGrounded);

        if (animator != null)
            animator.SetBool("Jump", false); // ← returns to RunMC

        isJumping = false;
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

    private IEnumerator TrapLockCoroutine(float duration) //when the player is trapped, bear trap or robber
    {
        Debug.Log(">> Player movement LOCKED");
        isTrapped = true;
        movement = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Trapped"); // trigger IdleMC

        yield return new WaitForSeconds(duration);

        isTrapped = false;

        if (animator != null)
            animator.SetTrigger("Run"); // trigger RunMC

        Debug.Log(">> Player movement UNLOCKED");
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
