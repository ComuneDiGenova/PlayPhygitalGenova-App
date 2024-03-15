using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalSitePage : MonoBehaviour
{
    const string privacyUrl = "";   // TO REMOVE
    const string transactionUrl = ""; // TO REMOVE
    const string registrationUrl = "";    // TO REMOVE
    
    public static void OpenTransactions(){
        string url = transactionUrl.Replace("*",GameConfig.userDrupalId);
        OpenLink(url);
    }
    public static void OpenPrivacy(){
        OpenLink(privacyUrl);
    }
    public static void OpenRegistration(){
        OpenLink(registrationUrl);
    }
    public static void OpenLink(string url){
        //Application.OpenURL(url);
        #if UNITY_ANDROID || UNITY_EDITOR
        Application.OpenURL(url);
        #endif
        #if UNITY_IOS && !UNITY_EDITOR
        Assets.SafariView.SafariViewController.OpenURL(url);
        #endif
    }

}
