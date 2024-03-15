using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImfoManager : MonoBehaviour
{
    [SerializeField] GameObject transaction;
    [SerializeField] GameObject infoApp;
    [SerializeField] GameObject avatars;
    [SerializeField] GameObject genovini;
    [SerializeField] GameObject geniviniWhite;

    private void OnEnable() {
        avatars.SetActive(!GameConfig.isLoggedPro);
        transaction.SetActive(!GameConfig.isLoggedPro);
        genovini.SetActive(!GameConfig.isLoggedPro);
        
        infoApp.SetActive(GameConfig.isLoggedPro);
        geniviniWhite.SetActive(GameConfig.isLoggedPro);
    }
}
