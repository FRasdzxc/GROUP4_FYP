using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
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

        lineRenderer.SetPosition(0, mainCamera.transform.position);
        lineRenderer.SetPosition(1, mousePosition);
    }
}
