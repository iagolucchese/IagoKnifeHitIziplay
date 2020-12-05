using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UICanvasGroup : MonoBehaviour
{
    public float fadeInDelay = 0.25f;
    public float fadeOutDelay = 0.25f;

    public GameObject selectedOnCanvasOpen;
    
    [SerializeField]
    private bool isShowingCanvas;
    public bool IsShowingCanvas
    {
        get => isShowingCanvas;
        set
        {
            isShowingCanvas = value;
            ToggleCanvasGroup(value);
        }
    }

    private CanvasGroup canvasGroup;
    private EventSystem eventSystem;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        IsShowingCanvas = isShowingCanvas; //sets the property, in case the bool got changed in the inspector
    }

    public TweenerCore<float, float, FloatOptions> ToggleCanvasGroup(bool doShow)
    {
        canvasGroup.DOKill();
        canvasGroup.blocksRaycasts = doShow;
        canvasGroup.interactable = doShow;
        if (doShow && eventSystem != null && selectedOnCanvasOpen != null)
        {
            eventSystem.SetSelectedGameObject(selectedOnCanvasOpen);
        }

        return canvasGroup.DOFade(doShow ? 1f : 0f, doShow ? fadeInDelay : fadeOutDelay);
    }
}
