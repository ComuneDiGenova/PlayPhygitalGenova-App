using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CTRL_AUDIO : MonoBehaviour
{
    public Button button;
    public Sprite pausa;
    public Sprite play;
    public GETAudio getAudio;
    public AudioManager audioManager;
    
    private void Start()
    {
        button.onClick.AddListener(() => PlayStopAudio());
    }

    public void Play(bool playVal=true, bool reset = true){
        button.GetComponent<Image>().sprite = playVal ? play : pausa;
        if(reset){
            audioManager.SetAudio(0);
        }
    }

    private void Update()
    {
        if (!getAudio.audioSource.isPlaying)
        {
            button.GetComponent<Image>().sprite = pausa;
        }
        else
        {
            button.GetComponent<Image>().sprite = play;
        }
    }

    public void PlayStopAudio()
    {
        if(getAudio.audioSource.clip!=null)
        {
            if (GETAudio.isPlaying)
            {
                //button.GetComponent<Image>().sprite = pausa;
                getAudio.audioSource.Pause();
                audioManager.slideAreaInput.value = audioManager.slideAreaRead.value;
            }
            else
            {
                //button.GetComponent<Image>().sprite = play;
                getAudio.audioSource.Play();
                audioManager.slideAreaInput.value = audioManager.slideAreaRead.value;
            }
            //GETAudio.PlayPause();
        }
        
    }
}
