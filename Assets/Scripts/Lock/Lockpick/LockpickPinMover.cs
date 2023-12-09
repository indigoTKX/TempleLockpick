using System;
using UnityEngine;
using Zenject;

public class LockpickPinMover : MonoBehaviour
{
    public event Action OnPinCdt;

    [SerializeField] private float _pinSpeed = 5f;
    [Space]
    [SerializeField] private string _pickSoundUid;
    [SerializeField] private string _wrongPickSoundUid;
    [Space]
    [SerializeField] private GameObject _wrongPickSfxPrefab;
    [SerializeField] private float _sfxLifeTime = 3f;

    [Inject] private AudioManager _audioManager;
    [Inject] private InventoryManager _inventoryManager;
    
    private PinMaterial _material;

    private void Awake()
    {
        _inventoryManager.OnLockpickChanged += HandleMaterialChanged;
    }

    private void OnDestroy()
    {
        _inventoryManager.OnLockpickChanged -= HandleMaterialChanged;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var pinController = col.gameObject.GetComponent<PinController>();
        if (pinController == null) return;

        OnPinCdt?.Invoke();
        
        if (pinController.GetMaterial() != _material)
        {
            _audioManager.PlayAudio(_wrongPickSoundUid);
            var contactPosition = col.GetContact(0).point;
            var contactPosition3d = new Vector3(contactPosition.x, contactPosition.y, transform.position.z);
            var sfx = Instantiate(_wrongPickSfxPrefab, contactPosition3d, Quaternion.identity);
            Destroy(sfx, _sfxLifeTime);
            return;
        }
        
        _audioManager.PlayAudio(_pickSoundUid);
        pinController.MoveDown(_pinSpeed);
    }

    private void HandleMaterialChanged(PinMaterial material)
    {
        _material = material;
    }
}
