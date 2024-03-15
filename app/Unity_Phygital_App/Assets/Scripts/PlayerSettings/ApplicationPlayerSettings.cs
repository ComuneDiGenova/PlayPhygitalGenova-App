using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ApplicationPlayerSettings : MonoBehaviour
{
    public static ApplicationPlayerSettings Instance;

    public bool useWSO2 = false;

    private void Awake()
    {
        GameConfig.useWSO2 = useWSO2;
        GameConfig.LoadPlayerPrefs();
    }

    private void Start() {
        GameConfig.ChangeLanguage(GameConfig.applicationLanguage);
    }

    

    [ContextMenu("ClearPlayerPrefs")]
    public void ClearPlayerPrefs()
    {
        GameConfig.ClearPlayerPrefs();
    }
}
