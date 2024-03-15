using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static OnlineMapsAMapSearchResult;

public class ShopPrefabHandler : MonoBehaviour
{
    public RawImage prefabImage;
    public TMP_Text prefabText;
    public string id;
    public ShopShortInfo shop;
    public ShopInformaitions info;

    //public OnlineMaps onlineMaps;
    //public UIManager uiManager;

    private void Awake()
    {
        //GETShops.OnExtendedInfo += OnExtInfo;
    }
    public void SetTexture(Texture2D texture){
        if(texture != null)
            prefabImage.texture = texture;
    }

    public void OnExtInfo(ShopInformaitions longinfo)
    {
        if(longinfo == null) return;
        info = longinfo;
        prefabText.text = info.nome;
        /*
        StartCoroutine(GETShops.GETShopImage(null, info.immagine_di_copertina, (texture) => {
            prefabImage.texture = texture;
        }));
    */
    }

    public void SetNearShopCoordinateClick()
    {
        UIManager.instance.CallCloseButton();
        OnlineMaps.instance.SetPosition(shop.lon, shop.lat);
        OnlineMaps.instance.zoom = 20;
        ShopMarkerTab.instance.GetClickedNameByShop(shop.id);
    }
}
