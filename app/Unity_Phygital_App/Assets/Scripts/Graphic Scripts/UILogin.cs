using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogin : MonoBehaviour
{
    [SerializeField] UIHome homeUIScript;

    [SerializeField] GameObject logInScreen;
    [SerializeField] GameObject welcomeScreen;
    [SerializeField] GameObject homeScreen;
    [SerializeField] GameObject proScreen;
    [SerializeField] GameObject buttonMain;
     [SerializeField] GameObject mapCover;
    


    private void Awake() {
        mapCover.SetActive(true);
        GETUserInfo.OnLoginUserInfo += LogInUI;
        //LoginSSO.OnLoginError += NoLogInUI;
        // agigungere evento per errore login
    }

    public void LogInUI()
    {
        // Questa funzione verrà chiamata dopo che le credenziali saranno verificate
        if(GameConfig.isLogged){
            Debug.Log("entrando utente");
            GoHomeUI();
        }
    }

    public void NoLogInUI(string message)
    {
        // Questa funzione verrà chiamata se non verrà effettuato l'accesso
        Debug.Log("entrando anonimo");
        GameConfig.userID = null;
        GameConfig.proUserRequest = false;
        GameConfig.isLogged = false;
        GameConfig.isLoggedPro = false;
        GameConfig.SetPlayerPrefs();
        GoHomeUI();
    }

    public void GoHomeUI()
    {
        // Questa funzione verr� chiamata per accedere alla schermata principale dalla schermata di benvenuto
        if(GameConfig.isLoggedPro){
            Debug.Log("pro");
            proScreen.SetActive(true);
            logInScreen.SetActive(false);
            buttonMain.SetActive(false);
            mapCover.SetActive(true);
        }else{
            homeScreen.SetActive(true);
            welcomeScreen.SetActive(false);
            logInScreen.SetActive(false);
            homeUIScript.LogIn();
            buttonMain.SetActive(true);
            proScreen.SetActive(false);
            mapCover.SetActive(false);
        }
    }

    public void BackLogInUI()
    {
        // Questa funzione riporter� alla schermata iniziale di logIn
        logInScreen.SetActive(true);
    }
}
