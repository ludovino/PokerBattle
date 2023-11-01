using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// TODO inky

public class OutcomeSelector : MonoBehaviour
{
    [SerializeField]
    private SimpleTooltipStyle _style;
    [SerializeField]
    private Button _buttonPrefab;
    [SerializeField]
    private List<Outcome> _outcomes;
    private List<Button> _buttons;

    [SerializeField]
    private UnityEvent _onOptionSelected;
    public UnityEvent OnOptionSelected => _onOptionSelected;
    [SerializeField]
    private UnityEvent _onComplete;
    public UnityEvent OnComplete => _onComplete;
    private void Awake()
    {
        _onOptionSelected ??= new UnityEvent();
        _onComplete ??= new UnityEvent();
        _outcomes ??= new List<Outcome>();
        _buttons = new List<Button>();
    }

    public void Start()
    {
        if(_outcomes != null) Init(_outcomes);
    }
    public void Init(List<Outcome> outcomes)
    {
        int childs = transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        _outcomes = outcomes;
        foreach(var outcome in _outcomes)
        {
            var button = Instantiate(_buttonPrefab, transform);
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            var desc = outcome.Description;
            //_style.Apply(ref desc);
            text.text = desc;
            button.onClick.AddListener(() => outcome.Execute(Complete));
            button.onClick.AddListener(OptionSelected);
            _buttons.Add(button);
        }
    }

    public void OptionSelected()
    {
        foreach(var button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        _onOptionSelected.Invoke();
    }

    public void Complete()
    {
        _onComplete.Invoke();
    }
}
