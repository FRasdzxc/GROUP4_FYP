using UnityEngine;
using UnityEngine.Events;

namespace PathOfHero.Managers.Data
{
    [CreateAssetMenu(fileName = "NewScoreEventChannel", menuName = "Path of Hero/Score Event Channel")]
    public class ScoreEventChannel : ScriptableObject
    {
        public UnityAction<string> OnLevelStart;
        public UnityAction OnLevelEnd;

        public UnityAction OnStepTaken;
        public UnityAction<string> OnWeaponUsed;
        public UnityAction<string> OnAbilityUsed;
        public UnityAction<float> OnDamageTaken;
        public UnityAction<float> OnDamageGiven;
        public UnityAction<string> OnMobKilled;

        public void LevelStarted(string mapId) => OnLevelStart?.Invoke(mapId);
        public void LevelEnded() => OnLevelEnd?.Invoke();
        public void StepTaken() => OnStepTaken?.Invoke();
        public void WeaponUsed(string weaponId) => OnWeaponUsed?.Invoke(weaponId);
        public void AbilityUsed(string abilityId) => OnAbilityUsed?.Invoke(abilityId);
        public void DamageTaken(float amount) => OnDamageTaken?.Invoke(amount);
        public void DamageGiven(float amount) => OnDamageGiven?.Invoke(amount);
        public void MobKilled(string modId) => OnMobKilled?.Invoke(modId);
    }
}
