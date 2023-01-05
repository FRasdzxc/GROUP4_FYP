using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string abilityName;
    public HeroClass heroClass;
    public float lifeTime; // seconds until destroy
    public float cooldownTime; // seconds until next use
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

    public async void DestroyGobj(GameObject gameObject) // destroy any instantiated objects after lifeTime
    {
        float interval = 0f;

        while (interval < lifeTime)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        DestroyImmediate(gameObject);
    }
}
