//using ARLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using UnityEngine.XR.ARFoundation;

public class ActivateAR : MonoBehaviour
{
    [SerializeField] GameObject NoArNotificationPanel;
    [SerializeField] GameObject ArNotificationPanel;
    [SerializeField] GameObject buttonAR;
    [SerializeField] GameObject buttonHelp;
    [SerializeField] GameObject buttonMenu;
    [SerializeField] GameObject buttonClose;
    [SerializeField] GameObject buttonFilter;


    [SerializeField] GameObject menuTab;
    [SerializeField] GameObject itinerariesTab;
    [SerializeField] GameObject homePanel;
    [SerializeField] GameObject AR;
    //[SerializeField] GameObject GPS_AR;
    [SerializeField] GameObject MAP;
    //[SerializeField] GameObject MAP_UI;
    [SerializeField] GameObject CTNlegenda;
    [SerializeField] GameObject TabRicerca;
    [SerializeField] GameObject mapCover;
    [SerializeField] GameObject arProximityTab;
    [SerializeField] Camera ARCamera;
    [SerializeField] UIActivateMarkerTab[] markerTabs;

    //[SerializeField] GameObject arLocationObject;
    //[SerializeField] GameObject paganini;
    //[SerializeField] GameObject contessa;
    [SerializeField] GameObject[] avatarPrefabs;

    //public List<GameObject> models = new List<GameObject>();

    public static bool isArActive = false;
    //
   

    public delegate void AvatarEvent(int avatar);
    public static event AvatarEvent OnAvatarSelect;
    public static event VoidDelegate OnAvatarOff;

    static public ActivateAR instance {get;private set;}
    private void Awake() {
        instance = this;
    }

    void Start()
    {   
        MAP.SetActive(true);
        //MAP_UI.SetActive(true);
        AR.SetActive(false);
        //GPS_AR.SetActive(false);
        StartCoroutine(ARCheck(1));
    }

    public static GameObject ActivateAvatar(ShortInfo poi, int avatar){

        Debug.Log("AVATAR: " + avatar.ToString());
        OnAvatarSelect?.Invoke(avatar);
        return null;
    }

    public static void AvatarOff()
    {
        Debug.Log("AVATAR off");
        OnAvatarOff?.Invoke();
    }

    //attivato da bottone ok dell'avviso di prossimità durante il percorso
    public void ActiveARCamera()
    {      
        if (menuTab.activeSelf)
            menuTab.SetActive(false);

        if (itinerariesTab.activeSelf)
            itinerariesTab.SetActive(false);

        //homePanel.SetActive(false);
        buttonAR.SetActive(false);
        buttonHelp.SetActive(false);
        buttonMenu.SetActive(false);
        buttonClose.SetActive(true);
        buttonFilter.SetActive(false);
        MAP.SetActive(false);
        //MAP_UI.SetActive(false);
        AR.SetActive(true);
        //GPS_AR.SetActive(true);
        mapCover.SetActive(true);
        CTNlegenda.SetActive(false);
        TabRicerca.SetActive(false);
        buttonClose.GetComponent<Button>().interactable = false;
        Invoke("ActivateCloseButton",5);
        foreach(var mt in markerTabs){
            mt.CloseMarkerTab();
        }

        //arLocationObject.GetComponent<ARInstantiatePOI>().enabled = true;

        isArActive = true;

        if (arProximityTab.activeSelf)
            arProximityTab.SetActive(false);
    }

    //NOTA FUNZIONA SOLO SE AR CORE E' OPZIONALE
    //NOTA SU REDMI E XIAOMI SEGNA FALSI POSITIVI
    IEnumerator ARCheck(float delay = 0){
        yield return new WaitForSeconds(delay);
        Debug.LogWarning("AR CHECK");
        yield return ARSession.CheckAvailability();
        if(ARSession.state == ARSessionState.None || ARSession.state == ARSessionState.Unsupported){
            Debug.LogError("NO AR SUPPORT!");
            NoArNotificationPanel.SetActive(true);
            ArNotificationPanel.SetActive(false);
        }else{
            Debug.LogWarning("AR SUPPORTED");
            NoArNotificationPanel.SetActive(false);
            ArNotificationPanel.SetActive(true);
            while (ARSession.state == ARSessionState.Installing) {
                yield return null;
            }
            if (ARSession.state == ARSessionState.NeedsInstall) {
                Debug.LogWarning("AR INSTALLING");
                yield return ARSession.Install();
            }
        }
        Debug.LogWarning("END AR CHECK");
    }

    void ActivateCloseButton(){
        buttonClose.GetComponent<Button>().interactable = true;
    }

    public void StopARCamera()
    {
        //homePanel.SetActive(true);
        buttonAR.SetActive(true);
        buttonHelp.SetActive(true);
        buttonMenu.SetActive(true);
        buttonClose.SetActive(false);
        buttonClose.GetComponent<Button>().interactable = true;
        buttonFilter.SetActive(true);
        MAP.SetActive(true);
        mapCover.SetActive(false);
        //MAP_UI.SetActive(true);
        AR.SetActive(false);
        /*
        foreach(var m in  avatarPrefabs){
            m.SetActive(false);
        }
        */
        isArActive = false;
        //Disabilito Audio e Avatar
        GETAudio.StopAudio();
    }
}
