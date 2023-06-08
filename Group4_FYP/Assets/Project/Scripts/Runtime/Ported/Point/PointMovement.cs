using UnityEngine;

public class PointMovement : MonoBehaviour
{
    string storedType;
    int storedValue;
    public float speed;

    private Rigidbody2D m_Rigidbody2D;
    private Transform m_MoveTarget;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (m_MoveTarget == null)
            return;

        float distance = Vector2.Distance(transform.position, m_MoveTarget.position);
        if (distance < 0.5f && m_MoveTarget.TryGetComponent<Hero>(out var hero) && m_MoveTarget.TryGetComponent<AbilityManager>(out var abilityManager))
        {
            switch (storedType)
            {
                case "coin":
                    hero.AddCoin(storedValue);
                    break;
                case "xp":
                    hero.AddEXP(storedValue);
                    break;
                case "hp":
                    hero.AddHealth(storedValue);
                    break;
                case "mp":
                    abilityManager.AddMana(storedValue);
                    break;
            }
            Destroy(gameObject);
        }
        else
            m_Rigidbody2D.MovePosition(Vector2.Lerp(transform.position, m_MoveTarget.position, speed * Time.fixedDeltaTime));
    }

    public void SetValue(string type, int value)
    {
        storedType = type;
        storedValue = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_MoveTarget == null && collision.CompareTag("Player"))
            m_MoveTarget = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_MoveTarget == collision.transform)
            m_MoveTarget = null;
    }
}
