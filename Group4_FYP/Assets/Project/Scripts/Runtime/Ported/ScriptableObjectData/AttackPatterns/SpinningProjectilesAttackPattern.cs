using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spinning Projectiles Attack Pattern", menuName = "Game/Attack Patterns/Spinning Projectiles")]
public class SpinningProjectilesAttackPattern : AttackPattern
{
    public GameObject projectileGobj;

    [Tooltip("How much projectiles will be shot simultaneously.\n Distance between projectiles is evenly spread out.")]
    public int simultaneousProjectileCount;

    [Tooltip("More Angle Count means more projectiles will be shot in total."), Range(1, 360)]
    public int angleCount = 1;

    [Tooltip("Unit: seconds\nTotal duration of the Attack Pattern.")]
    public float spinDuration;

    [Tooltip("Unit: seconds")]
    public float projectileLifeTime;

    public float projectileSpeed;

    public bool isCounterClockwise;

    protected List<AngleEntry> angleEntries;

    public async override Task Invoke(Transform origin)
    {
        SetUp();

        for (int i = 0; i < angleCount; i++)
        {
            foreach (AngleEntry ae in angleEntries)
            {
                float radians = ae.currentAngle * Mathf.Deg2Rad;
                Vector3 projectDir = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)).normalized;
                float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

                GameObject projectileClone = Instantiate(projectileGobj, origin.position + projectDir, Quaternion.Euler(0, 0, projectAngle));
                if (projectileClone.TryGetComponent<Projectile>(out var projectile))
                {
                    projectile.SelfDestruct = true;
                    projectile.SelfDestructTime = Time.time + projectileLifeTime;
                }
                projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);

                ae.currentAngle += 360 / angleCount * (isCounterClockwise ? -1 : 1);
            }

            await Task.Delay((int)(spinDuration / angleCount * 1000));
        }
    }

    protected void SetUp()
    {
        angleEntries = new List<AngleEntry>();
        for (int i = 0; i < simultaneousProjectileCount; i++)
            angleEntries.Add(new AngleEntry(i * (360 / simultaneousProjectileCount)));
    }
}

public class AngleEntry
{
    public AngleEntry(float angle)
        => currentAngle = angle;

    public float currentAngle;
}
