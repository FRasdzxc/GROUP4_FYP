using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject smoke;
    
    [SerializeField]
    private Color smokeColor = Color.white;

    private bool m_Destroying;

    public bool SelfDestruct { get; set; }
    public float SelfDestructTime { get; set; }

    private void FixedUpdate()
    {
        if (SelfDestruct && Time.time >= SelfDestructTime)
            DestroyGobj();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("HeroWeaponTrigger") && (collision.CompareTag("Object") || collision.CompareTag("Border")))
            DestroyGobj();
        else if (CompareTag("HeroWeaponTriggerStronger") && collision.CompareTag("Border"))
            DestroyGobj();
        else if (CompareTag("MobWeaponTrigger") && (collision.CompareTag("Object") || collision.CompareTag("Player") || collision.CompareTag("Border")))
            DestroyGobj();
        else if (CompareTag("MobWeaponTriggerStronger") && (collision.CompareTag("Player") || collision.CompareTag("Border")))
            DestroyGobj();
    }

    public IEnumerator AnimatedDestroy(float duration)
    {
        if (m_Destroying)
            yield break;

        m_Destroying = true;

        if (smoke)
        {
            GameObject smokeClone = Instantiate(smoke, gameObject.transform.position, Quaternion.identity);
            smokeClone.GetComponent<SpriteRenderer>().color = smokeColor;
        }
            
        yield return transform.DOScale(Vector3.zero, duration).WaitForCompletion();
        Destroy(gameObject);
    }

    public void DestroyGobj()
    {
        if (smoke)
        {
            GameObject smokeClone = Instantiate(smoke, gameObject.transform.position, Quaternion.identity);
            smokeClone.GetComponent<SpriteRenderer>().color = smokeColor;
        }
        Destroy(gameObject);
    }
}
