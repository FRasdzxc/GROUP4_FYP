using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HeroPanel : MonoBehaviour
{
    [SerializeField] private GameObject heroPanel;

    private bool isOpened;

    // Start is called before the first frame update
    void Start()
    {
        isOpened = false;
        heroPanel.SetActive(false);
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isOpened)
            {
                await HideHeroPanel();
            }
            else
            {
                await ShowHeroPanel();
            }

            isOpened = !isOpened;
        }
    }

    private async Task ShowHeroPanel()
    {
        heroPanel.SetActive(true);
    }

    private async Task HideHeroPanel()
    {
        heroPanel.SetActive(false);
    }
}
