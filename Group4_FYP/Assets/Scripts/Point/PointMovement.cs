using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMovement : MonoBehaviour
{

    GameObject player;
    string storedType;
    int storedValue;
    public float speed;
    float velocity;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(storedType + " " + storedValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            velocity = speed * Time.deltaTime;
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if(distance <= 0.5f)
            {
                if(storedType == "coin")
                {
                    //player.GetComponent<PlayerData>().AddCoin(storedValue);
                    player.GetComponent<Hero>().AddCoin(storedValue);
                }
                if(storedType == "xp")
                {
                    //player.GetComponent<PlayerData>().AddEXP(storedValue);
                    player.GetComponent<Hero>().AddEXP(storedValue);
                }
                if(storedType == "hp")
                {
                    //add hp maybe
                    player.GetComponent<Hero>().ChangeHealth(storedValue);
                }
                if(storedType == "mp")
                {
                    //add mp maybe
                    player.GetComponent<AbilityManager>().ChangeMana(storedValue);
                }
                Destroy(gameObject);
            }
        }

        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, velocity);
    }

    public void SetValue(string type, int value)
    {
        storedType = type;
        storedValue = value;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, velocity);
        }
    }
}
