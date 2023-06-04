using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aimed Projectile Attack Pattern", menuName = "Game/Attack Patterns/Aimed Projectile")]
public class AimedProjectileAttackPattern : AttackPattern
{
    public GameObject projectileGobj;

    [Tooltip("Unit: seconds")]
    public float projectileLifeTime;

    public float projectileSpeed;

    [Tooltip("How much Aimed Projectiles when this Attack Pattern invokes.\nX = Min inclusive, Y = Max inclusive.")]
    public Vector2Int randomRepeatCount;

    [Tooltip("Unit: seconds")]
    public float repeatInterval;

    [Tooltip("Unit: seconds\nTime needed before shooting projectiles after shoot angle is locked.")]
    public float aimTime;

    [Tooltip("How long the aiming ray is.")]
    public float aimRayLength;

    public async override Task Invoke(Transform origin)
    {
        LineRenderer lineRenderer = origin.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector2.zero);
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < Random.Range(randomRepeatCount.x, randomRepeatCount.y + 1); i++)
        {
            // calculate the project direction and angle
            Vector2 projectDir = (player.position - origin.position).normalized;
            float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

            // render the line renderer from the origin.position to the projectDir
            float calculatedAimRayLength = aimRayLength / projectDir.magnitude;
            lineRenderer.SetPosition(1, projectDir * calculatedAimRayLength);

            await Task.Delay((int)(aimTime * 1000));

            // clone the projectile and shoot towards the player
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
