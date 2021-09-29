using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControls : MonoBehaviour
{
    private bool restartShortcut;
    private bool displayDevConsole = false;

    private void Update()
    {
        restartShortcut = Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftControl);
        if (restartShortcut)
        {
            RestartApp();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            displayDevConsole = !displayDevConsole;
            Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
        }

        Debug.developerConsoleVisible = displayDevConsole;

    }

    public void RestartApp()
    {
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
    }
}
