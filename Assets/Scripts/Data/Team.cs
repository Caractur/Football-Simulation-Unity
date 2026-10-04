using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    [Header("Team Information")]
    public string teamName;
    public List<PlayerProfile> roster = new List<PlayerProfile>();
    
    [Header("Kit Colors")]
    public Color kitPrimary = Color.white;
    public Color kitSecondary = Color.black;
    
    [Header("Team Stats")]
    public int difficulty = 1; // 1-5 for league progression
    public float overallRating;
    
    public Team(string name = "New Team")
    {
        teamName = name;
        roster = new List<PlayerProfile>();
    }
    
    public void AddPlayer(PlayerProfile player)
    {
        if (player != null && !roster.Contains(player))
        {
            roster.Add(player);
            player.team = this;
            CalculateOverallRating();
        }
    }
    
    public void RemovePlayer(PlayerProfile player)
    {
        if (roster.Contains(player))
        {
            roster.Remove(player);
            player.team = null;
            CalculateOverallRating();
        }
    }
    
    public void CalculateOverallRating()
    {
        if (roster.Count == 0)
        {
            overallRating = 0f;
            return;
        }
        
        float totalRating = 0f;
        foreach (var player in roster)
        {
            totalRating += player.GetOverallRating();
        }
        
        overallRating = totalRating / roster.Count;
    }
    
    public List<PlayerProfile> GetPlayersByPosition(Position position)
    {
        List<PlayerProfile> players = new List<PlayerProfile>();
        foreach (var player in roster)
        {
            if (player.position == position)
            {
                players.Add(player);
            }
        }
        return players;
    }
    
    public PlayerProfile GetPlayerByName(string name)
    {
        return roster.Find(p => p.playerName == name);
    }
}
