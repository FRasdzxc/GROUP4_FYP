using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Soul Ring Ability Data", menuName = "Game/Abilities/Soul Ring Ability Data")]
public class SoulRingAbility : Ability
{
    public GameObject soulRing;
    public float rotationsPerSecond;

    private GameObject soulRingClone;

    public async override void Activate(GameObject character)
    {
        base.Activate(character);

        soulRingClone = Instantiate(soulRing, character.transform.position, Quaternion.identity, character.transform);
        if (soulRingClone.TryGetComponent<Spin>(out var spin))
        {
            spin.SelfDestruct = true;
            spin.SelfDestructTime = Time.time + lifeTime;
            spin.Setup(rotationsPerSecond);
        }
        soulRingClone.transform.localScale = new Vector2(0.5f, 0.5f);
        calculateAbilityDamage();

        await soulRingClone.transform.DOScale(Vector2.one, 0.25f).AsyncWaitForCompletion();
    }

    protected override void calculateAbilityDamage()
    {
        base.calculateAbilityDamage();

        foreach (Transform child in soulRingClone.transform) // loop through the 2 children in soulRingClone gobj
        {
            if (child.TryGetComponent<WeaponTrigger>(out WeaponTrigger weaponTrigger))
            {
                weaponTrigger.SetDamage(weaponTrigger.GetDamage() + abilityDamageUpgrade);
                weaponTrigger.SetCriticalDamage(weaponTrigger.GetCriticalDamage() + abilityDamageUpgrade);
            }
        }
    }
}
