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
    public void Fill(bool value, bool instant = false)
    {
        if (instant)
        {
            if (value)
                FillImage.color = new Color(FillImage.color.r, FillImage.color.g, FillImage.color.b, 1);
            else
                FillImage.color = new Color(FillImage.color.r, FillImage.color.g, FillImage.color.b, 0);
            return;
        }
        if (value)
            FillImage.DOFade(1, FillSpeed);
        else
            FillImage.DOFade(0, FillSpeed);
    }
    public void Highlight(bool value)
    {
        if (value)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.InsertCallback(0f, () => FillImage.DOColor(highlightColor, FillSpeed));

            sequence.SetAutoKill(false);
            sequence.Pause();
            highlightSequence = sequence;
            highlightSequence.Play();
        }
        else
        {
            highlightSequence.Kill();
            Fill(false, true);
        }
    }
}
