using UnityEngine;

public class ProjectileWeaponController : WeaponController
{
    [SerializeField]
    protected GameObject projectile;

    protected ProjectileWeaponData projectileWeaponData;
    protected float projectileLifeTime;
    protected float projectileSpeed;

    protected void Start()
    {
        projectileWeaponData = (ProjectileWeaponData)weaponData;
        projectileLifeTime = projectileWeaponData.projectileLifeTime;
        projectileSpeed = projectileWeaponData.projectileSpeed;
    }

    protected override void Attack(GameObject weapon)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = weapon.transform.position.z;

        Vector2 projectDir = (mousePos - weapon.transform.position).normalized;
        float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

        GameObject projectileClone = Instantiate(projectile, weapon.transform.position, Quaternion.Euler(0, 0, projectAngle));
        projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
        Destroy(projectileClone, projectileLifeTime);
    }
}
