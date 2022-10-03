using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCData", menuName = "Game/NPC Data")]
public class NPCData : ScriptableObject
{
    public string Name;
    public int Health;
    public int Mana;
}
