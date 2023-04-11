using PathOfHero.Telemetry;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField]
    protected WeaponData weaponData;

    public int WeaponTier => weaponData.weaponTier;

    protected float nextAttackTime;

    protected virtual void Update()
    {
        if (!GameManager.Instance.IsPlayingHostile())
            return;

        if (Input.GetMouseButton(0) && Time.time >= nextAttackTime)
        {
            Attack(gameObject);
            DataCollector.Instance?.WeaponUsed(weaponData.weaponName);
            nextAttackTime = Time.time + weaponData.cooldown;
        }
    }

    protected abstract void Attack(GameObject weapon);
}
