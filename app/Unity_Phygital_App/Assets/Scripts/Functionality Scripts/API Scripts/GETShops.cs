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
using System.Text.RegularExpressions;

public class GETShops : MonoBehaviour
{
    [SerializeField] InstantiateShopOnMap instantiateShop;

    static readonly string shopDeteilsEndpoint = "/jsonapi/post_shop_details";

    readonly string apiURL = "/jsonapi/get_shop_list";
    string completeURL;

    public static ShopInfoList DownloadedInformationShop = new ShopInfoList();
    public static Dictionary<ShopShortInfo, ShopInformaitions> DownloadedShopExtendedInfo = new Dictionary<ShopShortInfo, ShopInformaitions>();
    public delegate void ShopEvent(ShopInformaitions info);
    //public static event ShopEvent OnExtendedInfo;
    [SerializeField] ShopInfoList infoshops; //DA RIMUOVERE !!!!
    

    void Start()
    {
        // completeURL = AuthorizationAPI.baseURL + apiURL;
        //StartCoroutine(GETShopList(completeURL));
        GETUserInfo.OnLoginUserInfo += () =>
        {
            if (!GameConfig.isLoggedPro)
            {
                WSO2.GET(apiURL, (response) =>
                {
                    string json = "{\"shopInfos\" : " + response + "}";
                    try
                    {
                        DownloadedInformationShop = (JsonUtility.FromJson<ShopInfoList>(json));
                        DownloadedInformationShop.PurgeNullCoordinate();
                        DownloadedInformationShop.SetType();
                        infoshops = DownloadedInformationShop;
                        instantiateShop.InstantiateShop();
                    }catch(System.Exception e){
                        Debug.LogError(e.Message);
                    }
                });
            }
        };
    }
/*
    IEnumerator GETShopList(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            /////////////////////////// CERITFICATO NON VALIDO TOGLIERE STRINGA IN SEGUITO /////////////////////////////////////
            request.certificateHandler = new AcceptAnyCertificate();
            /////////////////////////// CERITFICATO NON VALIDO TOGLIERE STRINGA IN SEGUITO /////////////////////////////////////

            Debug.Log(url);

            // Chiamata per l'autorizzazione dell'header
            AuthorizationAPI.AddAuthRequestHeader(request);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error while reciving: " + request.error);
            }
            else
            {
                ////////////////////////////////////////
                //Debug.Log(request.downloadHandler.text);
                ////////////////////////////////////////

                string json = "{\"shopInfos\" : " + request.downloadHandler.text + "}";             

                DownloadedInformationShop = (JsonUtility.FromJson<ShopInfoList>(json));
                DownloadedInformationShop.PurgeNullCoordinate();
                infoshops = DownloadedInformationShop;
            }
            instantiateShop.InstantiateShop();
        }
    }
*/
    public static void ExtendedInfo(ShopShortInfo shortInfo, Action<ShopInformaitions> callBack){
        if(DownloadedShopExtendedInfo.ContainsKey(shortInfo) && DownloadedShopExtendedInfo[shortInfo].language == GameConfig.applicationLanguage) {
            callBack?.Invoke(DownloadedShopExtendedInfo[shortInfo]);
            return;
        }
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("nid", shortInfo.id.ToString());
        //headers.Add("language", GameConfig.applicationLanguage.ToString().ToLower());
        WSO2.POST(shopDeteilsEndpoint,headers, (response)=>{
             if(response == "[]"){
                callBack.Invoke(null);
                //OnExtendedInfo?.Invoke(null);
                return;
            }
            string json = "{\"shopInfoList\" : " + response + "}";
            ShopInformationsList infolist = null;
            try{
                infolist = JsonUtility.FromJson<ShopInformationsList>(json);
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
            if(infolist != null && infolist.shopInfoList != null && infolist.shopInfoList.Count == 1)
            {
                var info = infolist.shopInfoList[0];
                info.descrizione = System.Web.HttpUtility.HtmlDecode(info.descrizione);
                info.descrizione = Regex.Replace(info.descrizione, @"\s+|\t|\n|\r", " ");
                info.orari = System.Web.HttpUtility.HtmlDecode(info.orari);
                if(DownloadedShopExtendedInfo.ContainsKey(shortInfo))
                    DownloadedShopExtendedInfo[shortInfo]=info;
                else
                    DownloadedShopExtendedInfo.Add(shortInfo, info);
                //double lang
                headers.Add("language", GameConfig.applicationLanguage.ToString().ToLower());
                WSO2.POST(shopDeteilsEndpoint,headers, (response)=>{
                    if(response == "[]"){
                        callBack.Invoke(null);
                        //OnExtendedInfo?.Invoke(null);
                        return;
                    }
                    string json = "{\"shopInfoList\" : " + response + "}";
                    ShopInformationsList infolist2 = null;
                    try{
                        infolist2 = JsonUtility.FromJson<ShopInformationsList>(json);
                    }catch(System.Exception e){
                        Debug.LogError(e.Message);
                    }
                    //var infolist2 = JsonUtility.FromJson<ShopInformationsList>(json);
                    if(infolist2 != null && infolist2.shopInfoList != null && infolist.shopInfoList.Count == 1)
                    {
                        var info2 = infolist.shopInfoList[0];
                        info2.descrizione = System.Web.HttpUtility.HtmlDecode(info2.descrizione);
                        info2.descrizione = Regex.Replace(info2.descrizione, @"\s+|\t|\n|\r", " ");
                        info2.orari = System.Web.HttpUtility.HtmlDecode(info2.orari);
                        info2.language = GameConfig.applicationLanguage;
                        //DownloadedShopExtendedInfo.Add(shortInfo, info);
                        DownloadedShopExtendedInfo[shortInfo] = info2;
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
                callBack.Invoke(info);
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
            //OnExtendedInfo?.Invoke(info);
        },true);
    }
/*
    public static IEnumerator GETExtendedInfo(ShopShortInfo shortInfo, Action<ShopInformaitions> callBack)
    {
        string url = AuthorizationAPI.baseURL + shopDeteilsEndpoint;
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("nid", shortInfo.id.ToString());
        headers.Add("languages", GameConfig.applicationLanguage.ToString());

        using (UnityWebRequest request = UnityWebRequest.Post(url, headers))
        {
            /////////////////////////// CERITFICATO NON VALIDO TOGLIERE STRINGA IN SEGUITO /////////////////////////////////////
            request.certificateHandler = new AcceptAnyCertificate();
            /////////////////////////// CERITFICATO NON VALIDO TOGLIERE STRINGA IN SEGUITO /////////////////////////////////////

            // Chiamata per l'autorizzazione dell'header
            AuthorizationAPI.AddAuthRequestHeader(request);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error while reciving: " + request.error);
            }
            else
            {
                //Debug.Log(request.downloadHandler.text);

                string json = "{\"shopInfoList\" : " + request.downloadHandler.text + "}";

                //Debug.Log("dopo: " + json);

                var info = JsonUtility.FromJson<ShopInformationsList>(json).shopInfoList[0];
                info.descrizione = System.Web.HttpUtility.HtmlDecode(info.descrizione);
                info.orari = System.Web.HttpUtility.HtmlDecode(info.orari);
                DownloadedShopExtendedInfo.Add(shortInfo, info);
                callBack.Invoke(info);
                OnExtendedInfo?.Invoke(shortInfo);
            }
        }
    }
*/
/*
    public static IEnumerator GETShopImage(string url, string imageURL, Action<Texture2D> callback)
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
                Debug.Log("Error while reciving: " + request.error);
            }
            else
            {
                try
                {
                    var texture = DownloadHandlerTexture.GetContent(request);
                    callback.Invoke(texture);
                }
                catch (Exception ex) { Debug.Log("Error while assigning the image: " + ex.Message); }
            }
        }
    }
*/

}
