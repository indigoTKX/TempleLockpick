using System;
using UnityEngine;
using UnityEngine.UI;

public class LockpickButtonController : MonoBehaviour
{
    public event Action OnClick;
    
    [SerializeField] private Image _lockpickImage;

    private Button _button;

    public void SetSprite(Sprite sprite)
    {
        _lockpickImage.sprite = sprite;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(FireOnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(FireOnClick);
    }

    private void FireOnClick()
    {
        OnClick?.Invoke();
    }
}
