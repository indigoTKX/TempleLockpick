using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class LockpickAnimationController : MonoBehaviour
{
    public event Action OnPinCdt;
    
    [SerializeField] private float _moveAnimationTime = 0.5f;
    [SerializeField] private float _pickAngle = 10f;
    [SerializeField] private float _pickAnimationTime = 0.3f;
    [SerializeField] private float _changeAnimationTime = 0.5f;
    [Space]
    [SerializeField] private List<Transform> _pinTransforms;

    public void ResetPosition()
    {
        KillTweener();
        _currentPinIndex = 0;
        MoveToCurrentPin(true);
    }
    
    [Inject] private LockpickScreenController _lockpickScreenController;
    [Inject] private InventoryManager _inventoryManager;
    [Inject] private MaterialsLibrary _materialsLibrary;
    
    private Transform _transform;
    private LockpickPinMover _lockpickPinMover;
    private SpriteRenderer _spriteRenderer;
    
    private float _lockpickOffset;
    private Quaternion _initialRotation;
    private Vector3 _offscreenPosition;
    private int _currentPinIndex = 0;
    private PinMaterial _currentLockpickMaterial;

    private Tweener _pickTweener;

    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _lockpickPinMover = GetComponent<LockpickPinMover>();

        _offscreenPosition = transform.position;
        
        _lockpickOffset = _transform.position.x - _pinTransforms[_currentPinIndex].position.x ;
        _initialRotation = _transform.rotation;

        _inventoryManager.OnLockpickChanged += StartChangeAnimation;
    }

    private void OnDestroy()
    {
        _inventoryManager.OnLockpickChanged -= StartChangeAnimation;
        KillTweener();
    }

    private void OnEnable()
    {
        _currentLockpickMaterial = _inventoryManager.GetCurrentLockpick();
        UpdateCurrentLockpick();
        
        _lockpickScreenController.OnMoveLeft += MoveLeft;
        _lockpickScreenController.OnMoveRight += MoveRight;
        _lockpickScreenController.OnPick += StartPickAnimation;

        _lockpickPinMover.OnPinCdt += StartResetAnimation;
    }

    private void OnDisable()
    {
        _lockpickScreenController.OnMoveLeft -= MoveLeft;
        _lockpickScreenController.OnMoveRight -= MoveRight;
        _lockpickScreenController.OnPick -= StartPickAnimation;
        
        _lockpickPinMover.OnPinCdt -= StartResetAnimation;

        KillTweener();
    }

    private void MoveLeft()
    {
        _currentPinIndex -= 1;
        _currentPinIndex = Math.Clamp(_currentPinIndex, 0, _pinTransforms.Count - 1);
        MoveToCurrentPin();
    }

    private void MoveRight()
    {
        _currentPinIndex += 1;
        _currentPinIndex = Math.Clamp(_currentPinIndex, 0, _pinTransforms.Count - 1);
        MoveToCurrentPin();
    }

    private void MoveToCurrentPin(bool instantly = false)
    {
        var targetPosition = _transform.position;
        targetPosition.x = _pinTransforms[_currentPinIndex].position.x + _lockpickOffset;

        if (instantly)
        {
            _transform.position = targetPosition;
        }
        else
        {
            _pickTweener = _transform.DOMove(targetPosition, _moveAnimationTime);
        }
    }
    
    private void StartPickAnimation()
    {
        if (_pickTweener != null && _pickTweener.active) return;
        
        var targetRotation = _transform.eulerAngles;
        targetRotation.z -= _pickAngle;
        _pickTweener = _transform.DORotate(targetRotation, _pickAnimationTime);
        _pickTweener.onComplete += StartResetAnimation;
    }

    private void StartResetAnimation()
    {
        KillTweener();
        OnPinCdt?.Invoke();
        
        _pickTweener = _transform.DORotateQuaternion(_initialRotation, _pickAnimationTime);
    }

    private void StartChangeAnimation(PinMaterial material)
    {
        _currentLockpickMaterial = material;
        
        _pickTweener.Complete();

        _pickTweener = _transform.DOMove(_offscreenPosition, _changeAnimationTime);
        _pickTweener.onComplete += ChangeLockpickAndReset;
    }

    private void ChangeLockpickAndReset()
    {
        UpdateCurrentLockpick();
        
        _pickTweener.onComplete -= ChangeLockpickAndReset;
        MoveToCurrentPin();
    }

    private void UpdateCurrentLockpick()
    {
        // var currentLockpickColor = _materialsLibrary.GetColorForMaterial(_currentLockpickMaterial);
        // _spriteRenderer.color = currentLockpickColor;
        
        var spriteMaterial = _materialsLibrary.GetSpriteMaterial(_currentLockpickMaterial);
        _spriteRenderer.material = spriteMaterial;
    }

    private void KillTweener()
    {
        if (_pickTweener == null) return;
        
        _pickTweener.onComplete -= StartResetAnimation;
        _pickTweener.onComplete -= ChangeLockpickAndReset;
        _pickTweener.Kill();
    }
}
