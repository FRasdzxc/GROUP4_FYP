using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTowerController : MonoBehaviour
{
    // Start is called before the first frame update
    //Animator animator = new Animator();
    [SerializeField] private Animator animator;

   
    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("Trigger"))
        {
            if (animator.GetInteger("Ammo") == 1)
            {
                animator.SetFloat("ReloadTime", 0f);
                animator.SetInteger("Ammo", 0);
            }
            else
            {
                animator.SetFloat("ReloadTime", animator.GetFloat("ReloadTime") + Time.deltaTime);
                if(animator.GetFloat("ReloadTime") >= 1.5f)
                {
                    animator.SetInteger("Ammo", 1);
                }
            }
        }
        else
        {
            animator.SetInteger("Ammo", 1);
            animator.SetFloat("ReloadTime", 0f);
        }
    }
}
