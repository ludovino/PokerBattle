using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipCounter : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField]
    private string label;
    void Awake(){
        _text = GetComponent<TextMeshProUGUI>();
    }
    public void SetChips(int previous, int current, int change)
    {
        _text.text = $"{label}: {current}";
    }
}
