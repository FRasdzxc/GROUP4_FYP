using UnityEngine;
using PathOfHero.Managers.Data;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementControllerV2 : MonoBehaviour
{
    [SerializeField] protected InputReader m_InputReader;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float sprintMultiplier = 2f;
    [SerializeField] protected GameObject weaponHolder;
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] moveSoundClips;

    [SerializeField]
    protected ScoreEventChannel m_ScoreEventChannel;

    protected Vector2 newDir;
    protected Vector2 moveDir;
    protected bool sprinting;
    protected Rigidbody2D rb2D;

    protected AudioClip lastStepClip;
    protected float nextStepSound;

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnEnable()
    {
        m_InputReader.Move += OnMove;
        m_InputReader.Sprint += OnSprint;
        m_InputReader.SprintCanceled += OnSprintCanceled;
    }

    protected virtual void OnDisable()
    {
        m_InputReader.Move -= OnMove;
        m_InputReader.Sprint -= OnSprint;
        m_InputReader.SprintCanceled -= OnSprintCanceled;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        moveDir = newDir;
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
                nextStepSound = Time.time + newClip.length * (sprinting ? 0.9f : 1.2f);
                lastStepClip = newClip;

                m_ScoreEventChannel?.StepTaken();
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + moveDir * moveSpeed * Time.deltaTime);
    }

    private void OnMove(Vector2 direction)
        => newDir = direction;

    private void OnSprint()
        => sprinting = true;

    private void OnSprintCanceled()
        => sprinting = false;

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
