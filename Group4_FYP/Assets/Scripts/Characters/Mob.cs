using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TinyScript;
using PathOfHero.Telemetry;

public class Mob : MonoBehaviour
{
    [SerializeField] protected MobData mobData;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private LootDrop loot;
    [SerializeField] private int randomDropCount = 1;
    [SerializeField] private float dropRange = 0.5f;
    [SerializeField] private AudioClip[] damageSoundClips;
    [SerializeField] private AudioClip[] dieSoundClips;
    [SerializeField] private AudioSource audioSource;
    protected float health;
    protected float sightDistance;
    protected float attackDistance;
    protected float movementSpeed;
    protected GameObject player;
    protected bool isDead;
    protected PointDrop point;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb2D;
    protected Vector2 moveDir;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = mobData.health;
        sightDistance = mobData.sightDistance;
        attackDistance = mobData.attackDistance;
        // walkspeed = mobData.speed;
        movementSpeed = mobData.moveSpeed;
        player = GameObject.FindWithTag("Player");
        isDead = false; // preventive
        point = GetComponent<PointDrop>();
        sr = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        healthSlider.maxValue = health;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= attackDistance)
            {
                AttackPlayer();
            }
            else if (Vector2.Distance(transform.position, player.transform.position) <= sightDistance)
            {
                ChasePlayer();
            }
            else
            {
                WalkAround();
            }
        }
        else
        {
            sr.color = Color.red;
        }
    }

    protected virtual void AttackPlayer()
    {

    }

    protected virtual void AttackMethod()
    {

    }

    protected virtual void ChasePlayer()
    {
        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        //rb2D.MovePosition(new Vector2(player.transform.position.x, player.transform.position.y) + (movementSpeed * Time.deltaTime));
        moveDir = ((Vector2)player.transform.position - rb2D.position).normalized;
        //Debug.Log($"moveDir = {moveDir}");
        //rb2D.MovePosition(rb2D.position + ((Vector2)player.transform.position - rb2D.position).normalized * movementSpeed * Time.deltaTime);
        rb2D.MovePosition(rb2D.position + moveDir * movementSpeed * Time.deltaTime);
        //rb2D.AddRelativeForce(((Vector2)player.transform.position - rb2D.position).normalized/* * movementSpeed*/, ForceMode2D.Impulse);

        // LookAt();
    }

    protected virtual void LookAt()
    {
        if (moveDir.x < -0.5f)
        {
            transform.localScale = new Vector2(-1, 1);      // todo: make a sprite child for the sprite only and only change the scale of the sprite child
        }
        else if (moveDir.x > 0.5f)
        {
            transform.localScale = Vector2.one;
        }
    }

    private void WalkAround()
    {

    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        UpdateUI();
        PlaySound(damageSoundClips[Random.Range(0, damageSoundClips.Length)]);

        if (health <= 0)
        {
            Die();
        }
    }

    private async void Die()
    {
        isDead = true;
        health = 0;
        PlaySound(dieSoundClips[Random.Range(0, dieSoundClips.Length)]);
        UpdateUI();
        await transform.DOScale(0, 0.5f).AsyncWaitForCompletion();

        if (this) // trying to prevent MissingReferenceException
        {
            loot.SpawnDrop(transform, randomDropCount, dropRange);
            point.SpawnDrop();
            DataCollector.Instance?.MobsKilled(mobData.characterName);
            Destroy(gameObject);
        }
    }

    private void UpdateUI()
    {
        healthSlider.DOValue(health, 0.25f).SetEase(Ease.OutQuart);
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("HeroWeaponTrigger") || collision.CompareTag("HeroWeaponTriggerStronger")) && collision.GetComponent<WeaponTrigger>())
        {
            TakeDamage(collision.GetComponent<WeaponTrigger>().GetDamage(false));
            sr.color = Color.red;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        sr.color = Color.white;
    }
}
