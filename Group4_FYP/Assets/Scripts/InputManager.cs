using UnityEngine;

public class InputManager : MonoBehaviour
{
    private KeyCode? previousKeyPressed;
    private bool panelEnabled;

    private static InputManager instance;
    public static InputManager Instance => instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool GetKeyDown(KeyCode keyCode)
    {
        if (previousKeyPressed == null || panelEnabled == false)
        {
            previousKeyPressed = keyCode;
            panelEnabled = true;
            return true;
        }
        else if (panelEnabled == true && (keyCode == previousKeyPressed || keyCode == KeyCode.Escape))
        {
            panelEnabled = false;
            return true;
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
