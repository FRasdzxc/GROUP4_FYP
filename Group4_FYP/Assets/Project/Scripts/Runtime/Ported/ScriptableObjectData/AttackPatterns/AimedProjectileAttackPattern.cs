using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Aimed Projectile Attack Pattern", menuName = "Game/Attack Patterns/Aimed Projectile")]
public class AimedProjectileAttackPattern : AttackPattern
{
    public GameObject projectileGobj;

    [Tooltip("Unit: seconds")]
    public float projectileLifeTime;

    public float projectileSpeed;

    [Tooltip("How much Aimed Projectiles when this Attack Pattern invokes.")]
    public int repeatCount;

    [Tooltip("Unit: seconds")]
    public float repeatInterval;

    [Tooltip("Unit: seconds\nTime needed before shooting projectiles after shoot angle is locked.")]
    public float aimTime;

    public async override Task Invoke(Transform origin)
    {
        LineRenderer lineRenderer = origin.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, origin.position);
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < repeatCount; i++)
        {
            lineRenderer.SetPosition(1, player.position);

            Vector2 projectDir = (player.position - origin.position).normalized;
            float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

            await Task.Delay((int)(aimTime * 1000));

            GameObject projectileClone = Instantiate(projectileGobj, origin.position, Quaternion.Euler(0, 0, projectAngle));
            if (projectileClone.TryGetComponent<Projectile>(out var projectile))
            {
                projectile.SelfDestruct = true;
                projectile.SelfDestructTime = Time.time + projectileLifeTime;
            }

            projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);

            await Task.Delay((int)(repeatInterval * 1000));
        }

        lineRenderer.positionCount = 0;
    }
}
