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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // should be rewritten better
            if (equippedAbilities[0].IsReady() && (mana - equippedAbilities[0].manaCost) >= 0)
            {
                mana -= equippedAbilities[0].manaCost;
                equippedAbilities[0].Activate(gameObject); // test 
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            // should be rewritten better
            if (equippedAbilities[1].IsReady() && (mana - equippedAbilities[1].manaCost) >= 0)
            {
                mana -= equippedAbilities[1].manaCost;
                equippedAbilities[1].Activate(gameObject); // test
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            // should be rewritten better
            if (equippedAbilities[2].IsReady() && (mana - equippedAbilities[2].manaCost) >= 0)
            {
                mana -= equippedAbilities[2].manaCost;
                equippedAbilities[2].Activate(gameObject); // test
            }
        }

        if (mana < heroData.mana)
        {
            mana += Time.deltaTime * heroData.manaRegeneration;
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
            ReadyEquippedAbilities();
        }

        for (int i = 0; i < equippedAbilities.Length; i++)
        {
            hud.UpdateAbility(i, equippedAbilities[i].remainingCooldownTime);
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
        equippedAbilities = abilities; // not final: should be changed to be equipped inside inventory lateron

        for (int i = 0; i < equippedAbilities.Length; i++)
        {
            hud.SetupAbility(i, equippedAbilities[i].icon, equippedAbilities[i].cooldownTime);
        }

        ReadyEquippedAbilities();
    }

    private void ReadyEquippedAbilities()
    {
        foreach(Ability a in equippedAbilities)
        {
            //a.isReady = true;
            a.remainingCooldownTime = 0f;
        }
    }
}
