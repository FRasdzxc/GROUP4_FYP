using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Data", menuName = "Game/Hero Data")]
public class HeroData : ScriptableObject // todo: inherit characterdata
{
    public float health;
    public float healthRegeneration;
    public float mana;
    public float manaRegeneration;
    public float attackSpeed;
    // public float luck;
    [Tooltip("Value must be 1 or above")] public float defense;
    public float walkspeed;
}
