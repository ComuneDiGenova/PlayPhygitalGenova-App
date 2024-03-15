using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Login : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI log_text;
    [SerializeField] TMP_InputField redirect_uri;
    [SerializeField] string deepLinkTest_url = "";
    [SerializeField] bool useInAppBrower = true;
    [SerializeField] SSO_Manager.TokenPayload UserTokenInfo;

    WebViewObject webViewObject;

    private void Awake() {
        //LOG GENERICO
        /*
        SSO_Manager.RegisterDeepLink((dl) => {
            var host = SSO_Manager.ValidateDeepLink(dl);
            if (host != null){
                var param = SSO_Manager.GetDLParameters(dl);
                log_text.text = "DEEP LINK PARAM: " + param;
            }
        });
        */
        //AUTH TOKEN REQUEST
        /*
        SSO_Manager.RegisterDeepLink((dl) => {
            SSO_Manager.EvalDeepLink(dl,() => {
                SSO_Manager.AuthTokenDL(dl, (userinfo) =>
                {
                    if (userinfo.HasValue)
                    {
                        UserTokenInfo = userinfo.Value;
                        log_text.text = userinfo.ToString();
                    }
                    else
                        log_text.text = "FAILED AUTENTICATION";
                });
                }, 
                null,
                null);
        });
        */
    }

/*
    private void Start() {
        
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        
        webViewObject.Init(
        (msg) =>{
            Debug.Log($"JS[{msg}]");
        },
        (msg) =>{
            Debug.Log($"err[{msg}]");
        },
        (msg) =>{
            Debug.Log($"HttpErr[{msg}]");
        },
        (msg) =>{
            Debug.Log($"loaded[{msg}]");
            string auth = SSO_Manager.EvaluateRedirectCode(msg);
            if(auth != null){
                webViewObject.SetVisibility(false);
                SSO_Manager.AuthToken(msg, (userinfo) => {
                    if(userinfo.HasValue){
                        UserTokenInfo = userinfo.Value;
                        log_text.text = userinfo.ToString();
                    }else
                        log_text.text = "FAILED AUTENTICATION";
                });
            }
        },
        (msg) =>{
            Debug.Log($"Started[{msg}]");
        },
        (msg) =>{
            Debug.Log($"hooked[{msg}]");
        },
        (msg) =>{
            Debug.Log($"cookies[{msg}]");
        }
        );
        
        webViewObject.SetMargins(50,50,50,50);
        webViewObject.SetVisibility(false);
        

    }
*/
/*
    public void Auth(){
        Debug.Log(SSO_Manager.GetAuthURI());
        Application.OpenURL(SSO_Manager.GetAuthURI());
    }

    public void AuthWV(){
        if(!useInAppBrower){
            webViewObject.LoadURL(SSO_Manager.GetAuthURI());
            webViewObject.SetVisibility(true);
        }else{
            InAppBrowser.OpenURL(SSO_Manager.GetAuthURI());
        }
    }
*/
/*
    public void Token(){
        SSO_Manager.AuthToken(redirect_uri.text, (userinfo) => {
            if(userinfo.HasValue){
                UserTokenInfo = userinfo.Value;
                log_text.text = "SUCCESFULL AUTENTICATION\n";
                log_text.text += userinfo.ToString();
                Debug.LogWarning("AUTENTICATED");
                Debug.Log(UserTokenInfo.ToString());
            }else
                log_text.text = "FAILED AUTENTICATION";
                Debug.LogError("FAILED AUTENTICATION");
        });
    }
*/

    public void DeepLinkTest(){
        Application.OpenURL(deepLinkTest_url);
    }
    public void DeepLinkTestWV(){
        webViewObject.LoadURL(deepLinkTest_url);
        webViewObject.SetVisibility(true);
    }

    public void ResetWebView(){
        if(!useInAppBrower){
            webViewObject.ClearCache(false);
            webViewObject.ClearCookies();
            webViewObject.ClearCustomHeader();
        }else{
            InAppBrowser.ClearCache();
        }
    }
}
