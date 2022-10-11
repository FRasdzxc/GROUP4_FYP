using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D cursorDefaultTexture;
    [SerializeField] private Texture2D cursorCrosshairTexture;
    private Vector2 cursorCrosshairHotspot;

    // Start is called before the first frame update
    void Start()
    {
        cursorCrosshairHotspot = new Vector2(16, 16);

        Cursor.SetCursor(cursorDefaultTexture, Vector2.zero, CursorMode.ForceSoftware);
        // Cursor.SetCursor(cursorCrosshairTexture, cursorCrosshairHotspot, CursorMode.ForceSoftware);
    }
}
