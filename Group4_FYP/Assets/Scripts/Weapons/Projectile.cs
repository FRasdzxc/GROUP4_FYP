using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    private bool m_Destorying;

    public bool SelfDestruct { get; set; }
    public float SelfDestructTime { get; set; }

    private void FixedUpdate()
    {
        if (SelfDestruct && Time.time >= SelfDestructTime)
            StartCoroutine(AnimatedDestroy(0.25f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("HeroWeaponTrigger") && (collision.CompareTag("Object") || collision.CompareTag("Border")))
            StartCoroutine(AnimatedDestroy(0.05f));
        else if (CompareTag("HeroWeaponTriggerStronger") && collision.CompareTag("Border"))
            StartCoroutine(AnimatedDestroy(0.05f));
        else if (CompareTag("MobWeaponTrigger") && (collision.CompareTag("Object") || collision.CompareTag("Player") || collision.CompareTag("Border")))
            StartCoroutine(AnimatedDestroy(0.05f));
    }

    public IEnumerator AnimatedDestroy(float duration)
    {
        if (m_Destorying)
            yield break;

        m_Destorying = true;
        yield return transform.DOScale(Vector3.zero, duration).WaitForCompletion();
        Destroy(gameObject);
    }
}
