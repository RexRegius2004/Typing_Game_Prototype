using UnityEngine;
using DG.Tweening;
using TMPro;

public class FloatingTextPopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void SetText(string value)
    {
        text.text = value;
    }

    void Start()
    {
        transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack));
        seq.Append(transform.DOScale(0f, 0.8f).SetEase(Ease.InQuad));
        seq.Join(text.DOFade(0f, 0.8f));
    }
}
