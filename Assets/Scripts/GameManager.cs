using System.Collections;
using System.Collections.Generic;
using IagoUnityScriptsRepo;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : ASingleton<GameManager>
{
    private const string TopScoreKey = "TopScore";
    private const string TopStageKey = "TopStage";
    
    //Fields
    public bool GameIsActive { get; set; } = false;
    
    [Header("Stages")] 
    public List<StageParameters> allStages;
    public StageParameters currentStage;
    public float delayBetweenStages = 1f;
    
    [Header("Knives")]
    [SerializeField] private int currentAmmoCount;
    [SerializeField] private int currentMaxKnives;

    [Header("Score")] 
    [SerializeField] private int currentStageNumber;
    [SerializeField] private int currentScore;
    [SerializeField] private int topScore;
    [SerializeField] private int topStage;
    
    //Events
    public UnityAction<int> onAmmoCountChanged;
    public UnityAction<int> onMaxAmmoCountChanged;
    public UnityAction<StageParameters> onStageChanged;    
    /// <summary>
    /// bool: True if the stage ended with success for the player, false otherwise.
    /// </summary>
    public UnityAction<bool> onStageEnded;
    public UnityEvent onGameOver;
    
    //Properties
    public int TopScore
    {
        get => topScore;
        set
        {
            topScore = value;
            PlayerPrefs.SetInt(TopScoreKey, value);
            PlayerPrefs.Save();
        }
    }
    public int TopStage
    {
        get => topStage;
        set
        {
            topStage = value;
            PlayerPrefs.SetInt(TopStageKey, value);
            PlayerPrefs.Save();
        }
    }
    
    public int CurrentScore
    {
        get => currentScore;
        set
        {
            currentScore = value;
            TopScore = Mathf.Max(TopScore, value);
        }
    }
    public int CurrentStageNumber
    {
        get => currentStageNumber;
        set
        {
            currentStageNumber = value;
            TopStage = Mathf.Max(TopStage, value+1);
            
            //this repeat is to make the game loop through the stages if it runs out of them, so it only ends when the player fails
            currentStage = allStages[(int) Mathf.Repeat(currentStageNumber, allStages.Count)];
            
            onStageChanged?.Invoke(currentStage);
        }
    }
    public int CurrentAmmoCount
    {
        get => currentAmmoCount;
        set
        {
            currentAmmoCount = value;
            onAmmoCountChanged?.Invoke(currentAmmoCount);
        }
    }
    public int CurrentMaxKnives
    {
        get => currentMaxKnives;
        set
        {
            currentMaxKnives = value;
            onMaxAmmoCountChanged?.Invoke(currentMaxKnives);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        onStageChanged += StateChangeReset;
        GetTopScoresFromPlayerPrefs();
    }

    private void Start()
    {
        KnifeSpawnerBehavior.Instance.onKnifeTossHit += HandleKnifeTossHit;
        KnifeSpawnerBehavior.Instance.onKnifeTossed += HandleKnifeTossed;
    }

    public void StartGame()
    {
        CurrentStageNumber = 0;
        GameIsActive = true;
    }

    public IEnumerator EndStage(bool withSuccess)
    {
        GameIsActive = false;
        onStageEnded?.Invoke(withSuccess);
        
        yield return new WaitForSeconds(delayBetweenStages);

        if (withSuccess)
            CurrentStageNumber++;
        else
            onGameOver?.Invoke();
    }

    private void StateChangeReset(StageParameters stage)
    {
        CurrentAmmoCount = CurrentMaxKnives = stage.maxKnivesForStage;
        GameIsActive = true;
        if (CurrentStageNumber == 0)
            CurrentScore = 0;
    }
    
    private void GetTopScoresFromPlayerPrefs()
    {
        topScore = PlayerPrefs.GetInt(TopScoreKey);
        topStage = PlayerPrefs.GetInt(TopStageKey);
    }

    private void HandleKnifeTossHit(bool hit)
    {
        if (hit == false)
        {
            StartCoroutine(EndStage(false));
        }
        else
        {
            CurrentScore++;
            if (CurrentAmmoCount <= 0)
                StartCoroutine(EndStage(true));
        }
    }

    private void HandleKnifeTossed()
    {
        CurrentAmmoCount--;
    }
}
