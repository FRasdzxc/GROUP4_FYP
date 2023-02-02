using System;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private static CursorController instance;
    public static CursorController Instance
    {
        get
        {
            return instance;
        }
    }

    [Serializable]
    public struct CursorEntry
    {
        public CursorType cursorType;
        public Texture2D texture;
        public Vector2 hotspot;
    }

    [SerializeField] private List<CursorEntry> cursors;
    [SerializeField] private CursorType defaultCursorType;

    private CursorEntry currentCursor;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        SetCursor(defaultCursorType);
    }

    public void SetCursor(CursorType cursorType)
    {
        for (int i = 0; i < cursors.Count; i++)
        {
            if (cursors[i].cursorType == cursorType)
            {
                Cursor.SetCursor(cursors[i].texture, cursors[i].hotspot, CursorMode.Auto);
                currentCursor = cursors[i];
                return;
            }
        }
    }

    public void SetDefaultCursor()
    {
        SetCursor(defaultCursorType);
    }
}
