using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UICloseButton : MonoBehaviour
{
    [SerializeField] bool closeAppOnBack = true;

    // Questo script serve per capire quale schermata � attiva e chiuderla
    [SerializeField] UIMenu menuUIScript;
    [SerializeField] CameraScript cameraScript;
    [SerializeField] UIActivateMarkerTab markerScript;
    [SerializeField] ActivateAR arScript;

    // Home Tabs
    [SerializeField] GameObject homeTab;
    [SerializeField] GameObject welcomeTab;
    [SerializeField] GameObject menuTab;
    [SerializeField] GameObject helpTab;

    [SerializeField] GameObject arTab;
    //[SerializeField] GameObject arLocationObject;

    [SerializeField] GameObject qrTab;
    //[SerializeField] GameObject accountTab;   
    [SerializeField] GameObject pointsTab;
    [SerializeField] GameObject poiInfoTab;
    [SerializeField] GameObject poiExtendedInfoTab;

    // Pro User Tab
    [SerializeField] GameObject accountProTab;

    // Menu Tabs
    [SerializeField] GameObject nearMeTab;
    [SerializeField] GameObject itinerariesTab;
    [SerializeField] GameObject favouritesTab;
    [SerializeField] GameObject accountSettingsTab;

    [SerializeField] GameObject menuButton;
    [SerializeField] GameObject accountProButton;
    [SerializeField] GameObject closeButton;
    [SerializeField] GameObject closeProButton;
    [SerializeField] GameObject arButton;
    [SerializeField] GameObject helpButton;

    // Scrollview tab poi
    [SerializeField] Scrollbar poiScrollbar;

    // Ar tab
    [SerializeField] GameObject arPoiTab;
    [SerializeField] GameObject arShopTab;

    public void CloseTabUI()
    {
        // chiudo welcome
        if (welcomeTab.activeSelf == true)
        {
            Debug.Log("Chiuso welcome");
            homeTab.SetActive(true);

            welcomeTab.SetActive(false);

            menuButton.SetActive(true);
            helpButton.SetActive(true);
            arButton.SetActive(true);
            closeButton.SetActive(false);
        }

        // Chiudo menu
        if (menuTab.activeSelf == true)
        {
            Debug.Log("Chiuso menu");
            menuTab.SetActive(false);

            menuButton.SetActive(true);
            closeButton.SetActive(false);
        }

        // Chiudo aiuto
        if (helpTab.activeSelf == true)
        {
            Debug.Log("Chiuso aiuto");
            
            homeTab.SetActive(true);
            helpTab.SetActive(false);

            menuButton.SetActive(true);
            helpButton.SetActive(true);
            arButton.SetActive(true);
            closeButton.SetActive(false);
        }

        // Chiudo AR
        if (arTab.activeSelf == true)
        {
            // Chiudi Informazioni Estese POI
            if (poiExtendedInfoTab.activeSelf == true){
                poiExtendedInfoTab.SetActive(false);
                //non bloccare audio
            }else{
                arPoiTab.SetActive(false);
                arShopTab.SetActive(false);
                Debug.Log("Chiuso ar");
                arScript.StopARCamera();
                GETAudio.StopAudio();
                ActivateAR.AvatarOff();
            }
        }

        // Chiudo QR
        if (qrTab.activeSelf == true)
        {
            Debug.Log("Chiuso QR");

            qrTab.SetActive(false);
            accountProButton.SetActive(true);
            closeProButton.SetActive(false);
            cameraScript.Close();
        }

        /*
        // Chiudo Account
        if (accountTab.activeSelf == true)
        {
            Debug.Log("Chiuso account");

            homeTab.SetActive(true);
            accountTab.SetActive(false);

            menuButton.SetActive(true);
            helpButton.SetActive(true);
            arButton.SetActive(true);
            closeButton.SetActive(false);
        }
        */

        /*-------------------------------------------------------------
        * ------------------------ButtonMenu------------------------ *
        ------------------------------------------------------------- */

        // Chiudo Account Pro
        if (accountProTab.activeSelf == true) 
        {
            Debug.Log("Chiuso account pro");

            accountProTab.SetActive(false);
            accountProButton.SetActive(true);
            closeProButton.SetActive(false);
        }

        /*-------------------------------------------------------------
        * ------------------------ButtonMenu------------------------ *
        ------------------------------------------------------------- */

        // Chiudo Account
        if (accountSettingsTab.activeSelf == true)
        {
            Debug.Log("Chiuso impostazioni account");

            homeTab.SetActive(true);
            accountSettingsTab.SetActive(false);

            menuButton.SetActive(true);
            helpButton.SetActive(true);
            arButton.SetActive(true);
            closeButton.SetActive(false);
            closeProButton.SetActive(false);
            accountProButton.SetActive(true);
        }

        // Chiudo Punti
        if (pointsTab.activeSelf == true)
        {
            Debug.Log("Chiuso punti");

            homeTab.SetActive(true);
            pointsTab.SetActive(false);

            menuButton.SetActive(true);
            helpButton.SetActive(true);
            arButton.SetActive(true);
            closeButton.SetActive(false);
        }
        
        // Chiudi Vicino a Me
        if (nearMeTab.activeSelf == true)
        {
            Debug.Log("Chiuso Vicino a Me");

            menuUIScript.ActiveNearMeUI();
        }

        // Chiudi Itinerario
        if (itinerariesTab.activeSelf == true)
        {
            Debug.Log("Chiuso Itinerari");

            menuUIScript.ActiveItinerariesUI();
        }

        // Chiudi Preferiti
        if (favouritesTab.activeSelf == true)
        {
            Debug.Log("Chiuso Preferiti");

            menuUIScript.ActiveFavouritesUI();
        }

        // Chiudi Informazioni Estese POI
        if (poiExtendedInfoTab.activeSelf == true)
        {
            Debug.Log("Chiuso POI Esteso");

            poiScrollbar.value = 1f;
            poiExtendedInfoTab.SetActive(false);
            poiInfoTab.SetActive(false);
            markerScript.CloseMarkerTab();

            menuButton.SetActive(true);
            helpButton.SetActive(true);
            arButton.SetActive(true);
            closeButton.SetActive(false);

            GETAudio.StopAudio();
        }


        /*-------------------------------------------------------------
        * ------------------------CLOSE APP------------------------ *
        ------------------------------------------------------------- */

    }
}
