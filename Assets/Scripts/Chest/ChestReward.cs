using UnityEngine;
using Zenject;

public class ChestReward : MonoBehaviour
{
    [SerializeField] private GameObject _itemSfx;
    [SerializeField] private GameObject _pickupSfx;
    [SerializeField] private float _pickUpSfxLifeTime = 5f;
    [Space]
    [SerializeField] private string _pickupSoundUid = "PickupItem";

    [Inject] private GameStateManager _gameStateManager;
    [Inject] private AudioManager _audioManager;

    protected virtual void Awake()
    {
        Instantiate(_itemSfx, transform);
    }

    private void OnMouseDown()
    {
        if (_gameStateManager.GetCurrentState() != GameState.TEMPLE) return;
        
        _audioManager.PlayAudio(_pickupSoundUid);
        var sfx = Instantiate(_pickupSfx, transform.position, Quaternion.identity);
        Destroy(sfx, _pickUpSfxLifeTime);
        
        Destroy(gameObject);

        HandleOnClick();
    }

    protected virtual void HandleOnClick()
    {
        
    }
}
