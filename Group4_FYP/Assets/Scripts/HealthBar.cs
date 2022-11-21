using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    private MobData m_data;
    public GameObject obj;
    public float maxHealth;
    public float beforeHealth;
    public float currentHealth;
    public float damage;
    float currentVelocity = 0;
    float takeDamage;
    bool takenDamage;
    // Start is called before the first frame update
    void Start()
    {
        m_data = obj.GetComponent<Slime>().m_data;
        maxHealth = m_data.health;
        currentHealth = m_data.health;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.DOValue(gameObject.GetComponentInParent<Slime>().health, 1f).SetEase(Ease.OutCubic);
    }
}
