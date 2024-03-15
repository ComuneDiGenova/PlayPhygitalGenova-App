using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAvatar : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private GameObject avatarSCREWN;
    [SerializeField] private GameObject avatarPOS;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    public void CambioAvatar()
    {
        if (toggle.isOn)
        {
            avatarPOS.SetActive(false);
            avatarSCREWN.SetActive(true);
        }
        else
        {
            avatarPOS.SetActive(true);
            avatarSCREWN.SetActive(false);
        }
    }
}
