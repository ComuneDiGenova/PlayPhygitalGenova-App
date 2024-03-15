//using ARLocation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class ARShopBehaviour : MonoBehaviour
{
    [SerializeField] float activeArea = 30;
    [SerializeField] public UIActivateMarkerTab activeMarkerTab;

    public ShopShortInfo shop;

    private void Update() {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Shop colpito: " + shop.id);

        // alla pressione dovr� richiamare uno scrip che mi istanzi il modello
        // prendendo la distanza dal poi e lo sposto di qualche metro

        OnClickBehaviour();
    }

    void OnClickBehaviour()
    {
        var dist = (float)GeoCoordinate.Utils.HaversineDistance(shop.ToCoordinate(), new GeoCoordinate.Coordinate (OnlineMapsLocationService.instance.position));
        if( dist > activeArea) return; 

        var shopTrovato = GETShops.DownloadedInformationShop.shopInfos.Where((x) => (x.id.ToString() == shop.id)).FirstOrDefault();

        if (GETShops.DownloadedShopExtendedInfo.ContainsKey(shopTrovato))
        {
            var info = GETShops.DownloadedShopExtendedInfo[shopTrovato];
            activeMarkerTab.ActiveMarkerARTab("Negozio", info);
            //activeMarkerTab.ActiveMarkerARTab("Negozio", info.nome, info.categoria_del_negozio.nome, info.immagine_di_copertina);
        }
        else
        {
            /*
            StartCoroutine(GETShops.GETExtendedInfo(shopTrovato, (info) =>
            {
                activeMarkerTab.ActiveMarkerTab("Negozio", info.nome, info.categoria_del_negozio.nome, info.immagine_di_copertina);
            }));*/
            GETShops.ExtendedInfo(shopTrovato, (info) =>
            {
                activeMarkerTab.ActiveMarkerARTab("Negozio", info);
                //activeMarkerTab.ActiveMarkerARTab("Negozio", info.nome, info.categoria_del_negozio.nome, info.immagine_di_copertina);
            });
        }
    }
}

