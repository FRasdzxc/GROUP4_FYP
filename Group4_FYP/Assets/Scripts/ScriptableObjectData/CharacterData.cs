using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public float health;
    public float moveSpeed;
    public float sprintMultiplier;
}
