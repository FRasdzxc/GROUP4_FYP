using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMob : Mob
{
    [SerializeField]
    protected WeaponController[] weapons;

    protected BossMobData bossMobData;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        bossMobData = mobData as BossMobData;

        if (weapons.Length <= 0)
        {
            Debug.Log("[BossMob]: no weapons found");
            return;
        }

        StartCoroutine(AttackPattern());
    }

    private IEnumerator AttackPattern()
    {
        while (true)
        {
            foreach (WeaponController weapon in weapons)
            {
                weapon.Attack();
                yield return new WaitForSeconds(bossMobData.patternCooldown);
            }
        }
    }
}
