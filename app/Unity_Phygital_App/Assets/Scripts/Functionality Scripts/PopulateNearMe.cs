using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GeoCoordinate;
using System.Globalization;
using System.Linq;

public class PopulateNearMe : MonoBehaviour
{
    /// <summary>
    /// Script per cacare instanziare i prefab dei POI nella griglia
    /// della tab VicinoAMe
    ///</summary>
    [SerializeField] InstantiatePoiOnMap instantiatePoiScript;
    [SerializeField] InstantiateShopOnMap instantiateShopScript;

    //[SerializeField] ResizeNearMe resizeScript;
    [SerializeField] GETPointOfInterest getPoi;

    [SerializeField] GameObject gridStorico;
    [SerializeField] GameObject gridNegozio;
    [SerializeField] GameObject gridBottega;

    [SerializeField] GameObject prefabStorico;
    [SerializeField] GameObject prefabNegozio;
    [SerializeField] GameObject prefabBottega;

    [SerializeField] Texture2D placeholder;

    //[SerializeField] OnlineMaps onlineMaps;

    /// Qui avrà bisogno di accedere alla lista dei POI
    /// che presumo verrà data dal comune di genova
    /// per poi aggiungerli in una lista in base al tipo

    [SerializeField] Dictionary<ShortInfo,GameObject> objectStorici = new Dictionary<ShortInfo,GameObject>();
    [SerializeField] Dictionary<ShopShortInfo,GameObject> objectNegozi = new Dictionary<ShopShortInfo,GameObject>();


    InformationList data = GETPointOfInterest.DownloadedInformationPois;
    Dictionary<ShortInfo, GeoCoordinate.Coordinate> coordinateList = new Dictionary<ShortInfo, GeoCoordinate.Coordinate>();

    float longitude;
    float latitude;

    [SerializeField] int maxDownloads = 5;
    int downloads = 0;

    void OnEnable()
    {
        instantiatePoiScript.SearchNearMe();
        instantiateShopScript.SearchShopNearMe();
        downloads = 0;
        StartCoroutine(EvalStorici());
        StartCoroutine(EvalNegozi());
    }

    void OnDisable()
    {
        instantiatePoiScript.poiNelRaggioDiRicerca.Clear();
        instantiateShopScript.shopNelRaggioDiRicerca.Clear();
        /*
        foreach (Transform child in gridStorico.transform)
            Destroy(child.gameObject);

        foreach (Transform child in gridNegozio.transform)
            Destroy(child.gameObject);
        */
        foreach(var kvp in objectStorici){
            kvp.Value.SetActive(false);
        }
        foreach(var kvp in objectNegozi){
            kvp.Value.SetActive(false);
        }
    }

    IEnumerator EvalStorici()
    {
        foreach (ShortInfo poi in instantiatePoiScript.poiNelRaggioDiRicerca)
        {
            downloads++;
            POIPrefabHandler handler;
            if(objectStorici.ContainsKey(poi)){
                handler = objectStorici[poi].GetComponent<POIPrefabHandler>();
                objectStorici[poi].SetActive(true);
            }else{
                var storico = Instantiate(prefabStorico, gridStorico.transform);
                objectStorici.Add(poi,storico);
                handler = storico.GetComponent<POIPrefabHandler>();
                handler.id = poi.id;
                handler.prefabText.text = poi.title;
                handler.prefabImage.texture = placeholder;
                handler.poi = poi;
                //handler.onlineMaps = onlineMaps;
            }
            //Debug.Log(placeholder.ToString());

            //var poiTrovato = GETPointOfInterest.DownloadedExtendedInfo.Where((x) => (poi.id == x.Value.id)).FirstOrDefault();
            if(GETPointOfInterest.DownloadedExtendedInfo.ContainsKey(poi)){
                handler.OnExtInfo(GETPointOfInterest.DownloadedExtendedInfo[poi]);
                if(GETPointOfInterest.DownloadedExtendedInfo[poi].immagine_di_copertinaTexture == null){
                    GETPointOfInterest.GetImagePoi(GETPointOfInterest.DownloadedExtendedInfo[poi], (texture) => {
                        handler.SetTexture(texture);
                        downloads--;
                    });
                }else{
                    downloads--;
                }
            }else{
                GETPointOfInterest.ExtendedInfo(poi, (info) =>{
                    if(info==null) return;
                    Debug.Log("PoiExtended: " + info.id);
                    handler.OnExtInfo(info);
                    GETPointOfInterest.GetImagePoi(info, (texture) => {
                        handler.SetTexture(texture);
                        downloads--;
                    });
                });
            }

            //resizeScript.ResizeGridStorico();
            yield return new WaitUntil(() => downloads < maxDownloads);
            yield return new WaitForSeconds(0.25f);
        }
    }


    IEnumerator EvalNegozi()
    {
        foreach (ShopShortInfo shop in instantiateShopScript.shopNelRaggioDiRicerca)
        {
            downloads++;
            ShopPrefabHandler handler;
            if(objectNegozi.ContainsKey(shop)){
                handler = objectNegozi[shop].GetComponent<ShopPrefabHandler>();
                objectNegozi[shop].SetActive(true);
            }else{
                //Debug.Log(placeholder.ToString());
                var negozio = Instantiate(prefabNegozio, gridNegozio.transform);
                handler = negozio.GetComponent<ShopPrefabHandler>();
                handler.id = shop.id;
                handler.prefabText.text = shop.title;
                handler.prefabImage.texture = placeholder;
                handler.shop = shop;
                //handler.onlineMaps = onlineMaps;
                objectNegozi.Add(shop,negozio);
            }

            //var poiTrovato = GETPointOfInterest.DownloadedExtendedInfo.Where((x) => (poi.id == x.Value.id)).FirstOrDefault();
            if (GETShops.DownloadedShopExtendedInfo.ContainsKey(shop))
            {
                handler.OnExtInfo(GETShops.DownloadedShopExtendedInfo[shop]);
                if(GETShops.DownloadedShopExtendedInfo[shop].immagine_di_copertinaTexture == null){
                    GETPointOfInterest.GetImagePoi(GETShops.DownloadedShopExtendedInfo[shop], (texture) => {
                        handler.SetTexture(texture);
                        downloads--;
                    });
                }else{
                    downloads--;
                }
            }else{
                GETShops.ExtendedInfo(shop, (info) =>{
                    if(info==null) return;
                    Debug.Log("PoiExtended: " + info.id);
                    handler.OnExtInfo(info);
                    GETPointOfInterest.GetImagePoi(info, (texture) => {
                        handler.SetTexture(texture);
                        downloads--;
                    });
                });
            }

            //resizeScript.ResizeGridNegozio();
            yield return new WaitUntil(() => downloads < maxDownloads);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
