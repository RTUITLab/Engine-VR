using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControls : MonoBehaviour
{
    private bool restartShortcut;

    private void LateUpdate()
    {
        restartShortcut = Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftControl);
        if (restartShortcut)
        {
            RestartApp();
        }
    }

    public void RestartApp()
    {
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
    }
}
