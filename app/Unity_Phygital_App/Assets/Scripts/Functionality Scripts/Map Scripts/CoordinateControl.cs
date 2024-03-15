using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeoCoordinate;
using System.Globalization;
using System.Linq;
using UnityEngine.Timeline;

public delegate void MyEventHandler();

public class CoordinateControl : MonoBehaviour
{
    // delegate per richiamare un evento, una volta terminato di caricare la lista di punti

    public event MyEventHandler OnEventTriggered;

    [SerializeField] OnlineMaps onlineMaps;   
    [SerializeField] POIMarkerTab markerTabScript;
    [SerializeField] ShopMarkerTab shopMarkerTabScript;
    [SerializeField] InstantiatePoiOnMap instantiatePoiScript;
    [SerializeField] InstantiateShopOnMap instantiateShopScript;

    string coordinateString = "(8.93266215617905 44.40604327943526, 8.932524131901832 44.40601228591227, 8.932359430658943 44.405996835286025, 8.93207661034279 44.40596474551083, 8.931946845727186 44.40595880295797, 8.931883627068249 44.405962368489796, 8.931785471782122 44.40598376167611, 8.93166568905997 44.406009908893175, 8.931517013271835 44.40604674149274, 8.931644270656854 44.40620701031115, 8.931767094864945 44.40631137017433, 8.931939326744915 44.40647704135852, 8.932056121350431 44.406610424857305, 8.932478536495315 44.40696008912377, 8.932023606423044 44.40720417336542, 8.931597848307996 44.40737981195299, 8.93109413448181 44.40756830155794, 8.93087226053456 44.40767968149355, 8.931100131075016 44.40803952291286, 8.931310011836054 44.40824942938514, 8.931621834680811 44.40849360535409, 8.93188568478035 44.408707793961234, 8.93145393007221 44.40921755969168, 8.931236995407682 44.4091560126473, 8.931065401064807 44.409106149460136, 8.930896715100559 44.409108227093775, 8.930669861562503 44.4091934100099, 8.930495358840817 44.409268204663185, 8.930224879622424 44.40928274805689, 8.929974759054671 44.409272359918894, 8.929942766889134 44.40937624121568, 8.929570494416307 44.40938662933528, 8.929174954913956 44.40941156081472, 8.92888120866598 44.40943649228339)";

    public List<GeoCoordinate.Coordinate> coordinateList = new List<GeoCoordinate.Coordinate>();
    public List<OnlineMapsMarker> listaMarker = new List<OnlineMapsMarker>();
    public List<OnlineMapsMarker> shopMarkerList = new List<OnlineMapsMarker>();
    public List<GameObject> PuntiList = new List<GameObject>();

    [SerializeField] Texture2D RouteMarkerSprite;
    [SerializeField] Texture2D PoiSprite;

    float x;
    float z;
    static float scale = 10750000f;

    float maxScale = 1.3f;

    public static bool isMarkerActive;

   ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    

    public string RemoveParentheses(string input)
    {
        string newInput = input.Replace("(", "").Replace(")", "");
        return newInput;
    }

    public Array CreateArrayFromCoordinate(string input)
    {
        string[] coordinateArray = input.Split(new string[] { ", " }, StringSplitOptions.None);
        return coordinateArray;
    }
   


    /// <summary>
    /// Instantiate point of interest on map as markers
    /// </summary>
    /// <param name="coordinates"></param>
    /// <param name="data"></param>
    public void InstantiateMarkers(Dictionary<ShortInfo, GeoCoordinate.Coordinate> coordinates, InformationList data)
    {
        foreach (var kvp in coordinates)
        {
            ShortInfo poi = kvp.Key;          
            string type = poi.id_tipologia;
            GeoCoordinate.Coordinate coord = kvp.Value;

            //instantiatePoiScript.GetPOIType(type);
            //Texture2D icon = instantiatePoiScript.instantiatePoiIcon[instantiatePoiScript.iconIndex];
            Texture2D icon;
            if (GetTipologiePOI.GetSprite(type) != null)
                icon = GetTipologiePOI.GetSprite(type).texture;
            else
                icon = PoiSprite;
            var marker = OnlineMapsMarkerManager.CreateItem(coord.Longitude, coord.Latitude, icon, poi.id);
            marker.tags.Add("Punto Storico");

            marker.scale = maxScale;
 
        }

        AddMarkerClickBehaviour();
    }



    /// <summary>
    /// Instantiate shops on map as markers
    /// </summary>
    /// <param name="coordinates"></param>
    /// <param name="data"></param>
    public void InstantiateMarkers(Dictionary<ShopShortInfo, GeoCoordinate.Coordinate> coordinates, ShopInfoList data)
    {
        foreach (var kvp in coordinates)
        {
            try
            {
                ShopShortInfo shop = kvp.Key;
                GeoCoordinate.Coordinate coord = kvp.Value;
                if (shop.tipologia == "Botteghe Storiche")
                {
                    Texture2D icon = instantiateShopScript.bottegaIcon;
                    var marker = OnlineMapsMarkerManager.CreateItem(coord.Longitude, coord.Latitude, icon, shop.id);
                    marker.tags.Add("Bottega");
                    marker.scale = maxScale;
                }
                else
                {
                    Texture2D icon = instantiateShopScript.shopIcon;
                    var marker = OnlineMapsMarkerManager.CreateItem(coord.Longitude, coord.Latitude, icon, shop.id);
                    marker.tags.Add("Negozio");
                    marker.scale = 1.1f;
                }

            }
            catch { Debug.Log("Errore"); }
        }

        AddMarkerClickBehaviour();
    }

    // add marker click behaviour, to be called after markers got instantiated
    public void AddMarkerClickBehaviour()
    {
        foreach (OnlineMapsMarker marker in OnlineMapsMarkerManager.instance)
        {
            marker.OnClick += OnMarkerClick;
            listaMarker.Add(marker);
        }
    }

    private void OnMarkerClick(OnlineMapsMarkerBase marker)
    {
        if (isMarkerActive == false)
        {
            AddFavourite.favouriteClickedMarkerLabel = marker.label;

            if (marker.tags.Contains("Punto Storico"))
            {
                // Show in console marker label.
                Debug.Log("Punto Storico");
                isMarkerActive = true;
                Debug.Log(marker.ToJSON());
                //AddFavourite.favouriteClickedMarkerLabel = marker.label;
                onlineMaps.SetPosition(marker.position.x, marker.position.y);
                markerTabScript.GetClickedName(marker);
            }

            /// aggiunge categoria bottega, da usare se e quando risolveranno l'api
            if (marker.tags.Contains("Bottega"))
            {
                // Show in console marker label.
                Debug.Log("Bottega");
                isMarkerActive = true;
                Debug.Log(marker.label);
                //AddFavourite.favouriteClickedMarkerLabel = marker.label;
                onlineMaps.SetPosition(marker.position.x, marker.position.y);
                shopMarkerTabScript.GetShopClickedName(marker);
            }

            if (marker.tags.Contains("Negozio"))
            {
                // Show in console marker label.
                Debug.Log("Negozio");
                isMarkerActive = true;
                Debug.Log(marker.label);
                //AddFavourite.favouriteClickedMarkerLabel = marker.label;
                onlineMaps.SetPosition(marker.position.x, marker.position.y);
                shopMarkerTabScript.GetShopClickedName(marker);
            }
        }
    }

    void InstantiateList(List<GeoCoordinate.Coordinate> coordinates)
    {
        listaMarker.Add(OnlineMapsMarkerManager.instance.items[0]);
        listaMarker.Add(OnlineMapsMarkerManager.instance.items.Last());
    }

    void AddMarkerInspector(List<GeoCoordinate.Coordinate> coordinates)
    {
        OnlineMapsMarkerManager.CreateItem(coordinates[0].Longitude, coordinates[0].Latitude, RouteMarkerSprite, "Inizio");
        OnlineMapsMarkerManager.CreateItem(coordinates.Last().Longitude, coordinates.Last().Latitude, RouteMarkerSprite, "Fine");
    }

    public static Vector3 AlbersProjection(GeoCoordinate.Coordinate coordiante)
    {
        Vector2 coord = coordiante.ToVector2();
        return AlbersProjection(coord.y,coord.x);
    }

    public static Vector3 AlbersProjection(float latitude, float longitude)
    {
        // Coordinate di riferimento per Genova
        float centralMeridian = 8.9463f; // Longitudine di riferimento
        float standardParallel1 = 44f; // Primo parallelo standard
        float standardParallel2 = 45f; // Secondo parallelo standard

        // Conversione da gradi a radianti
        float latRad = Mathf.Deg2Rad * latitude;
        float longRad = Mathf.Deg2Rad * longitude;
        float centralMeridianRad = Mathf.Deg2Rad * centralMeridian;
        float stdParallel1Rad = Mathf.Deg2Rad * standardParallel1;
        float stdParallel2Rad = Mathf.Deg2Rad * standardParallel2;

        // Parametri per la proiezione di Albers
        float n = (Mathf.Sin(stdParallel1Rad) + Mathf.Sin(stdParallel2Rad)) / 2f;
        float c = Mathf.Pow(Mathf.Cos(stdParallel1Rad), 2f) + 2f * n * Mathf.Sin(stdParallel1Rad);
        float rho0 = Mathf.Sqrt(c - 2f * n * Mathf.Sin(stdParallel1Rad)) / n;

        // Calcolo della proiezione di Albers
        float rho = Mathf.Sqrt(c - 2f * n * Mathf.Sin(latRad)) / n;
        float theta = n * (longRad - centralMeridianRad);

        float x = scale * (rho * Mathf.Sin(theta));
        float z = scale * (rho0 - rho * Mathf.Cos(theta));

        return new Vector3(ScaleCoordinates(x)/2, 0, ScaleCoordinates(z)/2);
    }

    public static Vector3 GPSToMapCoords(float latitude, float longitude)
    {
        float radius = 6371000f; // raggio della Terra in metri
        float longitude0 = 0f; // longitudine di riferimento

        float latRad = Mathf.Deg2Rad * latitude;
        float longRad = Mathf.Deg2Rad * longitude;

        float x = scale*(radius * (longRad - Mathf.Deg2Rad * longitude0));
        float y = 0;
        float z = scale*(radius * Mathf.Log(Mathf.Tan(Mathf.PI / 4f + latRad / 2f)));

        return new Vector3(ScaleCoordinates(x), y, ScaleCoordinates(z));
    }

    //use this method to reduce the size of the number cause u really dont need all the firs part of the number, and smaller one can readed easily from unity transform!
    public static float ScaleCoordinates(float num)
    {
        // Converti il numero in una stringa
        string numString = num.ToString();

        // Rimuovi le prime quattro cifre decimali
        string scaledString = numString.Substring(2);

        // Converti la stringa risultante in float
        float scaledNum = float.Parse(scaledString);

        // Restituisce il numero con le cifre decimali ridotte
        return scaledNum;
    }

    /// <summary>
    /// metodi chiamati per attivare gli itinerari del comune
    /// </summary>
    public void ActiveItinerary()
    {
        InstantiateList(coordinateList);
        AddMarkerInspector(coordinateList);
    }
}
