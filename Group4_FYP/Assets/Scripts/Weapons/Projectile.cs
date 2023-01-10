using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        if (CompareTag("HeroWeaponPoint"))
        {
            if (collision.CompareTag("Object") || collision.CompareTag("Border"))
            {
                await transform.DOScale(0, 0.25f).AsyncWaitForCompletion();

                if (this) // trying to prevent MissingReferenceException
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (CompareTag("HeroWeaponPointStronger"))
        {
            if (collision.CompareTag("Border"))
            {
                await transform.DOScale(0, 0.25f).AsyncWaitForCompletion();

                if (this) // trying to prevent MissingReferenceException
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (CompareTag("MobWeaponPoint"))
        {
            if (collision.CompareTag("Object") || collision.CompareTag("Player") || collision.CompareTag("Border"))
            {
                await transform.DOScale(0, 0.25f).AsyncWaitForCompletion();

                if (this) // trying to prevent MissingReferenceException
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
