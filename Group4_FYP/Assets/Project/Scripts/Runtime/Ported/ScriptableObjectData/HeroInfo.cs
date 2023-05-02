using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Info", menuName = "Game/Hero Info")]
public class HeroInfo : ScriptableObject
{
    public string heroName;
    public string heroDesc;
    public Sprite heroSprite;
    public HeroData defaultStats;
}
