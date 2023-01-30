using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LaserMob : ProjectileMob
{
    [SerializeField] private GameObject defaultLaser;
    [SerializeField] private GameObject bigLaserBall;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    protected override void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        base.Start();
    }
    protected override void AttackPlayer()
    {
        FacingPlayer();
        if (isReady)
        {
            isReady = false;
            Cooldown();
            AttackMethod();
        }
        base.AttackPlayer();
    }

    protected override void ChasePlayer()
    {
        FacingPlayer();
        base.ChasePlayer();
    }

    protected override void AttackMethod()
    {
        Vector2 projectDir = (player.transform.position - transform.position).normalized;
        float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

        GameObject projectileClone = Instantiate(projectile, weapon.transform.position, projectile.transform.rotation * Quaternion.Euler(0, 0, projectAngle));
        projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
        DestroyGobj(projectileClone);
        base.AttackMethod();
    }


    void FacingPlayer()
    {
        Vector2 projectDir = (player.transform.position - transform.position).normalized;
        float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, projectAngle+45);
    }
}
