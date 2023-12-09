using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class FadeableScreenController : MonoBehaviour
{
    [SerializeField] private float _fadeAnimationTime = 1f;
    
    private CanvasGroup _canvasGroup;
    protected Tweener _alphaTweener;
    protected bool _isHiding;
    
    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }

    protected virtual void OnDestroy()
    {
        KillTweener();
    }

    protected virtual void Show()
    {
        KillTweener();
        
        gameObject.SetActive(true);
        _alphaTweener = _canvasGroup.DOFade(1f,_fadeAnimationTime);
    }

    protected virtual void Hide()
    {
        KillTweener();

        _isHiding = true;
        _alphaTweener = _canvasGroup.DOFade(0f,_fadeAnimationTime);
        _alphaTweener.onComplete += HideCompletely;
    }
    
    
    protected void HideCompletely()
    {
        _isHiding = false;
        gameObject.SetActive(false);
        KillTweener();
    }

    private void KillTweener()
    {
        if (_alphaTweener == null) return;
        
        _alphaTweener.onComplete -= HideCompletely;
        
        _alphaTweener.Kill();
        _alphaTweener = null;
    }
}
