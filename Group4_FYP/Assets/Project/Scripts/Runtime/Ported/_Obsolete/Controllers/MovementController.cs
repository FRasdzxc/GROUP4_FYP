using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float fMovementSpeed = 5f;
    [SerializeField] private float fSprintMultiplier = 2f;
    public Animator anim;
    private Camera mainCamera;
    private Rigidbody2D rb2D;
    private bool bIsSprinting;
    private float fXDir, fYDir;
    private Vector2 normalizedDir;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        fXDir = 0f;
        fYDir = 0f;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            bIsSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            bIsSprinting = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            fYDir = 1;
            anim.SetBool("W", true);
        }
        else
        {
            anim.SetBool("W", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            fYDir = -1;
            anim.SetBool("S", true);
        }
        else
        {
            anim.SetBool("S", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            fXDir = -1;
            anim.SetBool("A", true);
        }
        else
        {
            anim.SetBool("A", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            fXDir = 1;
            anim.SetBool("D", true);
        }
        else 
        { 
            anim.SetBool("D", false);
        }

        normalizedDir = new Vector2(fXDir, fYDir).normalized;
    }

    private void FixedUpdate()
    {
        if (bIsSprinting)
        {
            // transform.Translate(normalizedDir * fMovementSpeed * fSprintMultiplier * Time.deltaTime);
            rb2D.MovePosition(rb2D.position + normalizedDir * fMovementSpeed * fSprintMultiplier * Time.deltaTime);
        }
        else
        {
            // transform.Translate(normalizedDir * fMovementSpeed * Time.deltaTime);
            rb2D.MovePosition(rb2D.position + normalizedDir * fMovementSpeed * Time.deltaTime);
        }
    }
}
