using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    private float cooldown;
    protected bool isReady;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        cooldown = weaponData.cooldown;
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack(gameObject);
        }
    }

    protected virtual void Attack(GameObject weapon) { }

    protected async void Cooldown()
    {
        float interval = 0f;

        while (interval < cooldown)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        isReady = true;
    }
}
