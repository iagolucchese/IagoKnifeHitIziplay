using System.Collections;
using IagoUnityScriptsRepo;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class KnifeSpawnerBehavior : ASingletonDestroyedOnLoad<KnifeSpawnerBehavior>
{
    //Fields
    [Header("References")]
    [SerializeField] private Animator spawnerAnimator;
    [SerializeField] private Animator knifeAnimator;

    [Header("Knife Toss Params")]
    [SerializeField] private float knifeTossToHitDelay = 0.1f;
    [SerializeField] private float knifeReloadDelay = 0.2f;
    [SerializeField] private Collider2D knifeCheckCollider;
    [SerializeField] private LayerMask knifeCollisionMask;
    
    private float reloadDelay;
    private InputManager inputManager;
    private GameManager gameManager;
    
    //Events

    /// <summary>
    /// bool: true if the knife hit the target, false if it hit an obstacle.
    /// </summary>
    public UnityAction<bool> onKnifeTossHit;
    public UnityAction onKnifeTossed;

    //Animator
    private readonly int spawnerStateHash = Animator.StringToHash("SpawnerState");
    public void SetSpawnerState(int state) => spawnerAnimator.SetInteger(spawnerStateHash, state);
    
    private readonly int knifeTossHash = Animator.StringToHash("Toss");
    private readonly int knifeHitHash = Animator.StringToHash("Hit");
    private readonly int knifeMissHash = Animator.StringToHash("Miss");
    private readonly int knifeReloadHash = Animator.StringToHash("Reload");
    public void ReloadKnifeAnimTrigger() => knifeAnimator.SetTrigger(knifeReloadHash);
    
    private void Start()
    {
        inputManager = InputManager.Instance;
        gameManager = GameManager.Instance;    
    }

    private void Update()
    {
        if (gameManager.GameIsActive == false) return;
        
        ProcessFireInput();
        reloadDelay -= Time.deltaTime;
    }

    private void ProcessFireInput()
    {
        if (inputManager.fire1Down && reloadDelay <= 0f)
            StartCoroutine(KnifeTossCoroutine());
    }

    private IEnumerator KnifeTossCoroutine()
    {
        knifeAnimator.SetTrigger(knifeTossHash);
        reloadDelay = knifeReloadDelay;
        onKnifeTossed?.Invoke();

        yield return new WaitForSeconds(knifeTossToHitDelay);
        
        var result = CheckKnifeHitOnTarget();
        if (result)
        {
            knifeAnimator.SetTrigger(knifeHitHash);
        }
        else
        {
            knifeAnimator.SetTrigger(knifeMissHash);
            //if (gameManager.GameIsActive)
                //knifeAnimator.SetTrigger(knifeReloadHash);
        }
        
        onKnifeTossHit?.Invoke(result);
    }
    
    /// <summary>
    /// Collision check on Knife toss.
    /// </summary>
    /// <returns> bool: true if it hit the target, false if it hit an obstacle and missed. </returns>
    public bool CheckKnifeHitOnTarget()
    {
        var bounds = knifeCheckCollider.bounds;
        var results = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f, knifeCollisionMask);

        if (results != null && results.Length > 0)
            return false;
        
        return true;
    }
}
