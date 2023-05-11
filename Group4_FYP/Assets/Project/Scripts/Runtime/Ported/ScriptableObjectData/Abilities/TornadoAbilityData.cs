using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Tornado Ability Data", menuName = "Game/Abilities/Tornado Ability Data")]
public class TornadoAbilityData : Ability
{
    public GameObject tornado;
    public float projectileSpeed;
    public float endScale;
    public float scaleDuration;

    private GameObject projectileClone;

    public override void Activate(GameObject character)
    {        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = character.transform.position.z;
        Vector3 projectDir = (mousePos - character.transform.position).normalized;

        projectileClone = Instantiate(tornado, character.transform.position + projectDir, Quaternion.identity);
        if (projectileClone.TryGetComponent<Projectile>(out var projectile))
        {
            projectile.SelfDestruct = true;
            projectile.SelfDestructTime = Time.time + lifeTime;
        }
        projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
        projectileClone.transform.DOScale(endScale, scaleDuration);

        base.Activate(character);
    }

    protected override void calculateAbilityDamage()
    {
        base.calculateAbilityDamage();

        if (projectileClone.TryGetComponent<WeaponTrigger>(out WeaponTrigger weaponTrigger))
        {
            weaponTrigger.SetDamage(weaponTrigger.GetDamage() + abilityDamageUpgrade);
            weaponTrigger.SetCriticalDamage(weaponTrigger.GetCriticalDamage() + abilityDamageUpgrade);
        }
    }
}
