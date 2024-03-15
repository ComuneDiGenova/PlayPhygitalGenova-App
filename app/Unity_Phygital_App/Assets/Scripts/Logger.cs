using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Logger : MonoBehaviour
{
    [SerializeField] bool writeLogFile = false;
    string path;

    private void Awake() {
        if (writeLogFile) {
            path = Path.Combine(Application.temporaryCachePath,Application.productName+".txt");
            Debug.LogWarning($"Log Path: {path}");
            var text = $"{Application.productName} v{Application.version}\nUnity {Application.unityVersion}\r\n\n";
            File.WriteAllText(path, text, System.Text.Encoding.UTF8);
            Application.logMessageReceived += Log;
            Application.logMessageReceivedThreaded += Log;
        }
    }

    void Log(string condition, string stackTrace, LogType type){
        var text = $"{System.DateTime.Now:o} {type}\n{condition}\n\n{stackTrace}\r\n\n";
        File.AppendAllText(path, text, System.Text.Encoding.UTF8);
    }

    public void ExportLog(){
        if (!writeLogFile) return;
        NativeFilePicker.ExportFile(path, (success) =>
        {
            if(success){
                Debug.LogWarning("Log file succesfull exported");
            }else{
                Debug.LogError("Error exporting Log file");
            }
        });
    }
}
