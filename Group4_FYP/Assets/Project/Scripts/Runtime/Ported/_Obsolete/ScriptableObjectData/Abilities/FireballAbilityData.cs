using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fireball Ability Data", menuName = "Game/Abilities/Fireball Ability Data")]
public class FireballAbilityData : Ability
{
    public Transform fireball;
    public int fireballCount;

    public override async void Activate(GameObject character)
    {
        base.Activate(character);

        float currentAngle = 0;
        for (int i = 0; i < fireballCount; i++)
        {
            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 shootDir = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)).normalized;

            Transform fireballClone = Instantiate(fireball, character.transform.position + shootDir, Quaternion.identity);
            fireballClone.GetComponent<ProjectilesController>().Setup(shootDir);
            currentAngle += 360 / (float)fireballCount;
            await Task.Delay(50);
        }
    }
}
