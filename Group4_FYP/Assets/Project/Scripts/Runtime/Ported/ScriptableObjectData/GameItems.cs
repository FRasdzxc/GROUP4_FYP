using UnityEngine;

[CreateAssetMenu(fileName = "New Game Items List", menuName = "Game/Game Items List")]
public class GameItems : ScriptableObject
{
    public ItemData[] itemList;
}
