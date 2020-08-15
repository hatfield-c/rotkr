using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RepairHunkCell : MonoBehaviour
{
    [SerializeField] Color fillColor;
    [SerializeField] Color highlightColor;
    [SerializeField] Image FillImage = null;
    [SerializeField] Animator highlight = null;

    public float FillSpeed = .5f;

    Sequence highlightSequence;

    void Start()
    {
        highlightSequence = DOTween.Sequence();
        highlightSequence.InsertCallback(0f, () => FillImage.DOColor(highlightColor, FillSpeed));
        highlightSequence.SetAutoKill(false);
        highlightSequence.Pause();
        highlightSequence.OnComplete(() =>
        {
            highlightSequence.Restart();
            highlightSequence.Pause();
        });
    }
    public void Fill(bool value)
    {
        if (value)
            FillImage.color = fillColor;
        else
            FillImage.color = new Color(FillImage.color.r, FillImage.color.g, FillImage.color.b, 0);
    }
    public void Highlight()
    {
            FillImage.color = highlightColor;
    }
}
