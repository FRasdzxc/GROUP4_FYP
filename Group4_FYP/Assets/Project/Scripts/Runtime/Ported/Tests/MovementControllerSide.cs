using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementControllerSide : MovementControllerV2
{
    [SerializeField]
    protected float jumpForce = 12.5f;

    [SerializeField]
    protected float fallDamageDistance = 10f;

    [SerializeField]
    protected float fallDamageMultiplier = 3.5f;

    [SerializeField]
    protected Transform groundCollider;

    [SerializeField]
    protected LayerMask groundLayer;

    [SerializeField]
    protected GameObject smoke;

    [SerializeField]
    protected Vector2 smokeOffset;

    [SerializeField] [Tooltip("Unit: seconds")]
    protected float switchGravityDuration = 0.25f;

    [SerializeField]
    protected AudioClip[] jumpSounds;

    [SerializeField]
    protected AudioClip[] hitGroundSounds;

    [SerializeField]
    protected AudioClip[] hitSpongeSounds;

    protected float moveDirSide;
    protected bool isOnGround;
    protected Vector2 leftGroundPos;

    protected virtual void Start()
    {
        leftGroundPos = transform.position;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_InputReader.MoveSide += OnMoveSide;
        m_InputReader.JumpSide += OnJumpSide;
        m_InputReader.SwitchGravity += OnSwitchGravity;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_InputReader.MoveSide -= OnMoveSide;
        m_InputReader.JumpSide -= OnJumpSide;
        m_InputReader.SwitchGravity -= OnSwitchGravity;
    }

    // Update is called once per frame
    protected async override void Update()
    {
        ResetAnimatorParameters();

        if (moveDirSide != 0)
        {
            if (moveDirSide < -0.5f)
            {
                weaponHolder.transform.localScale = new Vector2(-1, 1);
                animator?.SetBool("Left", true); //A
            }
            if (moveDirSide > 0.5f)
            {
                weaponHolder.transform.localScale = new Vector2(1, 1);
                animator?.SetBool("Right", true); //D
            }

            if (sprinting)
                moveDirSide *= sprintMultiplier;

            if (Time.time >= nextStepSound)
            {
                // Don't repeat last used
                AudioClip newClip;
                do
                    newClip = moveSoundClips[Random.Range(0, moveSoundClips.Length)];
                while (newClip == lastStepClip);

                audioSource.PlayOneShot(newClip);
                m_ScoreEventChannel.StepTaken();
                nextStepSound = Time.time + newClip.length * (sprinting ? 0.9f : 1.2f);
                lastStepClip = newClip;
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.V)) // allow switching gravity even if player is not on ground; editor only
            {
                rb2D.gravityScale *= -1;
                await transform.DOScaleY(transform.localScale.y * -1, switchGravityDuration).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
            }
#endif
    }

    protected override void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapBox(groundCollider.position, groundCollider.localScale * 2f * (rb2D.gravityScale < 0 ? -1 : 1), 0, groundLayer);
        rb2D.velocity = new Vector2(moveDirSide * moveSpeed, rb2D.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        float distance = Vector2.Distance(transform.position, leftGroundPos);
        if (distance > 1f)
        {
            Instantiate(smoke, CalculateSmokeOffset(), Quaternion.identity);

            if (hitGroundSounds.Length > 0)
                audioSource.PlayOneShot(hitGroundSounds[Random.Range(0, hitGroundSounds.Length)]);
        }

        if (collision2D.collider.CompareTag("Sponge"))
        {
            leftGroundPos = transform.position;

            if (hitSpongeSounds.Length > 0)
                audioSource.PlayOneShot(hitSpongeSounds[Random.Range(0, hitSpongeSounds.Length)]);

            return;
        }

        if (distance > fallDamageDistance && isOnGround)
            Hero.Instance.TakeDamage((distance - fallDamageDistance) * fallDamageMultiplier);
    }

    void OnCollisionExit2D(Collision2D collision2D)
        => leftGroundPos = transform.position;

    private Vector2 CalculateSmokeOffset()
        => ((Vector2)transform.position + Vector2.Scale(smokeOffset, transform.localScale));

    private void OnMoveSide(float direction)
        => moveDirSide = direction;

    private void OnJumpSide()
    {
        if (!isOnGround)
            return;

        if (rb2D.velocity.y <= 0f)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce * (rb2D.gravityScale < 0 ? -1 : 1));
            Instantiate(smoke, CalculateSmokeOffset(), Quaternion.identity);
            leftGroundPos = transform.position;

            if (jumpSounds.Length > 0)
                audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
        }
    }

    private async void OnSwitchGravity()
    {
        if (!isOnGround)
            return;

        rb2D.gravityScale *= -1;
        await transform.DOScaleY(transform.localScale.y * -1, switchGravityDuration).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    }
}
