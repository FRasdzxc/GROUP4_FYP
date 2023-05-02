using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotationsPerSecond;

    public bool SelfDestruct { get; set; }
    public float SelfDestructTime { get; set; }

    private bool m_Destorying;

    void Update()
    {
        if (m_Destorying)
            return;

        if (SelfDestruct && Time.time >= SelfDestructTime)
        {
            StartCoroutine(AnimatedDestroy(0.25f));
            return;
        }

        transform.Rotate(new Vector3(0, 0, -(Time.deltaTime * 360 * rotationsPerSecond)));
    }

    public void Setup(float rotationsPerSecond)
    {
        this.rotationsPerSecond = rotationsPerSecond;
    }

    private IEnumerator AnimatedDestroy(float duration)
    {
        m_Destorying = true;
        yield return transform.DOScale(Vector3.zero, duration).WaitForCompletion();
        Destroy(gameObject);
    }
}
