using System;
using UnityEngine;
using Zenject;

public class GameStateManager : MonoBehaviour
{
    public event Action<GameState> OnStateChanged;

    [SerializeField] private string playThemeUid;

    public GameState GetCurrentState()
    {
        return _currentState;
    }

    public void SetLockpickState()
    {
        SetState(GameState.LOCKPICK);
    }

    public void SetTempleState()
    {
        SetState(GameState.TEMPLE);
    }

    public void SetGameWonState()
    {
        _audioManager.StopAudio(playThemeUid);
        SetState(GameState.LEVEL_WON);
    }

    public void SetScreenShownState()
    {
        SetState(GameState.SCREEN_SHOWN);
    }

    public void SetGameOverState()
    {
        _audioManager.StopAudio(playThemeUid);
        SetState(GameState.GAME_OVER);
    }

    [Inject] private AudioManager _audioManager;
    
    private GameState _currentState = GameState.TEMPLE;

    private void Start()
    {
        SetState(GameState.TEMPLE);
        _audioManager.PlayAudio(playThemeUid);
    }

    public void SetState(GameState state)
    {
        _currentState = state;
        OnStateChanged?.Invoke(_currentState);
        
        Debug.Log(_currentState.ToString());
    }
}
    
public enum GameState
{
    TEMPLE,
    LOCKPICK,
    SCREEN_SHOWN,
    GAME_OVER,
    LEVEL_WON
}


