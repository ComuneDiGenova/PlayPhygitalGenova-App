using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GeoCoordinate;
using System.Globalization;
using UnityEngine.Timeline;
using System;

public class InstantiateShopOnMap : MonoBehaviour
{
    [SerializeField] CoordinateControl ccScript;
    [SerializeField] float rangeDistance = 500f;


    public Texture2D shopIcon;
    public Texture2D bottegaIcon;

    public List<ShopShortInfo> shopNelRaggioDiRicerca = new List<ShopShortInfo>();

    public static Dictionary<ShopShortInfo, GeoCoordinate.Coordinate> shopCoordinateList = new Dictionary<ShopShortInfo, GeoCoordinate.Coordinate>();


    int i = 0;
    int j = 0;
    public int iconIndex = 0;

    float longitude;
    float latitude;

    public void InstantiateShop()
    {
        if (GETShops.DownloadedInformationShop != null)
        {
            foreach (var s in GETShops.DownloadedInformationShop.shopInfos)
            {
                shopCoordinateList.Add(s, s.ToCoordinate());
            }

            ccScript.InstantiateMarkers(shopCoordinateList, GETShops.DownloadedInformationShop);

        }
    }

    public void SearchShopNearMe()
    {
        /////////////////ESEMPIO RICERCA DISTANZA MINIMA /////////////
        //List<InfoPOI> poiNelRaggioDiRicerca = new List<InfoPOI>();//\
        if (OnlineMapsMarkerManager.instance.items.Count == 0) return;

        GeoCoordinate.Coordinate postition = new GeoCoordinate.Coordinate(OnlineMapsLocationService.instance.position);

        //Debug.Log(userLocation);

        foreach (var kvp in shopCoordinateList)
        {
            // Calcola la distanza tra il giocatore e i marker
            //double c = CalculateDistance(userLocation, kvp.Value.ToVector2());
            float dist = (float)GeoCoordinate.Utils.HaversineDistance(postition, kvp.Value);

            //Debug.Log(c);

            if (dist <= rangeDistance)
            {
                shopNelRaggioDiRicerca.Add(kvp.Key);
            }
        }
        foreach (var s in shopNelRaggioDiRicerca)
        {
            if (!GETShops.DownloadedShopExtendedInfo.ContainsKey(s))
            {
                /*
                StartCoroutine(GETShops.GETExtendedInfo(s, (info) =>
                {
                    Debug.Log("ShopExtended: " + s.id);
                }));
                */
                GETShops.ExtendedInfo(s, (info) =>
                {
                    Debug.Log("ShopExtended: " + s.id);
                });
            }

        }
    }

}