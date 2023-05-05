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

    protected void Start()
    {
        burstProjectileWeaponData = (BurstProjectileWeaponData)weaponData;
        projectileLifeTime = burstProjectileWeaponData.projectileLifeTime;
        projectileSpeed = burstProjectileWeaponData.projectileSpeed;
        projectileOffset = burstProjectileWeaponData.projectileOffset;
        projectilePerBurst = burstProjectileWeaponData.projectilePerBurst;
        burstCooldown = burstProjectileWeaponData.burstCooldown;
    }

    protected async override void Attack(GameObject weapon)
    {
        for (int i = 0; i < projectilePerBurst; i++)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = weapon.transform.position.z;

            Vector2 projectDir = (mousePos - (weapon.transform.position + new Vector3(projectileOffset.x, projectileOffset.y))).normalized;
            float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

            GameObject projectile = projectiles[Random.Range(0, projectiles.Length)];
            GameObject projectileClone = Instantiate(projectile, weapon.transform.position + new Vector3(projectileOffset.x, projectileOffset.y), Quaternion.Euler(0, 0, projectAngle));
            projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
            Destroy(projectileClone, projectileLifeTime);

            await Task.Delay((int)(burstCooldown * 1000));
        }
    }
}
