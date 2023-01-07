using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private Ability[] abilities;
    private Ability[] equippedAbilities;

    // Start is called before the first frame update
    void Start()
    {
        equippedAbilities = abilities; // not final: should be changed to be equipped inside inventory lateron

        ReadyEquippedAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            equippedAbilities[0].Activate(gameObject); // test
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            equippedAbilities[1].Activate(gameObject); // test
        }
    }

    public void ReadyEquippedAbilities()
    {
        foreach(Ability a in equippedAbilities)
        {
            a.isReady = true;
        }
    }
}
