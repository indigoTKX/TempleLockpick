using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class InventoryManager : MonoBehaviour
{
    public event Action<PinMaterial> OnLockpickChanged;
    public event Action<PinMaterial> OnLockpickPickedUp;
    public event Action OnTreasurePickedUp;
    
    [SerializeField] private List<PinMaterial> _initialLockpicks;

    public void AddLockpick(PinMaterial material)
    {
        _currentLockpicks.Add(material);
        OnLockpickPickedUp?.Invoke(material);
    }

    public void RegisterTreasure()
    {
        _treasureCount++;
    }
    
    public void PickupTreasure()
    {
        _treasureCount--;
        if (_treasureCount <= 0)
        {
            _gameStateManager.SetGameWonState();
        }
        else
        {
            OnTreasurePickedUp?.Invoke();
        }
    }

    public List<PinMaterial> GetAllLockpicks()
    {
        return _currentLockpicks;
    }

    public void SetLockpick(PinMaterial material)
    {
        _currentLockpickMaterial = material;
        OnLockpickChanged?.Invoke(material);
    }

    public PinMaterial GetCurrentLockpick()
    {
        return _currentLockpickMaterial;
    }

    [Inject] private GameStateManager _gameStateManager;
    
    private List<PinMaterial> _currentLockpicks;
    private PinMaterial _currentLockpickMaterial;

    private int _treasureCount;

    private void Awake()
    {
        _currentLockpicks = _initialLockpicks;
        _currentLockpickMaterial = _currentLockpicks.First();
    }
}
