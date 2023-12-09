using UnityEngine;
using Zenject;

public class LockpickReward : ChestReward
{
    [SerializeField] private PinMaterial _lockpickMaterial;

    [Inject] private InventoryManager _inventoryManager;

    protected override void HandleOnClick()
    {
        _inventoryManager.AddLockpick(_lockpickMaterial);
    }
}
