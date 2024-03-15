using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static OnlineMapsAMapSearchResult;

public class PopulateFavourites : MonoBehaviour
{
    /// <summary>
    /// Script per cacare instanziare i prefab dei POI nella griglia
    /// della tab VicinoAMe
    ///</summary>

    [SerializeField] GETPointOfInterest getPoi;
    [SerializeField] OnlineMapsMarkerManager markerManager;

    [SerializeField] GameObject gridPreferito;
    [SerializeField] GameObject prefabPreferito;
    [SerializeField] GameObject prefabShopPreferito;

    [SerializeField] Texture2D placeholder;
    [SerializeField] Texture2D favouriteIcon;

    //[SerializeField] OnlineMaps onlineMaps;

    [SerializeField] int maxDownloads = 5;
    int downloads;

    /// Qui avr� bisogno di accedere alla lista dei POI
    /// che presumo verr� data dal comune di genova
    /// per poi aggiungerli in una lista in base al tipo

    [SerializeField] List<ShortInfo> favouritePOI = new List<ShortInfo>();
    [SerializeField] List<ShopShortInfo> favouriteShop = new List<ShopShortInfo>();
    Dictionary<ShortInfo,GameObject> objectsPoi = new Dictionary<ShortInfo, GameObject>();
    Dictionary<ShopShortInfo,GameObject> objectsShop = new Dictionary<ShopShortInfo, GameObject>();

    private void Awake() {
        LoginSSO.OnLogOut += () => ClearAll();
    }

    void ClearAll(){
        Debug.Log("Clear all Favourites");
        foreach(var kvp in objectsPoi){
            Destroy(kvp.Value.gameObject);
        }
        foreach(var kvp in objectsShop){
            Destroy(kvp.Value.gameObject);
        }
        objectsPoi.Clear();
        objectsShop.Clear();
        favouritePOI.Clear();
    }

    void OnEnable()
    {
        StartCoroutine(GetFavourite());
    }

    void OnDisable()
    {
        ClearAll();
    }

    IEnumerator GetFavourite()
    {     
        downloads = 0;

        foreach (var fav in GETUserInfo.FavouriteResponse.favourites)
        {
            var poiTrovato = GETPointOfInterest.DownloadedInformationPois.infos.Where((x) => (x.id == fav.id)).ToList();           

            //Debug.Log(poiTrovato);
            favouritePOI.AddRange(poiTrovato);
        }

        foreach (var fav in GETUserInfo.FavouriteResponse.favourites)
        {
            var shopTrovato = GETShops.DownloadedInformationShop.shopInfos.Where((x) => (x.id == fav.id)).ToList();

            //Debug.Log(shopTrovato);
            favouriteShop.AddRange(shopTrovato);
        }
        if (favouritePOI.Count > 0)
        {
            foreach (ShortInfo poi in favouritePOI)
            {
                downloads++;
                POIPrefabHandler handler;
                if(objectsPoi.ContainsKey(poi)){
                    handler = objectsPoi[poi].GetComponent<POIPrefabHandler>();
                    objectsPoi[poi].SetActive(true);
                }else{
                    var favouritePoi = Instantiate(prefabPreferito, gridPreferito.transform);
                    handler = favouritePoi.GetComponent<POIPrefabHandler>();
                    handler.id = poi.id;
                    handler.prefabText.text = poi.title;
                    handler.prefabImage.texture = placeholder;
                    handler.poi = poi;
                    //handler.onlineMaps = onlineMaps;
                    objectsPoi.Add(poi,favouritePoi);
                }

                if (!GETPointOfInterest.DownloadedExtendedInfo.ContainsKey(poi))
                {
                    GETPointOfInterest.ExtendedInfo(poi, (info) =>
                    {
                        handler.OnExtInfo(info);
                        if(info.immagine_di_copertinaTexture == null){
                            GETPointOfInterest.GetImagePoi(info, (texture) => {
                                handler.SetTexture(texture);
                                downloads--;
                            });
                        }else{
                            handler.SetTexture(info.immagine_di_copertinaTexture);
                            downloads--;
                        }
                    });
                }
                else
                {
                    var info = GETPointOfInterest.DownloadedExtendedInfo[poi];
                    handler.OnExtInfo(info);
                    if(info.immagine_di_copertinaTexture == null){
                        GETPointOfInterest.GetImagePoi(info, (texture) => {
                            handler.SetTexture(texture);
                            downloads--;
                        });
                    }else{
                        handler.SetTexture(info.immagine_di_copertinaTexture);
                        downloads--;
                    }
                }
                yield return new WaitUntil(() => downloads < maxDownloads);
                yield return new WaitForSeconds(0.25f);
            }
        }

        if (favouriteShop.Count > 0)
        {
            foreach (ShopShortInfo shop in favouriteShop)
            {
                downloads++;
                ShopPrefabHandler handler;
                if(objectsShop.ContainsKey(shop)){
                    handler = objectsShop[shop].GetComponent<ShopPrefabHandler>();
                    objectsShop[shop].SetActive(true);
                }else{
                    var favouriteShop = Instantiate(prefabShopPreferito, gridPreferito.transform);
                    handler = favouriteShop.GetComponent<ShopPrefabHandler>();
                    handler.id = shop.id;
                    handler.prefabText.text = shop.title;
                    handler.prefabImage.texture = placeholder;
                    handler.shop = shop;
                    //handler.onlineMaps = onlineMaps;
                    objectsShop.Add(shop,favouriteShop);
                }


                if (!GETShops.DownloadedShopExtendedInfo.ContainsKey(shop))
                {
                    GETShops.ExtendedInfo(shop, (info) =>
                    {
                        if(info != null) {
                            handler.OnExtInfo(info);
                            if(info.immagine_di_copertinaTexture == null){
                                GETPointOfInterest.GetImagePoi(info, (texture) => {
                                    handler.SetTexture(texture);
                                    downloads--;
                                });
                            }else{
                                handler.SetTexture(info.immagine_di_copertinaTexture);
                                downloads--;
                            }
                        }
                    });
                }
                else
                {
                    var info = GETShops.DownloadedShopExtendedInfo[shop];
                    handler.OnExtInfo(info);
                    if(info.immagine_di_copertinaTexture == null){
                        GETPointOfInterest.GetImagePoi(info, (texture) => {
                            handler.SetTexture(texture);
                            downloads--;
                        });
                    }else{
                        handler.SetTexture(info.immagine_di_copertinaTexture);
                        downloads--;
                    }
                }
                
                yield return new WaitUntil(() => downloads < maxDownloads);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}