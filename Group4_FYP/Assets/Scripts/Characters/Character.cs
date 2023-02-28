using UnityEngine;

public class Character : MonoBehaviour
{
    protected float health;
    protected float maxHealth;

    protected virtual void TakeDamage(float value) { }
}
