using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Ball Settings")]
    public float maxVelocity = 20f;
    public float dragForce = 0.5f;
    public float bounceForce = 0.7f;
    public LayerMask groundLayer = 1;
    
    [Header("Possession")]
    public PlayerProfile currentPossessor;
    public float possessionRadius = 2f;
    public float possessionTransferDelay = 0.5f;
    
    private Rigidbody rb;
    private bool isGrounded;
    private float lastPossessionChange;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = dragForce;
        rb.angularDrag = dragForce;
    }
    
    private void Update()
    {
        CheckGroundContact();
        LimitVelocity();
        CheckPossessionTransfer();
    }
    
    private void CheckGroundContact()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);
    }
    
    private void LimitVelocity()
    {
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
    
    private void CheckPossessionTransfer()
    {
        if (Time.time - lastPossessionChange < possessionTransferDelay)
            return;
            
        // Find the closest player within possession radius
        PlayerProfile closestPlayer = FindClosestPlayer();
        
        if (closestPlayer != null && closestPlayer != currentPossessor)
        {
            TransferPossession(closestPlayer);
        }
    }
    
    private PlayerProfile FindClosestPlayer()
    {
        PlayerProfile[] allPlayers = FindObjectsOfType<PlayerProfile>();
        PlayerProfile closest = null;
        float closestDistance = possessionRadius;
        
        foreach (var player in allPlayers)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = player;
            }
        }
        
        return closest;
    }
    
    private void TransferPossession(PlayerProfile newPossessor)
    {
        // Remove possession from current possessor
        if (currentPossessor != null)
        {
            currentPossessor.RemovePossession();
        }
        
        // Give possession to new possessor
        currentPossessor = newPossessor;
        currentPossessor.GivePossession();
        
        lastPossessionChange = Time.time;
        
        Debug.Log($"Ball possession transferred to {newPossessor.playerName}");
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Handle ball bouncing
        if (collision.gameObject.layer == groundLayer.value)
        {
            Vector3 bounceVelocity = rb.velocity;
            bounceVelocity.y = -bounceVelocity.y * bounceForce;
            rb.velocity = bounceVelocity;
        }
    }
    
    public void Kick(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
        
        // Remove possession when ball is kicked
        if (currentPossessor != null)
        {
            currentPossessor.RemovePossession();
            currentPossessor = null;
        }
    }
    
    public void Pass(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
        
        // Remove possession when ball is passed
        if (currentPossessor != null)
        {
            currentPossessor.RemovePossession();
            currentPossessor = null;
        }
    }
    
    public bool IsInPossession()
    {
        return currentPossessor != null;
    }
    
    public PlayerProfile GetCurrentPossessor()
    {
        return currentPossessor;
    }
}
