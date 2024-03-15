using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GeoCoordinate;
using System.Globalization;
using UnityEngine.Timeline;
using System;

public class InstantiatePoiOnMap : MonoBehaviour
{
    [SerializeField] CoordinateControl ccScript;
    [SerializeField] float rangeDistance = 500f;

    //public List<Texture2D> instantiatePoiIcon = new List<Texture2D>();

    public List<ShortInfo> poiNelRaggioDiRicerca = new List<ShortInfo>();

    public static Dictionary<ShortInfo, GeoCoordinate.Coordinate> coordinateList = new Dictionary<ShortInfo, GeoCoordinate.Coordinate>();
    //InformationList data = GETPointOfInterest.DownloadedInformationPois;

    int i = 0;
    int j = 0;
    public int iconIndex = 0;

    private void Awake() {
        LoginSSO.OnLogOut += () =>
        {
            coordinateList.Clear();
            poiNelRaggioDiRicerca.Clear();
        };
    }

 
    public void InstantiatePoi()
    {
        //clear markers
        
        //
        if (GETPointOfInterest.DownloadedInformationPois != null)
        {
            foreach (var p in GETPointOfInterest.DownloadedInformationPois.infos)
            {
                var coordinate = p.ToCoordinate();
                if(coordinate != null)
                    coordinateList.Add(p,coordinate);
            }

            ccScript.InstantiateMarkers(coordinateList, GETPointOfInterest.DownloadedInformationPois);
        }

    }

    public void GetPOIType(string type)
    {
        switch (type)
        {
            case "133": // acquedotto storico
                iconIndex = 0;
                break;
            case "130": // arte e cultura
                iconIndex = 1;
                break;
            case "35": // botteghe storiche
                iconIndex = 2;
                break;
            case "57": // genova per i bambini
                iconIndex = 3;
                break;
            case "55": // genova by night
                iconIndex = 4;
                break;
            case "37": // luoghi da scoprire
                iconIndex = 5;
                break;
            case "38": // mare
                iconIndex = 6;
                break;
            case "185": // monumenti e luoghi sacri
                iconIndex = 7;
                break;
            case "187": // musei chiese e monumenti
                iconIndex = 7;
                break;
            case "417": // chiese e monumenti
                iconIndex = 7;
                break;
            case "53": // mura e forti
                iconIndex = 8;
                break;
            case "186": // forti
                iconIndex = 8;
                break;
            case "14": // musei
                iconIndex = 9;
                break;
            case "131": // outdoor
                iconIndex = 10;
                break;
            case "36": // palazzi dei Rolli
                iconIndex = 11;
                break;
            case "188": // palazzi dei Rolli - Patrimonio Unesco
                iconIndex = 11;
                break;
            case "33": // parchi ville e giardini
                iconIndex = 12;
                break;
            case "418": // parchi ville e orti botanici
                iconIndex = 12;
                break;
            case "134": // punti panoramici
                iconIndex = 13;
                break;
            case "15": // quartieri
                iconIndex = 14;
                break;
            case "54": // sport
                iconIndex = 15;
                break;
            case "132": // storia e tradizioni
                iconIndex = 16;
                break;
            case "52": // teatri
                iconIndex = 17;
                break;
            default:
                iconIndex = 17;
                break;
        }
    }

    public void SearchNearMe()
    {
        Debug.Log("Near POI");
        /////////////////ESEMPIO RICERCA DISTANZA MINIMA /////////////
        //List<InfoPOI> poiNelRaggioDiRicerca = new List<InfoPOI>();//\
        if(OnlineMapsMarkerManager.instance.items.Count == 0) return;
        
        GeoCoordinate.Coordinate position = new GeoCoordinate.Coordinate(OnlineMapsLocationService.instance.position);

        //Debug.Log(userLocation);

        foreach (var kvp in coordinateList)
        {
            // Calcola la distanza tra il giocatore e i marker
            //double c = CalculateDistance(userLocation, kvp.Value.ToVector2());
            float dist = (float)GeoCoordinate.Utils.HaversineDistance(position,kvp.Value);

            //Debug.Log($"{dist} / {rangeDistance}");

            if (dist <= rangeDistance)
            {
                poiNelRaggioDiRicerca.Add(kvp.Key);
            }
        }
        /*
        foreach(var p in poiNelRaggioDiRicerca)
        {
            if(!GETPointOfInterest.DownloadedExtendedInfo.ContainsKey(p))
            {
                GETPointOfInterest.ExtendedInfo(p, (info) =>
                {
                   Debug.Log("PoiExtended: " + p.id);
                });
            }
        }
        */
    }

}
