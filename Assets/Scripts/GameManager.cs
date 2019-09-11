using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject inGameScorePanel;
    [SerializeField] GameObject messagePanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject howToPlayPanel;
    public int score;
    [SerializeField] TextMeshProUGUI finalScore;
    [SerializeField] TextMeshProUGUI currentScore;

    public delegate void OnGameStateChanged();
    public event OnGameStateChanged OnGameStart;
    public event OnGameStateChanged OnGameEnd;
    public event OnGameStateChanged OnEvolving;
    public event OnGameStateChanged OnSpawnerStart;

    public delegate void OnEnemyDeathHandler();
    public event OnEnemyDeathHandler OnEnemyDeath;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)        
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        Time.timeScale = 1.5f;
        
        currentScore.text = "Pontos: 0" ;
        inGameScorePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        creditsPanel.SetActive(false);
        menu.SetActive(true);
        messagePanel.SetActive(false);
        scorePanel.SetActive(false);
    }

    public void IncreaseScore(int level)
    {
        score += level * 100;
        currentScore.text = "Pontos: " + score.ToString();
    }

    public void EnemyDied()
    {
        OnEnemyDeath?.Invoke();
    }

    public void GameOver()
    {
        OnGameEnd?.Invoke();
        menu.SetActive(false);
        inGameScorePanel.SetActive(false);
        messagePanel.SetActive(false);
        scorePanel.SetActive(true);
        finalScore.text = score.ToString();
    }

    public void Menu()
    {
        score = 0;
        scorePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        creditsPanel.SetActive(false);
        menu.SetActive(true);
    }

    public void Play()
    {
        OnGameStart?.Invoke();
        menu.SetActive(false);
        inGameScorePanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void Evolving()
    {
        OnEvolving?.Invoke();
    }

    public void SpawnerStarted()
    {
        OnSpawnerStart?.Invoke();
    }

    public void Credits()
    {
        menu.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void HowToPlay()
    {
        menu.SetActive(false);
        howToPlayPanel.SetActive(true);
    }
}