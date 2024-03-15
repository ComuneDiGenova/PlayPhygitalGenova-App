using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // UI Scripts
    [SerializeField] UILogin loginUIScript;
    [SerializeField] UIHome homeUIScript;
    [SerializeField] UIMenu menuUIScript;
    [SerializeField] UICloseButton closeButtonUIScript;
    [SerializeField] UIItineraries itinerariesUIScript;
    [SerializeField] UIPOIExtendedInfo extendedInfoUIScript;

    // Functionality Scripts
    [SerializeField] CameraScript cameraScript;
    [SerializeField] GETUserInfo userInfo;

    // GameObjects
    [SerializeField] GameObject loginTab;
    [SerializeField] GameObject homeTab;

    [SerializeField] GameObject mainButtons;

    public static UIManager instance {get; private set;}
    private void Awake() {
        instance=this;
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        if (GameConfig.isLogged == false)
        {
            loginTab.SetActive(true);
            homeTab.SetActive(false);
            mainButtons.SetActive(false);
        }

        if (GameConfig.isLogged == true)
        {
            //CallLogIn();
            userInfo.LoginUserInfo(true);
        }

        
        if (GameConfig.isProUser)
        {
            //CallSignUpPro();
        }
    }

     /*-------------------------------------------------------------
     * ------------------------CloseTabs------------------------ *
     ------------------------------------------------------------- */

    public void CallCloseButton()
    {
        closeButtonUIScript.CloseTabUI();
    }

    /*-------------------------------------------------------------
     * ------------------------LogInScreen------------------------ *
     ------------------------------------------------------------- */


    /************************/

/*
    public void CallLogIn()
    {
        //homeUIScript.LogIn();
        //loginUIScript.LogInUI();
        GameConfig.proUserRequest=false;
    }
*/
    public void CallNoLogIn()
    {
        //GameConfig.proUserRequest=false;
        homeUIScript.LogIn();
        loginUIScript.NoLogInUI(null);
        mainButtons.SetActive(true);
    }

    public void CallGoHome()
    {
        loginUIScript.GoHomeUI();
        homeUIScript.ActiveMainButtons();
    }
/*
    public void CallSignUpUser()
    {
        loginUIScript.SignUpUserUI();
    }

    public void CallSignUpPro()
    {
        //Debug.Log("Entrato commerciante");
        //GameConfig.isLogged = false;
        //GameConfig.isProUser = true;
        loginUIScript.SignUpProUI();

        //GameConfig.proUserRequest=true;
    }
*/
    /*-------------------------------------------------------------
     * ------------------------HomeScreen------------------------ *
     ------------------------------------------------------------- */

    public void CallActiveMenu()
    {
        homeUIScript.ActiveLoggedMenuUI();
    }

    public void CallActiveWelcome()
    {
        homeUIScript.ActiveWelcomeUI();
    }

    public void CallActiveHelp()
    {
        homeUIScript.ActiveHelpUI();
    }

    public void CallARUI()
    {
        homeUIScript.ActiveARUI();
        
    }

    public void CallQRReaderUI()
    {
        homeUIScript.ActiveQRUI();
        cameraScript.ActiveQRCamera();
    }

    public void CallActiveAccount()
    {
        homeUIScript.ActiveAccountUI();
    }

    public void CallActiveSelector()
    {
        homeUIScript.ActivePOISelectorUI();
    }

    public void CallBackLogin()
    {

        CallCloseButton();
        homeUIScript.BackToLogIn();
        LoginSSO.LogOut();
    }

    /*-------------------------------------------------------------
     * ------------------------MenuScreen------------------------- *
     ------------------------------------------------------------- */

    public void CallActiveNearMe()
    {
        menuUIScript.ActiveNearMeUI();
    }

    public void CallActivePoints()
    {
        menuUIScript.ActivePointsUI();
    }

    public void CallActiveItineraries()
    {
        menuUIScript.ActiveItinerariesUI();
        CallOpenItinieraries();
    }

    public void CallActiveFavourites()
    {
        menuUIScript.ActiveFavouritesUI();
    }

    public void CallActiveAS()
    {
        menuUIScript.ActiveAccountSettingsUI();
    }

    /*-------------------------------------------------------------
     * ------------------------ItinerariesScreen----------------- *
     ------------------------------------------------------------- */

    public void CallActiveDI()
    {
        itinerariesUIScript.ActiveDefaultItineraries();
    }

    public void CallActiveCI()
    {
        itinerariesUIScript.ActiveCustomItineraries();
    }

    public void CallMinimizeItinieraries()  //chiamato da selezione itinerario
    {
        itinerariesUIScript.MinimizeItineraries();
    }
     public void CallSwitchMinimizeItinieraries()  //chiamato da frecce
    {
        itinerariesUIScript.SwitchMinimizedItineraries();
    }

    public void CallOpenItinieraries()
    {
        
        itinerariesUIScript.OpenItineraries();
        //itinerariesUIScript.OpenMinimizedItineraries();
    }

    public void Resetitineraries(){
        itinerariesUIScript.ResetItinerariesTab();
    }

    /*-------------------------------------------------------------
    * -----------------------ExtendedInfoScreen------------------- *
    ------------------------------------------------------------- */

    public void CallActiveExtendedInfo()
    {
        extendedInfoUIScript.ActiveExtendedInfo();
    }

}

