using PathOfHero.Telemetry;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField]
    protected WeaponData weaponData;

    public int WeaponTier => weaponData.weaponTier;

    protected float nextAttackTime;

    protected Transform player;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        if (!GameManager.Instance.IsPlayingHostile())
            return;

        if (Input.GetMouseButton(0) && Time.time >= nextAttackTime)
        {
            Attack();
            //DataCollector.Instance?.WeaponUsed(weaponData.item?.itemID);
            nextAttackTime = Time.time + weaponData.cooldown;
        }
    }

    protected abstract void Attack();
}
