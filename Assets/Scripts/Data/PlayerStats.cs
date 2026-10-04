using System;
using UnityEngine;

[Serializable]
public struct PlayerStats
{
    [Range(1, 100)]
    public int Shooting;   // 1–100
    
    [Range(1, 100)]
    public int Defending;  // 1–100
    
    [Range(1, 100)]
    public int Pace;       // 1–100
    
    [Range(1, 100)]
    public int Passing;    // 1–100
    
    public PlayerStats(int shooting = 50, int defending = 50, int pace = 50, int passing = 50)
    {
        Shooting = Mathf.Clamp(shooting, 1, 100);
        Defending = Mathf.Clamp(defending, 1, 100);
        Pace = Mathf.Clamp(pace, 1, 100);
        Passing = Mathf.Clamp(passing, 1, 100);
    }
    
    public float GetAverageRating()
    {
        return (Shooting + Defending + Pace + Passing) / 4f;
    }
}
