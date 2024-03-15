using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;
using UnityEngine.UI;
using System.Linq;
using InfinityCode.OnlineMapsExamples;

public class ItinerariesManager : MonoBehaviour
{

    string itinerairesURL = "/jsonapi/post_itinerari_list";
    string itinerairesExtendedURL = "/jsonapi/post_itinerario_details"; //endpoint

    [SerializeField] RoutePercorso routePercorso;
    [SerializeField] GameObject areaComune;
    [SerializeField] GameObject areaUtente;
    [SerializeField] GameObject bottoneIC;
    [SerializeField] GameObject bottoneIU;
    [SerializeField] TMPro.TextMeshProUGUI testoSelezionato;
    [SerializeField] Sprite target;
    [SerializeField] Sprite compass;
    [SerializeField] Image imageTarget;

    public static ListaItinerari DownloadedItineraries;
    public static Dictionary<ItinerarioShort,ItinerarioDettaglio> DownloadedDettagli = new Dictionary<ItinerarioShort, ItinerarioDettaglio>();
    //[SerializeField] ItinerarioDettaglio itinerario;

    //ItinerarioJS itinerarioJS;

    public static ItinerarioShort itinerarioSelezionato;
    OnlineMapsDrawingElement percorsoMappa;

    List<Button> itineraryButtons = new List<Button>();

    // modifiche per comportamento bussola
    public int compassClickCount = 1;

    public static event VoidDelegate OnDownloadedItineraries;

    void Awake()
    {
        testoSelezionato.text = "";
        ClearAll();
        //StartCoroutine(GETItinerary(true));
        //StartCoroutine(GETItinerary(fakeUrl));
        //Itinerari(true);
        //LoginSSO.OnLoginSuccesfull += () => Itinerari(true);
        GETUserInfo.OnLoginUserInfo += () =>
        {
            if (!GameConfig.isLoggedPro) Itinerari(true);
        };
        LoginSSO.OnLogOut += () => ClearAll();
    }

    void Start(){
        OnlineMapsLocationService.instance.allowUpdatePosition = true;
        OnlineMapsLocationService.instance.updatePosition = true;
        OnlineMapsLocationService.instance.rotateCameraByCompass = false;
        OnlineMapsCameraOrbit.instance.rotation = Vector3.zero;
        compassClickCount = 1;
        RemoveActiveRoute();
    }

    void OnEnable (){
        /*
        foreach(var g in itineraryButtons){
            Destroy(g.gameObject);
        }
        itineraryButtons.Clear();
        */
    }

    void ClearAll(){
        Debug.Log("Clear All Itineraries");
        RemoveActiveRoute();
        DownloadedItineraries = null;
        DownloadedDettagli.Clear();
        foreach(var g in itineraryButtons){
            Destroy(g.gameObject);
        }
        itineraryButtons.Clear();
        var childrens = areaComune.transform.GetComponentsInChildren<Transform>();
        foreach(var g in childrens){
            if(g != areaComune.transform)
                Destroy(g.gameObject);
        }
        childrens = areaUtente.transform.GetComponentsInChildren<Transform>();
        foreach(var g in childrens){
            if(g != areaUtente.transform)
                Destroy(g.gameObject);
        }
    }

    void Itinerari(bool user){
        Dictionary<string, string> headers = new Dictionary<string, string>();
        if(LoginSSO.instance.fakeAuth){
            headers.Add("uid",LoginSSO.instance.fakeUserId);
        }else{
            if(GameConfig.userID!=null)
                headers.Add("uid",GameConfig.userID);
        }   
        WSO2.POST(itinerairesURL,headers,(response) => {
            string json = "{\"itinerari\" : " + response + "}";
            try
            {
                DownloadedItineraries = JsonUtility.FromJson<ListaItinerari>(json);
                OnDownloadedItineraries?.Invoke();
                PopulateButtons();
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        });
    }
    void ItinerarioDettaglio(string id, Action<ItinerarioJS> callback){
        var itinere = DownloadedItineraries.itinerari.Where(x=>x.id == id).FirstOrDefault();
        if(DownloadedDettagli.ContainsKey(itinere)){
            var itinerario = DownloadedDettagli[itinere];
            if(!string.IsNullOrEmpty(itinerario.sferiche)){
                try
                {
                    var itinerarioJS = JsonUtility.FromJson<ItinerarioJS>(itinerario.sferiche);
                    //Debug.Log(DownloadedInformationPois.infos.Count);
                    callback?.Invoke(itinerarioJS);
                }catch(System.Exception e){
                    Debug.LogError(e.Message);
                }
            }
        }else{
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("nid", id);
            string url = AuthorizationAPI.baseURL + itinerairesExtendedURL;
            WSO2.POST(itinerairesExtendedURL,headers,(response) => {
                string json = "{\"itinerario\" : " + response + "}";
                try
                {
                    var itineraryDetail = JsonUtility.FromJson<ListaItinerariDettaglio>(json);
                    if (itineraryDetail.itinerario.Count == 1)
                    {
                        var itinerario = itineraryDetail.itinerario[0];
                        DownloadedDettagli.Add(itinere, itinerario);
                        if (!string.IsNullOrEmpty(itinerario.sferiche))
                        {
                            try
                            {
                                var itinerarioJS = JsonUtility.FromJson<ItinerarioJS>(itinerario.sferiche);
                                //Debug.Log(DownloadedInformationPois.infos.Count);
                                callback?.Invoke(itinerarioJS);
                            }catch(System.Exception e) {
                                Debug.LogError(e.Message);
                            }
                        }
                    }
                }catch(System.Exception e){
                    Debug.LogError(e.Message);
                }
            },false);
        }
    }
    void OnDestroy(){
        ClearAll();
    }

    private void PopulateButtons(){
        foreach(var i in DownloadedItineraries.itinerari){
            GameObject b;
            if(i.predefinito==1){
                b = Instantiate(bottoneIC,areaComune.transform);
            }else{
                b = Instantiate(bottoneIU,areaUtente.transform);
            }
            b.SetActive(true);
            var bb = b.GetComponent<Button>();
            itineraryButtons.Add(bb);
            bb.onClick.AddListener(() => {
                SelectItinerary(i.id);
            });
            var t = b.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            t.text = i.title;
        }
        bottoneIC.SetActive(false);
        bottoneIU.SetActive(false);
    }

    public void SelectItinerary(string id){
        //seleziono
        Debug.Log("Seleziono itinerario: " + id);
        var it = DownloadedItineraries.itinerari.Where(x=>x.id == id).FirstOrDefault();
        if(it == null)
            return;
        itinerarioSelezionato = it;
        testoSelezionato.text = it.title;
        LoadingPanel.OpenPanel();
        ItinerarioDettaglio(id, (itinerarioJS) => {
            Debug.Log(itinerarioJS.ToString());
            routePercorso.EvalRoute(itinerarioJS, (percorso) =>  {
                LoadingPanel.ClosePanel();
                if(percorso != null){
                     //rimuovo precedente
                    if(percorsoMappa != null)
                        routePercorso.RemoveMapLine(percorsoMappa);
                    //disegno
                    percorsoMappa = routePercorso.DrawMapLine(percorso,itinerarioJS);
                    //OnlineMaps.instance.longitude = percorso[0].Longitude;
                    //OnlineMaps.instance.latitude = percorso[0].Latitude;
                    //OnlineMaps.instance.zoom = 19;
                    //OnlineMaps.instance.SetPositionAndZoom(percorso[0].Longitude,percorso[0].Latitude,19);
                    //StartCoroutine(ResetMap());
                    CenterMap(percorso);
                    OnlineMapsLocationService.instance.updatePosition = true;
                    if (GameConfig.isLogged && !GameConfig.isLoggedPro)
                    {
                        var point = new AddPoint()
                        {
                            user_id = GameConfig.userID,
                            action = "visualizzazione",
                            content_type = "itinerario",
                            content_id = itinerarioSelezionato.id
                        };
                        GETUserInfo.AddVisualizationPoints(point, (response) =>
                        {
                            Debug.Log(response);
                            if (response.result) PopUpPanel.OpenLanguage(GETUserInfo.pointsIdKeyLanguage, true, response.points.ToString());
                        });
                    }
                }
            });
        });
    }

    void CenterMap(List<GeoCoordinate.Coordinate> percorso){
        Debug.Log("Center Map");
        var markers = new List<OnlineMapsMarker>();
        foreach(var p in percorso){
            //var m = OnlineMapsMarkerManager.CreateItem(p.ToVector2(),null,"route");
            var m = new OnlineMapsMarker();
            m.latitude = p.Latitude;
            m.longitude = p.Longitude;
            markers.Add(m);
        }
        Vector2 center;
        int zoom;
        OnlineMapsUtils.GetCenterPointAndZoom(markers.ToArray(), out center, out zoom);
        OnlineMaps.instance.SetPositionAndZoom(center.x,center.y,zoom);
    }

    IEnumerator ResetMap(){
        yield return new WaitForSeconds(5);
        ResetMapPosition();
    }

    public void ResetMapPosition(){
        OnlineMapsLocationService.instance.allowUpdatePosition = true;
    }

    // Rotazione camera premendo bussola, ho dovuto sostituire questo script a quello precedente nel bottone della bussola
    public void CompassBehaviour()
    {
        if (compassClickCount == 0)
        {
            OnlineMaps.instance.SetPosition(OnlineMapsLocationService.instance.position.x,OnlineMapsLocationService.instance.position.y);
            OnlineMapsLocationService.instance.updatePosition = false;
            OnlineMapsLocationService.instance.rotateCameraByCompass = false;
            OnlineMapsCameraOrbit.instance.rotation = Vector3.zero;
            compassClickCount++;
            Debug.Log("Reset Rotation and lock");
            imageTarget.sprite = target;
        }
        else if (compassClickCount == 1)
        {
            OnlineMaps.instance.SetPosition(OnlineMapsLocationService.instance.position.x,OnlineMapsLocationService.instance.position.y);
            OnlineMapsLocationService.instance.updatePosition = true;
            OnlineMapsLocationService.instance.rotateCameraByCompass = true;
            compassClickCount = 0;
            Debug.Log("Compasss rotatrion enabled");
            imageTarget.sprite = compass;
        }
    }

    public void RemoveActiveRoute(){
        if(percorsoMappa != null)
            routePercorso.RemoveMapLine(percorsoMappa);
        percorsoMappa = null;
        itinerarioSelezionato = null;
        testoSelezionato.text = "";
        OnlineMapsLocationService.instance.updatePosition = false;
    }

}
