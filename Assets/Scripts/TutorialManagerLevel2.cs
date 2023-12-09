using UnityEngine;
using Zenject;

public class TutorialManagerLevel2 : MonoBehaviour
{
    [SerializeField] private TutorialScreenController _tutorialScreen;
    [SerializeField] private ChestController _chestController;

    [Inject] private LockController _lockController;
    
    private TutorialPhase _currentPhase;

    private void Awake()
    {
        _chestController.OnLockShown += SubscribeOnPinCdt;
    }

    private void OnDestroy()
    {
        _chestController.OnLockShown -= SubscribeOnPinCdt;
        _lockController.OnPinCdt -= SetDifferentLocksState;
        _tutorialScreen.OnClick -= SetNextDifferentLocksState;
    }

    private void SubscribeOnPinCdt()
    {
        _chestController.OnLockShown -= SubscribeOnPinCdt;
        _lockController.OnPinCdt += SetDifferentLocksState;
    }
    
    private void SetDifferentLocksState()
    {
        _lockController.OnPinCdt -= SetDifferentLocksState;
        _tutorialScreen.SetPhase(TutorialPhase.DIFFERENT_LOCKPICKS_1);
        _tutorialScreen.OnClick += SetNextDifferentLocksState;
    }

    private void SetNextDifferentLocksState()
    {
        _tutorialScreen.OnClick -= SetNextDifferentLocksState;
        _tutorialScreen.SetPhase(TutorialPhase.DIFFERENT_LOCKPICKS_2);
    }
}
