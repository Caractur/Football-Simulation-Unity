using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class League
{
    [Header("League Information")]
    public string leagueName = "Football League";
    public List<Team> teams = new List<Team>();
    
    [Header("League Progress")]
    public int currentWeek = 1;
    public int totalWeeks = 5;
    public Team currentOpponent;
    public Team playerTeam;
    
    public League()
    {
        teams = new List<Team>();
    }
    
    public void AddTeam(Team team)
    {
        if (team != null && !teams.Contains(team))
        {
            teams.Add(team);
            // Sort teams by difficulty for proper progression
            teams.Sort((a, b) => a.difficulty.CompareTo(b.difficulty));
        }
    }
    
    public void RemoveTeam(Team team)
    {
        if (teams.Contains(team))
        {
            teams.Remove(team);
        }
    }
    
    public Team GetTeamByDifficulty(int difficulty)
    {
        return teams.Find(t => t.difficulty == difficulty);
    }
    
    public Team GetCurrentOpponent()
    {
        if (currentWeek <= teams.Count)
        {
            return teams[currentWeek - 1];
        }
        return null;
    }
    
    public void AdvanceWeek()
    {
        if (currentWeek < totalWeeks)
        {
            currentWeek++;
            currentOpponent = GetCurrentOpponent();
        }
    }
    
    public bool IsLeagueComplete()
    {
        return currentWeek > totalWeeks;
    }
    
    public float GetLeagueProgress()
    {
        return (float)currentWeek / totalWeeks;
    }
    
    public void ResetLeague()
    {
        currentWeek = 1;
        currentOpponent = GetCurrentOpponent();
    }
}
