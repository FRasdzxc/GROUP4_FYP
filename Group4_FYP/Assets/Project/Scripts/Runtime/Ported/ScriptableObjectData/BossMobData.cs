using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Mob Data", menuName = "Game/Boss Mob Data")]
public class BossMobData : MobData // todo: inherit characterdata
{
    [Tooltip("Unit: seconds")]
    public float patternCooldown;
}
