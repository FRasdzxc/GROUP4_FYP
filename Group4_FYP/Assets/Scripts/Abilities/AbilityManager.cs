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
    private float maxMana;
    private float manaRegeneration;

    // Start is called before the first frame update
    void Awake()
    {
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
    }

    void Start()
    {
        Setup();
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

        if (mana < maxMana)
        {
            mana += Time.deltaTime * manaRegeneration;
            hud.UpdateMana(mana);
        }
        else
        {
            mana = maxMana;
            hud.UpdateMana(mana);
        }

        // test refill mana
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            mana = maxMana;
            hud.UpdateMana(mana);
            ReadyEquippedAbilities();
        }

        for (int i = 0; i < equippedAbilities.Length; i++)
        {
            hud.UpdateAbility(i, equippedAbilities[i].remainingCooldownTime);
        }
    }

    public void Setup()
    {
        hud.SetupMana(mana, maxMana);
        equippedAbilities = abilities; // not final: should be changed to be equipped inside inventory lateron

        for (int i = 0; i < equippedAbilities.Length; i++)
        {
            hud.SetupAbility(i, equippedAbilities[i].icon, equippedAbilities[i].cooldownTime);
        }

        ReadyEquippedAbilities();
    }

    public void SetMana(float mana)
    {
        this.mana = mana;
    }

    public void SetMaxMana(float maxMana)
    {
        this.maxMana = maxMana;
    }

    public void SetManaRegeneration(float manaRegeneration)
    {
        this.manaRegeneration = manaRegeneration;
    }

    public float GetMana()
    {
        return mana;
    }

    public float GetMaxMana()
    {
        return maxMana;
    }

    public float GetManaRegeneration()
    {
        return manaRegeneration;
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
