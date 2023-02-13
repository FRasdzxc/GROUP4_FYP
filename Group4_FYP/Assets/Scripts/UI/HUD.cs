using System;
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

    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private CanvasGroup objectivePanelCanvasGroup;
    [SerializeField] private Text objectiveText;

    private float upgradedMaxMana;
    private int maxXP;

    // Start is called before the first frame update
    void Start()
    {
        regionText.text = ""; // temporary?
        profileNameText.text = ProfileManagerJson.GetProfile(PlayerPrefs.GetString("selectedProfileName")).profileName;
    }

    // Update is called once per frame
    void Update()
    {
        mobCountText.text = "MOB COUNT: " + GameObject.FindGameObjectsWithTag("Mob").Length.ToString("n0");
    }

    // public void SetupHealth(float health, float maxHealth)
    // {
    //     healthSlider.maxValue = maxHealth;
    //     UpdateHealth(health);
    // }

    // public void SetupMana(float mana, float maxMana)
    // {
    //     this.maxMana = maxMana;
    //     manaSlider.maxValue = maxMana;
    //     UpdateMana(mana);
    // }

    public void SetupAbility(int slotNumber, Sprite icon, float cooldownTime)
    {
        if (slotNumber >= 0 && slotNumber < abilitySliders.Length)
        {
            abilityImages[slotNumber].sprite = icon;
            abilitySliders[slotNumber].maxValue = cooldownTime;
        }
    }

    public void SetupXP(int level, int maxXP)
    {
        this.maxXP = maxXP;
        xpSlider.maxValue = maxXP;
        UpdateXP(level, maxXP);
    }

    // public void UpdateHealth(float health)
    // {
    //     healthSlider.DOValue(health, 0.25f).SetEase(Ease.OutQuart);
    //     healthText.text = ((int)health).ToString("n0") + " HP";
    // }

    public void UpdateHealth(float health, float upgradedMaxHealth)
    {
        healthSlider.maxValue = upgradedMaxHealth;

        healthSlider.DOValue(health, 0.25f).SetEase(Ease.OutQuart);
        healthText.text = ((int)health).ToString("n0") + " HP";
    }

    // public void UpdateMana(float mana)
    // {
    //     manaSlider.DOValue(mana, 0.25f).SetEase(Ease.OutQuart);
    //     manaText.text = ((int)mana).ToString("n0") + "/" + maxMana.ToString("n0") + " MP";
    // }

    public void UpdateMana(float mana, float upgradedMaxMana)
    {
        this.upgradedMaxMana = upgradedMaxMana;
        manaSlider.maxValue = upgradedMaxMana;

        manaSlider.DOValue(mana, 0.25f).SetEase(Ease.OutQuart);
        manaText.text = ((int)mana).ToString("n0") + "/" + this.upgradedMaxMana.ToString("n0") + " MP";
    }

    public void UpdateAbility(int slotNumber, float remainingCooldownTime)
    {
        if (slotNumber >= 0 && slotNumber < abilitySliders.Length)
        {
            abilitySliders[slotNumber].DOValue(remainingCooldownTime, 0.25f).SetEase(Ease.OutQuart);

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

    public void UpdateXP(int level, int storedExp)
    {
        xpSlider.DOValue(storedExp, 0.25f).SetEase(Ease.OutQuart);
        //xpText.text = "level " + playerData.GetLevel() + " (" + playerData.GetStoredXP() + "/" + maxXP.ToString() + " XP)";
        xpText.text = "Level " + level.ToString("n0") + " (" + storedExp.ToString("n0") + "/" + maxXP.ToString("n0") + " XP)";
    }

    public void UpdateXP(int level, int storedExp, int requiredExp)
    {
        xpSlider.maxValue = requiredExp;

        xpSlider.DOValue(storedExp, 0.25f).SetEase(Ease.OutQuart);
        xpText.text = String.Format("Level {0} ({1}/{2} XP)", level.ToString("n0"), storedExp.ToString("n0"), requiredExp.ToString("n0"));
    }

    public async Task ShowHugeMessage(string message, Color color, float duration = 1.5f) // duration = seconds
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

    public void ShowObjective(string objective)
    {
        objectiveText.text = objective;

        objectivePanel.SetActive(true);
        objectivePanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart);
    }

    public async void HideObjective()
    {
        await objectivePanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        objectivePanel.SetActive(false);
    }
}
