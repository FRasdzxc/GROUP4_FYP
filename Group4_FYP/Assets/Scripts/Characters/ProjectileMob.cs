using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ProjectileMob : Mob
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject projectile;
    [SerializeField] private ProjectileWeaponData projectileWeaponData;
    private float projectileLifeTime;
    private float projectileSpeed;
    private float cooldown;
    private bool isReady;

    // Start is called before the first frame update
    protected override void Start()
    {
        projectileLifeTime = projectileWeaponData.projectileLifeTime;
        projectileSpeed = projectileWeaponData.projectileSpeed;
        cooldown = projectileWeaponData.cooldown;
        isReady = true;

        base.Start();
    }

    protected override void AttackPlayer()
    {
        if (isReady)
        {
            isReady = false;
            Cooldown();

            Vector2 projectDir = (player.transform.position - transform.position).normalized;
            float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

            GameObject projectileClone = Instantiate(projectile, weapon.transform.position, Quaternion.Euler(0, 0, projectAngle));
            projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
            DestroyGobj(projectileClone);
        }

        base.AttackPlayer();
    }

    public async void Cooldown()
    {
        float interval = 0f;

        while (interval < cooldown)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        isReady = true;
    }

    private async void DestroyGobj(GameObject gameObject)
    {
        float interval = 0f;

        while (interval < projectileLifeTime)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        DestroyImmediate(gameObject);
    }
}
