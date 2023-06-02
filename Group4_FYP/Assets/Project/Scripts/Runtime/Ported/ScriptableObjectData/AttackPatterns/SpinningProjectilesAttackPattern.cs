using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spinning Projectiles Attack Pattern", menuName = "Game/Attack Patterns/Spinning Projectiles")]
public class SpinningProjectilesAttackPattern : AttackPattern
{
    [Tooltip("One random Projectile GameObject will be chosen each time this Attack Pattern is invoked.")]
    public GameObject[] randomProjectileGobjs;

    [Tooltip("How much projectiles will be shot simultaneously.\n Distance between projectiles is evenly spread out.\nX = Min inclusive, Y = Max inclusive.")]
    public Vector2Int randomSimultaneousProjectileCount;

    [Tooltip("More Angle Count means more projectiles will be shot in total."), Range(1, 360)]
    public int angleCount = 1;

    [Tooltip("Unit: seconds\nTotal duration of the Attack Pattern.")]
    public float spinDuration;

    [Tooltip("Unit: seconds")]
    public float projectileLifeTime;

    public float projectileSpeed;

    public SpinningMode spinningMode;

    protected List<AngleEntry> angleEntries;

    protected SpinningMode randomizedSpinningMode;

    public async override Task Invoke(Transform origin)
    {
        SetUp();
        GameObject randomizedProjectileGobj = randomProjectileGobjs[Random.Range(0, randomProjectileGobjs.Length)];

        for (int i = 0; i < angleCount; i++)
        {
            foreach (AngleEntry ae in angleEntries)
            {
                float radians = ae.currentAngle * Mathf.Deg2Rad;
                Vector3 projectDir = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)).normalized;
                float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

                GameObject projectileClone = Instantiate(randomizedProjectileGobj, origin.position + projectDir, Quaternion.Euler(0, 0, projectAngle));
                if (projectileClone.TryGetComponent<Projectile>(out var projectile))
                {
                    projectile.SelfDestruct = true;
                    projectile.SelfDestructTime = Time.time + projectileLifeTime;
                }
                projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);

                ae.currentAngle += 360 / angleCount * (randomizedSpinningMode.Equals(SpinningMode.Clockwise) ? 1 : -1);
            }

            await Task.Delay((int)(spinDuration / angleCount * 1000));
        }
    }

    protected void SetUp()
    {
        if (spinningMode.Equals(SpinningMode.Random))
            randomizedSpinningMode = Random.Range(0, 2) == 0 ? SpinningMode.Clockwise : SpinningMode.CounterClockwise;
        else
            randomizedSpinningMode = spinningMode;

        int randomizedSimultaneousProjectileCount =
                Random.Range(randomSimultaneousProjectileCount.x, randomSimultaneousProjectileCount.y + 1);

        angleEntries = new List<AngleEntry>();
        for (int i = 0; i < randomizedSimultaneousProjectileCount; i++)
            angleEntries.Add(new AngleEntry(i * (360 / randomizedSimultaneousProjectileCount)));
    }
}

public class AngleEntry
{
    public AngleEntry(float angle)
        => currentAngle = angle;

    public float currentAngle;
}

public enum SpinningMode
{
    Clockwise,
    CounterClockwise,
    Random
}