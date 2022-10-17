using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    public GameObject mage;
    public GameObject fireball;
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private Vector2 mousePosition;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer = mainCamera.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 lookDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(fireball, mage.transform.position, Quaternion.Euler(0f, 0f, lookAngle));

            Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
            rigidbody.MovePosition(bullet.transform.position + Vector3.forward * 0.025f);
        }
        lineRenderer.SetPosition(0, mage.transform.position);
        lineRenderer.SetPosition(1, mousePosition);
    }
}
