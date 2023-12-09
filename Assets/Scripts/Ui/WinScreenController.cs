using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class WinScreenController : FadeableScreenController
{
    [SerializeField] private string _nextLevelSceneName;
    [SerializeField] private string _mainMenuSceneName;
    [Space]
    [SerializeField] private string _mainThemeUid;
    [SerializeField] private string _winSoundUid;
    [Space]
    [SerializeField] private Button _nextLevelButton;
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
        _audioManager.PlayAudio(_winSoundUid);
        _audioManager.PlayAudio(_mainThemeUid);
        
        _nextLevelButton.onClick.AddListener(StartNextLevel);
        _mainMenuButton.onClick.AddListener(StartMainMenu);
    }

    private void OnDisable()
    {
        _audioManager.StopAudio(_mainThemeUid);
        
        _nextLevelButton.onClick.RemoveListener(StartNextLevel);
        _mainMenuButton.onClick.RemoveListener(StartMainMenu);
    }

    protected override void Hide()
    {
        HideCompletely();
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.LEVEL_WON)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void StartNextLevel()
    {
        SceneManager.LoadScene(_nextLevelSceneName);
    }

    private void StartMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}
