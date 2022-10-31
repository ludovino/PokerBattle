using System;
using UnityEngine;
using UnityEngine.Events;

public class EncounterDisplay : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public void Start()
    {
        _tooltipManager = FindObjectOfType<TooltipManager>();
    }
    private Action onClick;
    [SerializeField]
    private BeginEncounter _beginEncounter;
    private TooltipManager _tooltipManager;
    private Encounter _encounter;
    public void Awake()
    {
        _beginEncounter = _beginEncounter ?? new BeginEncounter();
    }
    public void Init(Encounter encounter)
    {
        _encounter = encounter;
        onClick = null;
        if (encounter != null)
        {
            spriteRenderer.sprite = encounter.EncounterType?.mapSprite;
            onClick += encounter.BeginEncounter;
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }

    private void OnMouseDown()
    {
        if (_encounter != null)
        {
            _beginEncounter.Invoke(_encounter);
            onClick.Invoke();
        }
    }

    private void OnMouseEnter()
    {
        if (_encounter != null)
            _tooltipManager.ShowTooltip(_encounter.Tooltip);
    }
    private void OnMouseExit()
    {
        if (_encounter != null)
            _tooltipManager.HideTooltip();
    }
}
[Serializable]
public class BeginEncounter: UnityEvent<Encounter> { }