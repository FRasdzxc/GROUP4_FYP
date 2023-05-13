using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LaserMob : MonoDirectionalProjectileMob
{
    [SerializeField] private GameObject defaultLaser;
    [SerializeField] private GameObject bigLaserBall;
    [SerializeField] protected GameObject UI;
    private GameObject self;
    private Quaternion UIRotation;
    private Vector3 UIPosition;
    private int attackCounter = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        UI = GetComponentInChildren<Canvas>().gameObject;
        UIRotation = UI.transform.rotation;
        UIPosition = UI.transform.localPosition;
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
        if(attackCounter < 2)
        {
            GameObject projectileClone = Instantiate(defaultLaser, weapon.transform.position, defaultLaser.transform.rotation * Quaternion.Euler(0, 0, projectAngle));
            projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
            projectileClone.GetComponent<WeaponTrigger>().SetShootDir(projectDir);
            DestroyGobj(projectileClone);
            attackCounter++;
        }
        else
        {
            GameObject projectileClone = Instantiate(bigLaserBall, weapon.transform.position, bigLaserBall.transform.rotation * Quaternion.Euler(0, 0, projectAngle));
            projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
            projectileClone.GetComponent<WeaponTrigger>().SetShootDir(projectDir);
            DestroyGobj(projectileClone);
            attackCounter = 0;
        }

        //base.AttackMethod();
    }


    void FacingPlayer()
    {
        Vector2 projectDir = (player.transform.position - transform.position).normalized;
        float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, projectAngle+45);
        UI.transform.rotation = UIRotation;
        UI.transform.position = transform.position + UIPosition;
    }
}
