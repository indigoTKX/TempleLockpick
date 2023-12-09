using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LockController : MonoBehaviour
{
    public event Action OnOpen;
    public event Action OnPinCdt;

    [SerializeField] private LockpickAnimationController _lockpickController;
    [SerializeField] private List<PinController> _pins;
    [Space]
    [SerializeField] private string OpenLockSoundUid;

    public void SetPins(List<PinMaterial> pinMaterials)
    {
        for (int i = 0; i < _pins.Count; i++)
        {
            _pins[i].SetMaterial(pinMaterials[i]);
        }
    }

    [Inject] private GameStateManager _gameStateManager;
    [Inject] private AudioManager _audioManager;

    private LockAppearAnimationController _animationController;
    
    private int _currentHackedPins = 0;
    private int _pinsCount;

    private void Awake()
    {
        ResetLock();
        
        _animationController = GetComponent<LockAppearAnimationController>();
        _animationController.OnShow += SubscribeOnPins;
        _animationController.OnHide += ResetLock;

        _lockpickController.OnPinCdt += FireOnPinCdt;
    }

    private void OnDestroy()
    {
        _animationController.OnShow -= SubscribeOnPins;
        _animationController.OnHide -= ResetLock;
        
        _lockpickController.OnPinCdt -= FireOnPinCdt;
    }

    private void OnDisable()
    {
        ResetLock();
    }
    
    private void ResetLock()
    {
        _lockpickController.ResetPosition();
        
        _currentHackedPins = 0;
        _pinsCount = _pins.Count;
        
        foreach (var pin in _pins)
        {
            pin.OnHacked -= HandleOnPinHacked;
            pin.ResetPosition();
        }
    }
    
    private void SubscribeOnPins()
    {
        foreach (var pin in _pins)
        {
            pin.OnHacked -= HandleOnPinHacked;
            pin.OnHacked += HandleOnPinHacked;
        }
    }

    private void HandleOnPinHacked()
    {
        _currentHackedPins++;
        if (_currentHackedPins < _pinsCount) return;
        
        OnOpen?.Invoke();
        _gameStateManager.SetTempleState();
        _audioManager.PlayAudio(OpenLockSoundUid);
    }

    private void FireOnPinCdt()
    {
        OnPinCdt?.Invoke();
    }
}
