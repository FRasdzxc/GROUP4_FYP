using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Soul Ring Ability Data", menuName = "Game/Abilities/Soul Ring Ability Data")]
public class SoulRingAbility : Ability
{
    public GameObject soulRing;
    public float rotationsPerSecond;

    public async override void Activate(GameObject character)
    {
        base.Activate(character);
        
        GameObject soulRingClone = Instantiate(soulRing, character.transform.position, Quaternion.identity, character.transform);
        if (soulRingClone.TryGetComponent<Spin>(out var spin))
        {
            spin.SelfDestruct = true;
            spin.SelfDestructTime = Time.time + lifeTime;
            spin.Setup(rotationsPerSecond);
        }
        soulRingClone.transform.localScale = new Vector2(0.5f, 0.5f);
        await soulRingClone.transform.DOScale(Vector2.one, 0.25f).AsyncWaitForCompletion();
    }
}
