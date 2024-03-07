using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BreatheAnimate : MonoBehaviour
{
    [OnValueChanged("SetDotWeen")]
    public float scaleSpeed = 0.5f;

    [OnValueChanged("SetDotWeen")]
    public float scaleAmount = 0.05f;

    [OnValueChanged("SetDotWeen")]
    public Ease easeFunction;

    private SpriteRenderer _spriteRenderer;
    private Sequence _sequence;
    private Vector3 _originalScale;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalScale = _spriteRenderer.transform.localScale;
        SetDotWeen();
    }

    private void SetDotWeen()
    {
        _sequence.Kill();
        _spriteRenderer.transform.localScale = _originalScale;
        _sequence = DOTween.Sequence()
            .Append(_spriteRenderer.transform.DOScale(
                    new Vector3(_originalScale.x + scaleAmount, _originalScale.y + scaleAmount,
                        _originalScale.z + scaleAmount), scaleSpeed)
                .SetEase(easeFunction))
            .Append(_spriteRenderer.transform.DOScale(_originalScale, scaleSpeed).SetEase(easeFunction));

        _sequence.SetLoops(-1, LoopType.Restart);
    }
}