using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopMarkerTab : MonoBehaviour
{
    [SerializeField] UIActivateMarkerTab activeMarkerTab;
    [SerializeField] UIActivateMarkerTab ARMarkerTab;

    string markerName = "";
    string markerTag = "";

    public static ShopMarkerTab instance;


    void Awake(){
        instance = this;
    }

    public void GetClickedNameByShop(string shopID)
    {
        //try
        //{
            markerName = shopID;
            //Debug.Log("get clicked name by poi " + shopID);
            //Debug.Log(markerName);

            ActivateTab();
        //}
        //catch (System.Exception ex) { Debug.LogError("Informazioni POI nulle" + ex.Message); }
    }

    public void GetShopClickedName(OnlineMapsMarkerBase marker)
    {
        //try
        //{
            markerName = marker.label;
            markerTag = marker.tags[0];
            //Debug.Log(markerTag);
            //Debug.Log(markerName);

            ActivateTab();
        //}
        //catch (System.Exception ex) { Debug.LogError("Informazioni POI nulle" + ex.Message); }
    }

    public void OpenARTab(){
        var shopTrovato = CheckPOIProximity.notificatedShop;
        GETShops.ExtendedInfo(shopTrovato, (info) =>
        {
            activeMarkerTab.ActiveMarkerTabExt("Negozio", info);
        });
    }


    void ActivateTab()
    {
        var shopTrovato = GETShops.DownloadedInformationShop.shopInfos.Where((x) => (x.id == markerName)).FirstOrDefault();
        if(shopTrovato == null){
            Debug.LogError("POI NON TROVATO: " + markerName);
            return;
        }
        Debug.LogWarning(shopTrovato.ToString());
        //Debug.LogWarning(typeTrovato.nome);

        GETShops.ExtendedInfo(shopTrovato, (info) =>
        {
            activeMarkerTab.ActiveMarkerTabExt(markerTag, info);
        });

/*
        if (GETShops.DownloadedShopExtendedInfo.ContainsKey(shopTrovato))
        {
            var info = GETShops.DownloadedShopExtendedInfo[shopTrovato];
            activeMarkerTab.ActiveMarkerTabExt(markerTag, info);
            //activeMarkerTab.ActiveMarkerTab(markerTag, info.nome, info.categoria_del_negozio.nome, info.immagine_di_copertina);
        }
        else
        {
            GETShops.ExtendedInfo(shopTrovato, (info) =>
            {
                activeMarkerTab.ActiveMarkerTabExt(markerTag, info);
               // activeMarkerTab.ActiveMarkerTab(markerTag, info.nome, info.categoria_del_negozio.nome, info.immagine_di_copertina);
            });
        }
*/
    }

    public void ActivateExtendedTab()
    {
        Debug.Log("CHIAMATA NEGOZIO");
        var shopTrovato = GETShops.DownloadedInformationShop.shopInfos.Where((x) => (x.id.ToString() == markerName)).FirstOrDefault();
/*
        if (GETShops.DownloadedShopExtendedInfo.ContainsKey(shopTrovato))
        {
            var info = GETShops.DownloadedShopExtendedInfo[shopTrovato];
            activeMarkerTab.ActivePOITabExt(info.id, info.nome, info.tipologia, info.indirizzo, info.orari.ToString(), info.telefono, info.url, info.email, info.descrizione);
        }
        else
        {
            Debug.LogError("Tu non dovresti essere qua");
        }
*/
        GETShops.ExtendedInfo(shopTrovato, (info) =>
        {
            activeMarkerTab.ActivePOITabExt(info.id, info.nome, info.tipologia, info.indirizzo, info.orari.ToString(), info.telefono, info.url, info.email, info.descrizione,info.immagine_di_copertinaTexture);
        });


    }
}