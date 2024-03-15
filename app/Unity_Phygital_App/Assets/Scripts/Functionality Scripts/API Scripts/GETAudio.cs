using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GETAudio : MonoBehaviour
{

    // AUDIO
    [HideInInspector] public AudioClip audioClip;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] CTRL_AUDIO CTRLAudioPanel;
    [SerializeField] CTRL_AUDIO CTRLAudioARPanel;
    [SerializeField] CTRL_AUDIO CTRLAudioAR;
    static string lastPath;
    static ShortInfo lastInfo;
    static Info infoPoi;
    public static GETAudio instance;

    float listenedMin = 0.5f;
    float currentListening=0;
    bool listeningPoint = false;

    public static bool isPlaying {
        get { return instance.audioSource.isPlaying;}
    }
    

    private void Awake() {
        instance=this;
    }

    private void Start() {
        StopAudio();
    }

    void Update(){
        if(audioSource.isPlaying && !listeningPoint){
            currentListening += Time.deltaTime;
            if(currentListening/audioClip.length > listenedMin){
                listeningPoint = true;
                GETUserInfo.AddListeningPoints(infoPoi.id, (response)=>{
                    if(response.result) PopUpPanel.OpenLanguage(GETUserInfo.pointsIdKeyLanguage,true,response.points.ToString());
                });
            }
        }

    }

    public static void StopAudio(){
        Debug.Log("Stop Audio");
        instance.audioSource.Stop();
        instance.CTRLAudioPanel.gameObject.SetActive(false);
        instance.CTRLAudioARPanel.gameObject.SetActive(false);
        instance.CTRLAudioAR.gameObject.SetActive(false);
    }

    public static void PlayPause(){
        if(instance.audioSource.isPlaying)
            instance.audioSource.Pause();
        else
            instance.audioSource.UnPause();
    }

    public static void PlayAudio(ShortInfo poi, bool forceStart = true, Action callBack = null){
        if(poi == lastInfo && infoPoi != null && !string.IsNullOrEmpty(infoPoi.audio) && infoPoi.language == GameConfig.applicationLanguage){
            Debug.Log("Same Audio");
            PlayAudio(forceStart);
            callBack?.Invoke();
        }else{
            StopAudio();
            lastInfo = poi;
            GETPointOfInterest.ExtendedInfo(poi, (info) =>
            {
                infoPoi = info;
                if(!string.IsNullOrEmpty(infoPoi.audio)){
                    PlayAudio(infoPoi.audio,callBack);
                    //callBack?.Invoke();
                }
            });
        }
    }

    public static void PlayAudio(bool fromStart = true){
        Debug.Log(fromStart);
        if(instance.audioSource.clip == null) return;
        //if(fromStart) instance.audioSource.Play();
        //else instance.audioSource.UnPause();
        if(fromStart){
            instance.audioSource.Play();
            instance.CTRLAudioPanel.Play();
            instance.CTRLAudioARPanel.Play();
            instance.CTRLAudioAR.Play();
        }else{
            instance.CTRLAudioPanel.Play(true, false);
            instance.CTRLAudioARPanel.Play(true, false);
            instance.CTRLAudioAR.Play(true, false);
        }
        instance.CTRLAudioPanel.gameObject.SetActive(true);
        instance.CTRLAudioARPanel.gameObject.SetActive(true);
        instance.CTRLAudioAR.gameObject.SetActive(true);
        Debug.Log($"Play Audio: {instance.audioSource.clip.name} lenght {instance.audioSource.clip.length}");
    }

    public static void PlayAudio(string audioFilePath, Action callback = null){
        instance.StartCoroutine(instance.GETAudioFromFile(audioFilePath,callback));
    }

    public IEnumerator GETAudioFromFile(string audioFilePath, Action callback = null)
    {
        Debug.Log("Chiamata dell'audio: " + audioFilePath);
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioFilePath, AudioType.MPEG)) 
        {
            /////////////////////////// CERITFICATO NON VALIDO TOGLIERE STRINGA IN SEGUITO /////////////////////////////////////
            request.certificateHandler = new AcceptAnyCertificate();
            /////////////////////////// CERITFICATO NON VALIDO TOGLIERE STRINGA IN SEGUITO /////////////////////////////////////
            ///
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Errore durante il caricamento dell'audio: " + request.error);
                if(request.result == UnityWebRequest.Result.ConnectionError)
                    WSO2.NoConnectionError(false);
                yield break;
            }
            else
            {
                lastPath = audioFilePath;
                audioClip = null;
                audioClip = DownloadHandlerAudioClip.GetContent(request);
                audioClip.LoadAudioData();
                audioSource.clip = audioClip;
                audioClip.name = audioFilePath;
                listeningPoint=false;
                currentListening=0;
                PlayAudio();
                callback?.Invoke();
            }
        }
    }
}
