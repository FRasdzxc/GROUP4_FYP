using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotationsPerSecond;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -(Time.deltaTime * 360 * rotationsPerSecond)));
    }

    public void Setup(float rotationsPerSecond)
    {
        this.rotationsPerSecond = rotationsPerSecond;
    }
}
