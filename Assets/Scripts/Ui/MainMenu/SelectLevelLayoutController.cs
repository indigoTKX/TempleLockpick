using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelLayoutController : MonoBehaviour
{
    [SerializeField] private float _selectAnimationTime = 1f;
    [SerializeField] private RectTransform _previewLayout;
    [SerializeField] private List<float> _levelPreviewXOffsets;
    [Space]
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _backButton;
    [Space]
    [SerializeField] private AnimatedViewController _mainMenuLayout;

    private const string LEVEL_NAME = "level_";

    private int _currentLevelIdx = 0;

    private AnimatedViewController _viewController;

    private void Awake()
    {
        _viewController = GetComponent<AnimatedViewController>();
    }

    private void OnEnable()
    {
        _currentLevelIdx = 0;
        MoveToCurrentLevel(true);
        
        _leftButton.onClick.AddListener(MoveLeft);
        _selectButton.onClick.AddListener(PlayLevel);
        _rightButton.onClick.AddListener(MoveRight);
        
        _backButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(MoveLeft);
        _selectButton.onClick.RemoveListener(PlayLevel);
        _rightButton.onClick.RemoveListener(MoveRight);
        
        _backButton.onClick.RemoveListener(ReturnToMainMenu);
    }

    private void MoveLeft()
    {
        _currentLevelIdx--;
        if (_currentLevelIdx < 0)
        {
            _currentLevelIdx = 0;
        }
        
        MoveToCurrentLevel();
    }
    
    private void MoveRight()
    {
        _currentLevelIdx++;
        if (_currentLevelIdx >= _levelPreviewXOffsets.Count)
        {
            _currentLevelIdx = _levelPreviewXOffsets.Count - 1;
        }

        MoveToCurrentLevel();
    }

    private void MoveToCurrentLevel(bool instantly = false)
    {
        var targetXPos = _levelPreviewXOffsets[_currentLevelIdx];
        if (instantly)
        {
            _previewLayout.DOLocalMoveX(targetXPos, 0);
        }
        else
        {
            _previewLayout.DOLocalMoveX(targetXPos, _selectAnimationTime);
        }
    }

    private void PlayLevel()
    {
        _viewController.Hide();
        
        var levelName = LEVEL_NAME + (_currentLevelIdx + 1);
        SceneManager.LoadScene(levelName);
    }

    private void ReturnToMainMenu()
    {
        _viewController.Hide();
        _mainMenuLayout.Show();
    }
}
