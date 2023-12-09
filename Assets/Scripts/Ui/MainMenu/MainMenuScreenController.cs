using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenuScreenController : MonoBehaviour
{
    [SerializeField] private string _firstLevelSceneName;
    [Space]
    [SerializeField] private string _mainMenuThemeUid;
    [Space]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _selectLevelButton;
    [SerializeField] private Button _exitButton;
    [Space]
    [SerializeField] private AnimatedViewController _mainMenuLayout;
    [SerializeField] private AnimatedViewController _selectLevelLayout;

    [Inject] private AudioManager _audioManager;
    
    private void OnEnable()
    {
        _audioManager.PlayAudio(_mainMenuThemeUid);
        
        _playButton.onClick.AddListener(Play);
        _selectLevelButton.onClick.AddListener(ShowSelectLevelScreen);
        _exitButton.onClick.AddListener(Exit);
        
        _mainMenuLayout.Show();
        _selectLevelLayout.HideCompletely();
    }

    private void OnDisable()
    {
        _audioManager.StopAudio(_mainMenuThemeUid);
        
        _playButton.onClick.RemoveListener(Play);
        _selectLevelButton.onClick.RemoveListener(ShowSelectLevelScreen);
        _exitButton.onClick.RemoveListener(Exit);
    }

    private void Play()
    {
        SceneManager.LoadScene(_firstLevelSceneName);
    }

    private void ShowSelectLevelScreen()
    {
        _mainMenuLayout.Hide();
        _selectLevelLayout.Show();
    }

    private void Exit()
    {
        Application.Quit();
    }
}
