using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCData m_Data;


    // Start is called before the first frame update
    void Start()
    {
        Debug.LogFormat("Hello, I am {0}, having {1} hp and {2} mp", m_Data.Name, m_Data.Health, m_Data.Mana);
    }
}
