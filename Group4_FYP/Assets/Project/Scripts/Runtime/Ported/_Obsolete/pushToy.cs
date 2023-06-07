using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class pushToy : getTozTip
{
    public void OnPush(GameObject obj)
    {
        Debug.Log($"OnPush: {obj.name}");
    }

    
}
