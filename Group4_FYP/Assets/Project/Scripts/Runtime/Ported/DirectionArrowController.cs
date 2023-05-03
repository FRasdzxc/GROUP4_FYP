using UnityEngine;
using PathOfHero.Utilities;

public class DirectionArrowController : Singleton<DirectionArrowController>
{
    [SerializeField]
    private int activateCount = 5;

    [SerializeField]
    private int activateLimit = 10;

    [SerializeField]
    private GameObject directionArrow;

    [SerializeField]
    private GameObject directionArrowHolder;

    public bool Activated { get; set; } = false;

    void Update()
    {
        // if (!Activated && GameManager.Instance.MapType != MapType.Peaceful && GameManager.Instance.MobCount == activateCount)
        //     Activated = true;

        if (Activated)
        {
            directionArrowHolder.SetActive(true);
            if (GameManager.Instance.MobCount > activateLimit)
                Activated = false;
        }
        else
        {
            directionArrowHolder.SetActive(false);
            if (GameManager.Instance.MapType != MapType.Peaceful && GameManager.Instance.MobCount <= activateCount)
                Activated = true;
        }
    }

    public void AddDirection(DirectionType directionType, Transform mobTransform)
    {
        DirectionArrow clone = Instantiate(directionArrow, transform.position, Quaternion.identity, directionArrowHolder.transform).GetComponent<DirectionArrow>();
        clone.SetUp(directionType, mobTransform);
    }
}
