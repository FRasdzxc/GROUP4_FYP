using UnityEngine;

[RequireComponent(typeof(MovementControllerV2))]
public class Character : MonoBehaviour
{
    [SerializeField] protected CharacterData character;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] damageSoundClips;
    [SerializeField] protected AudioClip[] dieSoundClips;

    protected SpriteRenderer spriteRenderer;
    protected MovementControllerV2 movementController;
    protected float health;
    protected bool isDead;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementController = GetComponent<MovementControllerV2>();
    }

    protected virtual void TakeDamage(float value)
    {
        PlaySound(damageSoundClips[Random.Range(0, damageSoundClips.Length)]);

        if (health <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        PlaySound(dieSoundClips[Random.Range(0, damageSoundClips.Length)]);

        isDead = true;
        health = 0;
    }

    protected void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.color = Color.white;
    }
}
