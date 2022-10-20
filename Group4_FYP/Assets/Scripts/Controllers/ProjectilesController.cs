using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesController : MonoBehaviour
{
    private Vector3 shootDir;
    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void Update()
    {
        transform.position += shootDir * Time.deltaTime;
    }
}
