using UnityEngine;

public class InputManager : MonoBehaviour
{
    private KeyCode? previousKeyPressed;
    private bool panelEnabled;

    private static InputManager instance;
    public static InputManager Instance
    {
        get => instance;
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetKeyDown(KeyCode keyCode)
    {
        if (previousKeyPressed == null || panelEnabled == false)
        {
            previousKeyPressed = keyCode;
            panelEnabled = true;
            return true;
        }else if(panelEnabled == true && (keyCode == previousKeyPressed || keyCode == KeyCode.Escape))
        {
            panelEnabled = false;
            return true;
        }
        else 
        {
            return false;
        }

        // if (Input.GetKeyDown(keyCode) && (keyCode == previousKeyPressed || keyCode == KeyCode.Escape))
        // {
        //     // previousKeyPressed = null;
        //     return true;
        // }
        
        return false;
    }

    public void SetPreviousKeyPressed(KeyCode keyCode)
    {
        previousKeyPressed = keyCode;
    }
}
