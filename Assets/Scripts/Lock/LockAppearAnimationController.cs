using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class LockAppearAnimationController : MonoBehaviour
{
    public event Action OnShow;
    public event Action OnHide;
    
    [SerializeField] private Vector3 _centerPosition;
    [SerializeField] private float _moveAnimationTime = 1f;

    [Inject] private GameStateManager _gameStateManager;
    private Vector3 _initialPosition;

    private bool _isVisible;
    private Tweener _hideTweener;

    private void Awake()
    {
        _initialPosition = transform.position;

        _gameStateManager.OnStateChanged += HandleGameStateChanged;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= HandleGameStateChanged;
        KillTweener();
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.LOCKPICK)
        {
            Show();
        }else if (!(_isVisible && state == GameState.SCREEN_SHOWN))
        {
            Hide();
        }
    }

    private void Show()
    {
        KillTweener();
        
        gameObject.SetActive(true);
        transform.DOMove(_centerPosition, _moveAnimationTime);

        _isVisible = true;
        OnShow?.Invoke();
    }

    private void Hide()
    {
        KillTweener();
        _hideTweener = transform.DOMove(_initialPosition, _moveAnimationTime);
        _hideTweener.onComplete += HideCompletely;
        
        _isVisible = false;
        OnHide?.Invoke();
    }

    private void HideCompletely()
    {
        gameObject.SetActive(false);
        _hideTweener.onComplete -= HideCompletely;
    }
    
    private void KillTweener()
    {
        if (_hideTweener == null) return;
        
        _hideTweener.onComplete -= HideCompletely;
        _hideTweener.Kill();
    }
}
