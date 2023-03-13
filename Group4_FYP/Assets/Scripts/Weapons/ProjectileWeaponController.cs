using System.Threading.Tasks;
using UnityEngine;

public class ProjectileWeaponController : WeaponController
{
    [SerializeField] protected GameObject projectile;

    private ProjectileWeaponData projectileWeaponData;
    protected float projectileLifeTime;
    protected float projectileSpeed;

    // Start is called before the first frame update
    protected override void Start()
    {
        projectileWeaponData = (ProjectileWeaponData)weaponData;
        projectileLifeTime = projectileWeaponData.projectileLifeTime;
        projectileSpeed = projectileWeaponData.projectileSpeed;

        base.Start();
    }

    protected override void Attack(GameObject weapon)
    {
        if (isReady)
        {
            isReady = false;
            Cooldown();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = weapon.transform.position.z;

            Vector2 projectDir = (mousePos - weapon.transform.position).normalized;
            float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

            GameObject projectileClone = Instantiate(projectile, weapon.transform.position, Quaternion.Euler(0, 0, projectAngle));
            projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
            DestroyGobj(projectileClone);
        }
    }

    protected async void DestroyGobj(GameObject gameObject)
    {
        float interval = 0f;

        while (interval < projectileLifeTime)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        Destroy(gameObject);
    }
}
