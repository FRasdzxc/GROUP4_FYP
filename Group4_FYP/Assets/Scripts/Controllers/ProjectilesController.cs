using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesController : MonoBehaviour
{
    public ProjectileData p_data;
    public float damage;
    public float lifeTime;
    private Vector3 shootDir;
    public static float projectileSpeed;
    //public LayerMask mask;

    public void Setup(Vector3 shootDir)
    {
        Physics2D.IgnoreLayerCollision(3, 6);
        this.shootDir = shootDir;
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        damage = p_data.Damage;
        lifeTime = p_data.LifeTime;
        projectileSpeed = p_data.Magnitude;
        Shoot(projectileSpeed);
    }

    private void Update()
    {
        //shoot(projectileSpeed);
    }
    public void Shoot(float speed)
    {
        GetComponent<Rigidbody2D>().AddForce(shootDir * speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Projectile"))
        {
            Destroy(this.gameObject);
        }
    }
}
