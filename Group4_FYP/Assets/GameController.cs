using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool allMobsKilled;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!allMobsKilled && GameObject.FindGameObjectsWithTag("Mob").Length <= 0)
        {
            allMobsKilled = true;
            Debug.Log("All mobs killed");

            // spawn portal maybe
        }
    }
}
