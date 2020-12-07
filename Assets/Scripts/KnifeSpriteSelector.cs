using System.Collections.Generic;
using IagoUnityScriptsRepo;
using UnityEngine;
using UnityEngine.Events;

public class KnifeSpriteSelector : ASingleton<KnifeSpriteSelector>
{
    private const string SelectedKnifeKey = "SelectedKnife";
    
    //Fields
    [SerializeField] private int selectedKnifeSpriteIndex;
    [SerializeField] private List<Sprite> allKnifeSprites;
    
    //Events
    public UnityAction<Sprite> onKnifeSpriteChanged;
    
    //Properties
    public int SelectedKnifeSpriteIndex
    {
        get => selectedKnifeSpriteIndex;
        set
        {
            selectedKnifeSpriteIndex = (int)Mathf.Repeat(value, allKnifeSprites.Count);
            
            PlayerPrefs.SetInt(SelectedKnifeKey, selectedKnifeSpriteIndex);
            PlayerPrefs.Save();
            
            onKnifeSpriteChanged?.Invoke(SelectedKnifeSprite);
        }
    }
    public Sprite SelectedKnifeSprite => allKnifeSprites[SelectedKnifeSpriteIndex];

    public void CycleNextKnife() => SelectedKnifeSpriteIndex++;
    
    protected override void Awake()
    {
        base.Awake();
        SelectedKnifeSpriteIndex = PlayerPrefs.GetInt(SelectedKnifeKey);
    }
}
