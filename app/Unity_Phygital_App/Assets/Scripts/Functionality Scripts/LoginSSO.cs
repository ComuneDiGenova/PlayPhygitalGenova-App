using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

public class LoginSSO : MonoBehaviour
{
    [SerializeField] GETUserInfo userInfo;
    [SerializeField] UIManager uiManager;

    [Header("Debug")]
    [SerializeField] public bool fakeAuth = false;
    [SerializeField] public string fakeUserMail = "";
    [SerializeField] public bool fakeWrongAuth = false;
    [SerializeField] public bool fakeExchange = false;
    [SerializeField] public string fakeUserId = "";
    [SerializeField] public string fakeDrupalUserId = "";
    [SerializeField] SSO_Manager.TokenPayload UserTokenInfo;

    readonly string userIdEndpoint = "/jsonapi/user/get_user_code";
    readonly string userBaseUrl = "";  // TO REMOVE

    public delegate void SSOEvent();
    public delegate void SSOEventBool(bool anon = false);
    public delegate void SSOEventMessage(string message);
    public static event SSOEventBool OnLoginSuccesfull;
    public static event SSOEvent OnLogOut;
    public static event SSOEventMessage OnLoginError;


    public static LoginSSO instance {get;private set;}

    private void Awake() {
        instance = this;
        //AUTH TOKEN REQUEST
        /*
        SSO_Manager.RegisterDeepLink((dl) => {
            SSO_Manager.AuthTokenDL(dl, (userinfo) => {
                AuthToken(userinfo);
            });
        });
        */
        
        SSO_Manager.RegisterDeepLink((dl) => {
            Debug.LogWarning("DL: " +dl);
            #if UNITY_IOS && !UNITY_EDITOR
            Assets.SafariView.SafariViewController.Close();
            #endif
            SSO_Manager.EvalDeepLink(dl, () =>{
                SSO_Manager.AuthTokenDL(dl, (userinfo) => {
                    AuthToken(userinfo);
                });
            }, () =>{
                //logout
                Debug.LogWarning("Successful Sirac LogOut");
            }, () =>{
                Debug.LogError("Wrong Deeplink Host");
            });
        });
    }

    void AuthToken(SSO_Manager.TokenPayload? userinfo){
        if(userinfo.HasValue){
            UserTokenInfo = userinfo.Value;
            Debug.LogError("SIRAC AUTENTICATED");
            ExchangeUserToken();
        }else{
            Debug.LogError("FAILED AUTENTICATION");
            OnLoginError?.Invoke("SSO SIRAC AUTENTICATION FAILED");
        }
    }

    public void AuthSSO(bool proUser = false){
        GameConfig.proUserRequest = proUser;
        if(fakeAuth) {
            if(fakeWrongAuth){
                OnLoginError?.Invoke("Fake autentication: wrong login");
            }else{
                //GameConfig.userID = fakeUserId;
                //OnLoginSuccesfull?.Invoke();
                ExchangeUserToken();
            }
        }else{
            Debug.Log(SSO_Manager.GetAuthURI());
            #if UNITY_ANDROID || UNITY_EDITOR
            Application.OpenURL(SSO_Manager.GetAuthURI());
            #endif
            #if UNITY_IOS && !UNITY_EDITOR
            Assets.SafariView.SafariViewController.OpenURL(SSO_Manager.GetAuthURI());
            #endif
        }
    }

    /// <summary>
    /// Ottiene Token id Drupal da Token auth sirac
    /// </summary>
    private void ExchangeUserToken(){
        if(fakeExchange){
            GameConfig.userID = fakeUserId;
            GameConfig.userDrupalId = fakeDrupalUserId;
            OnLoginSuccesfull?.Invoke();
        }else{
            if(fakeAuth){
                UserTokenInfo = new SSO_Manager.TokenPayload(){
                    comge_emailAddressPersonale = fakeUserMail
                };
            }
            GetUserId((user,message) => {
                if(user == null){
                    Debug.Log("NO USER");
                    GameConfig.userID = null;
                    GameConfig.userDrupalId = null;
                    OnLoginError?.Invoke(null);
                }else{
                    Debug.Log("USER ID:" + user.uid);
                    GameConfig.userID = user.codice_utente;
                    GameConfig.userDrupalId = user.uid;
                    OnLoginSuccesfull?.Invoke();
                }
            });
        }
        //

    }

    async void GetUserId(Action<UserResponse,string> callBack){
        string url = userBaseUrl+userIdEndpoint;
        Dictionary<string,string> headers = new Dictionary<string, string>();
        if(string.IsNullOrEmpty(UserTokenInfo.comge_emailAddressPersonale))
        {
            Debug.LogError("No EMAIL from SIRAC");
            callBack?.Invoke(null,null);
        }
        headers.Add("email",UserTokenInfo.comge_emailAddressPersonale);
        if(!string.IsNullOrEmpty(UserTokenInfo.comge_codicefiscale))
            headers.Add("cf",UserTokenInfo.comge_codicefiscale);
        WSO2.POST(userIdEndpoint,headers,(response) => {
            Debug.Log("EXCHANGE USER ID RESPONSE: " + response);
            if(!string.IsNullOrEmpty(response)){
                try{
                    var user = JsonUtility.FromJson<UserResponse>(response);
                    if(user.result == true){
                        callBack?.Invoke(user,null);
                    }else{
                        Debug.LogError(user.message);
                        callBack?.Invoke(null,user.message);
                    }
                }catch(Exception e){
                    Debug.LogError(e.Message);
                    callBack?.Invoke(null,null);
                }
            }else{
                Debug.LogError("Null response from " + userIdEndpoint);
                callBack?.Invoke(null,"Null response from " + userIdEndpoint);
            }
        });
    }


    [Serializable]
    class UserResponse{
        public bool result = false;
        public string codice_utente;
        public string uid;
        public string message;
    }


    public  static void LogOut(){
        GameConfig.userID=null;
        GameConfig.isLogged = false;
        GameConfig.userDrupalId = null;
        GameConfig.userID = null;
        GameConfig.isProUser = false;
        GameConfig.isLoggedPro = false;
        GameConfig.proUserRequest=false;
        GameConfig.ClearPlayerPrefs();
        SSO_Manager.Logout();
        OnLogOut?.Invoke();
    }

    public void NoLogin(){
        GameConfig.userID=null;
        GameConfig.isLogged = false;
        GameConfig.userDrupalId = null;
        GameConfig.userID = null;
        GameConfig.isProUser = false;
        GameConfig.isLoggedPro = false;
        GameConfig.proUserRequest=false;
        uiManager.CallNoLogIn();
        OnLoginSuccesfull?.Invoke(true);
    }
}
