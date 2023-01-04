using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "Game/Skill Data")]
public class AbilityData : ScriptableObject
{
    public string SkillName;
    public HeroClass HeroClass;
    public float Cooldown;
}
