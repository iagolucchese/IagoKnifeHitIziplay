using System.Collections.Generic;
using IagoUnityScriptsRepo;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TargetBehavior : ASingletonDestroyedOnLoad<TargetBehavior>
{
    //Fields
    [Header("Drag these with the Inspector")]
    [Tooltip("Everything that should spin with the target should parent this.")]
    [SerializeField] private Transform spinParentRef;
    [SerializeField] private SpriteRenderer targetSpriteRef;
    [SerializeField] private Transform knivesStuckParentRef;
    [SerializeField] private Transform knifeSpawnPointRef;
    [SerializeField] private GameObject stuckKnifePrefab;

    [Header("Spin")]
    [SerializeField] private bool shouldSpin;
    private float spinDuration;

    private List<GameObject> allStuckKnives;
    private GameManager gameManager;
    
    //Animator
    private Animator animator;
    private readonly int ShowHash = Animator.StringToHash("Show");
    public void SetShow(bool doShow) => animator.SetBool(ShowHash, doShow);
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        allStuckKnives = new List<GameObject>();
    }

    private void OnEnable()
    {
        gameManager.onStageChanged += TargetOnStageChanged;
        gameManager.onStageEnded += TargetOnStageEnded;

        KnifeSpawnerBehavior.Instance.onKnifeTossHit += TargetOnKnifeTossed;
    }

    private void Update()
    {
        if (gameManager.GameIsActive == false) return;
        
        SpinTarget();
    }

    private void SpinTarget()
    {
        if (!shouldSpin) return;

        var rotationAngle = gameManager.currentStage.targetSpinCurve.Evaluate(spinDuration);
        rotationAngle *= gameManager.currentStage.targetSpinSpeed;
        
        spinParentRef.Rotate(Vector3.forward, rotationAngle);

        spinDuration += Time.deltaTime;
    }
    
    private void TargetOnStageChanged(StageParameters stage)
    {
        if (stage == null) return;
        
        SetShow(true);
        targetSpriteRef.sprite = stage.targetTexture;
        
        shouldSpin = true;
        spinDuration = 0f;
        spinParentRef.rotation = Quaternion.identity;

        if (allStuckKnives == null || allStuckKnives.Count <= 0) return;
        for (int i = allStuckKnives.Count - 1; i >= 0; i--)
        {
            Destroy(allStuckKnives[i]);
        }
    }

    private void TargetOnStageEnded(bool endedWithSuccess)
    {
        if (endedWithSuccess)
            SetShow(false);
        
        shouldSpin = false;
    }

    private void TargetOnKnifeTossed(bool hit)
    {
        if (!hit) return;

        var newKnife = Instantiate(stuckKnifePrefab, knifeSpawnPointRef.position, knifeSpawnPointRef.rotation, knivesStuckParentRef);
        allStuckKnives.Add(newKnife);
    }
}
