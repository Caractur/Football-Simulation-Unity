using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Team Data", menuName = "Football/Team Data")]
public class TeamData : ScriptableObject
{
    [Header("Team Information")]
    public string teamName;
    public Color kitPrimary = Color.white;
    public Color kitSecondary = Color.black;
    public int difficulty = 1;
    
    [Header("Player Data")]
    public List<PlayerData> playerDataList = new List<PlayerData>();
    
    [Header("Team Formation")]
    public FormationData formation;
    
    public Team CreateTeamInstance()
    {
        Team team = new Team(teamName)
        {
            kitPrimary = kitPrimary,
            kitSecondary = kitSecondary,
            difficulty = difficulty
        };
        
        return team;
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public PlayerStats stats;
    public Position position;
    public int squadNumber;
    public GameObject playerPrefab;
}

[System.Serializable]
public class FormationData
{
    public Vector2[] playerPositions = new Vector2[11]; // 2D positions for formation
    public string formationName = "4-4-2";
}
