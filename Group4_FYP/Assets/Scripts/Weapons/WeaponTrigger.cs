using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used for player/enemy weapon that will collide with player/enemy

public class WeaponTrigger : MonoBehaviour
{
    [SerializeField] private WeaponTriggerData weaponTriggerData;
    [TextArea(5, 5)] [SerializeField] private string notes = "Please assign either \"HeroWeaponTrigger\", \"HeroWeaponTriggerStronger\" or \"MobWeaponTrigger\" tag to the GameObject this script is attached to";

    private float damage;
    private float critialDamage;

    // Start is called before the first frame update
    void Start()
    {
        damage = weaponTriggerData.damage;
        critialDamage = weaponTriggerData.criticalDamage;
    }

    public float GetDamage(bool isCritical)
    {
        if (isCritical)
        {
            return critialDamage;
        }
        
        return damage;
    }
}
