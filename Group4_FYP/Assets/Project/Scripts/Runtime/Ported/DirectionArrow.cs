using UnityEngine;

public class DirectionArrow : Direction
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public Transform TargetTransform { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (TargetTransform)
            Rotate(TargetTransform.position);
        else
            Destroy(gameObject);
    }

    public void SetUp(DirectionType directionType, Transform targetTransform)
    {
        switch (directionType)
        {
            case DirectionType.Mob:
                spriteRenderer.color = Color.red;
                break;
            case DirectionType.Object:
                spriteRenderer.color = Color.yellow;
                break;
            case DirectionType.ReturnPortal:
                spriteRenderer.color = Color.magenta;
                break;
            default:
                break;
        }

        TargetTransform = targetTransform;
    }
}
