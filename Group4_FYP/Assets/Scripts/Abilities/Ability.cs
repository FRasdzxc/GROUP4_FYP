using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Ability : ScriptableObject
{
    public string abilityName;
    public HeroClass heroClass;
    public float lifeTime; // seconds until destroy
    public float cooldownTime; // seconds until next use
    public float manaCost;
    public Sprite icon;
    //[HideInInspector] public bool isReady = true;
    [HideInInspector] public float remainingCooldownTime;

    public virtual void Activate(GameObject character) { }

    public async void Cooldown()
    {
        //float interval = 0f;
        remainingCooldownTime = cooldownTime;

        //while (interval < cooldownTime)
        while (remainingCooldownTime > 0f)
        {
            //interval += Time.deltaTime;
            remainingCooldownTime -= Time.deltaTime;
            await Task.Yield();
        }

        remainingCooldownTime = 0f;
        //isReady = true;
    }

    public async void DestroyAfterLifeTime(GameObject gameObject) // destroy any instantiated objects after lifeTime
    {
        float interval = 0f;

        while (interval < lifeTime)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        if (this) // trying to prevent MissingReferenceException
        {
            await gameObject.transform.DOScale(Vector2.zero, 0.25f).AsyncWaitForCompletion();
            Destroy(gameObject);
        }
    }

    public bool IsReady()
    {
        return (remainingCooldownTime == 0);
    }
}
