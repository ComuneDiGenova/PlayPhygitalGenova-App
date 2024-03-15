//using ARLocation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using static OnlineMapsAMapSearchResult;

public class ARInstantiatePOI : MonoBehaviour
{
    [SerializeField] Transform AR;

    [SerializeField] UIActivateMarkerTab poiPreviewMarkerTab;
    [SerializeField] UIActivateMarkerTab shopPreviewMarkerTab;

    [SerializeField] GameObject arPoiPrefab;
    [SerializeField] GameObject arShopPrefab;
    [SerializeField] Texture2D shopTexture;

    [Header("Debug")]
    //[SerializeField] bool fakePoi=false;

    [SerializeField] List<GameObject> arPoiList = new List<GameObject>();
    [SerializeField] List<Texture2D> sprites = new List<Texture2D>();



    float dist;

    void OnEnable()
    {     
        PopulateLocations();
        Debug.Log("instanziando poi ar");
    }


    void OnDisable()
    {
        for (int i = 0; i < arPoiList.Count; i++)
        {
            Destroy(arPoiList[i]);
        }

        arPoiList.Clear();
        Debug.Log("distruggendo poi ar");
    }

    void PopulateLocations()
    {

        foreach (var poi in GETPointOfInterest.DownloadedInformationPois.infos)
        {
            /*
            Location location = new Location
            {
                Longitude = poi.lon,
                Latitude = poi.lat,
                Altitude = 1f,
                Label = poi.id,
            };

            var options = new PlaceAtLocation.PlaceAtOptions()
            {
                HideObjectUntilItIsPlaced = true,
                MaxNumberOfLocationUpdates = 0,
                MovementSmoothing = 0.1f,
                UseMovingAverage = true
            };
            */

            int iconIndex;

            GETPoiType(poi.id_tipologia, out iconIndex);
            

            var arPrefab = Instantiate(arPoiPrefab,AR);
            
            arPrefab.GetComponent<ARPOIBehaviour>().poi = poi;
            arPrefab.GetComponent<MeshRenderer>().material.mainTexture = sprites[iconIndex];
            arPrefab.GetComponent<ARPOIBehaviour>().activeMarkerTab = poiPreviewMarkerTab;
            //arPrefab.GetComponent<ARPOIBehaviour>().getAudio = getAudio;
            //arPrefab.GetComponent<ARPOIBehaviour>().audioSource = arAudioSource;


            arPoiList.Add(arPrefab);

            //PlaceAtLocation.AddPlaceAtComponent(arPrefab, location, options);
        }

        foreach (var shop in GETShops.DownloadedInformationShop.shopInfos)
        {
            /*
            Location location = new Location
            {
                Longitude = shop.lon,
                Latitude = shop.lat,
                Altitude = 1f,
                Label = shop.id,
            };

            var options = new PlaceAtLocation.PlaceAtOptions()
            {
                HideObjectUntilItIsPlaced = true,
                MaxNumberOfLocationUpdates = 0,
                MovementSmoothing = 0.1f,
                UseMovingAverage = true
            };
            */
            var arShopPrefab = Instantiate(this.arShopPrefab,AR);

            arShopPrefab.GetComponent<ARShopBehaviour>().shop = shop;
            arShopPrefab.GetComponent<MeshRenderer>().material.mainTexture = shopTexture;
            arShopPrefab.GetComponent<ARShopBehaviour>().activeMarkerTab = shopPreviewMarkerTab;

            arPoiList.Add(arShopPrefab);

            //PlaceAtLocation.AddPlaceAtComponent(arShopPrefab, location, options);
        }
/*
        if(fakePoi){
            // location finta per test
            Location fakeLocation = new Location
            {
                // 45.273891, 10.099237
                // BBS 45.586550, 10.096356
                //45.39948432745891, 10.62587973621628
                Longitude = 10.62587973621628,
                Latitude = 45.39948432745891,
                Altitude = 1f,
                Label = "Test"
            };

            var optionsFake = new PlaceAtLocation.PlaceAtOptions()
            {
                HideObjectUntilItIsPlaced = false,
                MaxNumberOfLocationUpdates = 0,
                MovementSmoothing = 0.1f,
                UseMovingAverage = true
            };
            
            var arFakePrefab = Instantiate(arPoiPrefab,AR);

            arFakePrefab.GetComponent<ARPOIBehaviour>().poi = new ShortInfo()
            {
                id = "1536",
                lon = 10.62587973621628,
                lat = 45.39948432745891,
                id_tipologia = "417",
                tipo = "poi"
            };
            arFakePrefab.GetComponent<ARPOIBehaviour>().activeMarkerTab = poiPreviewMarkerTab;

            arPoiList.Add(arFakePrefab);

            PlaceAtLocation.AddPlaceAtComponent(arFakePrefab, fakeLocation, optionsFake);
        }
        */
    }

    void GETPoiType(string type, out int iconIndex)
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
}
