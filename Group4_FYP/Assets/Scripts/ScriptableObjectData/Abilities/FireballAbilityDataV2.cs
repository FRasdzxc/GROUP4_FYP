using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fireball Ability Data V2", menuName = "Game/Abilities/Fireball Ability Data V2")]
public class FireballAbilityDataV2 : Ability
{
    public Transform fireball;
    public float projectileSpeed;
    public int fireballCount;

    public override async void Activate(GameObject character)
    {
        base.Activate(character);

        float currentAngle = 0;
        for (int i = 0; i < fireballCount; i++)
        {
            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 projectDir = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)).normalized;
            float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

            Transform projectileClone = Instantiate(fireball, character.transform.position + projectDir, Quaternion.Euler(0, 0, projectAngle));
            if (projectileClone.TryGetComponent<Projectile>(out var projectile))
            {
                projectile.SelfDestruct = true;
                projectile.SelfDestructTime = Time.time + lifeTime;
            }
            projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);

            currentAngle += 360 / (float)fireballCount;
            await Task.Delay(500 / fireballCount);
        }
    }
}
