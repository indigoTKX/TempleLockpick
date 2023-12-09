using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TutorialScreenController : FadeableScreenController, IPointerClickHandler
{
    public event Action OnClick;
    
    [SerializeField] private TMP_Text _tutorialText;
    [SerializeField] private List<GameObject> _characterPositions;
    [Space]
    [SerializeField] private List<string> _tutorialPhrases;

    public void SetPhase(TutorialPhase phase)
    {
        _currentShownCharacter = _characterPositions[(int)phase];
        _currentShownCharacter.SetActive(true);
        _tutorialText.text = _tutorialPhrases[(int)phase];
        
        Show();
    }
    
    [Inject] private GameStateManager _gameStateManager;

    private GameObject _currentShownCharacter;
    private GameState _previousState;

    protected override void Show()
    {
        base.Show();
        _previousState = _gameStateManager.GetCurrentState();
        _gameStateManager.SetScreenShownState();
    }

    protected override void Hide()
    {
        base.Hide();
        _gameStateManager.SetState(_previousState);
        _currentShownCharacter?.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_alphaTweener != null && _alphaTweener.IsPlaying()) return;
        
        Hide();
        OnClick?.Invoke();
    }
}

public enum TutorialPhase
{
    INTRODUCTION_1,
    INTRODUCTION_2,
    INTRODUCTION_3,
    LOCKPICKING,
    TIMER,
    DIFFERENT_LOCKPICKS_1,
    DIFFERENT_LOCKPICKS_2
}
