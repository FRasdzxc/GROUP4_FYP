using System.Threading.Tasks;
using UnityEngine;

public class ProjectileMob : Mob
{
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected ProjectileWeaponData projectileWeaponData;
    protected float projectileLifeTime;
    protected float projectileSpeed;
    protected float cooldown;
    protected bool isReady;

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
            AttackMethod();
        }

        base.AttackPlayer();
    }

    protected override void AttackMethod()
    {
        Vector2 projectDir = (player.transform.position - transform.position).normalized;
        float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

        GameObject projectileClone = Instantiate(projectile, weapon.transform.position, projectile.transform.rotation * Quaternion.Euler(0, 0, projectAngle));
        projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
        projectileClone.GetComponent<WeaponTrigger>().SetShootDir(projectDir);
        DestroyGobj(projectileClone);
        base.AttackMethod();
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

    protected async void DestroyGobj(GameObject gameObject)
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
