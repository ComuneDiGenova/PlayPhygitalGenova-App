using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class AddFavourite : MonoBehaviour
{
    [SerializeField] GameObject favouriteLogInTab;

    [SerializeField] Image favouriteImage;

    [SerializeField] Sprite notFavouriteSprite;
    [SerializeField] Sprite favouriteSprite;

    public static string favouriteClickedMarkerLabel;

    public void AddToFavourite()
    {
        if (!GameConfig.isLogged)
        {
            favouriteLogInTab.SetActive(true);
        }

        if ( GETUserInfo.FavouriteResponse == null) return;

        if (string.IsNullOrEmpty(favouriteClickedMarkerLabel))
        {
            Debug.LogError("Nessun Marker Trovato");
            return;
        }
  
        var favouritePoiTrovato = GETUserInfo.FavouriteResponse.favourites.Where((x) => (x.id.ToString() == favouriteClickedMarkerLabel)).FirstOrDefault();

        Debug.Log("Marker cliccato: " + favouriteClickedMarkerLabel);
        Debug.Log("Preferito trovato: " + favouritePoiTrovato);


        if (GameConfig.isLogged)
        {
            if (favouritePoiTrovato == null)
            {
                //StartCoroutine(POSTFavourites.POSTAddFavourite(favouriteClickedMarkerLabel, GetAddFavouriteResponse));
                POSTFavourites.AddFavourite(favouriteClickedMarkerLabel, GetAddFavouriteResponse);
            }
            else
            {
                //StartCoroutine(POSTFavourites.POSTRemoveFavourite(favouriteClickedMarkerLabel, GetRemoveFavouriteResponse));
                POSTFavourites.RemoveFavourite(favouriteClickedMarkerLabel, GetRemoveFavouriteResponse);
            }
            //StartCoroutine(GETUserInfo.GETFavourites(3));
        }

    }
    public void CloseFavLogInTab()
    {
        favouriteLogInTab.SetActive(false);
    }
    void GetAddFavouriteResponse(AddFavouriteResponse response)
    {
        if (response.result == true)
        {
            favouriteImage.sprite = favouriteSprite;
            SendExtendedPoint(favouriteClickedMarkerLabel);
        }
        else if (response.result == false)
        {
            Debug.Log(response.message + ".");
        }
        //StartCoroutine(GETUserInfo.GETFavourites(.1f));
        GETUserInfo.GetFavourites();
    }

    void GetRemoveFavouriteResponse(RemoveFavouriteResponse response)
    {

        if (response.result == true)
        {
            favouriteImage.sprite = notFavouriteSprite;
        }
        else if (response.result == false)
        {
            Debug.Log(response.message + ".");
        }
        //StartCoroutine(GETUserInfo.GETFavourites(.1f));
        GETUserInfo.GetFavourites();
    }

    void SendExtendedPoint(string id)
    {
        AddPoint AddPoint = new AddPoint();

        AddPoint.action = "preferiti";
        AddPoint.content_type = "poi";
        AddPoint.content_id = id;
        
        //StartCoroutine(GETUserInfo.ADDFavouritePoints(AddPoint, GetExtendedResponse));
    }

    void GetExtendedResponse(ResponseAddPoint response)
    {
        Debug.Log(response);
    }
}
