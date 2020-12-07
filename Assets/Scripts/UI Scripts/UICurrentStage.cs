using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICurrentStage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentStageText;
    [SerializeField] private List<Image> stageProgressIcons;

    private GameManager gameManager;
    
    private void Awake()
    {
        currentStageText = currentStageText == null 
            ? GetComponentInChildren<TextMeshProUGUI>() 
            : currentStageText;
    }
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onStageChanged += UpdateCurrentStageHud;
    }

    private void UpdateCurrentStageHud(StageParameters stage)
    {
        var stageNum = gameManager.CurrentStageNumber;
        
        currentStageText.text = $"Stage {stageNum + 1}";

        stageNum = (int) Mathf.Repeat(stageNum, 5);
        for (var i = 0; i < stageProgressIcons.Count; i++)
        {
            stageProgressIcons[i].color = i == stageNum ? Color.yellow : Color.white;
        }
    }
}
