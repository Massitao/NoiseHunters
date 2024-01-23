using UnityEngine;

public class CursorHandle
{
    public static void CursorLockHandle(CursorLockMode lockModeToChoose, bool isCursorVisible)
    {
        Cursor.visible = isCursorVisible;

        if (lockModeToChoose == CursorLockMode.Confined)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = lockModeToChoose;
        }
        else
        {
            Cursor.lockState = lockModeToChoose;
        }
    }
}
