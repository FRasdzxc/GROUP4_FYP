using UnityEngine;

public class MeleeMob : Mob
{
    [SerializeField]
    protected Transform weaponHolder;

    [SerializeField]
    protected WeaponData weaponData;

    protected float nextAttackTime = 0f;

    protected override void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            Instantiate(weaponData.weaponGobj, weaponHolder, false);
            nextAttackTime = Time.time + weaponData.cooldown;
        }

        base.AttackPlayer();
    }
}
