using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Text manaText;
    [SerializeField] private Text xpText;
    [SerializeField] private GameObject deathMessage;

    private float maxMana;
    private int maxXP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        UpdateHealth(maxHealth);
    }

    public void SetupMana(float maxMana)
    {
        manaSlider.maxValue = maxMana;
        UpdateMana(maxMana);
    }

    public void SetupXP(int maxXP)
    {
        this.maxXP = maxXP;
        xpSlider.maxValue = maxXP;
        UpdateXP(maxXP);
    }

    public void UpdateHealth(float health)
    {
        healthSlider.DOValue(health, 0.25f);
        healthText.text = health.ToString() + " HP";
    }

    public void UpdateMana(float mana)
    {
        manaSlider.DOValue(mana, 0.25f);
        manaText.text = ((int)mana).ToString() + "/" + maxMana.ToString() + " MP";
    }

    public void UpdateXP(int xp)
    {
        xpSlider.DOValue(xp, 0.25f);
        xpText.text = "level " + playerData.GetLevel() + " (" + playerData.GetStoredXP() + "/" + maxXP.ToString() + " XP)";
    }
}
