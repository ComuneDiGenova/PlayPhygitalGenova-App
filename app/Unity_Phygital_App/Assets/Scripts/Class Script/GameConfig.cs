using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Languages { IT = 0, EN = 1, FR = 2, ES = 4, DE = 3, RU = 5 }   
public static class GameConfig
{
    public static Languages applicationLanguage {get; private set;}

    public static bool? backgroundGPS = null;
    public static bool gpsWaiting = false;
    public static string userID = null;
    public static string userDrupalId = null;
    
    public static bool isLogged = false;

    public static bool isProUser = false;
    public static bool isLoggedPro = false;
    public static bool proUserRequest = false;

    public static bool useWSO2 = false;

    public delegate void LanguageChange(Languages language);
    public static event LanguageChange OnLanguageChange;
    public static void ChangeLanguage(Languages language){
        applicationLanguage = language;
        SetPlayerPrefs();
        OnLanguageChange?.Invoke(language);
    }

    public static void SetPlayerPrefs()
    {
        Debug.Log("Salvando PlayerPrefs");
        PlayerPrefs.SetString("gps", GameConfig.backgroundGPS == null ? "n" : (GameConfig.backgroundGPS == true ? "t" : "f"));
        PlayerPrefs.SetString("userID", GameConfig.userID);
        PlayerPrefs.SetString("drupalID", GameConfig.userDrupalId);
        PlayerPrefs.SetInt("isLogged", GameConfig.isLogged ? 1 : 0);
        PlayerPrefs.SetInt("isLoggedPro", GameConfig.isLoggedPro ? 1 : 0);
        //PlayerPrefs.SetInt("isProUser", GameConfig.isProUser ? 1 : 0);
        PlayerPrefs.SetInt("language", (int)applicationLanguage);
        PlayerPrefs.Save();
    }

    public static void LoadPlayerPrefs()
    {
        Debug.Log("Caricando PlayerPrefs");
        var gps = PlayerPrefs.GetString("gps", "n");
        switch(gps){
            case "n": GameConfig.backgroundGPS = null; break;
            case "t": GameConfig.backgroundGPS = true; break;
            case "f": GameConfig.backgroundGPS = false; break;
        }
        GameConfig.userID = PlayerPrefs.GetString("userID", null);
        GameConfig.userDrupalId = PlayerPrefs.GetString("drupalID", null);
        GameConfig.isLogged = PlayerPrefs.GetInt("isLogged",0) == 1;
        GameConfig.isLoggedPro = PlayerPrefs.GetInt("isLoggedPro",0) == 1;
        //GameConfig.isProUser = PlayerPrefs.GetInt("isProUser",0) == 1;
        GameConfig.applicationLanguage = (Languages)PlayerPrefs.GetInt("language", (int)GetSysLanguage());

        Debug.Log("Id utente" + GameConfig.userID + " | " + GameConfig.userDrupalId);
        Debug.Log("Is logged" + GameConfig.isLogged);
       // Debug.Log("Is Pro " + GameConfig.isProUser);
        Debug.Log("Is LoggedPro " + GameConfig.isLoggedPro);
        Debug.Log("Language " + GameConfig.applicationLanguage);
    }

    static Languages GetSysLanguage(){
        switch(Application.systemLanguage){
            case SystemLanguage.Italian: return Languages.IT;
            case SystemLanguage.English: return Languages.EN;
            case SystemLanguage.Spanish: return Languages.ES;
            case SystemLanguage.French: return Languages.FR;
            case SystemLanguage.German: return Languages.DE;
            case SystemLanguage.Russian: return Languages.RU;
            default: return Languages.IT;
        }
    }

    public static void ClearPlayerPrefs()
    {
        Debug.Log("Eliminando PlayerPrefs");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}

