using UnityEngine;
using DG.Tweening;

public class ExpandingProjectileWeaponController : ProjectileWeaponController
{
    [SerializeField] private float expandSize;
    [SerializeField] private float expandDuration;

    protected override void Attack()
    {
        base.Attack();
        projectileClone.transform.DOScale(new Vector2(expandSize, expandSize), expandDuration);
    }
}
