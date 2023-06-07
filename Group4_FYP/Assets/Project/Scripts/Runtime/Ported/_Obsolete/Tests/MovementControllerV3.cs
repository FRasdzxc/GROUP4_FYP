using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementControllerV3 : MonoBehaviour
{
    [SerializeField] private CharacterData character;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] moveSoundClips;

    protected Rigidbody2D rb2D;
    protected Vector2 moveDir;
    protected float moveSpeed;
    protected float sprintMultiplier;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        moveSpeed = character.moveSpeed;
        sprintMultiplier = character.sprintMultiplier;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + moveDir * moveSpeed * Time.deltaTime);
    }

    // protected void SetMoveSpeed(float value)
    // {
    //     moveSpeed = value;
    // }
}
