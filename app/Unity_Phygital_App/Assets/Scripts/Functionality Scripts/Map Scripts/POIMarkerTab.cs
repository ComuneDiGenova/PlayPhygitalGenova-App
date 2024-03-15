using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static OnlineMapsAMapSearchResult;

public class POIMarkerTab : MonoBehaviour
{
    [SerializeField] UIActivateMarkerTab activeMarkerTab;
     [SerializeField] UIActivateMarkerTab arMarkerTab;

    [SerializeField] GETAudio getAudio;

    //InformationList data = GETPointOfInterest.DownloadedInformationPois;

    public static POIMarkerTab instance;

    void Awake(){
        instance = this;
    }
   
    string markerName = "";
    string markerTag = "";

    ShortInfo poiTrovato;
    TypeInfo typeTrovato;
    Info info;

    public void GetClickedNameByPoi(string poiID)
    {
        //try
        //{
            markerName = poiID;
            Debug.Log("get clicked name by poi " + poiID);
            //Debug.Log(markerName);

            ActivateTab();
        //}
        //catch (System.Exception ex) { Debug.LogError("Informazioni POI nulle" + ex.Message); }
    }

    public void GetClickedName(OnlineMapsMarkerBase marker)
    {
        //try
        //{
            markerName = marker.label;
            markerTag = marker.tags[0];
            //Debug.Log("get clicked name by poi " + marker.ToJSON());
            //Debug.Log(markerTag);
            //Debug.Log(markerName);
        
            ActivateTab();
        //}
        //catch(System.Exception ex){ Debug.LogError("Informazioni POI nulle" + ex.Message); } 
    }

    //chiamato direttamente da bottoenavviso
    public void OpenARTab(){
        poiTrovato = CheckPOIProximity.notificatedInfo;
        Debug.Log(poiTrovato.ToString());
        if(poiTrovato != null){
            if(!string.IsNullOrEmpty(poiTrovato.id)){
                typeTrovato = GETPointOfInterest.DownloadedTypePois.typeList.Where((x) => (x.id == poiTrovato.id_tipologia)).FirstOrDefault();
                if(typeTrovato == null){
                    Debug.LogError("TYPE NON TROVATO");
                    typeTrovato = new TypeInfo(){nome="POI"};
                }
            }
        }else{
            Debug.LogError("POI NON TROVATO");
            return;
        }
        GETPointOfInterest.ExtendedInfo(poiTrovato, (infopoi) =>
        {
            info = infopoi;
            if(info==null) return;
            if(poiTrovato.tipo == "poi")
                arMarkerTab.ActiveMarkerARTab("Punto Storico", info, typeTrovato.nome);
            else if(poiTrovato.tipo == "negozio")
                arMarkerTab.ActiveMarkerARTab("Negozio", info, "Bottega Storica");
            else {
                Debug.LogWarning(info.ToString());
                arMarkerTab.ActiveMarkerARTab("Punto Storico", info, "NA");
            }
            //
            Debug.Log("URL audio tab AR: " + info.audio);
            if(!string.IsNullOrEmpty(info.audio))
                GETAudio.PlayAudio(poiTrovato);
            else
                GETAudio.StopAudio();
            //avatar
            if (!string.IsNullOrEmpty(info.audio))
                ActivateAR.ActivateAvatar(poiTrovato, info.avatar);
            else ActivateAR.AvatarOff();
        });
    }

    public void OpenTabFromNotification(){
        poiTrovato = CheckPOIProximity.notificatedInfo;
        Debug.Log(poiTrovato.ToString());
        if(poiTrovato != null){
            if(!string.IsNullOrEmpty(poiTrovato.id)){
                typeTrovato = GETPointOfInterest.DownloadedTypePois.typeList.Where((x) => (x.id == poiTrovato.id_tipologia)).FirstOrDefault();
                if(typeTrovato == null){
                    Debug.LogError("TYPE NON TROVATO");
                    typeTrovato = new TypeInfo(){nome="POI"};
                }
            }
        }else{
            Debug.LogError("POI NON TROVATO");
            return;
        }
        GETPointOfInterest.ExtendedInfo(poiTrovato, (infopoi) =>
        {
            info = infopoi;
            if(info==null) return;
            activeMarkerTab.ActiveMarkerTabExt(markerTag,info,typeTrovato.nome);               
        });
    }

    void ActivateTab()
    {
        Debug.Log(GETPointOfInterest.DownloadedInformationPois.infos.Count);
        poiTrovato = GETPointOfInterest.DownloadedInformationPois.infos.Where((x) => (x.id == markerName)).FirstOrDefault();
        if(poiTrovato != null){
            typeTrovato = GETPointOfInterest.DownloadedTypePois.typeList.Where((x) => (x.id == poiTrovato.id_tipologia)).FirstOrDefault();
            if(typeTrovato == null){
                Debug.LogError("POI NON TROVATO: " + markerName);
                return;
            }
        }else{
            Debug.LogError("POI NON TROVATO: " + markerName);
            return;
        }

        Debug.LogWarning(poiTrovato.ToString());
        Debug.LogWarning(typeTrovato.nome);

        GETPointOfInterest.ExtendedInfo(poiTrovato, (infopoi) =>
        {
            info = infopoi;
            activeMarkerTab.ActiveMarkerTabExt(markerTag,info,typeTrovato.nome);               
        });



        /*
        if (GETPointOfInterest.DownloadedExtendedInfo.ContainsKey(poiTrovato))
        {
            info = GETPointOfInterest.DownloadedExtendedInfo[poiTrovato];
            activeMarkerTab.ActiveMarkerTabExt(markerTag,info,typeTrovato.nome);
            //activeMarkerTab.ActiveMarkerTab(markerTag, info.nome, typeTrovato.nome, info.immagine_di_copertina);
        }
        else
        {
            GETPointOfInterest.ExtendedInfo(poiTrovato, (info) =>
            {
                activeMarkerTab.ActiveMarkerTabExt(markerTag,info,typeTrovato.nome);
                //activeMarkerTab.ActiveMarkerTab(markerTag, info.nome, typeTrovato.nome, info.immagine_di_copertina);               
            });
        }
        */
    }



    public void ActivateExtendedTab()
    {
        Debug.Log("sto attivando la tab estesa");
        /*
        if (ActivateAR.isArActive)
        {
            markerName = ARPOIBehaviour.selectedPoi.id;
            poiTrovato = ARPOIBehaviour.selectedPoi;
        }
        */

         //poiTrovato = GETPointOfInterest.DownloadedInformationPois.infos.Where((x) => (x.id.ToString() == markerName)).FirstOrDefault();
        typeTrovato = GETPointOfInterest.DownloadedTypePois.typeList.Where((x) => x.id.ToString() == poiTrovato.id_tipologia).FirstOrDefault();


        GETPointOfInterest.ExtendedInfo(poiTrovato, (infopoi) =>
        {
            info = infopoi;
            Debug.Log(info.id);
            activeMarkerTab.ActivePOITabExt(info.id, info.nome, typeTrovato.nome, info.indirizzo, info.orari, info.telefono, info.url, info.email, info.descrizione,info.immagine_di_copertinaTexture);
            SendExtendedPoint(info.id);

            Debug.Log("URL audio Extended: " + info.audio);
            if(!string.IsNullOrEmpty(info.audio))
                GETAudio.PlayAudio(poiTrovato, ActivateAR.isArActive ? false : true);
            else
                GETAudio.StopAudio();
            //avatar
            if(!string.IsNullOrEmpty(info.audio))
                ActivateAR.ActivateAvatar(poiTrovato,info.avatar);
        });
/*
        if (GETPointOfInterest.DownloadedExtendedInfo.ContainsKey(poiTrovato))
        {
            info = GETPointOfInterest.DownloadedExtendedInfo[poiTrovato];
            Debug.Log(info.id);
            activeMarkerTab.ActivePOITabExt(info.id, info.nome, typeTrovato.nome, info.indirizzo, info.orari, info.telefono, info.url, info.email, info.descrizione);
            SendExtendedPoint(info.id);

            Debug.Log("URL audio: " + info.audio);
            if(!string.IsNullOrEmpty(info.audio))
                GETAudio.PlayAudio(poiTrovato,true);
            else
                GETAudio.StopAudio();
        }
        else
        {
            Debug.LogError("Tu non dovresti essere qua");
        }
        */
    }

    void SendExtendedPoint(string id)
    {
        AddPoint AddPoint = new AddPoint();

        AddPoint.action = "visualizzazione";
        AddPoint.content_type = "poi";
        AddPoint.content_id = id;

        //StartCoroutine(GETUserInfo.ADDVisualizationPoints(AddPoint, GetExtendedResponse));
        GETUserInfo.AddVisualizationPoints(AddPoint, GetExtendedResponse);
    }

    void GetExtendedResponse(ResponseAddPoint response)
    {
        Debug.Log(response);
        if(response.result) PopUpPanel.OpenLanguage(GETUserInfo.pointsIdKeyLanguage,true,response.points.ToString());
    }
    
}