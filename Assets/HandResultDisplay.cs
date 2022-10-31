using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class HandResultDisplay : MonoBehaviour
{
    private Evaluation _evaluation;
    [SerializeField]
    private ResultStyle _winStyle; 
    [SerializeField]
    private ResultStyle _lossStyle;
    [SerializeField]
    private Display _playerDisplay;
    [SerializeField]
    private Display _enemyDisplay;
    [SerializeField]
    private HandResultEffect _winEffect;
    [SerializeField]
    private HandResultEffect _loseEffect;
    private void Start()
    {
        _playerDisplay.Init();
        _enemyDisplay.Init();
    }

    public void OnEvaluation(Evaluation evaluation)
    {
        _evaluation = evaluation;
        var coroutine = _evaluation.result switch
        {
            Result.PlayerWin => Win(),
            Result.PlayerLose => Lose(),
            _ => Draw()
        };
        CoroutineQueue.Defer(coroutine);
    }

    public IEnumerator Draw()
    {
        var seq = _playerDisplay.Show(_evaluation.playerHand, _lossStyle)
            .Append(_enemyDisplay.Show(_evaluation.enemyHand, _lossStyle))
            .AppendInterval(0.5f)
            .Append(_enemyDisplay.HideText())
            .Join(_playerDisplay.HideText())
            .Play();
        yield return seq.WaitForCompletion();
    }

    public IEnumerator Win()
    {
        _winEffect.Init(_evaluation.playerHand.cards);
        var seq = _enemyDisplay.Show(_evaluation.enemyHand, _lossStyle)
            .Append(_playerDisplay.Show(_evaluation.playerHand, _winStyle))
            .AppendCallback(() => _winEffect.Fire())
            .AppendCallback(() => SfxManager.Cheer())
            .AppendInterval(1f)
            .Append(_enemyDisplay.HideText())
            .Join(_playerDisplay.HideText())
            .Play();
        yield return seq.WaitForCompletion();
    }

    public IEnumerator Lose()
    {
        _loseEffect.Init(_evaluation.enemyHand.cards);
        var seq = _playerDisplay.Show(_evaluation.playerHand, _lossStyle)
            .Append(_enemyDisplay.Show(_evaluation.enemyHand, _winStyle))
            .AppendCallback(() => _loseEffect.Fire())
            .AppendCallback(() => SfxManager.Aww())
            .AppendInterval(1f)
            .Append(_enemyDisplay.HideText())
            .Join(_playerDisplay.HideText())
            .Play();

        yield return seq.WaitForCompletion();
    }

    [Serializable]
    public class Display
    {
        public TextMeshPro handText;
        public Transform textStart;
        private Vector3 _textEnd;
        public Transform cardDisplayCentre;
        public void Init()
        {
            _textEnd = handText.transform.position;
            handText.color = new Color(1, 1, 1, 0);
            handText.enabled = false;
        }
        public Sequence Show(RankedHand hand, ResultStyle style)
        {
            return ShowCards(hand, style)
                .AppendInterval(style.textDelay)
                .Append(ShowText(hand, style));
        }

        public Sequence ShowText(RankedHand hand, ResultStyle style)
        {
            handText.enabled = true;
            handText.text = hand.hand.DisplayName;
            handText.transform.position = textStart.position;
            handText.colorGradientPreset = style.colorGradient;
            handText.fontMaterial = style.material;

            Sequence sequence = DOTween.Sequence();
            return sequence
                .Append(handText.transform.DOMove(_textEnd, 0.2f).SetEase(Ease.OutBack))
                .Join(handText.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.5f))
                .Join(handText.DOFade(1f, 0.1f).SetEase(Ease.OutQuad));
        }

        public Tween HideText() 
        {
            return handText.DOFade(0, 0.3f).OnComplete(() => handText.enabled = false);
        }

        public Sequence ShowCards(RankedHand hand, ResultStyle style)
        {
            var seq = DOTween.Sequence();
            for(int i = 0; i < hand.cards.Length; i++)
            {
                var isRankingCard = hand.hand.rankingCardsCount > i;
                var card = hand.cards[i];
                var offset = new Vector3(i - 2, 0, 0);
                var targetPos = cardDisplayCentre.position + offset * style.cardScaleFactor;
                var move = card.transform.DOMove(targetPos, 0.2f).SetEase(Ease.InBack).OnStart(() => SfxManager.PlayCard(isRankingCard ? 4 : 3));
                var scale = card.transform.DOScale(style.cardScaleFactor, 0.2f).SetEase(Ease.OutElastic);
                seq.Append(move);
                seq.Join(scale);
                if (isRankingCard)
                {
                    var shake = card.transform.DOShakeRotation(0.4f, strength: 90);
                    seq.Join(shake);
                }
                var delay = hand.hand.rankingCardsCount > i + 1 ? style.rankCardsDelay : style.kickerDelay;
                seq.AppendInterval(delay);
            }
            return seq;
        }
    }



    [Serializable]
    public class ResultStyle
    {
        public float textColorCycleTime;
        public TMP_ColorGradient colorGradient;
        public Material material;
        public float cardScaleFactor;
        public float rankCardsDelay;
        public float kickerDelay;
        public float textDelay;
    }
}
