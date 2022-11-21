using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    public HeroData mageData;
    public GameObject mage;
    public Transform fireball;
    public float projectileSpeed = 10;
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private Vector2 mousePosition;
    private float attackSpeed;
    private bool[] isReady = { true, true, true, true, true};

    // Start is called before the first frame update
    void Start()
    {
        ProjectilesController.projectileSpeed = projectileSpeed;
        mainCamera = Camera.main;
        lineRenderer = mainCamera.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        attackSpeed = mageData.AttackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if(isReady[0] == true)
            {
                AutoAttack(mousePosition);
                StartCoroutine(StartCoolDown(attackSpeed, 0));
            }
            else
            {
                Debug.Log("Not ready");
            }
        }
        lineRenderer.SetPosition(0, mage.transform.position);
        lineRenderer.SetPosition(1, mousePosition);
    }

    void SetProjectileSpeed(float speed)
    {

    }
    IEnumerator StartCoolDown(float cooldown, int status)
    {
        isReady[status] = false;
        Debug.Log("Start Cooldown");
        yield return new WaitForSeconds(cooldown);
        isReady[status] = true;
    }
    void AutoAttack(Vector3 mousePosition)
    {
        Transform bulletTransform = Instantiate(fireball, mage.transform.position, Quaternion.identity);
        Vector3 shootDir = (mousePosition - mage.transform.position).normalized;
        bulletTransform.GetComponent<ProjectilesController>().Setup(shootDir);
    }

}
