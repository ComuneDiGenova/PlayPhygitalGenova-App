using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class GETUserInfo : MonoBehaviour
{
    [SerializeField] UserDataHandler dataHandlerScript;
    static readonly string userEndpoint = "/jsonapi/user-get-info";
    static readonly string favouriteEndpoint = "/jsonapi/get_preferiti_utente";
    static readonly string transactionEndpoint = "/jsonapi/post_transazioni_list";
    [SerializeField] bool fakeProUser = false;
    [SerializeField] float updateTime = 120;

    static readonly string pointsURL = "/jsonapi/user/add_points";
    public static readonly string pointsIdKeyLanguage = "ID_GetGenovini";

    static readonly string noUserMessage = "Per eseguire l’accesso all’applicazione è necessario registrarsi sul sito visitgenoa.it";

    static readonly string noProMessage = "Per eseguire l’accesso all’applicazione come Commerciante è necessario registrarsi sul sito visitgenoa.it";
    static readonly string noAuthMessage = "Attenzione:\n\nautenticazione fallita";
    static readonly string noAuthTextId = "ID_NoAuth_ALERT";
    static readonly string noProTextId = "ID_NoPro_ALERT";
    static readonly string noUserTextId = "ID_NoUser_ALERT";

    static readonly string registerLink = ""; // TO REMOVE

    public static InfoUser DownloadedUserInfo;
    //[SerializeField] InfoUser userInfo; //DA RIMUOVERE !!!!

    //public static AddPoint AddPoint;
    //public static ResponseAddPoint RequestResponse;
    public static FavouriteList FavouriteResponse;

    public static List<Transaction> LastTransactions = new List<Transaction>();

    public delegate void UserEvent();
    public static event UserEvent OnLoginUserInfo;

    float timer = 0f;


    void ClearAll(){
        DownloadedUserInfo = null;
        FavouriteResponse = null;
    }

    void Awake()
    {
        LoginSSO.OnLogOut += () => ClearAll();
        //StartCoroutine(GETUserData(completeUserURL));
        //GetUserInfo();
        LoginSSO.OnLoginSuccesfull += (anon) => {
            LoginUserInfo(false);
        };
        OnLoginUserInfo += () =>{
            /*
            if (GameConfig.userID != null)
            {
                AddAccesPoints((response) =>
                {
                    if (response.result) PopUpPanel.OpenLanguage(GETUserInfo.pointsIdKeyLanguage, true, response.points.ToString());
                });
            }
            */
        };
        LoginSSO.OnLoginError += (message) => {
            if (message != null)
            {
                AlertPanel.OpenAlert(message);
            }
            else
            {  //NO USER LOGIN
                AlertPanel.OpenAlert(noUserMessage, noUserTextId);
            }
            AlertPanel.SetButtonCallback(() => {
                ExternalSitePage.OpenLink(registerLink);
            }, "Vai","ID_ScontrinoApri_BTN");
           
        };
    }

    void Update()
    {
        if (timer <= updateTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            //StartCoroutine(GETUserData(completeUserURL));
            timer = 0f;
            GetUserInfo();
        }
    }

    public void GetUserInfo(){
        if(!GameConfig.isLogged) return;
        Debug.Log("Get User Info : " + GameConfig.userID);
        if(string.IsNullOrWhiteSpace(GameConfig.userID)) return;
        WSO2.GET(userEndpoint + "/" + GameConfig.userID,(response)=>{
            string json = response.Replace("[", "").Replace("]", "");
            Debug.Log(json);
            try{
                DownloadedUserInfo = JsonUtility.FromJson<InfoUser>(json);
                Debug.Log("Informazioni Utente: " + DownloadedUserInfo.ToString());
                dataHandlerScript.SetData(DownloadedUserInfo);
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        },false);
    }

    //CHIAMATO SEMPRE QUANDO SI ENTRA NELL'APP
    //1-da uimaanger per caricmanto salvataggio (DIRECT TRUE)
    //2-da login autenticato (DIRECT FALSE)
    public void LoginUserInfo(bool direct = true){
        if(GameConfig.userID == null) {
            GameConfig.isLogged = false;
            GameConfig.userDrupalId = null;
            GameConfig.userID = null;
            GameConfig.isProUser = false;
            GameConfig.isLoggedPro = false;
            GameConfig.proUserRequest=false;
            GameConfig.SetPlayerPrefs();
            Debug.LogWarning("Anon Login");
            OnLoginUserInfo?.Invoke();
            return;
        }
        Debug.Log("Get User Info : " + GameConfig.userID);
        WSO2.GET(userEndpoint + "/" + GameConfig.userID,(response)=>{
            string json = response.Replace("[", "").Replace("]", "");
            Debug.Log(json);
            //DownloadedUserInfo = JsonUtility.FromJson<InfoUser>(json);
            try{
                DownloadedUserInfo = JsonUtility.FromJson<InfoUser>(json);
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
            if(DownloadedUserInfo == null || DownloadedUserInfo.codice_utente == null){
                Debug.LogError("NO USER FOUND!");
                GameConfig.userID = null;
                GameConfig.userDrupalId = null;
                GameConfig.isLogged = false;
                GameConfig.isProUser = false;
                GameConfig.SetPlayerPrefs();
                //dataHandlerScript.SetData(DownloadedUserInfo);
                //AVVISO
                AlertPanel.OpenAlert(noUserMessage,noUserTextId);
                AlertPanel.SetButtonCallback(() => {
                    ExternalSitePage.OpenLink(registerLink);
                }, "Vai","ID_ScontrinoApri_BTN");
            }else{
                DownloadedUserInfo.EvalRuoli();
                GameConfig.isLogged = true;
                //Debug.LogWarning(DownloadedUserInfo.ruoli);
                //DownloadedUserInfo.ruoloTag.ToList().ForEach(x => Debug.LogWarning(x));
                //prouser
                if(DownloadedUserInfo.ruoloTag.Contains("Esercente")){
                    GameConfig.isProUser = true;
                }else{
                    GameConfig.isProUser = false;
                }
                //Debug.LogWarning("esercente: " + DownloadedUserInfo.ruoloTag.Contains("Esercente"));
                //Debug.LogWarning("IS pro user: " + GameConfig.isProUser);
                if(fakeProUser){
                    GameConfig.isProUser = true;
                }
                //Debug.Log("Informazioni Utente: " + DownloadedUserInfo.ToString());
                if(!direct){    //logind da pagina con selezione esercente
                    if(GameConfig.proUserRequest){
                        if(GameConfig.isProUser){
                            GameConfig.isLoggedPro = true;
                            OnLoginUserInfo?.Invoke();
                        }else{
                            DownloadedUserInfo = null;
                            GameConfig.isLogged = false;
                            GameConfig.isLoggedPro = false;
                            AlertPanel.OpenAlert(noProMessage,noProTextId);
                            AlertPanel.SetButtonCallback(() => {
                                ExternalSitePage.OpenLink(registerLink);
                            }, "Vai","ID_ScontrinoApri_BTN");
                        }
                    }else{
                        GameConfig.isLoggedPro = false;
                        OnLoginUserInfo?.Invoke();
                        AddAccesPoints((response) =>{
                            if (response.result) PopUpPanel.OpenLanguage(GETUserInfo.pointsIdKeyLanguage, true, response.points.ToString());
                        });
                    }
                }else{  //login diretto da playerpref
                    GameConfig.isProUser = GameConfig.isLoggedPro;
                    OnLoginUserInfo?.Invoke();
                }
                dataHandlerScript.SetData(DownloadedUserInfo);
                GameConfig.SetPlayerPrefs();
                if(!GameConfig.isLoggedPro){
                    GetFavourites();
                }
            }
        });
    }

    public static void AddPoints(AddPoint Addpoint, Action<ResponseAddPoint> callBack){
        if(GameConfig.userID == null) return;
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "user_id", GameConfig.userID },
            { "action", Addpoint.action.ToString() },
            { "content_type", Addpoint.content_type.ToString() },
            { "content_id", Addpoint.content_id.ToString() },
            { "data scontrino", Addpoint.data_scontrino.ToString() },
            { "importo", Addpoint.importo.ToString() },
            { "numero_scontrino", Addpoint.numero_scontrino.ToString() }
        };
        WSO2.POST(pointsURL,headers,(response)=>{
            try{
                var RequestResponse = JsonUtility.FromJson<ResponseAddPoint>(response);
                callBack?.Invoke(RequestResponse);
                Debug.Log("Esito operazione aggiunta punti scontrino: " + RequestResponse.message.ToString());
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        },false);
    }

    public static void AddVisualizationPoints(AddPoint Addpoint, Action<ResponseAddPoint> callBack){
        if(GameConfig.userID == null) return;
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "user_id", GameConfig.userID },
            { "action", Addpoint.action },
            { "content_type", Addpoint.content_type },
            { "content_id", Addpoint.content_id }
        };
        WSO2.POST(pointsURL,headers,(response)=>{
            try{
                var RequestResponse = JsonUtility.FromJson<ResponseAddPoint>(response);
                callBack?.Invoke(RequestResponse);
                Debug.Log("Esito operazione aggiunta punti visualizzazione: " + RequestResponse.message.ToString());
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        },false);
    }
    public static void AddAccesPoints(Action<ResponseAddPoint> callBack){
        if(GameConfig.userID == null) return;
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "user_id", GameConfig.userID },
            { "action", "accesso" },
            { "content_type", "utente" },
            { "content_id", GameConfig.userID }
        };
        WSO2.POST(pointsURL,headers,(response)=>{
            try{
                var RequestResponse = JsonUtility.FromJson<ResponseAddPoint>(response);
                callBack?.Invoke(RequestResponse);
                Debug.Log("Esito operazione aggiunta punti visualizzazione: " + RequestResponse.message.ToString());
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        },false);
    }
    public static void AddFavouritePoints(string id, Action<ResponseAddPoint> callBack){
        if(GameConfig.userID == null) return;
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "user_id", GameConfig.userID },
            { "action", "preferiti" },
            { "content_type", "poi" },
            { "content_id", id }
        };
        WSO2.POST(pointsURL,headers,(response)=>{
            try{
                var RequestResponse = JsonUtility.FromJson<ResponseAddPoint>(response);
                callBack?.Invoke(RequestResponse);
                Debug.Log("Esito operazione aggiunta punti visualizzazione: " + RequestResponse.message.ToString());
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        },false);
    }
    public static void AddListeningPoints(string id, Action<ResponseAddPoint> callBack){
        if(GameConfig.userID == null) return;
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "user_id", GameConfig.userID },
            { "action", "ascolto" },
            { "content_type", "poi" },
            { "content_id", id }
        };
        WSO2.POST(pointsURL,headers,(response)=>{
            try{
                var RequestResponse = JsonUtility.FromJson<ResponseAddPoint>(response);
                callBack?.Invoke(RequestResponse);
                Debug.Log("Esito operazione aggiunta punti visualizzazione: " + RequestResponse.message.ToString());
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        },false);
    }
    public static void GetFavourites(){
        if(GameConfig.userID == null) return;
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "uid", GameConfig.userID }
        };
        WSO2.POST(favouriteEndpoint,headers,(response)=>{
            string json = "{\"favourites\" : " + response + "}";
            try
            {
                FavouriteResponse = JsonUtility.FromJson<FavouriteList>(json);
                FavouriteResponse ??= new FavouriteList();
                Debug.Log("Esito ottenimento preferiti: " + FavouriteResponse.favourites.Count);
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        },false);
    }

    /////////////////////////////////////////////////////////////////////////
    public static void GetLastTransactions(Action<List<Transaction>> callback){
        if(!GameConfig.isLogged) return;
        Debug.Log("Get Last transaction : " + GameConfig.userID);
        if(string.IsNullOrWhiteSpace(GameConfig.userID)) return;
        Dictionary<string,string> headers = new Dictionary<string, string>
        {
            { "uid", GameConfig.userID }
        };
        WSO2.POST(transactionEndpoint,headers,(response)=>{
            string json = "{\"transazioni\" : " + response + "}";
            try{
                var trans = JsonUtility.FromJson<TransactionList>(json);
                LastTransactions = trans.transazioni;
                Debug.Log("Esito ottenimento transazioni: " + LastTransactions.Count);
                callback?.Invoke(LastTransactions);
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        },false);
    }
}
