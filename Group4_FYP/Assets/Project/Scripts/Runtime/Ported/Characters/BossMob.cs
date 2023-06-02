using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class BossMob : Mob
{
    protected BossMobData bossMobData;

    protected int lastAttackPatternIndex = -1;

    protected LineRenderer lineRenderer;

    protected Tilemap bossRoomTilemap;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        lineRenderer = GetComponent<LineRenderer>();

        bossMobData = mobData as BossMobData;
        if (bossMobData.attackPatterns.Length <= 0)
        {
            Debug.Log("[Boss Mob] No Attack Patterns found.");
            return;
        }
        StartCoroutine(DoAttackPattern());
    }

    void OnDisable()
        => StopCoroutine(DoAttackPattern());

    protected IEnumerator DoAttackPattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(bossMobData.patternCooldown);

            int random;
            do
                random = Random.Range(0, bossMobData.attackPatterns.Length);
            while (bossMobData.attackPatterns.Length > 1 && random == lastAttackPatternIndex);

            Task task = bossMobData.attackPatterns[random].Invoke(transform);
            yield return new WaitUntil(() => task.IsCompleted);

            lastAttackPatternIndex = random;
        }
    }
}
