using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AlertPanel : MonoBehaviour
{

    [SerializeField] GameObject alertPanel;
    [SerializeField] TMPro.TextMeshProUGUI alertText;
    [SerializeField] TMPro.TextMeshProUGUI callbackButtonText;
    [SerializeField] GameObject buttonLink;
    [SerializeField] GameObject buttonCallback;
    [SerializeField] GameObject buttonOK;
    [SerializeField] GameObject buttonNO;
    

    LocalizationText locText;
    LocalizationText locButton;

    static bool blocked=false;
    static bool opened=false;
    static string link;
    static AlertPanel instance;

    static System.Action alertCallback,okCall,noCall;

    private void Awake() {
        instance = this;
        locText = alertText.GetComponent<LocalizationText>();
        locButton = buttonCallback.GetComponentInChildren<LocalizationText>();
        locText.enabled = false;
        locButton.enabled = false;
        instance.alertPanel.SetActive(false);
        instance.buttonLink.SetActive(false);
        instance.buttonCallback.SetActive(false);
        instance.buttonNO.SetActive(false);
        instance.buttonOK.SetActive(false);
    }

    public static void OpenAlert(string message, string languageID = null, bool blocking = false){
        Debug.LogWarning("OpenAlert");
        if(blocked && opened) return;
        instance.alertPanel.SetActive(true);
        blocked = blocking;
        opened=true;
        instance.buttonLink.SetActive(false);
        instance.buttonCallback.SetActive(false);
        instance.buttonNO.SetActive(false);
        instance.buttonOK.SetActive(false);
        instance.locText.enabled = false;
        instance.alertText.text = message;
        if(!string.IsNullOrWhiteSpace(languageID)){
            instance.locText.key = languageID;
            instance.locText.enabled = true;
        }
    }

    public static void OpenQuestion(string message, System.Action okCallback, System.Action noCallback, string languageID = null){
        Debug.LogWarning("OpenQuestion");
        if(blocked && opened) return;
        instance.alertPanel.SetActive(true);
        blocked = true;
        opened = true;
        instance.buttonLink.SetActive(false);
        instance.buttonCallback.SetActive(false);
        instance.buttonNO.SetActive(true);
        instance.buttonOK.SetActive(true);
        instance.locText.enabled = false;
        instance.alertText.text = message;
        if(!string.IsNullOrWhiteSpace(languageID)){
            instance.locText.key = languageID;
            instance.locText.enabled = true;
        }
        okCall = okCallback;
        noCall = noCallback;
    }

    public static void SetButtonLink(string url){
        instance.buttonLink.SetActive(true);
        link = url;
    }
    public static void SetButtonCallback(System.Action callback, string buttonText = "OK", string languageID = null){
        instance.buttonCallback.SetActive(true);
        alertCallback = callback;
        //
        instance.locButton.enabled = false;
        instance.callbackButtonText.text = buttonText;
        if(!string.IsNullOrWhiteSpace(languageID)){
            instance.locButton.key = languageID;
            instance.locButton.enabled = true;
        }
    }

    public static void CloseAlert(){
        if(blocked) return;
        Close();
    }
    static void Close(){
        Debug.Log("CloseAlert");
        instance.alertPanel.SetActive(false);
        opened = false;
        link = null;
        alertCallback = null;
        okCall = null;
        noCall = null;
    }

    public static void AlertButtonCLick(){
        if(link!=null) ExternalSitePage.OpenLink(link);
        alertCallback?.Invoke();
        okCall?.Invoke();
        noCall?.Invoke();
        Close();
    }
    public static void OK(){
        okCall?.Invoke();
        Close();
    }
    public static void NO(){
        noCall?.Invoke();
        Close();
    }
}
