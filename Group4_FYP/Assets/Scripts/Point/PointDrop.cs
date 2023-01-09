using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDrop : MonoBehaviour
{
    public GameObject coin;
    public GameObject xp;
    public GameObject hp;
    public GameObject mp;
    public bool coinDrop;
    public bool xpDrop;
    public bool hpDrop;
    public bool mpDrop;
    public int coinAmount;
    public int xpAmount;
    public int hpAmount;
    public int mpAmount;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(coinDrop == true)
        {
            Vector2 randomPosition = new Vector2(transform.position.x + Random.Range(-offset, offset), transform.position.y + Random.Range(-offset, offset));
            GameObject coinObj = Instantiate(coin, randomPosition, Quaternion.identity);
            coinObj.GetComponent<PointMovement>().SetValue("coin", coinAmount);
        }
        if (xpDrop == true)
        {
            Vector2 randomPosition = new Vector2(transform.position.x + Random.Range(-offset, offset), transform.position.y + Random.Range(-offset, offset));
            GameObject xpObj = Instantiate(xp, randomPosition, Quaternion.identity);
            xpObj.GetComponent<PointMovement>().SetValue("xp", xpAmount);
        }
        if (hpDrop == true)
        {
            Vector2 randomPosition = new Vector2(transform.position.x + Random.Range(-offset, offset), transform.position.y + Random.Range(-offset, offset));
            GameObject hpObj = Instantiate(hp, randomPosition, Quaternion.identity);
            hpObj.GetComponent<PointMovement>().SetValue("hp", hpAmount);
        }
        if (mpDrop == true)
        {
            Vector2 randomPosition = new Vector2(transform.position.x + Random.Range(-offset, offset), transform.position.y + Random.Range(-offset, offset));
            GameObject mpObj = Instantiate(mp, randomPosition, Quaternion.identity);
            mpObj.GetComponent<PointMovement>().SetValue("mp", mpAmount);
        }

    }
}
