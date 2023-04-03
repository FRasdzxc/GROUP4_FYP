using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    //[SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("HeroWeaponTrigger"))
        {
            if (collision.CompareTag("Object") || collision.CompareTag("Border"))
            {
                //await transform.DOScale(0, 0.25f).AsyncWaitForCompletion();
                await transform.DOScale(0, 0.05f).AsyncWaitForCompletion();

                if (this) // trying to prevent MissingReferenceException
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (CompareTag("HeroWeaponTriggerStronger"))
        {
            if (collision.CompareTag("Border"))
            {
                //await transform.DOScale(0, 0.25f).AsyncWaitForCompletion();
                await transform.DOScale(0, 0.05f).AsyncWaitForCompletion();

                if (this) // trying to prevent MissingReferenceException
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (CompareTag("MobWeaponTrigger"))
        {
            if (collision.CompareTag("Object") || collision.CompareTag("Player") || collision.CompareTag("Border"))
            {
                //await transform.DOScale(0, 0.25f).AsyncWaitForCompletion();
                await transform.DOScale(0, 0.05f).AsyncWaitForCompletion();

                if (this) // trying to prevent MissingReferenceException
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
