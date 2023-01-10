using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Soul Ring Ability Data", menuName = "Game/Abilities/Soul Ring Ability Data")]
public class SoulRingAbility : Ability
{
    public GameObject soulRing;
    public float rotationsPerSecond;

    public override async void Activate(GameObject character)
    {
        //if (isReady)
        if (IsReady())
        {
            //isReady = false;
            Debug.Log("soul ring ability activated on " + character.name);

            Cooldown();

            GameObject soulRingClone = Instantiate(soulRing, character.transform.position, Quaternion.identity, character.transform);
            soulRingClone.GetComponent<Spin>().Setup(rotationsPerSecond);
            soulRingClone.transform.localScale = new Vector2(0.5f, 0.5f);
            await soulRingClone.transform.DOScale(Vector2.one, 0.25f).AsyncWaitForCompletion();
            DestroyAfterLifeTime(soulRingClone);
        }
        else
        {
            Debug.Log("soul ring ability not ready");

            // maybe show some warning on ui
        }
    }
}
