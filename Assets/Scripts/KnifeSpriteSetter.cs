using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class KnifeSpriteSetter : MonoBehaviour
{
    public bool updateSpriteAfterStart;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        var knifeSpriteSelector = KnifeSpriteSelector.Instance;
        UpdateSprite(knifeSpriteSelector.SelectedKnifeSprite);
        
        if(updateSpriteAfterStart)
            knifeSpriteSelector.onKnifeSpriteChanged += UpdateSprite;
    }

    private void UpdateSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
}
