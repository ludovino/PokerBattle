using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockPopup : MonoBehaviour
{
    [SerializeField]    
    private Image _sprite;
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _description;
    public void Init(UnlockDisplay unlockDisplay)
    {
        _sprite.sprite = unlockDisplay.Sprite;
        _name.text = unlockDisplay.Name;
        _description.text = unlockDisplay.Description;
    }
}

