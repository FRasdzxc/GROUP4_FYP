using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class MobProjectile : MonoBehaviour
{
    //[SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Object") || collision.CompareTag("Player"))
        {
            await transform.DOScale(0, 0.25f).AsyncWaitForCompletion();

            if (this) // trying to prevent MissingReferenceException
            {
                Destroy(gameObject);
            }
        }
    }
}
