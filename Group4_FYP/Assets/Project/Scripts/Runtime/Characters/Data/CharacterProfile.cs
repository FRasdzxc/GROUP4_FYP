using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathOfHero.Characters.Data
{
    public enum CharacterClass
    {
        Mage,
        Slime
    }

    public enum MovementType
    {
        None,
        Linear,
        Step
    }

    [CreateAssetMenu(fileName = "New Character Profile", menuName = "Path of Hero/Character/Profile")]
    public class CharacterProfile : ScriptableObject
    {
        public CharacterClass m_Class;

        // Movement
        public MovementType m_MovementType;

        // Linear movement
        public float m_MoveSpeed;
        [Min(1)]
        public float m_SprintFactor;

        // Step movement
        public float m_StepStartDelay;
        public float m_StepDistance;
        public float m_StepTime;

        // Items
        public bool m_CanPickupItems;

        // Stats
        public int m_BaseHealth;
        public float m_DamageEffectTime;

        // AI
        public bool m_AIControllable;
        public float m_DetectionRadius;
        public float m_AttackDistance;

        public CharacterProfileRuntime Build()
        {
            return new CharacterProfileRuntime()
            {
                Class = m_Class,

                MovementType = m_MovementType,
                MoveSpeed = m_MoveSpeed,
                SprintFactor = m_SprintFactor,
                StepStartDelay = m_StepStartDelay,
                StepDistance = m_StepDistance,
                StepTime = m_StepTime,

                CanPickupItems = m_CanPickupItems,

                BaseHealth = m_BaseHealth,
                DamageEffectTime = m_DamageEffectTime,

                DetectionRadius = m_DetectionRadius,
                AttackDistance = m_AttackDistance,
            };
        }
    }

    [Serializable]
    public class CharacterProfileRuntime
    {
        public CharacterClass Class { get; set; }

        public MovementType MovementType { get; set; }
        public float MoveSpeed { get; set; }
        public float SprintFactor { get; set; }
        public float StepStartDelay { get; set; }
        public float StepDistance { get; set; }
        public float StepTime { get; set; }

        public bool CanPickupItems { get; set; }

        public int BaseHealth { get; set; }
        public float DamageEffectTime { get; set; }

        public float DetectionRadius { get; set; }
        public float AttackDistance { get; set; }

        #region Utilitiy
        [NonSerialized]
        private static readonly List<CharacterClass> k_HeroClasses = new()
        {
            CharacterClass.Mage
        };

        [NonSerialized]
        private static readonly List<CharacterClass> k_MobClasses = new()
        {
            CharacterClass.Slime
        };

        public bool IsHero() => k_HeroClasses.Contains(Class);
        public bool IsMob() => k_MobClasses.Contains(Class);
        #endregion
    }
}
