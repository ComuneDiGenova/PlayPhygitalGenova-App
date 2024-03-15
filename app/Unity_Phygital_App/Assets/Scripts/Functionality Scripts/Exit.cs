using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public bool closeOnBack = false;
    private void Awake() {
        Input.backButtonLeavesApp = closeOnBack;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update() {
        /*
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
        */
    }

    public static void CloseApp(){
        Application.Quit();
    }
}
