using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class POSTFavourites : MonoBehaviour
{
    static readonly string addFavouriteURL = "/jsonapi/user/add_preferiti_mercato";
    static readonly string removeFavouriteURL = "/jsonapi/user/remove_preferiti_mercato";

    //public static FavouriteList DownloadedFavourtieList;
    //public static AddFavouriteResponse AddRequestResponse; 
    //public static RemoveFavouriteResponse RemoveRequestResponse; 

    public static void AddFavourite(string id, Action<AddFavouriteResponse> callBack){
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("nid", id);
        headers.Add("uid", GameConfig.userID);
        WSO2.POST(addFavouriteURL,headers, (response) =>{
            try{
                var AddRequestResponse = JsonUtility.FromJson<AddFavouriteResponse>(response);
                callBack.Invoke(AddRequestResponse);
                Debug.Log("Esito aggiunta ai preferiti: " + AddRequestResponse.message.ToString());
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        });
        GETUserInfo.AddFavouritePoints(id,(response)=>{
            if(response.result) PopUpPanel.OpenLanguage(GETUserInfo.pointsIdKeyLanguage,true,response.points.ToString());
        });
    }
    public static void RemoveFavourite(string id, Action<RemoveFavouriteResponse> callBack){
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("nid", id);
        headers.Add("uid", GameConfig.userID);
        WSO2.POST(removeFavouriteURL,headers, (response) =>{
            try{
                var RemoveRequestResponse = JsonUtility.FromJson<RemoveFavouriteResponse>(response);
                callBack.Invoke(RemoveRequestResponse);
                Debug.Log("Esito rimozione dai preferiti: " + RemoveRequestResponse.message.ToString());
            }catch(System.Exception e){
                Debug.LogError(e.Message);
            }
        });
    }
/*
    public static IEnumerator POSTAddFavourite(string id, Action<AddFavouriteResponse> callBack)
    {
        string url = AuthorizationAPI.baseURL + addFavouriteURL;
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("nid", id);
        headers.Add("uid", GameConfig.userID.ToString());

        Debug.Log("aggiungendo il poi ai preferiti");

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
                Debug.Log("Aggiunta ai preferiti fallita. " + "Error while reciving: " + request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);

                string json = request.downloadHandler.text;

                Debug.Log(json);

                AddRequestResponse = JsonUtility.FromJson<AddFavouriteResponse>(json);

                callBack.Invoke(AddRequestResponse);

                Debug.Log("Esito aggiunta ai preferiti: " + AddRequestResponse.message.ToString());
            }

        }
    }
*/
/*
    public static IEnumerator POSTRemoveFavourite(string id, Action<RemoveFavouriteResponse> callBack)
    {
        string url = AuthorizationAPI.baseURL + removeFavouriteURL;
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("nid", id);
        headers.Add("uid", GameConfig.userID.ToString());

        Debug.Log("rimuovendo il poi ai preferiti");

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
                Debug.Log("Rimozione dai preferiti fallita. " + "Error while reciving: " + request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);

                string json = request.downloadHandler.text;

                Debug.Log(json);

                RemoveRequestResponse = JsonUtility.FromJson<RemoveFavouriteResponse>(json);

                callBack.Invoke(RemoveRequestResponse);

                Debug.Log("Esito rimozione dai preferiti: " + RemoveRequestResponse.message.ToString());
                //CallCoroutine();
            }
        }
    }
    */
}
