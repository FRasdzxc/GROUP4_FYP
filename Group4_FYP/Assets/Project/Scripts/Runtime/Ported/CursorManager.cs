using UnityEngine;
using PathOfHero.Controllers;
using PathOfHero.Utilities;

public class CursorManager : Singleton<CursorManager>
{
    [SerializeField]
    private CursorController.CursorType defaultCursorType;

    [SerializeField]
    private CursorController cursorController;

    // Start is called before the first frame update
    void Start()
        => cursorController.ChangeCursor(defaultCursorType);

    public CursorController.CursorType GetDefaultCursorType() { return defaultCursorType; }
}
