using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class VersionManager : MonoBehaviour
{
    const string versionUrl = "https://dev.phygital.bbsitalia.com/sites/default/files/sferiche/versione_app.json";  // TO REMOVE

    [SerializeField] bool checkVersion = false;

    void Start(){
        if(!checkVersion) return;
        WSO2.GETDirectURL(versionUrl, (response) =>  {
            bool update = false;
            if(!string.IsNullOrEmpty(response)){
                try{
                    var jsonv = JsonUtility.FromJson<JsonVersion>(response);
                    //check
                    var remote = new Version(jsonv.versione);
                    var current = new Version(Application.version);
                    update = CompareVersion(remote,current);
                    if(update){
                        AlertPanel.OpenAlert("Nuova versione rilasciata. Aggiornare l'applicazione per proseguire.", "ID_Update", true);
                        if(Application.platform == RuntimePlatform.IPhonePlayer)
                            AlertPanel.SetButtonLink(jsonv.appstore);
                        if(Application.platform == RuntimePlatform.Android)
                            AlertPanel.SetButtonLink(jsonv.playstore);
                        #if UNITY_EDITOR
                            AlertPanel.SetButtonLink(jsonv.playstore);
                        #endif
                    }
                }catch(System.Exception e){
                    Debug.LogError(e.Message);
                }
            }
        },true);
    }

    bool CompareVersion(Version newVersion, Version currentVersion){
        Debug.Log($"Comparing: {newVersion.ToString()} -> {currentVersion.ToString()}");
        bool major = newVersion.major>currentVersion.major;
        bool minor = newVersion.minor>currentVersion.minor;
        bool patch = newVersion.patch>currentVersion.patch;
        if(newVersion.major>currentVersion.major){
            return true;
        }else if(newVersion.major==currentVersion.major){
            if(newVersion.minor>currentVersion.minor){
                return true;
            }else if(newVersion.minor==currentVersion.minor){
                if(newVersion.patch>currentVersion.patch){
                    return true;
                }else{
                    return false;
                }
            }else{
                return false;
            }
        }else{
            return false;
        }
    }

    struct JsonVersion
    {
        public string versione; 
        public string appstore;
        public string playstore;
    }

    struct Version{
        public float major;
        public float minor;
        public float patch;

        public Version(string versionString = null){
            major=0;
            minor=0;
            patch=0;
            if(versionString != null){
                var ver = new List<float>();
                var vstrings = versionString.Split(".");
                foreach(var vs in vstrings){
                    if(float.TryParse(vs, NumberStyles.Float, CultureInfo.InvariantCulture, out float vn)){
                        ver.Add(vn);
                    }
                }
                if(ver.Count > 0)
                    major = ver[0];
                if(ver.Count > 1)
                    minor = ver[1];
                if(ver.Count > 2)
                    patch = ver[2];
            }
        }

        public override string ToString()
        {
            return $"rev {major}.{minor}.{patch}";
        }
    }
}
