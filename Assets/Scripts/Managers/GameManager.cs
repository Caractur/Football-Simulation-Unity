using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    public LeagueData leagueData;
    public TeamData playerTeamData;
    
    [Header("Current Game State")]
    public League currentLeague;
    public Team playerTeam;
    public Team currentOpponent;
    public PlayerProfile currentProtagonist;
    
    [Header("Match State")]
    public bool isMatchActive = false;
    public int playerScore = 0;
    public int opponentScore = 0;
    public float matchTime = 0f;
    public float matchDuration = 90f; // 90 seconds for now, can be adjusted
    
    [Header("Events")]
    public UnityEvent<PlayerProfile> OnProtagonistChanged;
    public UnityEvent<Team> OnMatchStarted;
    public UnityEvent<Team> OnMatchEnded;
    public UnityEvent<int, int> OnScoreChanged;
    public UnityEvent OnLeagueComplete;
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeGame()
    {
        if (leagueData != null)
        {
            currentLeague = leagueData.CreateLeagueInstance();
            currentLeague.playerTeam = playerTeam;
            currentOpponent = currentLeague.GetCurrentOpponent();
        }
    }
    
    private void Update()
    {
        if (isMatchActive)
        {
            matchTime += Time.deltaTime;
            if (matchTime >= matchDuration)
            {
                EndMatch();
            }
        }
    }
    
    public void StartMatch()
    {
        if (currentOpponent == null)
        {
            Debug.LogError("No opponent available for match!");
            return;
        }
        
        isMatchActive = true;
        matchTime = 0f;
        playerScore = 0;
        opponentScore = 0;
        
        OnMatchStarted?.Invoke(currentOpponent);
        Debug.Log($"Match started: {playerTeam?.teamName} vs {currentOpponent.teamName}");
    }
    
    public void EndMatch()
    {
        isMatchActive = false;
        
        // Determine match result
        if (playerScore > opponentScore)
        {
            Debug.Log("Player wins!");
            currentLeague.AdvanceWeek();
        }
        else if (playerScore < opponentScore)
        {
            Debug.Log("Player loses!");
        }
        else
        {
            Debug.Log("Draw!");
        }
        
        OnMatchEnded?.Invoke(currentOpponent);
        
        // Check if league is complete
        if (currentLeague.IsLeagueComplete())
        {
            OnLeagueComplete?.Invoke();
            Debug.Log("League complete!");
        }
        else
        {
            currentOpponent = currentLeague.GetCurrentOpponent();
        }
    }
    
    public void SetProtagonist(PlayerProfile player)
    {
        if (currentProtagonist != null)
        {
            currentProtagonist.RemoveProtagonistStatus();
        }
        
        currentProtagonist = player;
        if (currentProtagonist != null)
        {
            currentProtagonist.SetAsProtagonist();
        }
        
        OnProtagonistChanged?.Invoke(currentProtagonist);
    }
    
    public void AddPlayerScore()
    {
        playerScore++;
        OnScoreChanged?.Invoke(playerScore, opponentScore);
    }
    
    public void AddOpponentScore()
    {
        opponentScore++;
        OnScoreChanged?.Invoke(playerScore, opponentScore);
    }
    
    public void ResetGame()
    {
        if (leagueData != null)
        {
            currentLeague = leagueData.CreateLeagueInstance();
            currentLeague.playerTeam = playerTeam;
            currentOpponent = currentLeague.GetCurrentOpponent();
        }
        
        isMatchActive = false;
        matchTime = 0f;
        playerScore = 0;
        opponentScore = 0;
        currentProtagonist = null;
    }
}
