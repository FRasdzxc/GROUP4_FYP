using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestHeroController : MonoBehaviour
{
    [SerializeField] private HeroData hero;
    [SerializeField] private ProjectileWeaponData projectileWeapon;
    [SerializeField] private ProjectileData projectile;   

    private Sprite weaponSprite;
    private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        weaponSprite = projectileWeapon.WeaponPrefab;
        inventory = new Inventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Item"))
        {
            //PickUpItem();
        }
    }

    private void Attack()
    {
        Sprite clone = Instantiate(projectile.Sprite);
        //clone.
    }

    private void PickUpItem(Item item)
    {
        inventory.AddItem(item);

    }

    private void DropItem()
    {

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
