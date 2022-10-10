using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    private bool bIsOpened = false;
    [SerializeField] private GameObject sideMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject pauseMenuBackground;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bIsOpened)
            {

            }
            else
            {
                sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.OutQuad);
            }
        }
    }
}
