using UnityEngine;
using Zenject;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float _timeLimit = 60;

    public float GetCurrentTime()
    {
        return _currentTimeLeft;
    }

    public float GetMaxTime()
    {
        return _timeLimit;
    }

    [Inject] private GameStateManager _gameStateManager;
    
    private float _currentTimeLeft;
    private bool _isTimerOn;

    private void Awake()
    {
        _currentTimeLeft = _timeLimit;
        _isTimerOn = true;

        _gameStateManager.OnStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= HandleGameStateChanged;
    }

    private void Update()
    {
        if (!_isTimerOn) return;
        
        _currentTimeLeft -= Time.deltaTime;

        if (_currentTimeLeft <= 0)
        {
            _gameStateManager.SetGameOverState();
        }
    }

    private void HandleGameStateChanged(GameState state)
    {
        _isTimerOn = state is not (GameState.GAME_OVER or GameState.LEVEL_WON or GameState.SCREEN_SHOWN);
    }
}
