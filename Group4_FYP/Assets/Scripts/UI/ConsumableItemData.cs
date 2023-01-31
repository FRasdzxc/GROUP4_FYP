using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item Data", menuName = "Game/Consumable Item Data")]
public class ConsumableItemData : ItemData
{
    [Serializable]
    public class Effect
    {
        public EffectType effectType;
        public float value;
    }

    public Effect[] effects;

    public override void Use()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Hero hero = player.GetComponent<Hero>();
        AbilityManager abilityManager = player.GetComponent<AbilityManager>();

        foreach (Effect effect in effects)
        {
            if (effect.effectType == EffectType.Health) // health
            {
                hero.ChangeHealth(effect.value);
            }
            else if (effect.effectType == EffectType.Mana) // mana
            {
                abilityManager.ChangeMana(effect.value);
            }
            else // exp
            {
                hero.AddEXP((int)effect.value);
            }
        }
    }
}
