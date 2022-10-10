using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasingModes : MonoBehaviour
{
    public float EaseInQuad(float value)
    {
        return value * value; 
    }

    public float EaseOutQuad(float value)
    {
        return 1 - (1 - value) * (1 - value);
    }
}
