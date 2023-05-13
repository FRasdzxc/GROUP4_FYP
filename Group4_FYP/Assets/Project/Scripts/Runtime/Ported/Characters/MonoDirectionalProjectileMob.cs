using UnityEngine;

public class MonoDirectionalProjectileMob : ProjectileMob
{
    [SerializeField]
    protected Vector2 projectDir = Vector2.zero;

    protected override void AttackMethod()
    {
        projectDir.Normalize();
        float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

        GameObject projectileClone = Instantiate(projectile, weapon.transform.position, projectile.transform.rotation * Quaternion.Euler(0, 0, projectAngle));
        projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
        projectileClone.GetComponent<WeaponTrigger>().SetShootDir(projectDir);
        DestroyGobj(projectileClone);
    }
}
