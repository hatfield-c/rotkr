using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class AView : MonoBehaviour, IView
{
    public enum ViewTransition { None, Fade };
    public ViewTransition Transition = ViewTransition.Fade;
    Sequence currentViewSequence = null;

    #region references
    CanvasGroup CanvasGroup;
    #endregion

    #region view variables
    public bool HideOnStart = true;
    public float fadeDuration = .5f;
    #endregion

    protected void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }
    protected void Start()
    {
        if (HideOnStart)
            InstantHide();
    }

    public virtual void Show()
    {
        currentViewSequence.Kill();
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => 
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        });

        switch (Transition)
        {
            case ViewTransition.None:
                break;
            case ViewTransition.Fade:
                sequence.Append(CanvasGroup.DOFade(1, fadeDuration));
                break;
        }

        sequence.AppendCallback(() =>
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        });

        sequence.SetAutoKill(false);
        sequence.Pause();
        currentViewSequence = sequence;
        currentViewSequence.Play();
    }
    public virtual void Hide()
    {
        currentViewSequence.Kill();
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        });

        switch (Transition)
        {
            case ViewTransition.None:
                break;
            case ViewTransition.Fade:
                sequence.Append(CanvasGroup.DOFade(0, fadeDuration));
                break;
        }

        sequence.SetAutoKill(false);
        sequence.Pause();
        currentViewSequence = sequence;
        currentViewSequence.Play();
    }

    void InstantHide()
    {
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.alpha = 0;
    }
}
