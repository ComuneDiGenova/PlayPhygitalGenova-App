/*

    SSO Manager
    SIRAC-SSO OpenId Connect 1.0 Autentication Service

    NOTA:
    NON E' UN SERVIZIO DI AUTH OpenID ALL PURPOSE: QUESTO SCRIPT SI APPLICA SOLO A SIRAC-SSO ma può essere adattato per qualsiasi OpenID Connect
    Il sistema è realizzato per funzionare solo con SIRAC-SSO allo stato dell'ultime relase del codice.
    Eventuali cambi di specifiche (Endpoint, Algoritmo Chiavi pubbliche, definizioni dei JWT o degli errori di risposta)


    Schema Semplice

    A- google & apple impediscono login in browser embeddati (webview)
    B- redirect link: per mobile necessario un DEEP LINK x ritornare in app con i paramentri necessari


    1- Client genera codici di sessione e registra deep link
    2- Utente chiede di fare login
    3- Client crea url di autenticazione per SIRAC-SSO
    4- Client apre webview NO! obsoleto! google & apple impediscono login in browser embeddati (webview)
    4- Client apre browser di sistema e va in BackGround
    5- SIRAC-SSO risponde con una pagina di scelta del tipo di autenticazione
    6- L’utente sceglie come autenticarsi (es. conGoogle)
    7- SIRAC-SSO invia una richiesta di autenticazione al social login provider (es. Google) -> !Attenzione! azione impedita da browser embeddati
    8- L’utente inserisce le proprie credenziali sulla pagina di login del social login provider
    9- Il social login provider fornisce una risposta con i dati dell’utente autenticato (nominativo e indirizzo email) a SIRAC-SSO
    10- SIRAC-SSO crea un Auth Code per l'accesso
    11- SIRAC-SSO fa un redirect con l'auth code come parametro all'url indicato al punto 3 -> !Attenzione! per mobile app da browser esterno necessario un DEEP LINK x ritornare i napp con i paramentri necessari
    12- Client torne in ForeGround e intercetta deeplink e legge i parametri del redirect
    12- Client fa richiesta di un token a SIRAC-SSO
    13- SIRAC-SSO emette un token per l’app mobile in cui inserisce i dati ricevuti dal provider di autenticazione (es. Google)
    14- SIRAC-SSO risponde direttamente con il token
    15- Client decripta il token JWT per accedere ai dati utente del login provider (es. Google)


    UTILIZZO:
    1- Registrare il metodo "AuthTokenDL" al DeepLink callback tramite "RegisterDeepLink"
        "AuthToken" : valuta la correttezza del redirect con "EvaluateRedirectCode" e poi fare una chiamata per ottenere il token di accesso tremite "CallTokenRequest"
        es:
        SSO_Manager.RegisterDeepLink((dl) => {
            SSO_Manager.AuthTokenDL(dl, (userinfo) => {
                if(userinfo.HasValue)
                    Debug.Log(userinfo.ToString());
                else
                    Debug.Log("FAILED AUTENTICATION");
            });
        });

    1bis- Registrare metodi anonimi all'evento "OnError" per poter intercettare gli errori.

    2- Aprire il Browser di sistema all'url creato con "GetAuthURI"
        es: 
        Application.OpenURL(SSO_Manager.GetAuthURI());

    3- Attendere che il metodo registrato nel DeepLink venga chiamato e salvare il valore di ritorno del callback che è la classe "TokenPayload"
        che contiene i dati dell'utente o è NULL se l'autorizzazione non è andata a buon fine.
    
    4- L'utente è registrato, si può ora richiedere i dati dell'uteste del SIRAC-SSO se servono



    TO DO:
    - testare tutti i tipi di accesso
    - registrare DL e testare callback
    - evento on error e enum tipo errori
    - call dati utente sirac
*/



using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

public class SSO_Manager
{
#region CONFIG
    // CONFIG VALUES
    const string ClientID = ""; // TO REMOVE
    const string ProClientID = ""; // TO REMOVE

    const string TokenIssuer = ""; // TO REMOVE
    const string AuthEndpoint = ""; // TO REMOVE
    const string TokenEndpoint = ""; // TO REMOVE
    const string EndSession_endpoint = "";  // TO REMOVE
    const string JWKS_URI = "";  // TO REMOVE



    const string deepLinkHost = "";  // TO REMOVE
    const string deepLinkLogIn = ""; // TO REMOVE
    const string deepLinkLogOut = "";  // TO REMOVE
    const string redirect_login_uri = "";  // TO REMOVE
    const string redirect_logout_uri = "";  // TO REMOVE

//redirecr


#endregion

#region SESSION

    // VALUES X SESSION
    static string correlation_state; //32 character base64 random string
    static string code_verifier; // 43-128 character base64 random string
    static string code_challenge; //sha256 in base64 of code_verifier
    static ResponseToken responseToken; //toke risposta token request (avvenuta autenticazione)
    public static TokenPayload? userInfo;   //info utente da token jwt

    static void InitializeSSO(bool clear = false){
        if(clear){
            correlation_state = null;
            code_verifier = null;
            code_challenge = null;
        }
        if(string.IsNullOrEmpty(correlation_state)){
            correlation_state = System.Guid.NewGuid().ToString("N");
        }
        if(string.IsNullOrEmpty(code_verifier)){
            code_verifier = System.Guid.NewGuid().ToString("N")+System.Guid.NewGuid().ToString("N");
        }
        if(string.IsNullOrEmpty(code_challenge)){
            code_challenge = EncodeSHA256Base64(code_verifier);
        }
        Debug.Log($"Initialize SSO: {correlation_state} | {code_verifier} | {code_challenge}");
    }

#endregion

#region DEEP LINKING
    
    // Deep link
    //ANDROID
    /*
        In the Project window, go to Assets > Plugins > Android.
        Create a new file and call it AndroidManifest.xml. Unity automatically processes this file when you build your application.
        Copy the following code sample into the new file and save it.
    
        change in file Asset/Plugin/Android/AndroidManifest.xml

        <data android:scheme="visitgenoa.phygital" android:host="sso" />
        <data android:scheme="visitgenoa.phygital" android:host="sso" />
    */
    // IOS
    /*
        Open the iOS Player Settings window (menu: Edit > Project Settings > Player Settings, then select iOS).
        Select Other, then scroll down to Configuration.
        Expand the Supported URL schemes section and, in the Element 0 field, enter the URL scheme to associate with your application. For example, unitydl.
    */
    public const string DeepLinkScheme = "";
    public static readonly string[] DeepLinkHosts = new string[2]{"",""};

    /// <summary>
    /// Register Deeplink callback
    /// </summary>
    /// <param name="DeepLinkCallback">If null a mock method with a debug log will be registered instead</param>
    public static void RegisterDeepLink(Action<string> DeepLinkCallback = null){
        Debug.Log("Deep Link Initialization : " + Application.absoluteURL);
        if(DeepLinkCallback != null)
            Application.deepLinkActivated += DeepLinkCallback;
        else
            Application.deepLinkActivated += onDeepLinkActivated;
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            // Cold start and Application.absoluteURL not null so process Deep Link.
            if(DeepLinkCallback != null)
                DeepLinkCallback.Invoke(Application.absoluteURL);
            else
                onDeepLinkActivated(Application.absoluteURL);
        }
    }

    public static void EvalDeepLink(string dl, Action loginCallBack, Action logoutCallBack, Action invalidCallBack){
        var vdl = ValidateDeepLink(dl);
        if(string.IsNullOrEmpty(vdl)){
            Debug.LogError($"'{vdl}' -> '{deepLinkHost}'");
            invalidCallBack?.Invoke();
            return;
        }else{
            Debug.Log($"Valid Host: {vdl}");
        }
        var link = GetDLink(dl);
        Debug.Log($"DL link: {link}");
        var param = GetDLParameters(dl);
        if(link == deepLinkLogIn){
            loginCallBack?.Invoke();
        }else if(link == deepLinkLogOut){
            logoutCallBack?.Invoke();
        }else{
            Debug.LogError($"UNKNOW DL");
        }
    }

    /// <summary>
    /// Validate DL Against Registered HOST
    /// </summary>
    /// <param name="url">DL Url</param>
    /// <returns>Host or null if none or invalid</returns>
    public static string ValidateDeepLink(string dl){
        var dlhost = dl.Split(":")[0];
        if (dlhost != deepLinkHost) return null;
        else return dlhost;
    }
    public static string GetDLink(string dl){
        try{
            var dlinks = dl.Split(":")[1].Split("?")[0].Replace("//","");
            return dlinks;
        }catch{
            return null;
        }
        
    }
    public static string GetDLParameters(string dl){
        //var dlhost = dl.Split(":")[1].Split("?");
        var param = dl.Split("?");
        if(param.Length == 1){
            Debug.LogWarning("DL NO PARAMETER");
            return null;
        }else{
            Debug.LogWarning("DL PARAMETER: " + param[1]);
            return param[1];
        }
    }

    static void onDeepLinkActivated(string url){
        // Decode the URL to determine action. 
        // In this example, the app expects a link formatted like this:
        // unitydl://mylink?value
        string value = url.Split('?')[1];
        Debug.Log("DeepLink Url: " + url);
    }

#endregion

#region UTILITY

    //GENERIC INTERN UTILITY

    static Dictionary<string,string> ParseUrl(string url){
        return url.Split("&").ToDictionary(x => x.Split("=")[0], x => x.Split("=")[1]);
    }

    /// <summary>
    /// Encode a Base64 string in Base64Url
    /// ref RFC 4648 - 
    /// </summary>
    /// <param name="base64">Base64 string to encode</param>
    /// <returns>Base64url string encoded</returns>
    static string Base64UrlEncoder(string base64){
        return base64.Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
    static string Base64UrlDecoder(string base64URL){
        var base64 = base64URL.Replace("-", "+").Replace("_", "/");
        while(base64.Length % 4 != 0){
            base64 += "=";
        }
        return base64;
    }

    static string EncodeSHA256Base64(string plain){
        if(string.IsNullOrEmpty(plain)){
            return null;
        }
        using (SHA256 mySHA256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(plain);
            var encoded = Base64UrlEncoder(Convert.ToBase64String(mySHA256.ComputeHash(bytes)));
            return encoded;
        }
    }


#endregion

#region PRIVATE AUTH TOKEN

    private static Dictionary<string,string> GetTokenPayload(string authCode){
        var parameters = new Dictionary<string,string>(){
            {"grant_type", "authorization_code"},
            {"code", authCode},
            {"client_id", GameConfig.proUserRequest ? ProClientID : ClientID},
            {"redirect_uri", redirect_login_uri},
            {"code_verifier", code_verifier}
        };
        string content = "";
        foreach(var kvp in parameters){
            content += "&"+kvp.Key + "=" + kvp.Value;
        }
        content = content.Substring(1,content.Length-1);
        Debug.Log(content);
        return parameters;
    }

    private static bool EvaluateResponseToken(ResponseToken token){
        if(token.session_state == correlation_state){
            return true;
        }else{
            return false;
        }
    }


    /// <summary>
    /// https://en.wikipedia.org/wiki/JSON_Web_Token#Structure
    /// </summary>
    /// <param name="token">JWT to Decrypt</param>
    /// <param name="validateToken">Validate time and user</param>
    /// <param name="checkSignature">Validate signature</param>
    /// <returns>Token Payload if succesful decrypt and validation, NULL if not</returns>
    private static async Task<TokenPayload?> DecryptJWToken(ResponseToken token, string jwt, bool validateToken = true, bool checkSignature = true){
        Debug.Log("Decrypt JWT");
        if(responseToken.id_token == null){
            Debug.LogError("NO RESPONSE TOKEN ID");
            return null;
        }
        var tks = token.id_token.Split(".");
        if(tks.Length != 3){
            Debug.LogError("Error in TOKEN STRUCTURE");
            return null;
        }
        var header = tks[0];
        var payload = tks[1];
        var signature = tks[2];
        try
        {
            string jhead = Encoding.UTF8.GetString(Convert.FromBase64String(Base64UrlDecoder(header)));
            TokenHeader tkhead = JsonUtility.FromJson<TokenHeader>(jhead);
            string jpay = Encoding.UTF8.GetString(Convert.FromBase64String(Base64UrlDecoder(payload)));
            TokenPayload tkpayload = JsonUtility.FromJson<TokenPayload>(jpay);
            tkpayload.jwt = jwt;
            //string hash = Encoding.UTF8.GetString(Convert.FromBase64String(Base64UrlDecoder(signature)));
       
            Debug.Log(tkhead.ToString());
            Debug.Log(tkpayload.ToString());

            if(string.IsNullOrEmpty(tkpayload.iss))
                return null;

            if(validateToken){
                if(tkpayload.iss != TokenIssuer || tkpayload.aud != (GameConfig.proUserRequest ? ProClientID : ClientID)){
                    Debug.Log("Invalid Token! iss or aud wrong");
                    return null;
                }
            }

            if(checkSignature){
                //TO DO
                var key = await GetKeysAsync(tkhead.kid);
                if(!key.HasValue)
                    return null;
                using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()){
                    rsa.ImportParameters(
                        new RSAParameters() {
                            Modulus = Convert.FromBase64String(Base64UrlDecoder(key.Value.n)),
                            Exponent = Convert.FromBase64String(Base64UrlDecoder(key.Value.e))
                    });
                    RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                    rsaDeformatter.SetHashAlgorithm("SHA256");
                    SHA256 sha256 = SHA256.Create();
                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(header+ "." + payload));
                    if (rsaDeformatter.VerifySignature(hash, Convert.FromBase64String(Base64UrlDecoder(signature)))){
                        Debug.Log("Signature is verified");
                    }else{
                        Debug.Log("Signature is NOT verified");
                        return null;
                    }
                }
            }
            return tkpayload;
        }catch(System.Exception e){
            Debug.LogError(e.Message);
            return null;
        }
    }

    static async Task<Key?> GetKeysAsync(string kid){
        LoadingPanel.OpenPanel();
        using(UnityWebRequest uwr = UnityWebRequest.Get(JWKS_URI)){
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if(uwr.result != UnityWebRequest.Result.Success){
                 Debug.LogError($"{JWKS_URI} | {uwr.result} | {uwr.error}");
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    WSO2.NoConnectionError(false);
                LoadingPanel.ClosePanel();
                return null;
            }else{
                Keys keys = JsonUtility.FromJson<Keys>(uwr.downloadHandler.text);
                var key = keys.keys.Where(x=>x.kid == kid).FirstOrDefault();
                LoadingPanel.ClosePanel();
                if(string.IsNullOrEmpty(key.kid))
                    return null;
                else
                    return key;
            }
        }
    }

    /// CAN RETURN NULL !
    static async Task<TokenPayload?> CallTokenRequest(string authcode){
        LoadingPanel.OpenPanel();
        Debug.Log("CALL REQUEST TOKEN");
        InitializeSSO();
        if(string.IsNullOrEmpty(authcode)){
            return null;
        }
        Debug.LogWarning("Token Endpoint: " + TokenEndpoint);
        var headers = GetTokenPayload(authcode);
        using(UnityWebRequest uwr = UnityWebRequest.Post(TokenEndpoint, headers)){
            //uwr.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            LogRepsonse(uwr);
            if(uwr.result != UnityWebRequest.Result.Success){
                Debug.LogError($"{TokenEndpoint} | {uwr.result} | {uwr.error}");
                Debug.LogError(uwr.downloadHandler.text);
                if(uwr.result == UnityWebRequest.Result.ConnectionError)
                    WSO2.NoConnectionError(false);
                userInfo = null;
            }else{
                Debug.Log($"OK: {uwr.downloadedBytes} bytes");
                responseToken = JsonUtility.FromJson<ResponseToken>(uwr.downloadHandler.text);
                Debug.Log(responseToken.ToString());
                userInfo = await DecryptJWToken(responseToken,uwr.downloadHandler.text);
            }
        }
        LoadingPanel.ClosePanel();
        return userInfo;
    }


#endregion

#region PUBBLIC CALL


    /// <summary>
    /// Evaluate redirect uri for error and extract auth code if auth successful
    /// </summary>
    /// <param name="uri">redirect uri</param>
    /// <returns>Auth code, null if error or no auth</returns>
    public static string EvaluateRedirectCode(string uri){
        if(!uri.StartsWith(redirect_login_uri)){
            Debug.LogError($"Redirect url NOT match: {redirect_login_uri} -> {uri}");
            return null;
        }else{
            Debug.Log($"Redirect url match!");
        }
        string code = uri.Split("?")[1];
        Debug.Log("Code: " + code);
        var values = ParseUrl(code);
        foreach(var kvp in values){
            Debug.Log($"{kvp.Key}:{kvp.Value}");
        }
        bool eval = true;
        //Check Errors
        if(values.ContainsKey("error")){
            eval = false;
            if(values.ContainsKey("error_description"))
                Debug.LogError($"Request Error: {values["error"]}, error_description : {values["error_description"]}");
            else
                Debug.LogError($"Request Error: {values["error"]}");
        }
        if(values.ContainsKey("error_uri")){
            eval = false;
            Debug.LogError($"Request Error: {values["error_uri"]}");
        }

        //Check CORRELATION STATE
        if(values.ContainsKey("state")){
            if(values["state"] != correlation_state){
                eval = false;
                Debug.Log($"WRONG key 'state' : {values["state"]} -> {correlation_state}");
            }
        }else{
            eval = false;
             Debug.Log("Missing KEY 'state'");
        }
        //Check SESSION
        if(values.ContainsKey("session_state")){
            //null
        }else{
            eval = false;
            Debug.Log("Missing KEY 'session_state'");
        }
        //GET AUTH
        if(!values.ContainsKey("code")){
            eval = false;
            Debug.Log("Missing KEY 'code'");
        }
        return eval ? values["code"] : null;
    }

    // AUTH REQUESTS //
    /// <summary>
    /// Get Authorization URI
    /// ALERT: RESET PREVIUS SESSION!
    /// </summary>
    /// <returns>auth uri</returns>
    public static string GetAuthURI(){
        InitializeSSO(true);
        var parameters = new Dictionary<string,string>(){
            {"response_type", "code"},
            {"client_id", GameConfig.proUserRequest ? ProClientID : ClientID},
            {"redirect_uri", redirect_login_uri},
            {"scope", "openid"},
            {"state", correlation_state},
            {"code_challenge", code_challenge},
            {"code_challenge_method","S256"}
        };
        string content = "";
        foreach(var kvp in parameters){
            content += "&"+kvp.Key + "=" + kvp.Value;
        }
        content = content.Substring(1,content.Length-1);
        return AuthEndpoint+"?"+content;
    }

    /// <summary>
    /// Aync Web Request at Token Endpoint
    /// </summary>
    /// <param name="authcode">Auth code from Auth request</param>
    /// <param name="callback">Callback for response token (can be Null)</param>
    /// <returns></returns>
    public static async void CallTokenRequest(string authcode, Action<TokenPayload?> callback){
        var response = await CallTokenRequest(authcode);
        callback.Invoke(response);
    }


    /// <summary>
    /// Evaluate redirect uri and Get Auth Token
    /// </summary>
    /// <param name="redirect_uri">redirect uri</param>
    /// <param name="callback">Token Payload, null if not succesfull auth or wrong redirect uri</param>
    /// <returns></returns>
    public static async void AuthToken(string redirect_uri, Action<TokenPayload?> callback){
        string authcode = EvaluateRedirectCode(redirect_uri);
        if(!string.IsNullOrEmpty(authcode)){
            var response = await CallTokenRequest(authcode); //can be null
            callback.Invoke(response);
        }else{
            callback.Invoke(null);
        }
    }

    /// <summary>
    /// Evaluate Deep Link redirect uri and Get Auth Token
    /// </summary>
    /// <param name="deepLink_url">deep link uri</param>
    /// <param name="callback">Token Payload, null if not succesfull auth or wrong redirect uri</param>
    /// <returns></returns>
    public static void AuthTokenDL(string deepLink_url, Action<TokenPayload?> callback){
        /*
        if(string.IsNullOrEmpty(ValidateDeepLink(deepLink_url)))
            callback.Invoke(null);
            */
        AuthToken(deepLink_url,callback);
    }

    // NOT USED
    /*
    public static async void CallAuthRequest(Action<string> callback){
        string response = await CallAuthRequest();
        callback.Invoke(response);
    }

    static async Task<string> CallAuthRequest(){
        string request_uri = GetAuthURI();
        Debug.Log(request_uri);
        using(UnityWebRequest uwr = UnityWebRequest.Get(request_uri)){
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if(uwr.result != UnityWebRequest.Result.Success){
                Debug.LogError(uwr.error);
            }else{
                Debug.Log($"OK: {uwr.downloadedBytes} bytes");
            }
            LogRepsonse(uwr);
            return uwr.downloadHandler.text;
        }
    }
*/
/*
https://endSessionEndpoint?
id_token_hint=<ID_TOKEN>&
post_logout_redirect_uri=<POST_LOGOUT_REDIRECT_U
RI>
*/
    public static async void Logout(){
        if(responseToken.id_token == null) {
            Debug.LogWarning("No Sirac Id TOKEN");
            return;
        }
        //string url = EndSession_endpoint + "?id_token_hint=" + responseToken.id_token +"&post_logout_redirect_uri=" + redirect_logout_uri;
        string url = EndSession_endpoint + "?post_logout_redirect_uri=" + redirect_logout_uri + "&id_token_hint=" + responseToken.id_token;
        Debug.Log("GET: " + url);
        LoadingPanel.OpenPanel();
        using(UnityWebRequest uwr = UnityWebRequest.Get(url)){
            //AuthorizationAPI.AddAnyCertificateHandler(uwr);
            uwr.SendWebRequest();
            while(!uwr.isDone){
                await Task.Yield();
            }
            if(uwr.result != UnityWebRequest.Result.Success){
                Debug.LogError($"{url} | {uwr.result} | {uwr.error}");
                Debug.LogError(uwr.downloadHandler.text);
            }else{
                //ok
                string json = uwr.downloadHandler.text;
                Debug.Log(json);
            }
        }
        LoadingPanel.ClosePanel();
    }

#endregion

#region Definitions

    /// <summary>
    /// SIRAC RESPONSE TOKEN
    /// </summary>
    [Serializable]
    struct ResponseToken{
        public string access_token;
        public int expires_in;
        public int refresh_expires_in;
        public string refresh_token;
        public string token_type;
        public string id_token; //token accesso jwt
        public string session_state;
        public string scope;
        
        public override string ToString()
        {
            return JsonUtility.ToJson(this,true);
        }
    }

/****** CERTIFICATE KEY */

    [Serializable]
    struct Keys{
        public Key[] keys;
    }
    [Serializable]
    struct Key{
        public string kid;
        public string kty;
        public string alg;
        public string use;
        public string n;
        public string e;
        public string[] x5c;
        public string x5t;
        //public string x5t#S256;
    }

    /// <summary>
    /// JWT TOKEN
    /// </summary>
    [Serializable]
    struct TokenHeader{
        public string alg;
        public string typ;
        public string kid;

        public override string ToString()
        {
            return JsonUtility.ToJson(this,true);
        }
    }

/*

*/
    [Serializable]
    public struct TokenPayload{
        public int exp;
        public int iat;
        public string iss;
        public string aud;
        public string comge_cognome;
        public string comge_nome;
        public string comge_emailAddress;
        public string comge_emailAddressPersonale;
        public string comge_codicefiscale;

        public string jwt; //raw jwt token

        public override string ToString()
        {
            return JsonUtility.ToJson(this,true);
        }
    }

/*

*/

#endregion

#region MISCELLANEA
    //VARIE

    private static string LogRepsonse(UnityWebRequest uwr){
        string responseLog = $"Response Code: {uwr.responseCode}\n";
        var headers = uwr.GetResponseHeaders();
        if(headers != null){
            foreach(var kvp in headers){
                responseLog += $"{kvp.Key} : {kvp.Value}\n";
            }
        }
        responseLog += "\nBody:\n"+uwr.downloadHandler.text;
        Debug.Log(responseLog);
        return responseLog;
    }

#endregion

}
