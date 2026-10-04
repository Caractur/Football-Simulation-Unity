using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerProfile))]
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Settings")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float rotationSpeed = 10f;
    
    [Header("Ball Interaction")]
    public float kickForce = 10f;
    public float passForce = 8f;
    public LayerMask ballLayer = 1;
    
    private PlayerProfile playerProfile;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isSprinting;
    private bool isKicking;
    private bool isPassing;
    
    private void Awake()
    {
        playerProfile = GetComponent<PlayerProfile>();
        rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }
    
    private void Update()
    {
        if (playerProfile.isProtagonist && playerProfile.hasPossession)
        {
            HandleMovement();
            HandleBallInteraction();
        }
    }
    
    private void HandleMovement()
    {
        if (moveInput.magnitude > 0.1f)
        {
            // Calculate movement direction
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            
            // Apply movement
            float currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
            Vector3 targetVelocity = moveDirection * currentSpeed;
            
            // Apply movement to rigidbody
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
            
            // Rotate towards movement direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Stop horizontal movement
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }
    
    private void HandleBallInteraction()
    {
        if (isKicking)
        {
            KickBall();
            isKicking = false;
        }
        
        if (isPassing)
        {
            PassBall();
            isPassing = false;
        }
    }
    
    private void KickBall()
    {
        // Find the ball in the scene
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                Vector3 kickDirection = transform.forward;
                ballRb.AddForce(kickDirection * kickForce, ForceMode.Impulse);
                
                // Remove possession after kicking
                playerProfile.RemovePossession();
            }
        }
    }
    
    private void PassBall()
    {
        // Find the ball in the scene
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                Vector3 passDirection = transform.forward;
                ballRb.AddForce(passDirection * passForce, ForceMode.Impulse);
                
                // Remove possession after passing
                playerProfile.RemovePossession();
            }
        }
    }
    
    // Input System callbacks
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }
    
    public void OnKick(InputValue value)
    {
        if (value.isPressed)
        {
            isKicking = true;
        }
    }
    
    public void OnPass(InputValue value)
    {
        if (value.isPressed)
        {
            isPassing = true;
        }
    }
    
    public void OnSwitchPlayer(InputValue value)
    {
        if (value.isPressed)
        {
            // This will be implemented later for player switching during stoppages
            Debug.Log("Player switch requested (not implemented yet)");
        }
    }
}
