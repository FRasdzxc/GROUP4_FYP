using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider xpSlider;

    [SerializeField] private Text profileNameText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text manaText;
    [SerializeField] private Text xpText;
    [SerializeField] private Text mobCountText;

    [SerializeField] private GameObject hugeMessage;
    [SerializeField] private Text regionText;
    [SerializeField] private Slider[] abilitySliders;
    [SerializeField] private Image[] abilityImages;
    [SerializeField] private GameObject[] abilityCooldownText;

    private float maxMana;
    private PlayerData playerData;
    private int maxXP;

    // Start is called before the first frame update
    void Start()
    {
        regionText.text = ""; // temporary
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        profileNameText.text = ProfileManagerJson.GetProfile(PlayerPrefs.GetString("selectedProfileName")).profileName;
    }

    // Update is called once per frame
    void Update()
    {
        mobCountText.text = "MOB COUNT: " + GameObject.FindGameObjectsWithTag("Mob").Length.ToString();
    }

    public void SetupHealth(float health, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        UpdateHealth(health);
    }

    public void SetupMana(float maxMana)
    {
        this.maxMana = maxMana;
        manaSlider.maxValue = maxMana;
        UpdateMana(maxMana);
    }

    public void SetupAbility(int slotNumber, Sprite icon, float cooldownTime)
    {
        if (slotNumber >= 0 && slotNumber < abilitySliders.Length)
        {
            abilityImages[slotNumber].sprite = icon;
            abilitySliders[slotNumber].maxValue = cooldownTime;
        }
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
        healthText.text = ((int)health).ToString() + " HP";
    }

    public void UpdateMana(float mana)
    {
        manaSlider.DOValue(mana, 0.25f);
        manaText.text = ((int)mana).ToString() + "/" + maxMana.ToString() + " MP";
    }

    public void UpdateAbility(int slotNumber, float remainingCooldownTime)
    {
        if (slotNumber >= 0 && slotNumber < abilitySliders.Length)
        {
            abilitySliders[slotNumber].DOValue(remainingCooldownTime, 0.25f);

            if (remainingCooldownTime <= 0)
            {
                abilityCooldownText[slotNumber].SetActive(false);
            }
            else
            {
                abilityCooldownText[slotNumber].SetActive(true);
                abilityCooldownText[slotNumber].GetComponent<Text>().text = remainingCooldownTime.ToString("0.0");
            }
        }
    }

    public void UpdateXP(int xp)
    {
        xpSlider.DOValue(xp, 0.25f);
        xpText.text = "level " + playerData.GetLevel() + " (" + playerData.GetStoredXP() + "/" + maxXP.ToString() + " XP)";
    }

    public async Task ShowHugeMessage(string message, float duration, Color color) // duration = seconds
    {
        hugeMessage.transform.localScale = new Vector2(0, 1);

        Text text = hugeMessage.GetComponent<Text>();
        text.text = message;
        text.color = color;

        hugeMessage.SetActive(true);
        await hugeMessage.transform.DOScaleX(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        await Task.Delay((int)(duration * 1000));
        await hugeMessage.transform.DOScaleX(0, 0.25f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
        hugeMessage.SetActive(false);
    }
}
