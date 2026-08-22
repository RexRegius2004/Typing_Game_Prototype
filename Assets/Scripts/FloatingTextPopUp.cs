using UnityEngine;
using DG.Tweening;
using TMPro;

public class FloatingTextPopUp : MonoBehaviour
{
    [SerializeField] TextMeshPro text;

    Color baseColor;

    void Awake()
    {
        if (text != null)
            baseColor = text.color;
    }

    public void SetText(string value)
    {
        text.text = value;
    }

    public void SetColor(Color color)
    {
        text.color = color;
        baseColor = color;
    }

    void OnEnable()
    {
        transform.DOKill();
        transform.localScale = Vector3.zero;

        Color c = baseColor;
        c.a = 1f;
        text.color = c;

        Sequence seq = DOTween.Sequence();
        seq.SetTarget(transform);
        seq.Append(transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack));
        seq.Append(transform.DOScale(0f, 0.8f).SetEase(Ease.InQuad));
        seq.Join(DOTween.ToAlpha(() => text.color, col => text.color = col, 0f, 0.8f));
    }

    void OnDisable()
    {
        transform.DOKill();
    }
}
