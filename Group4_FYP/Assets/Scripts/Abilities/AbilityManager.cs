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
    private float _maxManaUpgrade;
    public float MaxManaUpgrade
    {
        get => _maxManaUpgrade;
        private set
        {
            _maxManaUpgrade = value;
            upgradedMaxMana = maxMana + _maxManaUpgrade;
        }
    }
    private float upgradedMaxMana;
    private float manaRegeneration;
    private float manaRegenerationUpgrade;

    private static AbilityManager instance;
    public static AbilityManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
    }

    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.IsPlayingHostile())
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

            // test refill mana
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                mana = upgradedMaxMana;
                // hud.UpdateMana(mana);
                hud.UpdateMana(mana, upgradedMaxMana);
                ReadyEquippedAbilities();
            }
        }

        if (mana < upgradedMaxMana)
        {
            mana += Time.deltaTime * (manaRegeneration + manaRegenerationUpgrade);
        }
        mana = Mathf.Clamp(mana, 0, upgradedMaxMana);
        // hud.UpdateMana(mana);
        hud.UpdateMana(mana, upgradedMaxMana);

        for (int i = 0; i < equippedAbilities.Length; i++)
        {
            hud.UpdateAbility(i, equippedAbilities[i].remainingCooldownTime);
        }
    }

    public void Setup()
    {
        // hud.SetupMana(mana, upgradedMaxMana);
        hud.UpdateMana(mana, upgradedMaxMana);
        equippedAbilities = abilities; // not final: should be changed to be equipped inside inventory later on

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

    public void SetMaxManaUpgrade(float value)
    {
        this.MaxManaUpgrade = value;
    }

    public void SetManaRegeneration(float manaRegeneration)
    {
        this.manaRegeneration = manaRegeneration;
    }

    public void SetManaRegenerationUpgrade(float value)
    {
        this.manaRegenerationUpgrade = value;
    }

    public float GetMana()
    {
        return mana;
    }

    public float GetMaxMana()
    {
        return maxMana;
    }

    public float GetMaxManaUpgrade()
    {
        return MaxManaUpgrade;
    }

    public float GetManaRegeneration()
    {
        return manaRegeneration;
    }

    public float GetManaRegenerationUpgrade()
    {
        return manaRegenerationUpgrade;
    }

    public void AddMana(float value)
    {
        mana += value;
    }

    public void AddMaxManaUpgrade(float value)
    {
        MaxManaUpgrade += value;
        // hud.SetupMana(mana, upgradedMaxMana);
    }

    public void AddManaRegenerationUpgrade(float value)
    {
        manaRegenerationUpgrade += value;
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
