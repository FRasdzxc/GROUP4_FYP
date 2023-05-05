using UnityEngine;
using DG.Tweening;

public class ExpandingProjectileWeaponController : ProjectileWeaponController
{
    [SerializeField] private float expandSize;
    [SerializeField] private float expandDuration;

    protected override void Attack(GameObject weapon)
    {
        base.Attack(weapon);
        projectileClone.transform.DOScale(new Vector2(expandSize, expandSize), expandDuration);
    }
}
