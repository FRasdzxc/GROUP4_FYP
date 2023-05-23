using System.Threading.Tasks;

public class DungeonMapController : HostileMapController
{
    protected DungeonMapData dungeonMapData;

    protected override void Start()
    {
        dungeonMapData = mapData as DungeonMapData;
        base.Start();
    }

    protected async override Task ShowMapMessage()
        => await HUD.Instance.ShowHugeMessageAsync(dungeonMapData.mapName, $"{dungeonMapData.dungeonType} {dungeonMapData.mapType} / {dungeonMapData.mapDifficulty}");
}
