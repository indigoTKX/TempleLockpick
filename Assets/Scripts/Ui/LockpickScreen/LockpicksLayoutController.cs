using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LockpicksLayoutController : MonoBehaviour
{
    [SerializeField] private LockpickButtonController _lockpickButtonPrefab;
    
    [Inject] private InventoryManager _inventoryManager;
    [Inject] private MaterialsLibrary _materialsLibrary;

    private List<LockpickButtonController> _lockpickButtons = new List<LockpickButtonController>();
    private Dictionary<LockpickButtonController, Action> _onClickHandlers = new Dictionary<LockpickButtonController, Action>();

    private void OnEnable()
    {
        ClearButtons();

        var lockpickMaterials = _inventoryManager.GetAllLockpicks();
        if (lockpickMaterials == null) return;
        
        foreach (var lockpickMaterial in lockpickMaterials)
        {
            var button = Instantiate(_lockpickButtonPrefab, transform);
            var lockpickSprite = _materialsLibrary.GetLockpickForMaterial(lockpickMaterial);
            button.SetSprite(lockpickSprite);

            _onClickHandlers.Add(button, () => SetLockpick(lockpickMaterial));
            button.OnClick += _onClickHandlers[button];
            
            _lockpickButtons.Add(button);
        }
    }

    private void OnDisable()
    {
        ClearButtons();
    }

    private void SetLockpick(PinMaterial material)
    {
        _inventoryManager.SetLockpick(material);
    }

    private void ClearButtons()
    {
        foreach (var button in _lockpickButtons)
        {
            button.OnClick -= _onClickHandlers[button];
            _onClickHandlers.Remove(button);
            Destroy(button.gameObject);
        }
        _lockpickButtons.Clear();
    }
}
