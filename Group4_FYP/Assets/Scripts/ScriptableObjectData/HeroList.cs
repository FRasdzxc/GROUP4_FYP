using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero List", menuName = "Game/Hero List")]
public class HeroList : ScriptableObject
{
    public HeroEntry[] heros;

    public HeroInfo GetHeroInfo(HeroClass type)
    {
        foreach (var hero in heros)
        {
            if (hero.heroClass.Equals(type))
                return hero.heroInfo;
        }

        Debug.LogError($"Missing HeroInfo for class: {type}");
        return null;
    }

    [Serializable]
    public class HeroEntry
    {
        public HeroClass heroClass;
        public HeroInfo heroInfo;
    }
}
