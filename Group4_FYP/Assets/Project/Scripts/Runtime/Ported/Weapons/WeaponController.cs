using UnityEngine;
using PathOfHero.Managers.Data;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField]
    protected WeaponData weaponData;

    [SerializeField]
    protected ScoreEventChannel m_ScoreEventChannel;

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
            m_ScoreEventChannel.WeaponUsed(weaponData.item?.itemID);
            nextAttackTime = Time.time + weaponData.cooldown;
        }
    }

    public abstract void Attack();
}
