using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New League Data", menuName = "Football/League Data")]
public class LeagueData : ScriptableObject
{
    [Header("League Information")]
    public string leagueName = "Football League";
    public int totalWeeks = 5;
    
    [Header("Teams")]
    public List<TeamData> teamDataList = new List<TeamData>();
    
    [Header("League Settings")]
    public int pointsForWin = 3;
    public int pointsForDraw = 1;
    public int pointsForLoss = 0;
    
    public League CreateLeagueInstance()
    {
        League league = new League
        {
            leagueName = leagueName,
            totalWeeks = totalWeeks
        };
        
        // Create team instances from TeamData
        foreach (var teamData in teamDataList)
        {
            if (teamData != null)
            {
                Team team = teamData.CreateTeamInstance();
                league.AddTeam(team);
            }
        }
        
        return league;
    }
    
    public void ValidateTeams()
    {
        // Ensure we have exactly 5 teams for the league
        if (teamDataList.Count != 5)
        {
            Debug.LogWarning($"League should have exactly 5 teams. Current count: {teamDataList.Count}");
        }
        
        // Validate team difficulties
        for (int i = 0; i < teamDataList.Count; i++)
        {
            if (teamDataList[i] != null && teamDataList[i].difficulty != i + 1)
            {
                Debug.LogWarning($"Team {teamDataList[i].teamName} should have difficulty {i + 1}, but has {teamDataList[i].difficulty}");
            }
        }
    }
}
