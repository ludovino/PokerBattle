using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChipPile : MonoBehaviour
{
    [SerializeField]
    private Stack[] _stacks;
    [SerializeField]
    private TextMeshPro _text;
    [SerializeField] 
    private TextMeshPro _changeText;
    private Vector3 _changeTextOrigin;
    [SerializeField]
    private int _chips;
    [SerializeField]
    private SpriteRenderer _chipPrefab;
    [SerializeField]
    private float _chipHeight;
    [SerializeField]
    private int pileHeight;
    [SerializeField]
    private float stackTime;
    private Queue<SpriteRenderer> _pool;
    private Sequence _changeSequence;
    [SerializeField]
    private bool _showChange;
    [SerializeField]
    private Color _positive;
    [SerializeField]
    private Color _negative;
    [SerializeField]
    private EntityData _entityData;

    private void Awake()
    {
        _pool = new Queue<SpriteRenderer>();
    }
    private void Start()
    {
        _changeText.color = new Color(1, 1, 1, 0);
        _changeTextOrigin = _changeText.transform.position;
        SetValue(0);
        if(_entityData != null)
        {
            _entityData.OnChangeChips.AddListener(SetChips);
            SetChips(0, _entityData.Chips, _entityData.Chips);
        }
    }

    private void OnDestroy()
    {
        if (_entityData != null)
        {
            _entityData.OnChangeChips.RemoveListener(SetChips);
        }
    }

    private void OnDrawGizmos()
    {
        foreach(var stack in _stacks)
        {
            Gizmos.DrawWireSphere(stack.stackOrigin.position, 0.3f * transform.localScale.x);
        }
    }
    public void SetChips(int previous, int current, int change)
    {
        SetValue(current);
    }
    private void SetValue(int value)  //67
    {
        CoroutineQueue.Defer(CR_SetValue(value));
    }

    public IEnumerator CR_SetValue(int value)
    {
        var change = value - _chips;
        
        _chips = value;
        _text.text = _chips > 0 ? _chips.ToString() : "";
        var runningTotal = 0;
        var seq = DOTween.Sequence();
        if (change > 0 && _showChange)Increase(change);
        if (change < 0 && _showChange)Decrease(change);
        for (int i = 0; i < _stacks.Length - 1; i++)
        {
            Stack stack = _stacks[i];
            var remainder = (value - runningTotal) % _stacks[i + 1].chip.Value; // 2
            var stackValue = Mathf.Min(remainder + 5 * stack.chip.Value, value - runningTotal); // 12
            runningTotal += stackValue; //12
            var stackHeight = stackValue / stack.chip.Value; //12
            var stackSeq = SetStackHeight(stack, stackHeight);
            if (stackSeq != null) seq.Insert(0, stackSeq);
        }
        var lastStack = _stacks.Last();
        var lastValue = value - runningTotal;
        var lastHeight = lastValue / lastStack.chip.Value;
        var lastStackSeq = SetStackHeight(lastStack, lastHeight);
        if (lastStackSeq != null) seq.Insert(0, lastStackSeq);

        seq.Play();
        yield return seq.WaitForCompletion();
    }

    private void Decrease(int change)
    {
        var seq = DOTween.Sequence();
        _changeSequence.Kill();
        _changeSequence = seq;
        _changeText.transform.position = _changeTextOrigin;
        seq.Append(_changeText.transform.DOMove(_changeTextOrigin + new Vector3(0, -0.5f, 0), 1f).SetEase(Ease.OutQuad));
        _changeText.color = _negative;
        seq.Join(_changeText.DOFade(0, 0.8f).SetEase(Ease.InFlash));
        _changeText.text = $"{change}";
        seq.Play();
    }

    private void Increase(int change)
    {
        var seq = DOTween.Sequence();
        _changeSequence.Kill();
        _changeText.transform.position = _changeTextOrigin;
        seq.Append(_changeText.transform.DOMove(_changeTextOrigin + new Vector3(0, 0.5f, 0), 1f).SetEase(Ease.OutQuad));
        _changeText.color = _positive;
        seq.Join(_changeText.DOFade(0, 0.8f).SetEase(Ease.InFlash));
        _changeText.text = $"+{change}";
    }
    public Sequence SetStackHeight(Stack stack, int height)
    {
        if(stack.stack.Count > height)
        {
            return RemoveFromStack(stack, stack.stack.Count - height);
        }
        if (stack.stack.Count < height)
            return AddToStack(stack, height - stack.stack.Count);
        return null;
    }

    public Sequence AddToStack(Stack stack, int chips)
    {
        var animateTime = stackTime / chips;
        var seq = DOTween.Sequence();
        for(int i = chips; i > 0; i--)
        {
            var fadeOutColor = new Color(1, 1, 1, 0);
            var spriteRenderer = _pool.Count > 0 ? _pool.Dequeue() : Instantiate(_chipPrefab);
            spriteRenderer.sprite = stack.chip.Sprite;
            spriteRenderer.color = fadeOutColor;
            stack.stack.Push(spriteRenderer);
            spriteRenderer.transform.parent = stack.stackOrigin;
            spriteRenderer.transform.localScale = Vector3.one;
            float offset = (stack.stack.Count - 1) * _chipHeight;
            seq.Append(spriteRenderer.DOFade(1, animateTime));
            spriteRenderer.transform.localPosition = new Vector3(0, 2, -offset);
            var targetPos = new Vector3(0, offset, -offset);
            seq.Join(spriteRenderer.transform.DOLocalMove(targetPos, animateTime).SetEase(Ease.InCubic).OnStart(() => SfxManager.ChipClick(0)));
        }
        return seq;
    }

    public Sequence RemoveFromStack(Stack stack, int chips)
    {
        var animateTime = stackTime / chips;
        var seq = DOTween.Sequence();
        var toRemove = Mathf.Min(stack.stack.Count, chips);
        for (int i = toRemove; i > 0; i--)
        {
            var spriteRenderer = stack.stack.Pop();
            float offset = stack.stack.Count * _chipHeight;
            seq.Append(spriteRenderer.DOFade(0, animateTime));
            var targetPos = new Vector3(0, 2, -offset);
            seq.Join(spriteRenderer.transform.DOLocalMove(targetPos, animateTime).SetEase(Ease.OutCubic).OnStart(() => SfxManager.ChipClick(1)));
        }
        return seq;
    }

    [Serializable]
    public class Stack
    {
        Stack()
        {
            stack = new Stack<SpriteRenderer>();
        }
        public Chip chip;
        public Stack<SpriteRenderer> stack;
        public Transform stackOrigin;
    }
}

