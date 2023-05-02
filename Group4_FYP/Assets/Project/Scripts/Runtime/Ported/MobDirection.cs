using UnityEngine;

public class MobDirection : Direction
{
    public Transform MobTransform { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (MobTransform)
            Rotate(MobTransform.position);
        else
            Destroy(gameObject);
    }
}
