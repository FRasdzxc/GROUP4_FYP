using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float _cycleLength = 2;
    [SerializeField] private Vector3 _offset = Vector3.one;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
        transform.DOLocalMove(initialPosition + _offset, _cycleLength).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
