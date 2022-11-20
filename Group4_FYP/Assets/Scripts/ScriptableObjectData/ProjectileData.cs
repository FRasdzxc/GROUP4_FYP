using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public string Name;
    public float Damage;
    public float Magnitude; // the speed the projectile will go
    public float LifeTime; // seconds until destroy
    public Sprite Sprite;
}