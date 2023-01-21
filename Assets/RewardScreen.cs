using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardScreen : MonoBehaviour
{
    [SerializeField]
    private Button _buttonPrefab;
    [SerializeField]
    private List<RewardGenerator> _rewardGenerators;
    private List<IReward> _rewards;
    private List<Button> _buttons;
    [SerializeField]
    private Transform _buttonListTransform;
    private CanvasGroup _canvas;
    [SerializeField]
    private UnityEvent _onComplete;
    public UnityEvent OnComplete => _onComplete;
    private void Awake()
    {
        _onComplete ??= new UnityEvent();
        _rewardGenerators ??= new List<RewardGenerator>();
        _buttons = new List<Button>();
        _rewards = new List<IReward>();
        _canvas = GetComponent<CanvasGroup>();
    }

    public void Start()
    {
        if (_rewardGenerators != null) Init(_rewardGenerators);
    }
    public void Init(List<RewardGenerator> rewardGenerators)
    {
        int childs = _buttonListTransform.childCount;
        _rewards.Clear();
        _buttons.Clear();
        for (int i = childs - 1; i >= 0; i--)
        {
            Destroy(_buttonListTransform.GetChild(i).gameObject);
        }

        _canvas.interactable = false;
        _canvas.blocksRaycasts = false;
        _canvas.alpha = 0;

        _rewardGenerators = rewardGenerators;
        foreach (var rewardGenerator in _rewardGenerators)
        {
            var button = Instantiate(_buttonPrefab, _buttonListTransform);
            var reward = rewardGenerator.Generate();
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            var image = button.GetComponentsInChildren<Image>().Last();
            var tooltip = button.GetComponentInChildren<SimpleTooltip>();
            if (string.IsNullOrWhiteSpace(reward.Tooltip)) tooltip.enabled = false;
            tooltip.infoLeft = reward.Tooltip;
            text.text = reward.Name;
            button.onClick.AddListener(reward.OpenReward);
            image.sprite = reward.Sprite;
            reward.Completed.AddListener(RewardClaimed);
            _rewards.Add(reward);
            _buttons.Add(button);
        }
    }

    public void Open()
    {
        CoroutineQueue.Defer(CR_Open());
    }

    private IEnumerator CR_Open()
    {
        yield return _canvas.DOFade(1, 0.2f);
        _canvas.interactable = true;
        _canvas.blocksRaycasts = true;
    }
    private IEnumerator CR_Close()
    {
        yield return _canvas.DOFade(0f, 0.2f);
        _canvas.interactable = false;
        _canvas.blocksRaycasts = false;
    }
    public void RewardClaimed()
    {
        for(int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].gameObject.SetActive(!_rewards[i].Complete);
        }
        if (_rewards.All(r => r.Complete)) EndRewardScreen();
    }

    public void EndRewardScreen()
    {
        foreach (var button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        _onComplete.Invoke();

        CoroutineQueue.Defer(CR_Close());
    }
}

