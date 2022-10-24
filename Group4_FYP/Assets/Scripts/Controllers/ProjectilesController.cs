using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesController : MonoBehaviour
{
    private Vector3 shootDir;
    public static float projectileSpeed;
    //public LayerMask mask;

    public void Setup(Vector3 shootDir)
    {
        Physics2D.IgnoreLayerCollision(3, 6);
        this.shootDir = shootDir;
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        transform.eulerAngles = new Vector3(0, 0, angle);

        shoot(projectileSpeed);
    }

    private void Update()
    {
        //shoot(projectileSpeed);
    }
    public void shoot(float speed)
    {
        GetComponent<Rigidbody2D>().AddForce(shootDir * speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            Destroy(this.gameObject);
        }
    }

}
