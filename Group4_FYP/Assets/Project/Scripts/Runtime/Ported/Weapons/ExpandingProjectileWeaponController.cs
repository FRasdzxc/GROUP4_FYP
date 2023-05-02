using UnityEngine;
using DG.Tweening;

public class ExpandingProjectileWeaponController : ProjectileWeaponController
{
    [SerializeField] private float expandSize;
    [SerializeField] private float expandDuration;

    protected override void Attack(GameObject weapon)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = weapon.transform.position.z;

        Vector2 projectDir = (mousePos - weapon.transform.position).normalized;
        float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

        GameObject projectileClone = Instantiate(projectile, weapon.transform.position, Quaternion.Euler(0, 0, projectAngle));
        projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
        // ExpandGobj(projectileClone);
        projectileClone.transform.DOScale(new Vector2(expandSize, expandSize), expandDuration);
        Destroy(projectileClone, projectileLifeTime);
    }

    // public async void ExpandGobj(GameObject gameObject)
    // {
    //     float interval = 0;

    //     while (interval < expandDuration)
    //     {
    //         interval += Time.deltaTime;
    //         float size = expandSize * (interval / expandDuration);
    //         gameObject.transform.localScale = new Vector2(size, size);

    //         await Task.Yield();
    //     }

    //     gameObject.transform.localScale = new Vector2(expandSize, expandSize); // preventive
    // }
}
