using System;
using UnityEngine;

public class PinHolder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        var pinController = col.GetComponent<PinController>();
        if (pinController == null)
        {
            return;
        }
        
        pinController.LockPosition();
    }
}
