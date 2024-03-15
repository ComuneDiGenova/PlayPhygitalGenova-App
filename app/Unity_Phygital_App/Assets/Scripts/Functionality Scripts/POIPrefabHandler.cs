using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class POIPrefabHandler : MonoBehaviour
{
    public RawImage prefabImage;
    public TMP_Text prefabText;
    public string id;
    public ShortInfo poi;
    public Info info;

    //public OnlineMaps onlineMaps;
    //public UIManager uiManager;

    private void Awake() {
        //GETPointOfInterest.OnExtendedInfo += OnExtInfo;
    }

    private void Start() {
        
    }

    public void SetTexture(Texture2D texture){
        if(texture != null)
            prefabImage.texture = texture;
    }

    public void OnExtInfo(Info info){
        if(info==null) return;
        this.info = info;
        prefabText.text = info.nome;
        if(info.immagine_di_copertinaTexture == null){
            /*
            StartCoroutine(GETPointOfInterest.GETImage(null, info.immagine_di_copertina, (texture) => {
                info.immagine_di_copertinaTexture = texture;
                prefabImage.texture = texture;
            }));
            */
            /*
            GETPointOfInterest.GetImagePoi(info, (texture) => {
                prefabImage.texture = texture;
            });
            */
        }else{
            prefabImage.texture = info.immagine_di_copertinaTexture;
        }
        
    }

    public void SetNearPoiCoordinateClick()
    {
        UIManager.instance.CallCloseButton();
        OnlineMaps.instance.SetPosition(poi.lon, poi.lat);
        OnlineMaps.instance.zoom = 20;
        POIMarkerTab.instance.GetClickedNameByPoi(poi.id);
    }
}
