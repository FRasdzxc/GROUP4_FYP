using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private Ability[] abilities;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Ability a in abilities)
        {
            //a.abilityState = AbilityState.ready;
            a.isReady = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            abilities[0].Activate(gameObject); // test
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            abilities[1].Activate(gameObject); // test
        }
    }
}
