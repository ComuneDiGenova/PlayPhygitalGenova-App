using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    

    public Slider slideAreaInput;
    public Slider slideAreaRead;


    // Start is called before the first frame update
    void OnStart()
    {
        slideAreaRead.interactable = false;
        //slideAreaInput.onValueChanged.AddListener((float value) => { audioSource.time = value * audioSource.clip.length;});
    }

    void Update()
    {
        if(GETAudio.instance.audioSource.clip != null){
            slideAreaRead.value = GETAudio.instance.audioSource.time / GETAudio.instance.audioSource.clip.length;
        }else{
            slideAreaRead.value = 0;
        }
        
    }

    public void SetAudio(float value){
        
        if(GETAudio.instance.audioSource.clip == null) return;
            GETAudio.instance.audioSource.time = value * GETAudio.instance.audioSource.clip.length;
        
    }



}
