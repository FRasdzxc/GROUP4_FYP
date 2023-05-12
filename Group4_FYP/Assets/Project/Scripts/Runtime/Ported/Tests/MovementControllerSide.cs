using PathOfHero.Telemetry;
using UnityEngine;
using UnityEngine.InputSystem;
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
    protected InputAction moveActionSide;
    protected InputAction switchGravityAction;
    protected InputAction jumpAction;
    protected float moveDirSide;
    protected bool isOnGround;
    protected Vector2 leftGroundPos;

    protected override void Start()
    {
        base.Start();

        moveActionSide = playerInput.actions["MoveSide"];
        moveActionSide.Enable();

        switchGravityAction = playerInput.actions["SwitchGravity"];
        switchGravityAction.Enable();

        jumpAction = playerInput.actions["JumpSide"];
        jumpAction.Enable();
    }

    // Update is called once per frame
    protected async override void Update()
    {
        moveDirSide = moveActionSide.ReadValue<float>();
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

            var sprinting = sprintAction.ReadValue<float>() == 1;
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
                //DataCollector.Instance?.StepsTaken();
                nextStepSound = Time.time + newClip.length * (sprinting ? 0.9f : 1.2f);
                lastStepClip = newClip;
            }
        }

        if (isOnGround) {
            if (switchGravityAction.triggered)
            {
                rb2D.gravityScale *= -1;
                await transform.DOScaleY(transform.localScale.y * -1, 0.5f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
            }

            if (jumpAction.triggered && rb2D.velocity.y <= 0f)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce * (rb2D.gravityScale < 0 ? -1 : 1));
                Instantiate(smoke, transform.position, Quaternion.identity);
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.V)) // allow switching gravity even if player is not on ground; editor only
            {
                rb2D.gravityScale *= -1;
                await transform.DOScaleY(transform.localScale.y * -1, 0.5f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
            }
#endif
    }

    protected override void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapBox(groundCollider.position, groundCollider.localScale * 1.5f * (rb2D.gravityScale < 0 ? -1 : 1), 0, groundLayer);
        rb2D.velocity = new Vector2(moveDirSide * moveSpeed, rb2D.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        float distance = Vector2.Distance(transform.position, leftGroundPos);
        if (distance > 1f)
            Instantiate(smoke, transform.position, Quaternion.identity);

        if (collision2D.collider.CompareTag("Sponge"))
        {
            leftGroundPos = transform.position;
            return;
        }

        if (distance > fallDamageDistance)
            Hero.Instance.TakeDamage((distance - fallDamageDistance) * fallDamageMultiplier);
        
        Debug.Log(distance);
    }

    void OnCollisionExit2D(Collision2D collision2D)
        => leftGroundPos = transform.position;
}
