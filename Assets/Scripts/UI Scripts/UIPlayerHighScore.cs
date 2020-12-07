using TMPro;
using UnityEngine;

public class UIPlayerHighScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI topStageText;
    [SerializeField] private TextMeshProUGUI topScoreText;

    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        UpdateHighScoreHud();
    }

    private void UpdateHighScoreHud()
    {
        topStageText.text = $"Stage {gameManager.TopStage}";
        topScoreText.text = $"Score {gameManager.TopScore}";
    }
}
