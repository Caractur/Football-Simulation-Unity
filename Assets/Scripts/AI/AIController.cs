using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerProfile))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    [Header("AI Settings")]
    public float decisionRate = 0.5f;
    public float visionRange = 10f;
    public float passRange = 15f;
    public float shootRange = 8f;
    
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float sprintSpeed = 5f;
    
    private PlayerProfile playerProfile;
    private NavMeshAgent agent;
    private BallController ball;
    private GameManager gameManager;
    
    private float lastDecisionTime;
    private Vector3 targetPosition;
    private bool isMovingToBall;
    private bool isMovingToPosition;
    
    private void Awake()
    {
        playerProfile = GetComponent<PlayerProfile>();
        agent = GetComponent<NavMeshAgent>();
        ball = FindObjectOfType<BallController>();
        gameManager = GameManager.Instance;
        
        // Configure NavMeshAgent
        agent.speed = moveSpeed;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
    }
    
    private void Start()
    {
        // Set initial position based on formation
        SetFormationPosition();
    }
    
    private void Update()
    {
        if (playerProfile.isProtagonist || !gameManager.isMatchActive)
            return;
            
        if (Time.time - lastDecisionTime > decisionRate)
        {
            MakeDecision();
            lastDecisionTime = Time.time;
        }
        
        HandleMovement();
    }
    
    private void MakeDecision()
    {
        if (ball == null) return;
        
        float distanceToBall = Vector3.Distance(transform.position, ball.transform.position);
        
        // If we have possession
        if (playerProfile.hasPossession)
        {
            HandlePossession();
        }
        // If ball is within vision range
        else if (distanceToBall < visionRange)
        {
            // Move towards ball if we're the closest player or if no one has possession
            if (!ball.IsInPossession() || IsClosestToBall())
            {
                MoveToBall();
            }
            else
            {
                // Support play - move to open space
                MoveToSupportPosition();
            }
        }
        else
        {
            // Return to formation position
            ReturnToFormation();
        }
    }
    
    private void HandlePossession()
    {
        // Check if we should shoot
        if (IsInShootingPosition())
        {
            Shoot();
            return;
        }
        
        // Check if we should pass
        PlayerProfile passTarget = FindPassTarget();
        if (passTarget != null)
        {
            PassToPlayer(passTarget);
            return;
        }
        
        // Dribble towards goal
        DribbleTowardsGoal();
    }
    
    private bool IsInShootingPosition()
    {
        // Simple check - if we're close to the opponent's goal
        Vector3 goalPosition = GetOpponentGoalPosition();
        float distanceToGoal = Vector3.Distance(transform.position, goalPosition);
        
        return distanceToGoal < shootRange;
    }
    
    private void Shoot()
    {
        Vector3 goalDirection = (GetOpponentGoalPosition() - transform.position).normalized;
        ball.Kick(goalDirection, 15f);
        Debug.Log($"{playerProfile.playerName} shoots!");
    }
    
    private PlayerProfile FindPassTarget()
    {
        PlayerProfile[] teammates = GetTeammates();
        PlayerProfile bestTarget = null;
        float bestScore = 0f;
        
        foreach (var teammate in teammates)
        {
            if (teammate == playerProfile) continue;
            
            float distance = Vector3.Distance(transform.position, teammate.transform.position);
            if (distance < passRange)
            {
                // Simple scoring based on distance and position
                float score = 1f / distance;
                if (teammate.position == Position.ST || teammate.position == Position.CF)
                {
                    score *= 1.5f; // Prefer forwards
                }
                
                if (score > bestScore)
                {
                    bestScore = score;
                    bestTarget = teammate;
                }
            }
        }
        
        return bestTarget;
    }
    
    private void PassToPlayer(PlayerProfile target)
    {
        Vector3 passDirection = (target.transform.position - transform.position).normalized;
        ball.Pass(passDirection, 10f);
        Debug.Log($"{playerProfile.playerName} passes to {target.playerName}!");
    }
    
    private void DribbleTowardsGoal()
    {
        Vector3 goalPosition = GetOpponentGoalPosition();
        Vector3 direction = (goalPosition - transform.position).normalized;
        ball.Kick(direction, 5f);
    }
    
    private bool IsClosestToBall()
    {
        if (ball == null) return false;
        
        PlayerProfile[] allPlayers = FindObjectsOfType<PlayerProfile>();
        float myDistance = Vector3.Distance(transform.position, ball.transform.position);
        
        foreach (var player in allPlayers)
        {
            if (player == playerProfile) continue;
            
            float distance = Vector3.Distance(player.transform.position, ball.transform.position);
            if (distance < myDistance)
            {
                return false;
            }
        }
        
        return true;
    }
    
    private void MoveToBall()
    {
        if (ball != null)
        {
            targetPosition = ball.transform.position;
            isMovingToBall = true;
            isMovingToPosition = false;
        }
    }
    
    private void MoveToSupportPosition()
    {
        // Simple support positioning - move to open space
        Vector3 ballPosition = ball.transform.position;
        Vector3 supportPosition = ballPosition + Random.insideUnitSphere * 5f;
        supportPosition.y = transform.position.y;
        
        targetPosition = supportPosition;
        isMovingToBall = false;
        isMovingToPosition = true;
    }
    
    private void ReturnToFormation()
    {
        SetFormationPosition();
        isMovingToBall = false;
        isMovingToPosition = true;
    }
    
    private void HandleMovement()
    {
        if (isMovingToBall || isMovingToPosition)
        {
            agent.SetDestination(targetPosition);
            
            // Update speed based on urgency
            if (isMovingToBall)
            {
                agent.speed = sprintSpeed;
            }
            else
            {
                agent.speed = moveSpeed;
            }
        }
    }
    
    private void SetFormationPosition()
    {
        // This is a simplified formation system
        // In a full implementation, you'd load formation data from TeamData
        Vector3 formationPos = GetFormationPosition(playerProfile.position);
        targetPosition = formationPos;
        isMovingToPosition = true;
    }
    
    private Vector3 GetFormationPosition(Position position)
    {
        // Simple 4-4-2 formation positions
        Vector3 basePosition = Vector3.zero;
        
        switch (position)
        {
            case Position.GK:
                basePosition = new Vector3(0, 0, -40);
                break;
            case Position.CB:
                basePosition = new Vector3(0, 0, -35);
                break;
            case Position.LB:
                basePosition = new Vector3(-10, 0, -35);
                break;
            case Position.RB:
                basePosition = new Vector3(10, 0, -35);
                break;
            case Position.CM:
                basePosition = new Vector3(0, 0, -25);
                break;
            case Position.LM:
                basePosition = new Vector3(-10, 0, -25);
                break;
            case Position.RM:
                basePosition = new Vector3(10, 0, -25);
                break;
            case Position.ST:
                basePosition = new Vector3(0, 0, -15);
                break;
            default:
                basePosition = new Vector3(0, 0, -25);
                break;
        }
        
        return basePosition;
    }
    
    private Vector3 GetOpponentGoalPosition()
    {
        // Simple - assume opponent goal is at the opposite end
        return new Vector3(0, 0, 40);
    }
    
    private PlayerProfile[] GetTeammates()
    {
        if (playerProfile.team == null) return new PlayerProfile[0];
        
        return playerProfile.team.roster.ToArray();
    }
}
