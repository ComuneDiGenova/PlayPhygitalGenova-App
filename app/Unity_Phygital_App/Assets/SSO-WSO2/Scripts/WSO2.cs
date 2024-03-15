using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using System.Linq;

public class WSO2 : MonoBehaviour
{
    const string wso2BaseUrl = "";   // TO REMOVE

    const string wso2ApiUrl = "/gwe/visitgenoa_phygital_app";   // TO REMOVE

    static readonly CredentialResponse credential = new CredentialResponse() {
        access_token = ""
    }; // TO REMOVE

    //
    const string userIdEndpoint = "/jsonapi/user/get_user_code";
    static long userId;

    const bool LogOutOnConnectionError = true;
    static readonly string NoConnectionMessage = "Attenzione:\n\nErrore di connessione";
    static readonly string NoConnectionTextId = "ID_NoConnection_ALERT";

    [Serializable]
    class CredentialResponse{
        public string access_token;
        public string scope;
        public string token_type;
        public int expires_in;
    }


/**********************************************************************************************/

    public static async void GET(string endpoint, Action<string> callBack, bool panel = true, Action errorCallBack = null){
        if(!GameConfig.useWSO2){
            GETDirect(endpoint,callBack,panel);
            return;
        }
        string url = wso2BaseUrl + wso2ApiUrl + endpoint;
        Debug.Log(url);
        if(credential == null){
            Debug.LogError("No Credentials from WSO2");
            return;
        }
        if(panel)
            LoadingPanel.OpenPanel();
        using(UnityWebRequest uwr = UnityWebRequest.Get(url)){
            uwr.SetRequestHeader("Authorization","Bearer " + credential.access_token);
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if(uwr.result != UnityWebRequest.Result.Success){
                Debug.LogError($"{endpoint} | {url} | {uwr.result} | {uwr.error}");
                Debug.LogError(uwr.downloadHandler.text);
                errorCallBack?.Invoke();
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    NoConnectionError();
            }else{
                //ok
                string json = uwr.downloadHandler.text;
                //Debug.Log(json);
                callBack?.Invoke(json);
            }
        }
        if(panel)
            LoadingPanel.ClosePanel();
    }

    public static async void POST(string endpoint, Dictionary<string, string> headers, Action<string> callBack, bool panel = true, Action errorCallBack = null){
        if(!GameConfig.useWSO2){
            POSTDirect(endpoint,headers,callBack,panel);
            return;
        }
        string url = wso2BaseUrl + wso2ApiUrl + endpoint;
        Debug.Log(url);
        if(credential == null){
            Debug.LogError("No Credentials from WSO2");
            return;
        }
        if(panel)
            LoadingPanel.OpenPanel();
        if(headers == null)
            headers = new Dictionary<string, string>();
        using(UnityWebRequest uwr = UnityWebRequest.Post(url,headers)){
            uwr.SetRequestHeader("Authorization","Bearer " + credential.access_token);
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if(uwr.result != UnityWebRequest.Result.Success){
                Debug.LogError($"{endpoint} | {url} | {uwr.result} | {uwr.error}");
                Debug.LogError(uwr.downloadHandler.text);
                errorCallBack?.Invoke();
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    NoConnectionError();
            }else{
                //ok
                string json = uwr.downloadHandler.text;
                Debug.Log(json);
                callBack?.Invoke(json);
            }
        }
        if(panel)
            LoadingPanel.ClosePanel();
    }

    public static async void GETDirect(string endpoint, Action<string> callBack, bool panel = true, Action errorCallBack = null){
        string url =  AuthorizationAPI.baseURL + endpoint;
        Debug.Log("GET Direct: " + url);
        if(panel)
            LoadingPanel.OpenPanel();
        using(UnityWebRequest uwr = UnityWebRequest.Get(url)){
            AuthorizationAPI.AddAuthRequestHeader(uwr);
            AuthorizationAPI.AddAnyCertificateHandler(uwr);
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if(uwr.result != UnityWebRequest.Result.Success){
                Debug.LogError($"{endpoint} | {url} | {uwr.result} | {uwr.error}");
                Debug.LogError(uwr.downloadHandler.text);
                errorCallBack?.Invoke();
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    NoConnectionError();
            }else{
                //ok
                string json = uwr.downloadHandler.text;
                //Debug.Log(json);
                callBack?.Invoke(json);
            }
        }
        if(panel)
            LoadingPanel.ClosePanel();
    }
    public static async void GETDirectURL(string url, Action<string> callBack, bool panel = true, Action errorCallBack = null){
        Debug.Log("GET Direct URL: " + url);
        if(panel)
            LoadingPanel.OpenPanel();
        using(UnityWebRequest uwr = UnityWebRequest.Get(url)){
            AuthorizationAPI.AddAuthRequestHeader(uwr);
            AuthorizationAPI.AddAnyCertificateHandler(uwr);
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if(uwr.result != UnityWebRequest.Result.Success){
                Debug.LogError($"{url} | {uwr.result} | {uwr.error}");
                Debug.LogError(uwr.downloadHandler.text);
                errorCallBack?.Invoke();
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    NoConnectionError();
            }else{
                //ok
                string json = uwr.downloadHandler.text;
                Debug.Log(json);
                callBack?.Invoke(json);
            }
        }
        if(panel)
            LoadingPanel.ClosePanel();
    }

    public static async void GETImage(string url, Action<Texture2D> callBack, bool panel = true, Action errorCallBack = null){
        Debug.Log("GET Image: " + url);
        if(panel)
            LoadingPanel.OpenPanel();
        using(UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url)){
            AuthorizationAPI.AddAuthRequestHeader(uwr);
            AuthorizationAPI.AddAnyCertificateHandler(uwr);
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if(uwr.result != UnityWebRequest.Result.Success){
                Debug.LogError($"{url} | {uwr.result} | {uwr.error}");
                Debug.LogError(uwr.downloadHandler.text);
                errorCallBack?.Invoke();
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    NoConnectionError();
            }else{
                //ok
                var txt = DownloadHandlerTexture.GetContent(uwr);
                txt.name = System.IO.Path.GetFileName(url);
                callBack?.Invoke(txt);
            }
        }
        if(panel)
            LoadingPanel.ClosePanel();
    }

    public static async void POSTDirect(string endpoint, Dictionary<string, string> headers, Action<string> callBack, bool panel = true, Action errorCallBack = null){
        string url =  AuthorizationAPI.baseURL + endpoint;
        Debug.Log("POST Direct: " + url);
        if(panel)
            LoadingPanel.OpenPanel();
        if(headers == null)
            headers = new Dictionary<string, string>();
        //headers.Keys.ToList().ForEach(x=>{Debug.Log(x + ";" + headers[x]);});
        using(UnityWebRequest uwr = UnityWebRequest.Post(url,headers)){
            AuthorizationAPI.AddAuthRequestHeader(uwr);
            AuthorizationAPI.AddAnyCertificateHandler(uwr);
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if(uwr.result != UnityWebRequest.Result.Success){
                Debug.LogError($"{endpoint} | {url} | {uwr.result} | {uwr.error}");
                Debug.LogError(uwr.downloadHandler.text);
                errorCallBack?.Invoke();
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    NoConnectionError();
            }else{
                //ok
                string json = uwr.downloadHandler.text;
                //Debug.Log(json);
                callBack?.Invoke(json);
            }
        }
        if(panel)
            LoadingPanel.ClosePanel();
    }


    public static void NoConnectionError(bool logout = LogOutOnConnectionError){
        AlertPanel.OpenAlert(NoConnectionMessage,NoConnectionTextId);
        LoadingPanel.ClosePanel();
        if(logout)
            UIManager.instance.CallBackLogin();
    }

}
