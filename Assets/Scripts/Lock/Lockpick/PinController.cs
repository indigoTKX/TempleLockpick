using System;
using UnityEngine;
using Zenject;

public class PinController : MonoBehaviour
{
    public event Action OnHacked;
    
    [SerializeField] private float _maxUpSpeed = 3f;
    [SerializeField] private float _retentionAcceleration = 1f;
    [Space]
    [SerializeField] private string _pinLockSound;
    [SerializeField] private GameObject _pinLockSfx;
    [SerializeField] private float _sfxLifeTime = 3f;
    [SerializeField] private string _pinLockBlastSound;
    [Space]
    [SerializeField] private SpriteRenderer _lowPinSprite;
    [SerializeField] private SpriteRenderer _springSprite;

    public void SetMaterial(PinMaterial material)
    {
        _currentMaterial = material;

        // var materialColor = _materialsLibrary.GetColorForMaterial(material);
        // _pinSprite.color = materialColor;
        // _lowPinSprite.color = materialColor;
        // _springSprite.color = materialColor;
        
        var spriteMaterial = _materialsLibrary.GetSpriteMaterial(material);
        _pinSprite.material = spriteMaterial;
        _lowPinSprite.material = spriteMaterial;
        _springSprite.material = spriteMaterial;
    }

    public PinMaterial GetMaterial()
    {
        return _currentMaterial;
    }
    
    public void MoveDown(float speed)
    {
        if (_isLocked)
        {
            return;
        }
        
        _currentSpeed -= speed;
    }

    public void LockPosition()
    {
        _audioManager.PlayAudio(_pinLockSound);
        var sfx = Instantiate(_pinLockSfx, transform.position, Quaternion.identity);
        Destroy(sfx, _sfxLifeTime);
        _audioManager.PlayAudio(_pinLockBlastSound);
        
        _currentSpeed = 0;
        _isLocked = true;
        
        OnHacked?.Invoke();
    }

    public void ResetPosition()
    {
        _isLocked = false;
        transform.localPosition = _initialPosition;
    }

    [Inject] private AudioManager _audioManager;
    [Inject] private MaterialsLibrary _materialsLibrary;

    private PinMaterial _currentMaterial;
    private SpriteRenderer _pinSprite;

    private Vector3 _initialPosition;
    private float _currentSpeed;
    private bool _isLocked;

    private void Awake()
    {
        _initialPosition = transform.localPosition;
        _currentSpeed = _maxUpSpeed;
        _pinSprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        var currentPosition = transform.localPosition;

        if (currentPosition.y >= _initialPosition.y && _currentSpeed >= 0 || _isLocked)
        {
            _currentSpeed = 0;
            return;
        }

        _currentSpeed += _retentionAcceleration;
        if (_currentSpeed > _maxUpSpeed)
        {
            _currentSpeed = _maxUpSpeed;
        }
        
        var targetPosition = currentPosition + Vector3.up * (_currentSpeed * Time.fixedDeltaTime);
        transform.localPosition = targetPosition;
    }
}

public enum PinMaterial
{
    SILVER,
    GOLD,
    SAPHIRE
}
