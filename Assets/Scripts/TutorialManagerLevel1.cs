using System;
using System.Collections;
using UnityEngine;

public class TutorialManagerLevel1 : MonoBehaviour
{
    [SerializeField] private TutorialScreenController _tutorialScreen;
    [SerializeField] private ChestController _chestController;
    [Space]
    [SerializeField] private float _timeWaitForTimerHint = 5f;
    
    private TutorialPhase _currentPhase;
    
    private void Start()
    {
        _currentPhase = TutorialPhase.INTRODUCTION_1;
        _tutorialScreen.SetPhase(_currentPhase);
        _tutorialScreen.OnClick += SetNextIntroPhase;
    }

    private void OnDestroy()
    {
        _tutorialScreen.OnClick -= SetNextIntroPhase;
        _chestController.OnLockShown -= SetLockpickPhase;
        _tutorialScreen.OnClick -= StartWaitingCoroutine;
        StopAllCoroutines();
    }

    private void SetNextIntroPhase()
    {
        _currentPhase++;
        _tutorialScreen.SetPhase(_currentPhase);

        if (_currentPhase == TutorialPhase.INTRODUCTION_3)
        {
            _tutorialScreen.OnClick -= SetNextIntroPhase;
            _chestController.OnLockShown += SetLockpickPhase;
        }
    }

    private void SetLockpickPhase()
    {
        _chestController.OnLockShown -= SetLockpickPhase;
        _currentPhase++;
        _tutorialScreen.SetPhase(_currentPhase);
        _tutorialScreen.OnClick += StartWaitingCoroutine;
    }

    private void StartWaitingCoroutine()
    {
        _tutorialScreen.OnClick -= StartWaitingCoroutine;
        StartCoroutine(WaitForTimerHint());
    }
    
    private IEnumerator WaitForTimerHint()
    {
        yield return new WaitForSeconds(_timeWaitForTimerHint);    
        _currentPhase++;
        _tutorialScreen.SetPhase(_currentPhase);
    }
}
