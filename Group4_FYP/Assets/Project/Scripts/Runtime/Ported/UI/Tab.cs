using UnityEngine;

public abstract class Tab : MonoBehaviour
{
    void OnEnable()
        => PrepareTab();

    protected abstract void PrepareTab();
}
