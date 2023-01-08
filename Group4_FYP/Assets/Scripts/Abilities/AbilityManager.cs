using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private Ability[] abilities;

    private Ability[] equippedAbilities;
    private HUD hud;
    private HeroData heroData;
    private float mana;

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
            if (equippedAbilities[0].isReady) // should be rewritten better
            {
                mana -= equippedAbilities[0].manaCost;
            }

            equippedAbilities[0].Activate(gameObject); // test            
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (equippedAbilities[1].isReady) // should be rewritten better
            {
                mana -= equippedAbilities[1].manaCost;
            }

            equippedAbilities[1].Activate(gameObject); // test
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (equippedAbilities[2].isReady) // should be rewritten better
            {
                mana -= equippedAbilities[2].manaCost;
            }

            equippedAbilities[2].Activate(gameObject); // test
        }

        if (mana < heroData.mana)
        {
            mana += Time.deltaTime; // temporary only
            hud.UpdateMana(mana);
        }
        else
        {
            mana = heroData.mana;
            hud.UpdateMana(mana);
        }

        // test refill mana
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
        {
            mana = heroData.mana;
            hud.UpdateMana(mana);
        }
    }

    public void Initialize(HUD hud, HeroData heroData)
    {
        this.hud = hud;
        this.heroData = heroData;
        Setup();
    }

    public void Setup()
    {
        mana = heroData.mana;
        hud.SetupMana(mana);
    }

    public void ReadyEquippedAbilities()
    {
        foreach(Ability a in equippedAbilities)
        {
            a.isReady = true;
        }
    }
}
