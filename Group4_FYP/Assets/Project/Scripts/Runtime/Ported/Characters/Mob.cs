using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TinyScript;
using PathOfHero.Telemetry;

public class Mob : MonoBehaviour
{
    [SerializeField] protected MobData mobData;
    [SerializeField] protected GameObject mobGobj;
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected LootDrop loot;
    [SerializeField] protected int randomDropCount = 1;
    [SerializeField] protected float dropRange = 0.5f;
    [SerializeField] protected AudioClip[] damageSoundClips;
    [SerializeField] protected AudioClip[] dieSoundClips;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected GameObject smoke;
    [SerializeField] protected Color smokeColor = Color.white;
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
    protected Animator animator;

    private int sliderValue;

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
        rb2D = GetComponent<Rigidbody2D>();
        UpdateUI();

        if (mobGobj)
        {
            sr = mobGobj.GetComponent<SpriteRenderer>();
            animator = mobGobj.GetComponent<Animator>();
        }
        else
            sr = GetComponent<SpriteRenderer>();

        if (CompareTag("Mob"))
            DirectionArrowController.Instance.AddDirection(DirectionType.Mob, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= attackDistance)
            {
                if (!DialogueController.Instance.GetIsInConversation())
                    AttackPlayer();
            }
            else if (Vector2.Distance(transform.position, player.transform.position) <= sightDistance)
                ChasePlayer();
            else
                WalkAround();

            if (animator)
            {
                ResetAnimatorParameters();

                if (rb2D.velocity.magnitude <= 0f)
                    animator.SetBool("IsMoving", false);
                else
                    animator.SetBool("IsMoving", true);
            }
        }
        else
            sr.color = Color.red;
    }

    protected virtual void AttackPlayer()
    {
        if (animator)
        {
            ResetAnimatorParameters();
            animator.SetBool("IsAttacking", true);
        }
    }

    protected virtual void AttackMethod() { }

    protected virtual void ChasePlayer()
    {
        moveDir = ((Vector2)player.transform.position - rb2D.position).normalized;
        // rb2D.MovePosition(rb2D.position + moveDir * movementSpeed * Time.deltaTime);
        rb2D.velocity = moveDir * movementSpeed;
        
        LookAt();
    }

    protected virtual void LookAt()
    {
        if (!mobGobj)
            return;

        if (moveDir.x < -0.5f)
            mobGobj.transform.localScale = new Vector2(-1, 1);      // todo: make a sprite child for the sprite only and only change the scale of the sprite child
        else if (moveDir.x > 0.5f)
            mobGobj.transform.localScale = Vector2.one;
    }

    private void WalkAround() { }

    private void TakeDamage(float damage)
    {
        if (damageSoundClips.Length > 0)
            audioSource.PlayOneShot(damageSoundClips[Random.Range(0, damageSoundClips.Length)]);

        //DataCollector.Instance?.DamageGiven(damage);
        health = Mathf.Clamp(health - damage, 0, mobData.health);
        UpdateUI();
        if (health <= 0)
            Die();
    }

    private async void Die()
    {
        isDead = true;
        health = 0;

        if (dieSoundClips.Length > 0)
                audioSource.PlayOneShot(dieSoundClips[Random.Range(0, dieSoundClips.Length)]);
            
        UpdateUI();

        if (smoke)
        {
            GameObject smokeClone = Instantiate(smoke, gameObject.transform.position, Quaternion.identity);
            smokeClone.GetComponent<SpriteRenderer>().color = smokeColor;
        }

        await transform.DOScale(0, 0.5f).AsyncWaitForCompletion();

        if (this) // trying to prevent MissingReferenceException
        {
            if (loot)
                loot.SpawnDrop(transform, randomDropCount, dropRange);
            point.SpawnDrop();
            //DataCollector.Instance?.MobsKilled(mobData.characterName);
            Destroy(gameObject);
        }
    }

    private void UpdateUI()
    {
        bool forceUpdate = false;
        if (healthSlider.maxValue != mobData.health)
        {
            healthSlider.maxValue = mobData.health;
            forceUpdate = true;
        }

        if (forceUpdate || sliderValue != (int)health)
        {
            healthSlider.DOValue(health, 0.25f).SetEase(Ease.OutQuart);
            sliderValue = (int)health;
        }
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
        => sr.color = Color.white;

    protected void ResetAnimatorParameters()
    {
        foreach (AnimatorControllerParameter acp in animator.parameters)
        {
            if (acp.type.Equals(AnimatorControllerParameterType.Bool))
                animator.SetBool(acp.name, false);
        }
    }
}
