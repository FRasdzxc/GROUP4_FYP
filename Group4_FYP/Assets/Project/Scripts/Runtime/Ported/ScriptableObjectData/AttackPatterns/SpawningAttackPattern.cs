using System.Threading.Tasks;
using UnityEngine;
using HugeScript;

[CreateAssetMenu(fileName = "New Spawning Attack Pattern", menuName = "Game/Attack Patterns/Spawning")]
public class SpawningAttackPattern : AttackPattern
{
    [Tooltip("One random Mob Table will be chosen each time this Attack Pattern is invoked.")]
    public MobTable[] randomMobTables;

    [Tooltip("Unit: seconds\nWait for seconds after spawning is done.")]
    public float cooldownTime;

    public override async Task Invoke(Transform origin)
    {
        if (GameObject.FindGameObjectWithTag("BossRoomGround").TryGetComponent<MobSpawner>(out MobSpawner mobSpawner))
        {
            mobSpawner.MobTable = randomMobTables[Random.Range(0, randomMobTables.Length)];
            mobSpawner.Spawn();
        }
        
        await Task.Delay((int)(cooldownTime * 1000));
    }
}
