using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PickupScreenController : FadeableScreenController, IPointerClickHandler
{
    [SerializeField] private string _lockpickText = "Lockpick found!";
    [SerializeField] private string _treasureText = "Treasure found!";
    [Space]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private GameObject _treasureUiPrefab;
    
    [Inject] private InventoryManager _inventoryManager;
    [Inject] private MaterialsLibrary _materialsLibrary;
    [Inject] private GameStateManager _gameStateManager;

    private GameObject _spawnedUiPrefab;
    
    protected override void Awake()
    {
        base.Awake();
        _inventoryManager.OnLockpickPickedUp += ShowWithLockpick;
        _inventoryManager.OnTreasurePickedUp += ShowWithTreasure;
    }

    protected override void OnDestroy()
    {
        _inventoryManager.OnLockpickPickedUp -= ShowWithLockpick;
        _inventoryManager.OnTreasurePickedUp -= ShowWithTreasure;
        base.OnDestroy();
    }

    protected override void Hide()
    {
        _gameStateManager.SetTempleState();
        base.Hide();
    }

    private void ShowWithLockpick(PinMaterial material)
    {
        _gameStateManager.SetScreenShownState();
        
        var prefab = _materialsLibrary.GetUiLockpickPrefab(material);
        _spawnedUiPrefab = Instantiate(prefab, transform);
        _titleText.text = _lockpickText;
        Show();
    }

    private void ShowWithTreasure()
    {
        _gameStateManager.SetScreenShownState();
        
        _spawnedUiPrefab = Instantiate(_treasureUiPrefab, transform);
        _titleText.text = _treasureText;
        Show();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isHiding) return;

        if (_spawnedUiPrefab != null)
        {
            Destroy(_spawnedUiPrefab);
        }
        
        Hide();
    }
}
