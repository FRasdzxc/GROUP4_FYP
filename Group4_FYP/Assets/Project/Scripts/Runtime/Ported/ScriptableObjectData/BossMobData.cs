using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Mob Data", menuName = "Game/Boss Mob Data")]
public class BossMobData : MobData // todo: inherit characterdata
{
    [Header("Boss")]
    public AttackPattern[] attackPatterns;

    [Tooltip("Unit: seconds")]
    public float patternCooldown;
}
