//using ARLocation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem.HID;
using System.Linq;
using TMPro;
using UnityEngine.Lumin;

public class ARPOIBehaviour : MonoBehaviour
{
    [SerializeField] float activeArea = 30;

    [HideInInspector] public UIActivateMarkerTab activeMarkerTab;
    //public GETAudio getAudio;

    //public AudioSource audioSource; // dev'essere la stessa di GETAudio

    [HideInInspector] public GameObject instantiatedAvatar;

    public static ShortInfo selectedPoi;
    public static ARPOIBehaviour activeArPoi;

    public ShortInfo poi;
    Info infoPoi;

    int avatar;
    //bool modelInstantiated = false;
    [SerializeField] bool active = false;
    [SerializeField] float dist;

    Animator animator;

    private void OnDisable() {
        //Disable();
    }

    public void Disable(){
        if(instantiatedAvatar)
            instantiatedAvatar.SetActive(false);
        instantiatedAvatar = null;
        GETAudio.StopAudio();
        selectedPoi = null;
        activeArPoi = null;
    }
    
    void Update()
    {
        dist = (float)GeoCoordinate.Utils.HaversineDistance(poi.ToCoordinate(), new GeoCoordinate.Coordinate (OnlineMapsLocationService.instance.position));
        active = dist < activeArea;
        /*
        if(active && !instantiatedAvatar.activeSelf){
            instantiatedAvatar.SetActive(true);
            GETAudio.PlayAudio(poi,true);
            ActivateAR.SetAudioPanel(true);
        }
        if(!active && instantiatedAvatar.activeSelf){
            instantiatedAvatar.SetActive(false);
            GETAudio.StopAudio();
            ActivateAR.SetAudioPanel(false);
        }
        */
        //
        /*
        if (instantiatedAvatar != null && active){
            animator.SetBool("parla", GETAudio.isPlaying);
        }*/
        if(instantiatedAvatar != null)
            animator.SetBool("parla", GETAudio.isPlaying);
    }

    private void OnMouseDown()
    {      
        Debug.Log("Poi colpito: " + poi.id);
        OnClickBehaviour();
    }

    [ContextMenu("OpenClick")]
    public void OnClickBehaviour()
    {
    #if !UNITY_EDITOR
        if(!active){
            Debug.LogWarning($"ARPOI too distant: {dist} / {activeArea}");
            return;
        }
    #endif
        selectedPoi = poi;

        //gameObject.GetComponent<MeshRenderer>().enabled = false;

        //var poiTrovato = GETPointOfInterest.DownloadedInformationPois.infos.Where((x) => (x.id.ToString() == poi.id)).FirstOrDefault();
        //arClickedPoi = poiTrovato.id;
        //Debug.Log("cliccato su poi: " + poiTrovato.id);
        GETAudio.PlayAudio(poi);

        if(infoPoi != null){
            ActivateAvatar(infoPoi);
        }else{

            if (GETPointOfInterest.DownloadedExtendedInfo.ContainsKey(poi))
            {
                ActivateAvatar(GETPointOfInterest.DownloadedExtendedInfo[poi]);
            }
            else
            {
                GETPointOfInterest.ExtendedInfo(poi, ActivateAvatar);
            }
        }
    }

    void ActivateAvatar(Info info){
        infoPoi = info;
        activeMarkerTab.ActiveMarkerARTab("Punto Storico", info,null);
        avatar = info.avatar;
        //InstantiateModel();
        instantiatedAvatar = ActivateAR.ActivateAvatar(poi,avatar);
        if(instantiatedAvatar == null)
            Debug.LogError("Avaar missing: " + avatar);
        else
            animator = instantiatedAvatar.GetComponentInChildren<Animator>();
    }
/*
    void InstantiateModel()
    {
        //if (!modelInstantiated)
        {
            
            Location location = new Location
            {
                Longitude = poi.lon,
                Latitude = poi.lat,
                Altitude = 0f,
                Label = poi.id
            };

            var options = new PlaceAtLocation.PlaceAtOptions()
            {
                HideObjectUntilItIsPlaced = false,
                MaxNumberOfLocationUpdates = 0,
                MovementSmoothing = 0.1f,
                UseMovingAverage = true
            };
            
            //instantiatedAvatar = Instantiate(ActivateAR.models[avatar]);
            
            //instantiatedAvatar.AddComponent<LookAtCamera>();
            //instantiatedAvatar.transform.localScale = new Vector3(1, 1, 1);

            //instantiatedAvatar = ActivateAR.models[avatar];
            //instantiatedAvatar.SetActive(true);
            //modelInstantiated = true;

            //PlaceAtLocation.AddPlaceAtComponent(instantiatedAvatar, location, options);
            //AvatarTalkingAnimation();
        }
    }
*/
/*
    void AvatarTalkingAnimation()
    {
        animator.SetBool("parla", true);
    }

    void AvatarStopAnimation()
    {
        animator.SetBool("parla", false);
    }
*/
}

