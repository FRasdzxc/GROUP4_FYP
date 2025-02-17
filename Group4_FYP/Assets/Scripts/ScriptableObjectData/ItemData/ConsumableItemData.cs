using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item Data", menuName = "Game/Items/Consumable Item Data")]
public class ConsumableItemData : ItemData
{
    [Serializable]
    public class Effect
    {
        public EffectType effectType;
        public float value;

        public override string ToString()
        {
            return String.Format("+{0} {1}", value.ToString("n0"), effectType.ToString());
        }
    }

    public Effect[] effects;

    public override void Use()
    {
        base.Use();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Hero hero = player.GetComponent<Hero>();
        AbilityManager abilityManager = player.GetComponent<AbilityManager>();

        foreach (Effect effect in effects)
        {
            if (effect.effectType == EffectType.Health) // health
            {
                hero.AddHealth(effect.value);
            }
            else if (effect.effectType == EffectType.Mana) // mana
            {
                abilityManager.AddMana(effect.value);
            }
            else // exp
            {
                hero.AddEXP((int)effect.value, false);
            }
        }
    }
}
