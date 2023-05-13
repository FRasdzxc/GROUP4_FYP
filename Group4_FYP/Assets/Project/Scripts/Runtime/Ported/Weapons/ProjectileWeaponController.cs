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

    protected override void Start()
    {
        base.Start();

        projectileWeaponData = (ProjectileWeaponData)weaponData;
        projectileLifeTime = projectileWeaponData.projectileLifeTime;
        projectileSpeed = projectileWeaponData.projectileSpeed;
        projectileOffset = projectileWeaponData.projectileOffset;
    }

    public override void Attack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        var projectPos = transform.position + Vector3.Scale(projectileOffset, player.localScale);
        Vector2 projectDir = (mousePos - projectPos).normalized;
        float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

        projectileClone = Instantiate(projectile, projectPos, Quaternion.Euler(0, 0, projectAngle));
        projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
        Destroy(projectileClone, projectileLifeTime);
    }
}
