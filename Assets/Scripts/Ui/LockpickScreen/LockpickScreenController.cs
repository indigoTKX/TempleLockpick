using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LockpickScreenController : FadeableScreenController
{
    public event Action OnMoveLeft;
    public event Action OnMoveRight;
    public event Action OnPick;

    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _pickButton;
    [Space]
    [SerializeField] private Button _backButton;

    [Inject] private GameStateManager _gameStateManager;

    protected override void Awake()
    {
        base.Awake();
        _gameStateManager.OnStateChanged += HandleGameStateChanged;
    }

    protected override void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= HandleGameStateChanged;
        base.OnDestroy();
    }

    private void OnEnable()
    {
        _leftButton.onClick.AddListener(FireMoveLeft);
        _rightButton.onClick.AddListener(FireMoveRight);
        _pickButton.onClick.AddListener(FirePick);
        
        _backButton.onClick.AddListener(HandleBack);
    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(FireMoveLeft);
        _rightButton.onClick.RemoveListener(FireMoveRight);
        _pickButton.onClick.RemoveListener(FirePick);
        
        _backButton.onClick.RemoveListener(HandleBack);
    }

    private void FireMoveLeft()
    {
        OnMoveLeft?.Invoke();
    }

    private void FireMoveRight()
    {
        OnMoveRight?.Invoke();
    }

    private void FirePick()
    {
        OnPick?.Invoke();
    }

    private void HandleBack()
    {
        _gameStateManager.SetTempleState();
    }
    
    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.LOCKPICK)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
