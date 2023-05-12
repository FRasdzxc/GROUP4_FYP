using PathOfHero.Telemetry;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementControllerV2 : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float sprintMultiplier = 2f;
    [SerializeField] protected GameObject weaponHolder;
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] moveSoundClips;

    protected PlayerInput playerInput;
    protected InputAction moveAction;
    protected InputAction sprintAction;
    protected Vector2 moveDir;
    protected Rigidbody2D rb2D;

    protected AudioClip lastStepClip;
    protected float nextStepSound;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        moveAction = playerInput.actions["Move"];
        moveAction.Enable();

        sprintAction = playerInput.actions["Sprint"];
        sprintAction.Enable();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        moveDir = moveAction.ReadValue<Vector2>();
        ResetAnimatorParameters();

        if (moveDir.magnitude > 0)
        {
            if (moveDir.x < -0.5f)
            {
                weaponHolder.transform.localScale = new Vector2(-1, 1);
                animator?.SetBool("Left", true); //A
            }
            if (moveDir.x > 0.5f)
            {
                weaponHolder.transform.localScale = new Vector2(1, 1);
                animator?.SetBool("Right", true); //D
            }
            if (moveDir.x == 0)
            {
                if (moveDir.y < -0.5f)
                    animator?.SetBool("Left", true); //S
                if (moveDir.y > 0.5f)
                    animator?.SetBool("Right", true); //W
            }
            
            // if (moveDir.x > 0.5f && moveDir.y > 0.5f)
            //     animator?.SetBool("Right", true); //WD
            // if (moveDir.x < -0.5f && moveDir.y < -0.5f)
            //     animator?.SetBool("Left", true); //AS
            // if (moveDir.x > 0.5f && moveDir.y < -0.5f)
            //     animator?.SetBool("Right", true); //SD
            // if (moveDir.x < -0.5f && moveDir.y > 0.5f)
            //     animator?.SetBool("Left", true); //AW

            var sprinting = sprintAction.ReadValue<float>() == 1;
            if (sprinting)
                moveDir *= sprintMultiplier;

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
    }

    protected virtual void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + moveDir * moveSpeed * Time.deltaTime);
    }

    public void ResetAnimatorParameters()
    {
        if (animator == null)
            return;

        foreach (AnimatorControllerParameter parameter in animator.parameters)
            animator.SetBool(parameter.name, false);
    }

    public void SetMovementSpeed(float value)
    {
        moveSpeed = value;
    }
}
