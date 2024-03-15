using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButtonsSprite : MonoBehaviour
{
    // Near Me Buttons
    [SerializeField] GameObject nearMeButton;
    [SerializeField] Sprite nearMeDisactiveImage;
    [SerializeField] Sprite nearMeActiveImage;
    [SerializeField] TMP_Text nearMeText;

    // Favourties Button
    [SerializeField] GameObject favouritesButton;
    [SerializeField] Sprite favouritesDisactiveImage;
    [SerializeField] Sprite favouritesActiveImage;
    [SerializeField] TMP_Text favouritesText;

    //Itineraries Button
    [SerializeField] GameObject itinerariesButton;
    [SerializeField] Sprite itinerariesDisactiveImage;
    [SerializeField] Sprite itinerariesActiveImage;
    [SerializeField] TMP_Text itinerariesText;

    // Colors
    Color defaultColor = new Color(0, 0, 0, 255); 
    Color activeColor = new Color(255, 255, 255, 255);

    // Bools
    bool nearMeActive = false;
    bool favouritesActive = false;
    bool itinerariesActive = false;

    public void SetNearMeSprite()
    {
        // Questa funzione cambia lo sprite del bottone Vicino a Me
        if (!nearMeActive)
        {
            nearMeButton.GetComponent<Image>().sprite = nearMeActiveImage;
            nearMeText.GetComponent<TMP_Text>().color = activeColor;
            nearMeActive = true;
        }
        else if (nearMeActive)
        {
            nearMeButton.GetComponent<Image>().sprite = nearMeDisactiveImage;
            nearMeText.GetComponent<TMP_Text>().color = defaultColor;
            nearMeActive = false;
        }
    }

    public void SetFavouriteSprite() 
    {
        // Questa funzione cambia lo sprite del bottone Preferiti
        if (!favouritesActive)
        {
            favouritesButton.GetComponent<Image>().sprite = favouritesActiveImage;
            favouritesText.GetComponent<TMP_Text>().color = activeColor;
            favouritesActive = true;
        }
        else if (favouritesActive) 
        {
            favouritesButton.GetComponent<Image>().sprite = favouritesDisactiveImage;
            favouritesText.GetComponent<TMP_Text>().color = defaultColor;
            favouritesActive = false;
        }
    }

    public void SetItinerariesSprite()
    {
        // Questa funzione cambia lo sprite del bottone Preferiti

        if (!itinerariesActive)
        {
            itinerariesButton.GetComponent<Image>().sprite = itinerariesActiveImage;
            itinerariesText.GetComponent<TMP_Text>().color = activeColor;
            itinerariesActive = true;
        }
        else if (itinerariesActive)
        {
            itinerariesButton.GetComponent<Image>().sprite = itinerariesDisactiveImage;
            itinerariesText.GetComponent<TMP_Text>().color = defaultColor;
            itinerariesActive = false;
        }
    }

    public void ResetNavButtons()
    {
        // Resetta il bottone "Vicino a Me"
        nearMeButton.GetComponent<Image>().sprite = nearMeDisactiveImage;
        nearMeText.GetComponent<TMP_Text>().color = defaultColor;
        nearMeActive = false;

        // Resetta il bottone "Preferiti"
        favouritesButton.GetComponent<Image>().sprite = favouritesDisactiveImage;
        favouritesText.GetComponent<TMP_Text>().color = defaultColor;
        favouritesActive = false;

        // Resetta il bottone "Itinerari"
        itinerariesButton.GetComponent<Image>().sprite = itinerariesDisactiveImage;
        itinerariesText.GetComponent<TMP_Text>().color = defaultColor;
        itinerariesActive = false;
    }
}
