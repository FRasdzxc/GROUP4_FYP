using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Tornado Ability Data", menuName = "Game/Abilities/Tornado Ability Data")]
public class TornadoAbilityData : Ability
{
    public GameObject tornado;
    public float projectileSpeed;
    public float endScale;
    public float scaleDuration;

    public override void Activate(GameObject character)
    {
        base.Activate(character);
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = character.transform.position.z;
        Vector3 projectDir = (mousePos - character.transform.position).normalized;

        GameObject projectileClone = Instantiate(tornado, character.transform.position + projectDir, Quaternion.identity);
        if (projectileClone.TryGetComponent<Projectile>(out var projectile))
        {
            projectile.SelfDestruct = true;
            projectile.SelfDestructTime = Time.time + lifeTime;
        }
        projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
        projectileClone.transform.DOScale(endScale, scaleDuration);
    }

    protected override void calculateAbilityOutput()
    {
        base.calculateAbilityOutput();

        if (tornado.TryGetComponent<WeaponTrigger>(out WeaponTrigger weaponTrigger))
        {
            weaponTrigger.SetDamage(weaponTrigger.GetDamage() * abilityOutputUpgrade);
            weaponTrigger.SetCriticalDamage(weaponTrigger.GetCriticalDamage() * abilityOutputUpgrade);
        }
        projectileSpeed *= abilityOutputUpgrade;
        endScale *= abilityOutputUpgrade;
    }
}
