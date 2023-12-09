using System;
using UnityEngine;

public class SpringScaleController : MonoBehaviour
{
    [SerializeField] private Transform _lowPinTransform;

    private Transform _transform;
    private float _scaleHeightFraction;
    
    private void OnEnable()
    {
        _transform = transform;

        var initialScale = _transform.localScale;
        var initialHeight = _lowPinTransform.position.y - _transform.position.y;
        _scaleHeightFraction = initialScale.y / initialHeight;
    }

    private void FixedUpdate()
    {
        var currentHeight = _lowPinTransform.position.y - _transform.position.y;
        
        var targetScale = _transform.localScale;
        targetScale.y = currentHeight * _scaleHeightFraction;
        _transform.localScale = targetScale;
    }
}
