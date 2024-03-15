using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class RemoteErrorLog
{
    const string LogEndpoint = null;

    public static void LogError(string message){
        if (string.IsNullOrEmpty(LogEndpoint)) return;
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "message", message }
        };
        WSO2.POST(LogEndpoint, headers, null, true, () =>
        {
            Debug.LogError($"Cannot remote log error: {message}");
        });
    }
    public static void LogError(string message, string endpoint_url, string error_code, string error_response){
        if (string.IsNullOrEmpty(LogEndpoint)) return;
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "message", message },
            { "endpoint", endpoint_url },
            { "err_code", error_code },
            { "err_response", error_response }
        };
        WSO2.POST(LogEndpoint, headers, null, true, () =>
        {
            Debug.LogError($"Cannot remote log error: {message}");
        });
    }
}
