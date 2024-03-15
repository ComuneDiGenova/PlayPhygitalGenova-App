using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] UIHome homeUIScript;
    [SerializeField] UIButtonsSprite buttonsSpriteUIScript;
    [SerializeField] UIItineraries itinerariesUIScript;
    
    //[SerializeField] GameObject nearMeButton;
    //[SerializeField] GameObject pointsButton;
    //[SerializeField] GameObject itinerariesButton;
    //[SerializeField] GameObject favouritesButton;
    //[SerializeField] GameObject accountSettingsButton;

    [SerializeField] GameObject GPRPoiPanel;

    [SerializeField] GameObject menuTab;
    [SerializeField] GameObject nearMeTab;
    [SerializeField] GameObject pointsTab;
    //[SerializeField] GameObject pointsProTab;
    [SerializeField] GameObject accountSettingsTab;
    [SerializeField] GameObject itinerariesTab;
    [SerializeField] GameObject favouritesTab;

    [SerializeField] GameObject closeItinerariesPanel;

    private void Start() {
        nearMeTab.SetActive(false);
        favouritesTab.SetActive(false);
        GPRPoiPanel.SetActive(false);
    }

    public void ActiveNearMeUI()
    {
        // Questa funzione attiva la schermata vicino a me

        if (itinerariesTab.activeSelf == true)
            ActiveItinerariesUI();

        if (favouritesTab.activeSelf == true)
            ActiveFavouritesUI();
        
        nearMeTab.SetActive(!nearMeTab.activeSelf);
        GPRPoiPanel.SetActive(nearMeTab.activeSelf);

        buttonsSpriteUIScript.SetNearMeSprite();
    }

    public void ActiveFavouritesUI()
    {
        // Questa funzione attiva la schermata preferiti

        if (itinerariesTab.activeSelf == true)
            ActiveItinerariesUI();

        if (nearMeTab.activeSelf == true)
            ActiveNearMeUI();


        favouritesTab.SetActive(!favouritesTab.activeSelf);
        GPRPoiPanel.SetActive(favouritesTab.activeSelf);

        buttonsSpriteUIScript.SetFavouriteSprite();
    }

    public void ActivePointsUI()
    {
        // Questa funzione attiva la schermata punti

        if (menuTab.activeSelf == true)
            menuTab.SetActive(false);

        pointsTab.SetActive(true);

        homeUIScript.DisactiveMainButtons();
    }

    public void ActiveItinerariesUI()
    {
        // Questa funzione attiva la schermata itinerari

        if (nearMeTab.activeSelf == true)
            ActiveNearMeUI();

        if (favouritesTab.activeSelf == true)
            ActiveFavouritesUI();



        if (itinerariesTab.activeSelf == false)
        {
            itinerariesTab.SetActive(true);
            if (ItinerariesManager.itinerarioSelezionato != null)
            {
                closeItinerariesPanel.SetActive(false);
            }
            else closeItinerariesPanel.SetActive(true);
            //closeItinerariesPanel.SetActive(true);
        }
        else
        {
            //itinerariesUIScript.ResetItinerariesTab();
            itinerariesTab.SetActive(false);
        }

        buttonsSpriteUIScript.SetItinerariesSprite();
    }

    

    public void ActiveAccountSettingsUI()
    {
        // Questa funzione attiva la schermata impostazioni

        if (menuTab.activeSelf == true)
            menuTab.SetActive(false);

        accountSettingsTab.SetActive(true);

        homeUIScript.DisactiveMainButtons();
    }
}
