using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // temporary only

    private bool allMobsKilled;
    private HUD hud;
    private SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        sceneController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<SceneController>();
    }

    // Update is called once per frame
    async void Update()
    {
        if (!allMobsKilled && GameObject.FindGameObjectsWithTag("Mob").Length <= 0)
        {
            allMobsKilled = true;
            Debug.Log("All mobs killed");

            // spawn portal maybe

            await hud.ShowHugeMessage("All Clear", 1.5f, Color.green); // temporary only
            NextLevel();
        }
    }

    private void NextLevel() // temporary only
    {
        if (nextSceneName != null && nextSceneName != "")
        {
            sceneController.ChangeScene(nextSceneName);
        }
        else
        {
            sceneController.ChangeScene("StartScene");
        }
    }
}
