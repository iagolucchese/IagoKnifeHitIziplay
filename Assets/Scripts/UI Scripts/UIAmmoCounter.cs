using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoCounter : MonoBehaviour
{
    [SerializeField] private List<Image> allKnifeIcons;

    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onMaxAmmoCountChanged += ResetMaxKnives;
        gameManager.onAmmoCountChanged += UpdateKnifeCounter;
    }

    private void ResetMaxKnives(int max)
    {
        for (var i = 0; i < allKnifeIcons.Count; i++)
        {
            allKnifeIcons[i].enabled = i < max;
            allKnifeIcons[i].color = Color.white;
        }
    }

    private void UpdateKnifeCounter(int currentAmmoCount)
    {
        for (var i = 0; i < gameManager.CurrentMaxKnives; i++)
        {
            allKnifeIcons[i].color = i < currentAmmoCount ? Color.white : Color.black;
        }
    }
}
