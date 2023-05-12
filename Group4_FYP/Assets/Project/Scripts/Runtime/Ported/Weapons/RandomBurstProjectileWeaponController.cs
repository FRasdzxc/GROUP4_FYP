using System.Threading.Tasks;
using UnityEngine;

public class RandomBurstProjectileWeaponController : WeaponController
{
    [SerializeField]
    protected GameObject[] projectiles;

    protected BurstProjectileWeaponData burstProjectileWeaponData;
    protected float projectileLifeTime;
    protected float projectileSpeed;
    protected Vector2 projectileOffset;
    protected int projectilePerBurst;
    protected float burstCooldown;

    protected override void Start()
    {
        base.Start();

        burstProjectileWeaponData = (BurstProjectileWeaponData)weaponData;
        projectileLifeTime = burstProjectileWeaponData.projectileLifeTime;
        projectileSpeed = burstProjectileWeaponData.projectileSpeed;
        projectileOffset = burstProjectileWeaponData.projectileOffset;
        projectilePerBurst = burstProjectileWeaponData.projectilePerBurst;
        burstCooldown = burstProjectileWeaponData.burstCooldown;
    }

    protected async override void Attack()
    {
        for (int i = 0; i < projectilePerBurst; i++)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;

            var projectPos = transform.position + new Vector3(projectileOffset.x * player.localScale.x, projectileOffset.y * player.localScale.y);
            Vector2 projectDir = (mousePos - projectPos).normalized;
            float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

            GameObject projectile = projectiles[Random.Range(0, projectiles.Length)];
            GameObject projectileClone = Instantiate(projectile, projectPos, Quaternion.Euler(0, 0, projectAngle));
            projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
            Destroy(projectileClone, projectileLifeTime);

            await Task.Delay((int)(burstCooldown * 1000));
        }
    }
}
