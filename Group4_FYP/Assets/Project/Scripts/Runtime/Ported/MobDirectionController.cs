using UnityEngine;

public class MobDirectionController : MonoBehaviour
{
    [SerializeField]
    private int activateCount = 5;

    [SerializeField]
    private int activateLimit = 10;

    [SerializeField]
    private GameObject mobDirectionGobj;

    [SerializeField]
    private GameObject mobDirectionHolder;

    public bool Activated { get; set; } = false;

    private static MobDirectionController m_instance;
    public static MobDirectionController Instance => m_instance;

    void Awake()
    {
        if (!m_instance)
            m_instance = this;
    }

    void Update()
    {
        // if (!Activated && GameManager.Instance.MapType != MapType.Peaceful && GameManager.Instance.MobCount == activateCount)
        //     Activated = true;

        if (Activated)
        {
            mobDirectionHolder.SetActive(true);
            if (GameManager.Instance.MobCount > activateLimit)
                Activated = false;
        }
        else
        {
            mobDirectionHolder.SetActive(false);
            if (GameManager.Instance.MapType != MapType.Peaceful && GameManager.Instance.MobCount <= activateCount)
                Activated = true;
        }
    }

    public void AddDirection(Transform mobTransform)
    {
        if (!mobTransform.CompareTag("Mob"))
            return;

        MobDirection clone = Instantiate(mobDirectionGobj, transform.position, Quaternion.identity, mobDirectionHolder.transform).GetComponent<MobDirection>();
        clone.MobTransform = mobTransform;
    }
}
