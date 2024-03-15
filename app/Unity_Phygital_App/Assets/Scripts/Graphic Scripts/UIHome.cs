using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : MonoBehaviour
{
    [SerializeField] GameObject logoButton;
    [SerializeField] GameObject accountButton;
    [SerializeField] GameObject accountProButton;
    [SerializeField] GameObject arButton;
    [SerializeField] GameObject menuButton;
    [SerializeField] GameObject closeMenuButton;
    [SerializeField] GameObject closeProButton;
    [SerializeField] GameObject settingsProButton;
    [SerializeField] GameObject helpButton;
    [SerializeField] GameObject mainButton;
    [SerializeField] GameObject poiSelectorButton;

    [SerializeField] GameObject menuPointsButton;
    [SerializeField] GameObject menuAccountButton;
    [SerializeField] GameObject menuFavouritesButton;
    [SerializeField] GameObject menuMyItinerariesButton;

    [SerializeField] GameObject welcomeNextButton;

    [SerializeField] GameObject logInTab;
    [SerializeField] GameObject homeTab;
    [SerializeField] GameObject welcomeTab;
    [SerializeField] GameObject menuTab;
    [SerializeField] GameObject helpTab;
    [SerializeField] GameObject qrReaderTab;
    [SerializeField] GameObject accountTab;
    [SerializeField] GameObject accountProTab;
    [SerializeField] GameObject poiSelectorTab;
    [SerializeField] GameObject userSettingTab;
    [SerializeField] GameObject proUserTab;

    // NO LOG IN BUTTONS
    [SerializeField] GameObject homeLoginButton;

    [SerializeField] GameObject goNextButton;


    public void ActiveMainButtons()
    {
        mainButton.SetActive(true);
    }

    public void ActiveWelcomeUI()
    {
        closeMenuButton.SetActive(true);

        welcomeNextButton.SetActive(false);

        menuButton.SetActive(false);
        helpButton.SetActive(false);
        arButton.SetActive(false);

        welcomeTab.SetActive(true);
        if (homeTab.activeSelf == true)
            homeTab.SetActive(false);

        if (menuTab.activeSelf == true)
            menuTab.SetActive(false);
    }

    public void ActiveLoggedMenuUI()
    {
        // Questa funzione attiva il men� principale

        closeMenuButton.SetActive(true);
        menuButton.SetActive(false);

        menuTab.SetActive(true);       
    }


    public void LogIn()
    {
 
        accountButton.GetComponent<Button>().interactable = GameConfig.isLogged;
        menuPointsButton.GetComponent<Button>().interactable = GameConfig.isLogged;
        menuFavouritesButton.GetComponent<Button>().interactable = GameConfig.isLogged;
        menuMyItinerariesButton.GetComponent<Button>().interactable = GameConfig.isLogged;

        //accountButton.SetActive(GameConfig.isLogged);
        accountButton.SetActive(false);
        homeLoginButton.SetActive(!GameConfig.isLogged);
        menuAccountButton.SetActive(GameConfig.isLogged);

        if (GameConfig.isLogged)
        {
            if (!GameConfig.isLoggedPro)
            {
                ActiveMainButtons();
                ActiveLoggedMenuUI();
            }
            else
            {
                mainButton.SetActive(false);
            }
        }

    }

    public void BackToLogIn()
    {
        logInTab.SetActive(true);
        welcomeTab.SetActive(false);
        mainButton.SetActive(false);
        menuTab.SetActive(false);
        closeMenuButton.SetActive(false);
        menuButton.SetActive(true);
        arButton.SetActive(true);
        helpButton.SetActive(true);
        proUserTab.SetActive(false);

        goNextButton.SetActive(true);

        userSettingTab.SetActive(false);

        accountProTab.SetActive(false);
        proUserTab.SetActive(false);
        closeProButton.SetActive(false);
        settingsProButton.SetActive(true);

        homeTab.SetActive(false);
    }

    public void ActiveHelpUI()
    {
        // Questa funzione attiva la scheda di aiuto

        //homeTab.SetActive(false);

        if(menuTab.activeSelf == true)
            menuTab.SetActive(false);

        helpTab.SetActive(true);

        helpButton.SetActive(false);
        arButton.SetActive(false);

        closeMenuButton.SetActive(true);
        menuButton.SetActive(false);
    }

    public void ActiveARUI()
    {
        // Questa funzione attiva la scheda AR

        homeTab.SetActive(false);

        if (menuTab.activeSelf == true)
            menuTab.SetActive(false);

        DisactiveMainButtons();
    }

    public void ActiveQRUI()
    {
        // Questa funzione attiva il lettore QR

        qrReaderTab.SetActive(true);
        closeProButton.SetActive(true);

        DisactiveMainButtons();
    }

    public void ActiveAccountUI()
    {
        // Questa funzione attiva la scheda AR
        
        homeTab.SetActive(false);
        
        if (menuTab.activeSelf == true)
            menuTab.SetActive(false);
/*
        if (!GameConfig.isLoggedPro)
            accountTab.SetActive(true);
        else
        {
            accountProTab.SetActive(true);
            closeProButton.SetActive(true);
        }
*/
        accountTab.SetActive(true);
        if (GameConfig.isLoggedPro)
            closeProButton.SetActive(true);
        DisactiveMainButtons();
    }

    public void ActivePOISelectorUI()
    {
        if (!poiSelectorTab.activeSelf)
        {
            poiSelectorTab.SetActive(true);
        }
        else if (poiSelectorTab.activeSelf)
        {
            poiSelectorTab.SetActive(false);
        }
    }

    public void DisactiveMainButtons()
    {
        // Funzione per gestire i bottoni generali della schermata home

        helpButton.SetActive(false);
        arButton.SetActive(false);

        closeMenuButton.SetActive(true);
        menuButton.SetActive(false);

        if(GameConfig.isLoggedPro)
            accountProButton.SetActive(false);
    }
}
