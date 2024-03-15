using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif 

public class CheckPOIProximity : MonoBehaviour
{
  
    [Header("UI")]
    [SerializeField] CoordinateControl ccScript;
    [SerializeField] GameObject notificationTab;
    
    [Header("Config")]
    [SerializeField] bool onlyRoutePois = false;
    [SerializeField] bool alertGpsInEditor = false;
    
    [Header("Background Service Config")]
    [SerializeField] bool useNotificationService = true;
    [SerializeField] double _entryRadius = 40.0;
    [SerializeField] double _exitRadius = 35.0;
    [SerializeField] double _durationPerNotificationForPOIInMins = 10;
    [SerializeField] int _configLocationUpdateInMeters = 1;
    [SerializeField] int _configLocationUpdateInSeconds = 1;
    
    [Header("Game Config")]
    [SerializeField] float rangeDistance = 40f;
    [SerializeField] float ceckTime = 10f;
    [SerializeField] float notoficiationTimeout = 600f;
    [SerializeField] bool useLocalMobileNotification = false;


    GeoCoordinate.Coordinate userPosition;

    //List<ShortInfo> infos= new List<ShortInfo>();
    Dictionary<ShortInfo,Notification> infoDict = new Dictionary<ShortInfo, Notification>();
    //public List<Notification> notif = new List<Notification>();
    public static ShortInfo notificatedInfo;
    public static ShopShortInfo notificatedShop;

    [Serializable]
    public class Notification{
        public ShortInfo info = null;
        public float distance = 0;
        public bool notificated = false;
        public DateTime notificationTime;
    }

    float time = 0f;
    bool eval = false;

    object configParams = null;

    static CheckPOIProximity instance;

    private void Awake() {
        instance = this;
        LoginSSO.OnLogOut += () => {
            infoDict.Clear();
            notificationTab.SetActive(false);
            notificatedInfo = null;
            time = 0;
            eval = true;
            //NativeBridge.StopBackgroundService();
        };
        /*
        configParams = new
        {
            entryRadius = _entryRadius, //40.0
            exitRadius = _exitRadius, //35.00
            configLocationUpdateInMeters = _configLocationUpdateInMeters,
            configLocationUpdateInSeconds = _configLocationUpdateInSeconds
        };*/

        GETPointOfInterest.OnDownloadedInfos += async () => {
            eval = true;
            await System.Threading.Tasks.Task.Yield();
            if (!GameConfig.isLoggedPro && useNotificationService && GameConfig.backgroundGPS == true)
            {
                //NativeBridge.StartBackgroundService(onlyRoutePois ? RoutePercorso.PercorsoPoi :GETPointOfInterest.DownloadedInformationPois.infos, configParams);
                //NativeBridge.StartBackgroundService(GETPointOfInterest.DownloadedInformationPois.infos, configParams);
                StartBackgroundService();
            }
        };
    }

    public static void StartBackgroundService(){
        object configParams = new
        {
            entryRadiusFromPOI = instance._entryRadius,
            exitRadiusFromPOI = instance._exitRadius,
            durationPerNotificationForPOIInMins = instance._durationPerNotificationForPOIInMins
        };
        NativeBridge.StartBackgroundService(GETPointOfInterest.DownloadedInformationPois.infos, configParams);
    }


    private void OnApplicationFocus(bool focusStatus) {
        Debug.Log("Focus: " + focusStatus);
    }
    private void OnApplicationPause(bool pauseStatus) {
        Debug.Log("Pause: " + pauseStatus);
    }

    private void OnDestroy() {
        //NativeBridge.StopBackgroundService();
    }

    void Update()
    {
        if(eval) ProximityTimer();
    }

    void ProximityTimer()
    {
        if (time < ceckTime)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
            if(Input.location.isEnabledByUser){
                userPosition = new GeoCoordinate.Coordinate(OnlineMapsLocationService.instance.position);
                SearchProximity(userPosition);
            }else{
                if(Application.isEditor){
                    if(alertGpsInEditor && !GameConfig.isLoggedPro) AlertPanel.OpenAlert("Attiva il GPS per usare correttamente l'applicazione");
                }else{
                    if(!GameConfig.isLoggedPro) AlertPanel.OpenAlert("Attiva il GPS per usare correttamente l'applicazione");
                }
            }
        }
    }

    void SearchProximity(GeoCoordinate.Coordinate position)
    {
        //Debug.Log("Chack Proximity");
        /////////////////ESEMPIO RICERCA DISTANZA MINIMA /////////////
        //List<InfoPOI> poiNelRaggioDiRicerca = new List<InfoPOI>();//\
        
        //Vector2 userLocation = allMarkers[0].position;
        
        //infos.Clear();

        //Debug.LogWarning(position.ToString());
        var poilist = RoutePercorso.PercorsoPoi;
        if(!onlyRoutePois)
            poilist = GETPointOfInterest.DownloadedInformationPois.infos;

        foreach (var info in poilist)
        {
            if(info.id == "custom" || info.ToCoordinate() == null) continue;

            // Calcola la distanza tra il giocatore e i marker
            //double c = CalculateDistance(userLocation, kvp.Value.ToVector2());
            //var poiCoordinate = new GeoCoordinate.Coordinate(kvp.lon, kvp.lat);
            float dist = (float)GeoCoordinate.Utils.HaversineDistance(position, info.ToCoordinate());
            
            //Debug.Log($"{dist} / {rangeDistance}");

            if (dist <= rangeDistance)
            {
                //Debug.LogWarning($"Vicino: {info.id}, {info.nome}");
                //infos.Add(info);
                if(!infoDict.ContainsKey(info))
                    infoDict.Add(info,new Notification(){info = info, distance = dist});
            }
            if(infoDict.ContainsKey(info)) infoDict[info].distance = dist;
        }
        var list = infoDict.ToList();
        foreach(var kvp in list)
        {
            //NO TENGO IN MEMORIA PER TIMOUT ED ERRORE MISIN KEY SE APR PANNELLLO E SONO USCITO
            /*
            float dist = (float)GeoCoordinate.Utils.HaversineDistance(position, kvp.Key.ToCoordinate());
            if(dist > rangeDistance){
                //infoDict.Remove(kvp.Key);
            }
            */
            if(kvp.Value.notificated){
                var delta = (DateTime.UtcNow - kvp.Value.notificationTime).TotalSeconds;
                if(delta > notoficiationTimeout) kvp.Value.notificated = false;
            }
        }
        list = null;
        //Debug.LogWarning("Proximity poi: " + infoDict.Count);
        if(infoDict.Count(x=>x.Value.notificated == false) > 0)
            ActiveNotification();
        //
        /*
        foreach(var kvp in infoDict){
            Debug.Log(kvp.Value + " | " + kvp.Key.ToString());
        }
        */
        //notif = infoDict.Values.ToList();
    }

    public void ActiveNotification()
    {
        if(ActivateAR.isArActive) return;
        if(notificatedInfo != null) return;
        Debug.Log("Poi in area: " + infoDict.Count(x=>x.Value.notificated == false));
        //notificatedInfo = infoDict.Where(x=>x.Value.Item1 == true).FirstOrDefault().Key;
        //prendo i lpiu vicin odi quelli non ancora visualizzati
        notificatedInfo = infoDict.Where(x=>x.Value.notificated == false).OrderBy(x=>x.Value.distance).FirstOrDefault().Key;
        if(notificatedInfo == null)
            return;
        Debug.Log("Notifica: " + notificatedInfo.id);
        infoDict[notificatedInfo].notificationTime = DateTime.UtcNow;
        notificationTab.SetActive(true);
        notificationTab.transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = notificatedInfo.title;
        if(SystemInfo.supportsVibration)
            Handheld.Vibrate();
        else
            Debug.LogWarning("Device not support Vibration");
        if(useLocalMobileNotification)
            MobileNotification.SendPOINotification(notificatedInfo.title);
    }


    public void DisactiveNotification()
    {
        notificationTab.SetActive(false);
        infoDict[notificatedInfo].notificated = true;
        Debug.Log("Notifica Rimossa per: " + notificatedInfo.id);
        notificatedInfo = null;
    }

}
