using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class RoutePercorso : MonoBehaviour
{
    OnlineMapsDrawingElement percorso;
    private readonly ItinerarioJS demoItineriarioJS = new ItinerarioJS() {
        nome = "demo",
        lingua = 0,
        id = 0,
        rgb = "#000000",
        lista_poi = new List<PoiJS>{
            new PoiJS(){    //viene trovato da listapois
                id_poi = "1555"
            },
            new PoiJS(){    //viene scartato xè non trovato
                id_poi = "Test"
            },
            new PoiJS(){    //viene creato poi custom da coordinate
                //id_poi = "1558"
                //id_poi = "1622"
                id_poi = "8.928490670689506,44.40866040003329"
            }
        }
    };

    [Header("Route")]
    [SerializeField] float maxWaypointDistance =  10;
    [SerializeField] bool percorsoDemo = false;

    [Header("Line")]
    [SerializeField] Color lineColor = Color.cyan;
    [SerializeField] float lineWidth = 5;

    List<GeoCoordinate.Coordinate> coordinateList = new List<GeoCoordinate.Coordinate>();
    List<Vector2> line;
    
    public delegate void PercorsoEvent();
    public static event PercorsoEvent OnRouteComplete;
    public static List<ShortInfo> PercorsoPoi = new List<ShortInfo>();

    private void Start() {
        if(percorsoDemo)
            EvalRoute(demoItineriarioJS,(cl) => {
                var percorso = DrawMapLine(cl,demoItineriarioJS);
            });
    }

    public void EvalRoute(ItinerarioJS itinerario, Action<List<GeoCoordinate.Coordinate>> callBack){
        StartCoroutine(Percorso(itinerario, callBack));
    }

    ShortInfo CheckCustomPoi(PoiJS pjs){
        var cc = pjs.id_poi.Split(",");
        if(cc.Length == 2){
            double dlat,dlon;
            if(double.TryParse(cc[1].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out dlat) &&
                double.TryParse(cc[0].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out dlon)){
                return new ShortInfo(){ id="custom", lat = dlat, lon = dlon};
            }else return null;
        }else return null;
    }

    IEnumerator Percorso(ItinerarioJS itinerario, Action<List<GeoCoordinate.Coordinate>> callBack){
        Debug.Log("itineraio: " + JsonUtility.ToJson(itinerario));
        //aspetto finche api x dettaglio poi non è popolato
        yield return new WaitUntil(() => GETPointOfInterest.DownloadedInformationPois != null);
        //identifico info poi del percorso
        List<ShortInfo> pois = new List<ShortInfo>();
        foreach(var p in itinerario.lista_poi){
            var cpoi = CheckCustomPoi(p);
            if(cpoi != null){
                pois.Add(cpoi);
            }else{
                foreach(var i in GETPointOfInterest.DownloadedInformationPois.infos){
                    if(p.id_poi == i.id && i.lat !=0){
                        pois.Add(i);
                        //Debug.Log(i.ToString());
                        break;
                    }
                }
            }
        }
        Debug.LogWarning($"percorso poi: {itinerario.lista_poi.Count}, trovati: {pois.Count}");
        if(pois.Count < 2)
            yield break;
        PercorsoPoi = pois;
        //creo percorso da route service con infopoi
        List<GeoCoordinate.Coordinate> route = null;
        List<GeoCoordinate.Coordinate> waypois = new List<GeoCoordinate.Coordinate>();
        foreach(var p in pois){
            //Debug.Log(p.ToString());
            waypois.Add(p.ToCoordinate());
        };
        RouteManager.EvaluateRoute(waypois,(coords)=>{
            route = coords;
        });
        Debug.Log("Wait For Route");
        //aspetto che abbia ottenut oun percorso valido
        yield return new WaitUntil(()=> route != null);
        Debug.LogWarning("Route WP: " + route.Count);
        //Interpolate Route Points
        List<GeoCoordinate.Coordinate> tmp_route = new List<GeoCoordinate.Coordinate>();
        for(int i = 0; i<route.Count-1; i++){
            var dist = GeoCoordinate.Utils.HaversineDistance(route[i],route[i+1]);
            //Debug.Log(dist);
            if( dist > maxWaypointDistance){
                var wp = GeoCoordinate.Utils.InterpolatePoints(route[i],route[i+1],maxWaypointDistance,dist);
                tmp_route.AddRange(wp);
            }else{
                tmp_route.Add(route[i]);
                tmp_route.Add(route[i+1]);
            }
        }
        route = tmp_route;
        Debug.Log("Interpolated Route Waypoints: " + route.Count);
        if(route.Count > 2){
            //assegno coordinateList
            coordinateList = route;
            callBack?.Invoke(coordinateList);
        }else{
            Debug.LogError("No Route");
            callBack?.Invoke(null);
        }
        OnRouteComplete?.Invoke();
    }


    public OnlineMapsDrawingElement DrawMapLine(List<GeoCoordinate.Coordinate> coordinate, ItinerarioJS itinerario){
        Debug.Log("Draw MAP ROUTE");
        line = new List<Vector2>();
        for(int i = 0; i<= coordinate.Count-1; i++)
        {
            Vector2 vector2 = new Vector2((float)coordinate[i].Longitude, (float)coordinate[i].Latitude);
            line.Add(vector2);
        }
        Color color;
        if (string.IsNullOrEmpty(itinerario.rgb) || !ColorUtility.TryParseHtmlString(itinerario.rgb, out color)) color = lineColor;
        var percorso = OnlineMapsDrawingElementManager.AddItem(new OnlineMapsDrawingLine(line, color, lineWidth));
        return percorso;
    }

    public void RemoveMapLine(OnlineMapsDrawingElement percorso)
    {
        OnlineMapsDrawingElementManager.RemoveItem(percorso);
    }
}
