using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    [Header("Player Information")]
    public string playerName;
    public PlayerStats stats;
    public Position position;
    
    [Header("Components")]
    public Animator animator;
    
    [Header("Team Information")]
    public Team team;
    public int squadNumber;
    
    [Header("Gameplay")]
    public bool isProtagonist = false;
    public bool hasPossession = false;
    
    private void Awake()
    {
        // Ensure we have an animator component
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    
    public void SetAsProtagonist()
    {
        isProtagonist = true;
        // Additional protagonist-specific setup can be added here
    }
    
    public void RemoveProtagonistStatus()
    {
        isProtagonist = false;
    }
    
    public void GivePossession()
    {
        hasPossession = true;
        // Trigger possession-related events or animations
    }
    
    public void RemovePossession()
    {
        hasPossession = false;
    }
    
    public float GetOverallRating()
    {
        return stats.GetAverageRating();
    }
}
