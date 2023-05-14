using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When player enters this trigger, activate the moving platform
public class MovingPlatformTrigger : MonoBehaviour
{
    [SerializeField]
    private MovingPlatform movingPlatform;

    private bool isTriggered;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player") && !isTriggered)
        {
            movingPlatform.ActivateMove();
            isTriggered = true;
        }
    }
}
