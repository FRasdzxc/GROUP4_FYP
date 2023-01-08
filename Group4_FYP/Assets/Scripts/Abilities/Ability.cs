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
    [HideInInspector] public bool isReady = true;

    public virtual void Activate(GameObject character) { }

    public async void Cooldown()
    {
        float interval = 0f;

        while (interval < cooldownTime)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        isReady = true;
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
}
