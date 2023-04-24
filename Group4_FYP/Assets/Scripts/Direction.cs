using UnityEngine;

public class Direction : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Rotate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    protected void Rotate(Vector3 target)
    {
        Vector2 dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
