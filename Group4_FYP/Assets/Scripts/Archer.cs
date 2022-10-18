using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Archer : MonoBehaviour // rename and use as general character?
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private GameObject weaponGobj;
    [SerializeField] private ScriptableObject[] weapons;
    private ScriptableObject currentWeapon;
    private bool canAttack;
    private LineRenderer tLineRenderer;
    private Vector2 tMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        tLineRenderer = GetComponent<LineRenderer>();
        tLineRenderer.startWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        tMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tLineRenderer.SetPosition(0, transform.position);
        tLineRenderer.SetPosition(1, tMousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private async void Attack()
    {
        if (canAttack)
        {
            canAttack = false;



            await WaitFor(0.25325f); // currentWeapon.AttackCooldown
        }
    }

    private async Task WaitFor(float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            await Task.Yield();
        }

        canAttack = true;
    }
}
