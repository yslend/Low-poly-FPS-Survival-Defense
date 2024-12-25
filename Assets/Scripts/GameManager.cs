using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;

    public static bool IsOpenInventory = false;
    public static bool IsOpenCraftManual = false;
    public static bool IsOpenArchemyTable = false;
    public static bool IsOpenComputerKit = false;

    public static bool isPause = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOpenInventory || IsOpenCraftManual || IsOpenComputerKit || IsOpenArchemyTable || isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }
            
    }
}
