using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string abilityName;
    public HeroClass heroClass;
    public float lifeTime;
    public float cooldownTime;
    [HideInInspector] public AbilityState abilityState = AbilityState.ready;

    public virtual void Activate(GameObject character) { }
    public virtual void Cooldown() { }
}
