using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fireball Ability Data", menuName = "Game/Fireball Ability Data")]
public class FireballAbilityData : Ability
{
    public Transform fireball;
    public int fireballCount;

    public override async void Activate(GameObject character)
    {
        if (isReady)
        {
            isReady = false;
            Debug.Log("fireball ability activated on " + character.name);

            Cooldown();

            float currentAngle = 0;
            for (int i = 0; i < fireballCount; i++)
            {
                float radians = currentAngle * Mathf.Deg2Rad;
                Vector3 shootDir = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)).normalized;

                Transform fireballClone = Instantiate(fireball, character.transform.position + shootDir, Quaternion.identity);
                fireballClone.GetComponent<ProjectilesController>().Setup(shootDir);
                DestroyGobj(fireballClone.gameObject);

                currentAngle += 360 / (float)fireballCount;
                await Task.Delay(50);
            }
        }
        else
        {
            Debug.Log("fireball ability not ready");

            // maybe show some warning on ui
        }
    }
}
