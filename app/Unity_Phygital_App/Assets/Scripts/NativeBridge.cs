using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class NativeBridge
{
    public static bool isActive = false;

    //Importing iOS Bridge functions as a DLL.
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void initializeiOSPlugin();

        [DllImport("__Internal")]
        private static extern void startListenToLocationUpdates(string arrayData, string configParams);

        [DllImport("__Internal")]
        private static extern void stopListenToLocationUpdates();
#endif

    //Function is called to check for locaiton and notification permission is provided for the application
    // It will also request permission from the plugin
    public static void InitializeNativePlugins()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            initializeiOSPlugin();
        }
#endif

    }

    //Function is called to initiate locaiton updates and and throw notification based on two params sent.
    public static void StartBackgroundService(List<ShortInfo> listData, object configParams)
    {
        if (isActive) StopBackgroundService();
        Debug.LogWarning("Starting Background Service");
        isActive = true;
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            startListenToLocationUpdates(JsonConvert.SerializeObject(listData), JsonConvert.SerializeObject(configParams));
        }
#endif
        if (Application.platform == RuntimePlatform.Android)
        {
            BGServiceInstance().CallStatic("StartService", GetUnityActivity(), JsonConvert.SerializeObject(listData), JsonConvert.SerializeObject(configParams));
        }
    }

    //Function is called to check for stop location updates from the native plugin application
    public static void StopBackgroundService()
    {
        Debug.LogWarning("Stopping Background Service");
        isActive = false;
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            stopListenToLocationUpdates();
        }
#endif
        if (Application.platform == RuntimePlatform.Android)
        {
            BGServiceInstance().CallStatic("StopService", GetUnityActivity());
        }
    }

    //Fetch the Unity Activity instance in application
    private static AndroidJavaObject GetUnityActivity()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Retrieve the UnityPlayer class.
            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            // Retrieve the UnityPlayerActivity object 
            AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            return unityActivity;
        }
        return null;
    }

    //Bridge Background service to Android plugin application
    private static AndroidJavaObject BGServiceInstance()
    {
        return new AndroidJavaObject("com.technerdssolutions.locationservicelib.BackgroundServicePlugin");
    }
}

