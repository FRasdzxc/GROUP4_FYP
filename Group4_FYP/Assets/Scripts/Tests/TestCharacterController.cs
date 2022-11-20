using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestCharacterController : MonoBehaviour
{
    [SerializeField] private HeroData hero;
    [SerializeField] private ProjectileWeaponData projectileWeapon;
    [SerializeField] private ProjectileData projectile;   

    private Sprite weaponSprite;

    // Start is called before the first frame update
    void Start()
    {
        weaponSprite = projectileWeapon.WeaponPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Sprite clone = Instantiate(projectile.Sprite);
        //clone.
    }

    private async Task DestroyAfterSeconds(float seconds, GameObject gameObject)
    {
        float elapsedTime = 0f;

        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;

            await Task.Yield();
        }

        DestroyImmediate(gameObject);
    }
}
