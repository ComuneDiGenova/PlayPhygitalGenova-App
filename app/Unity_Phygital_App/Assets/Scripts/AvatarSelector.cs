using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelector : MonoBehaviour
{
    [SerializeField] GameObject[] avatars;
    int defaultAvatar;

    private void Start() {
        ActivateAR.OnAvatarSelect += (avatar) => {
            foreach (var a in avatars)
            {
                a.SetActive(false);
            }

            //Debug.Log($"{avatar} / {avatars.Length}");
            if (avatar > avatars.Length || avatar < 0){
                avatar = defaultAvatar;
                Debug.Log("Sto attivando l'avatar di default");
            } 
            avatars[avatar].SetActive(true);
            Debug.Log("Ho attivato l'avatar" + avatar);
        };
        //
        foreach(var a in avatars){
            a.SetActive(false);
        }
        ActivateAR.OnAvatarOff += SpegniAvatar;
    }

    private void SpegniAvatar()
    {
        foreach (var a in avatars)
        {
            a.SetActive(false);
            Debug.Log("Sto spegnendo l'avatar " + a);

        }

    }
}
