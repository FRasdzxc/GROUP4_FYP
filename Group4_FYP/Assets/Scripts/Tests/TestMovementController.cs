using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovementController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;

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
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * movementSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDir *= sprintMultiplier;
        }
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + moveDir * Time.deltaTime);
    }
}
