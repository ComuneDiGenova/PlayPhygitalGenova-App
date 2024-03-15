using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class GPSAsk : MonoBehaviour
{
    [SerializeField] string textID = "ID_gps";

 #if UNITY_ANDROID
    //PermissionCallbacks callbacks = new PermissionCallbacks();
#endif

    //async 
    async void Start()
    {
        await System.Threading.Tasks.Task.Yield();
        bool permissions = true;
#if UNITY_ANDROID && !UNITY_EDITOR
        permissions = Permission.HasUserAuthorizedPermission(Permission.FineLocation) && Permission.HasUserAuthorizedPermission("android.permission.ACCESS_BACKGROUND_LOCATION") && Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS");
#endif
        if (GameConfig.backgroundGPS != false && !permissions) {
            AskGps(true);
        }
    }

    public void AskGps(bool start = false){
        GameConfig.gpsWaiting = true;
        AlertPanel.OpenQuestion("GPS", () => {
            GameConfig.gpsWaiting = false;
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.LogWarning("Permission request Background");
            PermissionCallbacks callbacks1 = new PermissionCallbacks();
            callbacks1.PermissionGranted += (content) =>
            {
                Debug.LogWarning("Permission GRANTED " + content);
                PermissionCallbacks callbacks2 = new PermissionCallbacks();
                callbacks2.PermissionGranted += (content) =>
                {
                    Debug.LogWarning("Permission GRANTED " + content);
                    GameConfig.backgroundGPS = true;
                    GameConfig.SetPlayerPrefs();
                    PermissionCallbacks callbacks3 = new PermissionCallbacks();
                    callbacks3.PermissionGranted += (content) =>
                    {
                        Debug.LogWarning("Permission GRANTED " + content);
                        GameConfig.backgroundGPS = true;
                        GameConfig.SetPlayerPrefs();
                        if (!start && !NativeBridge.isActive) CheckPOIProximity.StartBackgroundService();
                    };
                    Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS", callbacks3);
                };
                Permission.RequestUserPermission("android.permission.ACCESS_BACKGROUND_LOCATION", callbacks2);
                
            };
            Permission.RequestUserPermission(Permission.FineLocation, callbacks1);
#endif
#if UNITY_IOS && !UNITY_EDITOR
            GameConfig.backgroundGPS = true;
            GameConfig.SetPlayerPrefs();
            if (!start && !NativeBridge.isActive) CheckPOIProximity.StartBackgroundService();
#endif
        }, () => { 
            GameConfig.backgroundGPS = false;
            GameConfig.gpsWaiting = false;
            GameConfig.SetPlayerPrefs();
            if (!start && NativeBridge.isActive) NativeBridge.StopBackgroundService();
        }, textID);
    }

    
    

}
