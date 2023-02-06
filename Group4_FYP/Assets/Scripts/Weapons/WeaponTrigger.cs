using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// used for player/enemy weapon that will collide with player/enemy

public class WeaponTrigger : MonoBehaviour
{
    [SerializeField] private WeaponTriggerData weaponTriggerData;
    [TextArea(5, 5)] [SerializeField] private string notes = "Please assign either \"HeroWeaponTrigger\", \"HeroWeaponTriggerStronger\" or \"MobWeaponTrigger\" tag to the GameObject this script is attached to";

    private float damage;
    private float critialDamage;
    private float projectileSpeed;
    private bool splitable;
    private GameObject splitProjectile;
    private int splitAmount;
    private float splitTime;
    private float splitAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        damage = weaponTriggerData.damage;
        critialDamage = weaponTriggerData.criticalDamage;
        projectileSpeed = weaponTriggerData.projectileSpeed;
        splitable = weaponTriggerData.splitable;
        splitProjectile = weaponTriggerData.splitProjectile;
        splitAmount = weaponTriggerData.splitAmount;
        splitTime = weaponTriggerData.splitTime;
    }

    void Update()
    {
        if (splitable)
        {
            Split(splitProjectile);
        }
    }

    public float GetDamage(bool isCritical)
    {
        if (isCritical)
        {
            return critialDamage;
        }
        
        return damage;
    }

    // ***** make a child class script for shoot and split?
    public void Shoot(Vector3 shootDir, float speed) // *****
    {
        GetComponent<Rigidbody2D>().AddForce(shootDir * speed, ForceMode2D.Impulse);
    }

    private async void Split(GameObject projectile)
    {
        float interval = 0f;

        while (interval < splitTime)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        for(int i = 0; i < splitAmount; i++)
        {
            splitAngle += i * (360 / splitAmount);
            float radians = splitAngle * Mathf.Deg2Rad;
            Vector3 shootDir = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)).normalized;
            Instantiate(splitProjectile, gameObject.transform.position, Quaternion.identity);
            Shoot(shootDir, 1); // *****
        }
        Destroy(this.gameObject);
    }
}
