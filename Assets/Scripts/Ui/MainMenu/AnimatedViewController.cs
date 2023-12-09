using UnityEngine;

public class AnimatedViewController : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);

        if (_animator == null)
        {
            Awake();
        }
        _animator.SetTrigger(SHOW_ANIMATION_UID);
    }

    public void Hide()
    {
        _animator.SetTrigger(HIDE_ANIMATION_UID);
    }

    //called via animation event
    public void HideCompletely()
    {
        gameObject.SetActive(false);
    }

    private const string SHOW_ANIMATION_UID = "Show";
    private const string HIDE_ANIMATION_UID = "Hide";
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
