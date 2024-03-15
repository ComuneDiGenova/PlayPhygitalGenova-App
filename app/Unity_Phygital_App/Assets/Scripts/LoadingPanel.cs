using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadingPanel : MonoBehaviour
{
    static LoadingPanel instance;
    private void Awake() {
        instance = this;
        ClosePanel();
    }
    
    static int openings = 0;


    public static void OpenPanel(){
        ++openings;
        if (openings > 0) instance.gameObject.SetActive(true);
        //Debug.LogWarning("open " + openings);
    }

    public static void ClosePanel(bool force = false){
        if(force) openings = 0;
        else --openings;
        if (openings <= 0)
        {
            instance.gameObject.SetActive(false);
            openings = 0;
        }
        //Debug.LogWarning("close " + openings);
    }
}
