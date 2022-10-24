using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    public GameObject mage;
    public Transform fireball;
    public float projectileSpeed = 10;
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private Vector2 mousePosition;


    // Start is called before the first frame update
    void Start()
    {
        ProjectilesController.projectileSpeed = projectileSpeed;
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
            AutoAttack(mousePosition);
            //Vector2 lookDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            //float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            //GameObject bullet = Instantiate(fireball, mage.transform.position, Quaternion.Euler(0f, 0f, lookAngle));

            //Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
            //rigidbody.MovePosition(bullet.transform.position + Vector3.forward * 0.025f);
        }
        lineRenderer.SetPosition(0, mage.transform.position);
        lineRenderer.SetPosition(1, mousePosition);
    }

    void SetProjectileSpeed(float speed)
    {

    }

    void AutoAttack(Vector3 mousePosition)
    {
        Transform bulletTransform = Instantiate(fireball, mage.transform.position, Quaternion.identity);
        Vector3 shootDir = (mousePosition - mage.transform.position).normalized;
        bulletTransform.GetComponent<ProjectilesController>().Setup(shootDir);
    }

}
