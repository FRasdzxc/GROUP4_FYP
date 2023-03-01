using UnityEngine;

[CreateAssetMenu(fileName = "New Game Maps List", menuName = "Game/Game Maps List")]
public class GameMaps : ScriptableObject
{
    public MapData[] maps;
}
