using UnityEngine;
//using UnityEngine.InputSystem;

public class MovementControllerV2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] moveSoundClips;

    private Vector2 moveDir;
    private Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (moveDir == Vector2.zero)
        {
            ResetAnimatorParameters();
        }
        else
        {
            if (moveDir.x < -0.5f)
            {
                weaponHolder.transform.localScale = new Vector2(-1, 1);

                if (animator)
                {
                    ResetAnimatorParameters();
                    animator.SetBool("Left", true); //A
                }
            }
            if (moveDir.x > 0.5f)
            {
                weaponHolder.transform.localScale = new Vector2(1, 1);

                if (animator)
                {
                    ResetAnimatorParameters();
                    animator.SetBool("Right", true); //D
                }
            }
            if (moveDir.y < -0.5f)
            {
                if (animator)
                {
                    ResetAnimatorParameters();
                    animator.SetBool("Left", true); //S
                }
            }
            if (moveDir.y > 0.5f)
            {
                if (animator)
                {
                    ResetAnimatorParameters();
                    animator.SetBool("Right", true); //W
                }
            }
            if (moveDir.x > 0.5f && moveDir.y > 0.5f)
            {
                if (animator)
                {
                    ResetAnimatorParameters();
                    animator.SetBool("Right", true); //WD
                }
            }
            if (moveDir.x < -0.5f && moveDir.y < -0.5f)
            {
                if (animator)
                {
                    ResetAnimatorParameters();
                    animator.SetBool("Left", true); //AS
                }
            }
            if (moveDir.x > 0.5f && moveDir.y < -0.5f)
            {
                if (animator)
                {
                    ResetAnimatorParameters();
                    animator.SetBool("Right", true); //SD
                }
            }
            if (moveDir.x < -0.5f && moveDir.y > 0.5f)
            {
                if (animator)
                {
                    ResetAnimatorParameters();
                    animator.SetBool("Left", true); //AW
                }
            }


            if (!audioSource.isPlaying)
            {
                audioSource.clip = moveSoundClips[Random.Range(0, moveSoundClips.Length)];
                audioSource.Play();
            }
        }
        

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDir *= sprintMultiplier;
        }
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + moveDir * moveSpeed * Time.deltaTime);
    }

    public void ResetAnimatorParameters()
    {
        if (animator)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }

    public void SetMovementSpeed(float value)
    {
        moveSpeed = value;
    }
}
