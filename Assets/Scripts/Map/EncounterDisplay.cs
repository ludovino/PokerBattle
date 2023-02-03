using System;
using UnityEngine;
using UnityEngine.Events;

public class EncounterDisplay : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private SimpleTooltip _tooltip;
    private Action onClick;
    [SerializeField]
    private BeginEncounter _beginEncounter;
    private Encounter _encounter;
    [SerializeField]
    private UnityEvent _encounterSelected;
    public void Awake()
    {
        _encounterSelected ??= new UnityEvent();
        _beginEncounter = _beginEncounter ?? new BeginEncounter();
        _tooltip = GetComponent<SimpleTooltip>();
    }
    public void Init(Encounter encounter)
    {
        _encounter = encounter;
        onClick = null;
        if (encounter != null)
        {
            _tooltip.infoLeft = encounter.Tooltip;
            _tooltip.infoRight = $"`{encounter.EncounterType?.DisplayName}";
            spriteRenderer.sprite = encounter.EncounterType?.mapSprite;
            _encounterSelected.Invoke();
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
}
[Serializable]
public class BeginEncounter: UnityEvent<Encounter> { }