using UnityEngine;
using Zenject;

public class Level1Installer : MonoInstaller
{
    [SerializeField] private GameStateManager _gameStateManager;
    [SerializeField] private LockpickScreenController _lockpickScreenController;
    [SerializeField] private LockController _lockController;
    [SerializeField] private MaterialsLibrary _materialsLibrary;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private TimerManager _timerManager;
    
    public override void InstallBindings()
    {
        Container.Bind<GameStateManager>().FromInstance(_gameStateManager).AsSingle();
        Container.Bind<LockpickScreenController>().FromInstance(_lockpickScreenController).AsSingle();
        Container.Bind<LockController>().FromInstance(_lockController).AsSingle();
        Container.Bind<MaterialsLibrary>().FromScriptableObject(_materialsLibrary).AsSingle();
        Container.Bind<InventoryManager>().FromInstance(_inventoryManager).AsSingle();
        Container.Bind<TimerManager>().FromInstance(_timerManager).AsSingle();
    }
}