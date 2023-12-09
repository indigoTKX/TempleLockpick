using Zenject;

public class TreasureReward : ChestReward
{
    [Inject] private InventoryManager _inventoryManager;

    protected override void HandleOnClick()
    {
        _inventoryManager.PickupTreasure();
    }
}
