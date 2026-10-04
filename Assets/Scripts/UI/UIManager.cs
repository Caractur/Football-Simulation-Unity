using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Match UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI matchInfoText;
    
    [Header("Player UI")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerStatsText;
    public Image possessionIndicator;
    
    [Header("League UI")]
    public TextMeshProUGUI leagueProgressText;
    public TextMeshProUGUI opponentNameText;
    public TextMeshProUGUI weekText;
    
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button continueButton;
    public Button restartButton;
    
    private GameManager gameManager;
    private PlayerProfile currentProtagonist;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        
        // Subscribe to events
        if (gameManager != null)
        {
            gameManager.OnProtagonistChanged.AddListener(OnProtagonistChanged);
            gameManager.OnMatchStarted.AddListener(OnMatchStarted);
            gameManager.OnMatchEnded.AddListener(OnMatchEnded);
            gameManager.OnScoreChanged.AddListener(OnScoreChanged);
            gameManager.OnLeagueComplete.AddListener(OnLeagueComplete);
        }
        
        // Set up buttons
        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueToNextMatch);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        UpdateUI();
    }
    
    private void Update()
    {
        UpdateMatchTime();
        UpdatePossessionIndicator();
    }
    
    private void UpdateMatchTime()
    {
        if (timeText != null && gameManager != null)
        {
            float remainingTime = gameManager.matchDuration - gameManager.matchTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    
    private void UpdatePossessionIndicator()
    {
        if (possessionIndicator != null && currentProtagonist != null)
        {
            possessionIndicator.gameObject.SetActive(currentProtagonist.hasPossession);
        }
    }
    
    private void OnProtagonistChanged(PlayerProfile protagonist)
    {
        currentProtagonist = protagonist;
        UpdatePlayerUI();
    }
    
    private void OnMatchStarted(Team opponent)
    {
        UpdateMatchUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
    
    private void OnMatchEnded(Team opponent)
    {
        ShowMatchResult();
    }
    
    private void OnScoreChanged(int playerScore, int opponentScore)
    {
        UpdateScoreUI();
    }
    
    private void OnLeagueComplete()
    {
        ShowLeagueComplete();
    }
    
    private void UpdateUI()
    {
        UpdateScoreUI();
        UpdateMatchUI();
        UpdatePlayerUI();
        UpdateLeagueUI();
    }
    
    private void UpdateScoreUI()
    {
        if (scoreText != null && gameManager != null)
        {
            scoreText.text = $"{gameManager.playerScore} - {gameManager.opponentScore}";
        }
    }
    
    private void UpdateMatchUI()
    {
        if (matchInfoText != null && gameManager != null && gameManager.currentOpponent != null)
        {
            matchInfoText.text = $"vs {gameManager.currentOpponent.teamName}";
        }
    }
    
    private void UpdatePlayerUI()
    {
        if (currentProtagonist != null)
        {
            if (playerNameText != null)
            {
                playerNameText.text = currentProtagonist.playerName;
            }
            
            if (playerStatsText != null)
            {
                playerStatsText.text = $"Shooting: {currentProtagonist.stats.Shooting}\n" +
                                      $"Defending: {currentProtagonist.stats.Defending}\n" +
                                      $"Pace: {currentProtagonist.stats.Pace}\n" +
                                      $"Passing: {currentProtagonist.stats.Passing}";
            }
        }
    }
    
    private void UpdateLeagueUI()
    {
        if (gameManager != null && gameManager.currentLeague != null)
        {
            if (leagueProgressText != null)
            {
                float progress = gameManager.currentLeague.GetLeagueProgress();
                leagueProgressText.text = $"League Progress: {progress:P0}";
            }
            
            if (weekText != null)
            {
                weekText.text = $"Week {gameManager.currentLeague.currentWeek} of {gameManager.currentLeague.totalWeeks}";
            }
            
            if (opponentNameText != null && gameManager.currentOpponent != null)
            {
                opponentNameText.text = $"Next: {gameManager.currentOpponent.teamName}";
            }
        }
    }
    
    private void ShowMatchResult()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (gameOverText != null)
            {
                string result = "";
                if (gameManager.playerScore > gameManager.opponentScore)
                {
                    result = "Victory!";
                }
                else if (gameManager.playerScore < gameManager.opponentScore)
                {
                    result = "Defeat!";
                }
                else
                {
                    result = "Draw!";
                }
                
                gameOverText.text = $"{result}\n{gameManager.playerScore} - {gameManager.opponentScore}";
            }
        }
    }
    
    private void ShowLeagueComplete()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (gameOverText != null)
            {
                gameOverText.text = "League Complete!\nCongratulations!";
            }
        }
    }
    
    private void ContinueToNextMatch()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        // Start next match if league isn't complete
        if (gameManager != null && !gameManager.currentLeague.IsLeagueComplete())
        {
            gameManager.StartMatch();
        }
    }
    
    private void RestartGame()
    {
        if (gameManager != null)
        {
            gameManager.ResetGame();
            gameManager.StartMatch();
        }
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
}
