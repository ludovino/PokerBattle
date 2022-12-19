using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CardEffectDisplay : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sprite;
    [SerializeField]
    private Transform _effectTransform;
    public void Awake()
    {
        _sprite.enabled = false;
    }

    public void Init(PlayContext context, AudioClip soundEffect)
    {
        var suit = context.Card.suit;
        _sprite.sprite = suit.sprite;
        var spriteColor = suit.Color.Value;
        spriteColor.a = 0;
        _sprite.color = spriteColor;
        CoroutineQueue.Defer(Animate(context, soundEffect));
    }

    private IEnumerator Animate(PlayContext context, AudioClip soundEffect)
    {
        yield return null;
        _sprite.enabled = true;
        _effectTransform.position = context.Card.transform.position;
        var seq = DOTween.Sequence();
        seq.Append(_effectTransform.DOPunchScale(_effectTransform.localScale * 1.5f, 0.3f));
        seq.Join(_sprite.DOFade(1f, 0.2f));
        seq.InsertCallback(0.1f, () => SfxManager.Play(soundEffect));
        seq.Append(_sprite.DOFade(0f, 0.2f));
        seq.OnComplete(() => Destroy(gameObject));
        seq.Play();
    }
}