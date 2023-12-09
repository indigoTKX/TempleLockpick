using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UiTimerSliderController : MonoBehaviour
{
    private Slider _slider;

    [Inject] private TimerManager _timerManager;

    private float _maxTime;

    private void Awake()
    {
        _slider = GetComponent<Slider>();

        _maxTime = _timerManager.GetMaxTime();
    }

    private void Update()
    {
        var newValue = _timerManager.GetCurrentTime() / _maxTime;
        _slider.DOValue(newValue, Time.deltaTime);
    }
}
