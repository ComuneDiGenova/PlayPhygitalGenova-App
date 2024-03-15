//using ARLocation.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using static System.Net.WebRequestMethods;
using ZXing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class GETPointOfInterest : MonoBehaviour
{
    [SerializeField] InstantiatePoiOnMap instantiatePoi;

    static string poiDeteilsEndpoint = "/jsonapi/post_poi_details";

    readonly string poiURL = "/jsonapi/get_poi_list";
    readonly string typeURL = "/jsonapi/get_tipologie_poi";
    string completePoiURL;
    string completeTypeURL;

    public static InformationList DownloadedInformationPois = new InformationList();
    public static TypeList DownloadedTypePois = new TypeList();
    public static Dictionary<ShortInfo, Info> DownloadedExtendedInfo = new Dictionary<ShortInfo, Info>();
    public delegate void POIEvent(Info info);
    //public static event POIEvent OnExtendedInfo;
    [Header("DEBUG")]   //DEBUG!
    [SerializeField] InformationList infopois; //DEBUG DA RIMUOVERE !!!!
    [SerializeField] TypeList infotype; //DEBUG DA RIMUOVERE !!!!

    public static event VoidDelegate OnDownloadedInfos;
    public static event VoidDelegate OnDownloadedType;

    public bool debugTypePoiError = true;

    void Awake(){
        GETUserInfo.OnLoginUserInfo += () =>{
            DownloadedExtendedInfo.Clear();
            if(!GameConfig.isLoggedPro) GetPoiTypes();
        };
        GetTipologiePOI.OnTipologiePoiTrovate += () => { GetPois();};
        LoginSSO.OnLogOut += () => { ClearAllMarker(); };
    }

    void GetPois(){
        if (!GameConfig.isLoggedPro) {
            Debug.LogWarning("Get Poi List");
            WSO2.GET(poiURL, async (response) =>
            {
                if(string.IsNullOrEmpty(response) || response == "[]"){
                    //alert
                    AlertPanel.OpenAlert("Errore: Impossibile scaricare i punti di interesse.", "ID_PoiError");
                    AlertPanel.SetButtonCallback(GetPois,"Retry","ID_Retry");
                    return;
                }else{
                    string json = "{\"infos\" : " + response + "}";
                    try{
                        DownloadedInformationPois = JsonUtility.FromJson<InformationList>(json);
                    }catch(System.Exception ex){
                        //alert
                        Debug.LogError(ex.Message);
                        Debug.LogError("alert poi type");
                        AlertPanel.OpenAlert("Errore: Impossibile scaricare i punti di interesse.", "ID_PoiError");
                        AlertPanel.SetButtonCallback(GetPois,"Retry","ID_Retry");
                        return;
                    }
                    DownloadedInformationPois.PurgeNullCoordinate();
                    DownloadedInformationPois.PurgeBotteghe();
                    infopois = DownloadedInformationPois;
                    PurgeDuplicatePois(DownloadedInformationPois);
                    //wait for gps response
                    while(GameConfig.gpsWaiting){
                        await Task.Yield();
                    }
                    OnDownloadedInfos?.Invoke();
                    instantiatePoi.InstantiatePoi();
                }
            }, true);
        }
    }

    public static void ClearAllMarker(){
        OnlineMapsMarkerManager.RemoveAllItems();
        OnlineMapsLocationService.marker = null;
    }

    private void Start() {
        OnlineMapsMarkerManager.instance.items.Clear();
        DownloadedInformationPois.infos.Clear();
        DownloadedExtendedInfo.Clear();
    }

    void GetPoiTypes(){
        Debug.LogWarning("Get Poi Type");
        WSO2.GET(typeURL,(response)=>{
            if (debugTypePoiError) response = "dsff";
            if(!string.IsNullOrEmpty(response) && response != "[]"){
                string json = "{\"typeList\" : " + response + "}";
                try{
                    DownloadedTypePois = JsonUtility.FromJson<TypeList>(json);
                }catch(System.Exception ex){
                    //alert
                    Debug.LogError(ex.Message);
                    Debug.LogError("alert poi type");
                    AlertPanel.OpenAlert("Errore: Impossibile scaricare i punti di interesse.", "ID_PoiError");
                    AlertPanel.SetButtonCallback(GetPoiTypes,"Retry","ID_Retry");
                    return;
                }
                infotype = DownloadedTypePois;
                WSO2.GET(typeURL+"?lang="+GameConfig.applicationLanguage.ToString().ToLower(),(response)=>{
                    if(response != "[]"){
                        string json = "{\"typeList\" : " + response + "}";
                        try{
                            var pois = JsonUtility.FromJson<TypeList>(json);
                            foreach(var pt in DownloadedTypePois.typeList){
                                foreach(var ppt in pois.typeList){
                                    if(ppt.id == pt.id){
                                        pt.nome = ppt.nome;
                                    }
                                }
                            }
                        }catch(System.Exception ex){
                            //alert
                            Debug.LogError(ex.Message);
                            //ok no problem non trovate per la lingua correntes
                        }
                    }//ok no problem non trovate per la lingua corrente
                    OnDownloadedType?.Invoke();
                });
            }else{
                //alert
                AlertPanel.OpenAlert("Errore: Impossibile scaricare i punti di interesse.", "ID_PoiError");
                AlertPanel.SetButtonCallback(GetPoiTypes,"Retry","ID_Retry");
                return;
            }
        });
    }

    //void Start()
    //{
    //    GetPoiTypes();
        
    //}

    private void PurgeDuplicatePois(InformationList pois)
    {
        Debug.Log("RAW:" + pois.infos.Count);
        var newpois = new InformationList();
        foreach (var i in pois.infos)
        {
            if (newpois.infos.Where(x => x.id == i.id).Count() == 0)
            {
                newpois.infos.Add(i);
            }
        }
        Debug.Log("DISTINCT POI: " + newpois.infos.Count);
        Debug.Log("PURGED POI: " + (pois.infos.Count - newpois.infos.Count));
        
        pois.infos = newpois.infos;
    } 
    
    public static void ExtendedInfo(ShortInfo shortInfo, Action<Info> callBack){
        if(DownloadedExtendedInfo.ContainsKey(shortInfo) && DownloadedExtendedInfo[shortInfo].language == GameConfig.applicationLanguage ) {
            callBack?.Invoke(DownloadedExtendedInfo[shortInfo]);
            return;
        }
        string url = AuthorizationAPI.baseURL + poiDeteilsEndpoint;
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("nid", shortInfo.id.ToString());
        WSO2.POST(poiDeteilsEndpoint,headers, (response) =>{
            if(response == "[]"){
                callBack.Invoke(null);
                //OnExtendedInfo?.Invoke(null);
                return;
            }
            string json = "{\"infos\" : " + response + "}";
            InfoList infolist = null;
            try{
                infolist = JsonUtility.FromJson<InfoList>(json);
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
            if(infolist != null && infolist.infos != null && infolist.infos.Count == 1)
            {
                var info = infolist.infos[0];
                info.descrizione = System.Web.HttpUtility.HtmlDecode(info.descrizione);
                info.descrizione = Regex.Replace(info.descrizione, @"\s+|\t|\n|\r", " ");
                info.orari = System.Web.HttpUtility.HtmlDecode(info.orari);
                info.url = System.Web.HttpUtility.HtmlDecode(info.url);
                if(DownloadedExtendedInfo.ContainsKey(shortInfo))
                    DownloadedExtendedInfo[shortInfo]=info;
                else
                    DownloadedExtendedInfo.Add(shortInfo, info);
                //double lang
                headers.Add("language", GameConfig.applicationLanguage.ToString().ToLower());
                WSO2.POST(poiDeteilsEndpoint,headers, (response) =>{
                    if(response == "[]"){
                        callBack.Invoke(info);
                        //OnExtendedInfo?.Invoke(info);
                        return;
                    }
                    string json = "{\"infos\" : " + response + "}";
                    InfoList infolist2 = null;
                    try{
                        infolist2 = JsonUtility.FromJson<InfoList>(json);
                    }catch(System.Exception e){
                        Debug.LogError(e.Message);
                    }
                    if(infolist2 != null && infolist2.infos != null && infolist2.infos.Count == 1)
                    {
                        var info2 = infolist2.infos[0];
                        info2.descrizione = System.Web.HttpUtility.HtmlDecode(info2.descrizione);
                        info2.descrizione = Regex.Replace(info2.descrizione, @"\s+|\t|\n|\r", " ");
                        info2.orari = System.Web.HttpUtility.HtmlDecode(info2.orari);
                        info2.url = System.Web.HttpUtility.HtmlDecode(info2.url);
                        info2.language = GameConfig.applicationLanguage;
                        //DownloadedExtendedInfo.Add(shortInfo, info2);
                        DownloadedExtendedInfo[shortInfo] = info2;
                        callBack.Invoke(info2);
                        //OnExtendedInfo?.Invoke(info2);
                    }else{
                        Result result = null;
                        try{
                            result = JsonUtility.FromJson<Result>(json);
                            if(result != null && result.result == false){
                                Debug.LogError(result.message);
                            }
                        }catch(System.Exception e){
                            Debug.LogError(e.Message);
                        }
                    }
                },true);
                //
                //callBack.Invoke(info);
                //OnExtendedInfo?.Invoke(info);
            }else{
                Result result = null;
                try{
                    result = JsonUtility.FromJson<Result>(json);
                    if(result != null && result.result == false){
                        Debug.LogError(result.message);
                    }
                }catch(System.Exception e){
                    Debug.LogError(e.Message);
                }
                callBack.Invoke(null);
            }
        },true);
    }


    public static async void GetImagePoi(Info info, Action<Texture2D> callback){
        //string url = System.Web.HttpUtility.UrlDecode(info.immagine_di_copertina, System.Text.Encoding.ASCII);
        if(string.IsNullOrEmpty(info.immagine_di_copertina)){
            callback?.Invoke(null);
            return;
        }
        Debug.Log(info.immagine_di_copertina);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(info.immagine_di_copertina))
        {
            AuthorizationAPI.AddAnyCertificateHandler(uwr);
            AuthorizationAPI.AddAuthRequestHeader(uwr);
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + info.immagine_di_copertina + " | Poi immagine, Error while reciving: " + uwr.error);
                //Debug.LogError(uwr.downloadHandler.text);
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    WSO2.NoConnectionError(false);
                try
                {
                    callback?.Invoke(null);
                }
                catch (Exception ex) { Debug.Log("Error while assigning the image: " + ex.Message); }
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                texture.name = info.id;
                info.immagine_di_copertinaTexture = texture;
                try
                {
                    callback?.Invoke(texture);
                }
                catch (Exception ex) { Debug.Log("Error while assigning the image: " + ex.Message); }
            }
        }
    }
    public static async void GetImagePoi(ShopInformaitions info, Action<Texture2D> callback){
        if(string.IsNullOrEmpty(info.immagine_di_copertina)){
            callback?.Invoke(null);
            return;
        }
        //string url = System.Web.HttpUtility.UrlDecode(info.immagine_di_copertina, System.Text.Encoding.ASCII);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(info.immagine_di_copertina))
        {
            AuthorizationAPI.AddAnyCertificateHandler(uwr);
            AuthorizationAPI.AddAuthRequestHeader(uwr);
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + info.immagine_di_copertina + "Poi immagine, " + "Error while reciving: " + uwr.error);
                Debug.LogError(uwr.downloadHandler.text);
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    WSO2.NoConnectionError(false);
                try
                {
                    callback?.Invoke(null);
                }
                catch (Exception ex) { Debug.Log("Error while assigning the image: " + ex.Message); }
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                texture.name = info.id;
                info.immagine_di_copertinaTexture = texture;
                try
                {
                    callback?.Invoke(texture);
                }
                catch (Exception ex) { Debug.Log("Error while assigning the image: " + ex.Message); }
            }
        }
    }


/*
    public static IEnumerator GETImage(string url, string imageURL, Action<Texture2D> callback)
    {
        string completeURL = url + imageURL;

        Debug.Log("GETImage complete url: " + completeURL);

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(completeURL))
        {
            /////////////////////////// CERITFICATO NON VALIDO TOGLIERE STRINGA IN SEGUITO /////////////////////////////////////
            request.certificateHandler = new AcceptAnyCertificate();
            /////////////////////////// CERITFICATO NON VALIDO TOGLIERE STRINGA IN SEGUITO /////////////////////////////////////

            // Chiamata per l'autorizzazione dell'header
            AuthorizationAPI.AddAuthRequestHeader(request);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + completeURL + "Poi immagine, " + "Error while reciving: " + request.error);
                Debug.LogError(request.downloadHandler.text);
            }
            else
            {
                try
                {
                    var texture = DownloadHandlerTexture.GetContent(request);
                    callback?.Invoke(texture);
                }
                catch (Exception ex) { Debug.Log("Error while assigning the image: " + ex.Message); }
            }
        }
    }
    */

    /*
    IEnumerator GetAudioFromFile(string audioFilePath, Action<AudioClip> callback)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioFilePath, AudioType.MPEG))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Errore durante il caricamento dell'audio: " + request.error);
                yield break;
            }
            Destroy(audioClip);
            Debug.Log(audioClip);
            audioClip = DownloadHandlerAudioClip.GetContent(request);
            audioClip.LoadAudioData();
            callback.Invoke(audioClip);
        }
    }
    */
}
