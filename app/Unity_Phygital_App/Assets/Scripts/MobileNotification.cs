using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using System;

public class MobileNotification : MonoBehaviour
{
    [SerializeField] bool useLocalNotification = false;
    float delay = 0.2f;

#if UNITY_ANDROID
    static AndroidNotificationChannel poi_channel = new AndroidNotificationChannel(){
        Id = "poi_channel",
        Name = "Poi Channel",
        Importance = Importance.Default,
        Description = "Near Poi Notification"
    };

    AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
    delegate(AndroidNotificationIntentData data)
    {
        var msg = "Notification received : " + data.Id + "\n";
        msg += "\n Notification received: ";
        msg += "\n .Title: " + data.Notification.Title;
        msg += "\n .Body: " + data.Notification.Text;
        msg += "\n .Channel: " + data.Channel;
        Debug.Log(msg);
    };

    IEnumerator RequestNotificationPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;
        // here use request.Status to determine users response
    }

    #endif


    #if UNITY_IOS

    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
    }

    #endif

        private void PostRequest() {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
#endif 
    }

    void Init(){
    #if UNITY_ANDROID
        StartCoroutine(RequestNotificationPermission());
        AndroidNotificationCenter.RegisterNotificationChannel(poi_channel);
        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
    #endif
    #if UNITY_IOS
        StartCoroutine(RequestAuthorization());
    #endif
    }

    void Start(){
        Invoke("PostRequest",delay);
        if(useLocalNotification) Invoke("Init",delay+0.2f);
    }

    public static void SendPOINotification(string poi_name = null){
        #if UNITY_ANDROID

        var poi_notification = new AndroidNotification(){
            Title = "Punto di interesse vicino",
            Text = $"{poi_name} nelle vicinanze",
            SmallIcon = "icon_small",
            LargeIcon = "icon_large",
            FireTime = System.DateTime.Now.AddSeconds(1)
        };
            AndroidNotificationCenter.SendNotification(poi_notification, poi_channel.Id);

        #endif

        #if UNITY_IOS

            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(0, 0, 1),
                Repeats = false
            };

            var ipoi_notification = new iOSNotification()
            {
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = "_poi_near_01",
                Title = "Punto di interesse vicino",
                Body = $"{poi_name} nelle vicinanze",
                Subtitle = "This is a subtitle, something, something important...",
                ShowInForeground = false,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_poi",
                ThreadIdentifier = "thread_poi",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(ipoi_notification);

        #endif

        Debug.LogWarning("Notifica inviata: " + poi_name);
    }


}
