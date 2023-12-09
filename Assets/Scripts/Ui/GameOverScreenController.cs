using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverScreenController : FadeableScreenController
{
    [SerializeField] private string _mainMenuSceneName;
    [Space]
    [SerializeField] private string _loseSoundUid;
    [SerializeField] private string _loseThemeUid;
    [Space]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;
    
    [Inject] private GameStateManager _gameStateManager;
    [Inject] private AudioManager _audioManager;
    
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
        _audioManager.PlayAudio(_loseSoundUid);
        _audioManager.PlayAudio(_loseThemeUid);
        
        _retryButton.onClick.AddListener(RestartLevel);
        _mainMenuButton.onClick.AddListener(StartMainMenu);
    }

    private void OnDisable()
    {
        _audioManager.StopAudio(_loseThemeUid);
        
        _retryButton.onClick.RemoveListener(RestartLevel);
        _mainMenuButton.onClick.RemoveListener(StartMainMenu);
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.GAME_OVER)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    
    private void RestartLevel()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void StartMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}
