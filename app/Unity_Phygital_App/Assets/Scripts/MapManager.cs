using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject mapObject;
    private void Awake() {
        LoginSSO.OnLoginSuccesfull += (anon) =>
        {
            if(!GameConfig.isLoggedPro){
                mapObject.SetActive(true);
            }
        };
        LoginSSO.OnLogOut += () =>
        {
            mapObject.SetActive(false);
        };
        //
        mapObject.SetActive(false);
    }

    private void Start() {
        if(GameConfig.isLogged && !GameConfig.isLoggedPro) mapObject.SetActive(true);
    }
}
