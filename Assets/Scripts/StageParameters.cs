using UnityEngine;

[CreateAssetMenu(menuName = "Game Design/New Stage", fileName = "NewStage", order = 0)]
public class StageParameters : ScriptableObject
{
    [Tooltip("Positive numbers will spin the target anti-clockwise, negative clockwise.")]
    public AnimationCurve targetSpinCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    
    [Tooltip("Multiplies the speed the curve describes.")]
    public float targetSpinSpeed = 1f;
    
    [Range(1,12)]
    [Tooltip("How many knives the player has for that stage.")]
    public int maxKnivesForStage = 6;

    public Sprite targetTexture;
}
