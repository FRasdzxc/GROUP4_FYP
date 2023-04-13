using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;

    private Slime m_slime;
    private MobData m_data;
    private int m_health;

    private void Awake()
    {
        m_slime = gameObject.GetComponentInParent<Slime>();
        m_data = m_slime.m_data;
    }
    
    private void Update()
    {
        bool forceUpdate = false;
        if (healthBar.maxValue != m_data.health)
        {
            healthBar.maxValue = m_data.health;
            forceUpdate = true;
        }

        if (forceUpdate || m_health != (int)m_slime.health)
        {
            healthBar.DOValue(m_slime.health, 1f).SetEase(Ease.OutCubic);
            m_health = (int)m_slime.health;
        }
    }
}
