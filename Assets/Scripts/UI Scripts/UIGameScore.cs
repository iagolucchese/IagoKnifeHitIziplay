using TMPro;
using UnityEngine;

public class UIGameScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentStageText;

    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        UpdateScoreHud();
    }

    private void UpdateScoreHud()
    {
        currentStageText.text = $"{gameManager.CurrentScore}";
    }
}
