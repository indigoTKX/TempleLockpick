using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChestController : MonoBehaviour
{
    public event Action OnLockShown;
    
    [SerializeField] private List<PinMaterial> _pinMaterials;
    [Space]
    [SerializeField] private ChestReward _rewardPrefab;

    //called via animation event
    public void DisableChest()
    {
        Destroy(gameObject);
    }
    
    //called via animation event
    public void SpawnGoods()
    {
        _diContainer.InstantiatePrefab(_rewardPrefab, transform.position, Quaternion.identity, null);
    }
    
    private const string OPEN_ANIMATION_UID = "Open";
    
    [Inject] private GameStateManager _gameStateManager;
    [Inject] private LockController _lockController;
    [Inject] private InventoryManager _inventoryManager;

    private DiContainer _diContainer;
    
    private Animator _animator;
    private bool _isClickable;
    private bool _isOpen;
    
    [Inject]
    private void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _gameStateManager.OnStateChanged += HandleOnGameStateChanged;

        var treasure = _rewardPrefab as TreasureReward;
        if (treasure != null)
        {
            _inventoryManager.RegisterTreasure();
        }
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= HandleOnGameStateChanged;
        _lockController.OnOpen -= OpenChest;
    }

    private void OnMouseDown()
    {
        if (!_isClickable || _isOpen) return;
        
        SetupLock();
        _lockController.OnOpen += OpenChest;
    }

    private void HandleOnGameStateChanged(GameState state)
    {
        _isClickable = state == GameState.TEMPLE;

        if (_isClickable)
        {
            _lockController.OnOpen -= OpenChest;
        }
    }

    private void SetupLock()
    {
        _gameStateManager.SetLockpickState();
        _lockController.SetPins(_pinMaterials);
        OnLockShown?.Invoke();
    }

    private void OpenChest()
    {
        _isOpen = true;
        _animator.SetTrigger(OPEN_ANIMATION_UID);
    }
}
