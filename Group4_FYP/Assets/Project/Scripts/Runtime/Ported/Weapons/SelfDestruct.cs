using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField]
    private float lifeTime;

    [SerializeField]
    private GameObject smoke;
    
    [SerializeField]
    private Color smokeColor = Color.white;

    private float selfDestructTime;

    private bool isDestroying;

    void Start()
        => selfDestructTime = Time.time + lifeTime;

    void FixedUpdate()
    {
        if (!isDestroying && Time.time >= selfDestructTime)
        {
            isDestroying = true;

            if (smoke)
            {
                GameObject smokeClone = Instantiate(smoke, gameObject.transform.position, Quaternion.identity);
                smokeClone.GetComponent<SpriteRenderer>().color = smokeColor;
            }
            Destroy(gameObject);
        }
    }
}
