using UnityEngine;
using UnityEngine.InputSystem;
using PathOfHero.Utilities;

public class AbilityManager : Singleton<AbilityManager>
{
    [SerializeField] private InputReader m_InputReader;
    [SerializeField] private InputActionReference[] m_AbilityActions;
    [SerializeField] private Ability[] abilities;

    private Ability[] equippedAbilities;
    private HUD hud;
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
    private float abilityDamageUpgrade;

    protected override void Awake()
    {
        base.Awake();
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
    }

    private void OnEnable()
    {
        m_InputReader.Ability1 += OnAbility1;
        m_InputReader.Ability2 += OnAbility2;
        m_InputReader.Ability3 += OnAbility3;
    }

    private void OnDisable()
    {
        m_InputReader.Ability1 -= OnAbility1;
        m_InputReader.Ability2 -= OnAbility2;
        m_InputReader.Ability3 -= OnAbility3;
    }

    void Start()
        => Setup();

    void Update()
    {
#if UNITY_EDITOR
        if (GameManager.Instance.IsPlayingHostile())
        {
            // test refill mana
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                mana = upgradedMaxMana;
                // hud.UpdateMana(mana);
                hud.UpdateMana(mana, upgradedMaxMana);
                ReadyEquippedAbilities();
            }
        }
#endif

        if (mana < upgradedMaxMana)
            mana += Time.deltaTime * (manaRegeneration + manaRegenerationUpgrade);
        mana = Mathf.Clamp(mana, 0, upgradedMaxMana);
        hud.UpdateMana(mana, upgradedMaxMana);

        for (int i = 0; i < equippedAbilities.Length; i++)
            hud.UpdateAbility(i, equippedAbilities[i].Cooldown);
    }

    public void Setup()
    {
        hud.UpdateMana(mana, upgradedMaxMana);
        equippedAbilities = abilities; // not final: should be changed to be equipped inside inventory later on

        for (int i = 0; i < equippedAbilities.Length; i++)
            hud.SetupAbility(i, equippedAbilities[i].icon, equippedAbilities[i].cooldownTime, m_AbilityActions[i].action.GetBindingDisplayString());

        ReadyEquippedAbilities();
    }

    #region Setters/Getters/AddSubtract
    // Setters
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

    public void SetAbilityDamageUpgrade(float value)
    {
        this.abilityDamageUpgrade = value;
    }

    // Getters
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

    public float GetAbilityDamageUpgrade()
    {
        return abilityDamageUpgrade;
    }

    // AddSubtract
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

    public void AddAbilityDamageUpgrade(float value)
    {
        abilityDamageUpgrade += value;
    }
    #endregion
    
    private void OnAbility1()
        => ActivateAbility(0);

    private void OnAbility2()
        => ActivateAbility(1);

    private void OnAbility3()
        => ActivateAbility(2);

    private void ActivateAbility(int index)
    {
        if (equippedAbilities[index].IsReady && (mana - equippedAbilities[index].manaCost) >= 0)
        {
            mana -= equippedAbilities[index].manaCost;
            equippedAbilities[index].Activate(gameObject); 
        }
    }

    private void ReadyEquippedAbilities()
    {
        foreach(Ability a in equippedAbilities)
        {
            //a.isReady = true;
            a.Cooldown = 0f;
        }
    }
}
