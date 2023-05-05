using UnityEngine;

public class ProjectileWeaponController : WeaponController
{
    [SerializeField]
    protected GameObject projectile;

    protected ProjectileWeaponData projectileWeaponData;
    protected float projectileLifeTime;
    protected float projectileSpeed;
    protected Vector2 projectileOffset;
    protected GameObject projectileClone;

    protected void Start()
    {
        projectileWeaponData = (ProjectileWeaponData)weaponData;
        projectileLifeTime = projectileWeaponData.projectileLifeTime;
        projectileSpeed = projectileWeaponData.projectileSpeed;
        projectileOffset = projectileWeaponData.projectileOffset;
    }

    protected override void Attack(GameObject weapon)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = weapon.transform.position.z;

        Vector2 projectDir = (mousePos - (weapon.transform.position + new Vector3(projectileOffset.x, projectileOffset.y))).normalized;
        float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

        projectileClone = Instantiate(projectile, weapon.transform.position + new Vector3(projectileOffset.x, projectileOffset.y), Quaternion.Euler(0, 0, projectAngle));
        projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
        Destroy(projectileClone, projectileLifeTime);
    }
}
