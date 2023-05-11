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
    private float force;
    private bool splitable;
    private GameObject splitProjectile;
    private GameObject projectile;
    private int splitAmount;
    private float splitProjectileSpeed;
    private float splitTime;
    private float splitAngle = 0;
    private Vector3 shootDir;

    void Awake()
    {
        damage = weaponTriggerData.damage;
        critialDamage = weaponTriggerData.criticalDamage;
        projectileSpeed = weaponTriggerData.projectileSpeed;
        force = weaponTriggerData.force;
        splitable = weaponTriggerData.splitable;
        splitProjectile = weaponTriggerData.splitProjectile;
        splitAmount = weaponTriggerData.splitAmount;
        splitProjectileSpeed = weaponTriggerData.splitProjectileSpeed;
        splitTime = weaponTriggerData.splitTime;
    }

    void Update()
    {
        if (splitable && this)
        {
            Split(splitProjectile);
        }
    }

    public float GetDamage(bool isCritical)
    {
        if (isCritical)
            return critialDamage;
        
        return damage;
    }

    public void SetShootDir(Vector2 dir)
    {
        shootDir = dir;
    }

    public void push(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().AddForce(shootDir * force, ForceMode2D.Force);
    }

    // ***** make a child class script for shoot and split?
    public void Shoot(Vector3 shootDir, float speed, GameObject x) // *****
    {
        x.GetComponent<Rigidbody2D>().AddForce(shootDir * speed, ForceMode2D.Impulse);
    }

    private async void Split(GameObject projectile)
    {
        splitAngle = 0;
        float interval = 0f;

        while (interval < splitTime)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        for(int i = 0; i < splitAmount; i++)
        {
            splitAngle = i * (360 / splitAmount);
            float radians = splitAngle * Mathf.Deg2Rad;
            shootDir = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)).normalized;
            projectile = Instantiate(splitProjectile, gameObject.transform.position, Quaternion.identity);
            Shoot(shootDir, splitProjectileSpeed, projectile); // *****
        }
        Destroy(this.gameObject);
    }

    #region Setters/Getters
    //
    public void SetDamage(float value)
    {
        damage = value;
    }

    public void SetCriticalDamage(float value)
    {
        critialDamage = value;
    }

    // Getters
    public float GetDamage()
    {
        return damage;
    }

    public float GetCriticalDamage()
    {
        return critialDamage;
    }
    #endregion
}
