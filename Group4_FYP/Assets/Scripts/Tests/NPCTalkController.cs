using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalkController : MonoBehaviour
{
    public string[] content;
    public string defaultContent;
    public Text NPCTalk;
    public GameObject panel;
    bool firstTalking = true;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            panel.SetActive(true);
            if (firstTalking == true)
            {
                NPCTalk.text = content[i];
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            i++;
            if (i < content.Length)
            {
                NPCTalk.text = content[i];
            }
            else
            {
                firstTalking = false;
                NPCTalk.text = defaultContent;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panel.SetActive(false);
        }
    }
}
