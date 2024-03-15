using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using TMPro;

public class FilterSearch : MonoBehaviour
{
    [SerializeField] TMP_InputField inp_search;
    [SerializeField] GameObject cnt_toggle;
    [SerializeField] GameObject cnt_search;
    [SerializeField] GameObject bar_toggle;
    [SerializeField] GameObject bar_search;
    [SerializeField] RectTransform entryParent;
    [SerializeField] GameObject entryPrefab;
    [SerializeField] POIMarkerTab markerTabScript;
    [SerializeField] ShopMarkerTab shopMarkerTabScript;
    [SerializeField] UIManager uiManager;

    Dictionary<BaseShortInfo, SearchEntry> entryList = new Dictionary<BaseShortInfo, SearchEntry>();

    private void OnEnable() {
        SelectSearch();
        //inp_search.text = "";
        //ClearEntries();
    }

    public void SelectFilter(){
        //cnt_toggle.SetActive(true);
        //cnt_search.SetActive(false);
        bar_toggle.SetActive(true);
        bar_search.SetActive(false);
    }
    public void SelectSearch(){
        //cnt_toggle.SetActive(false);
        //cnt_search.SetActive(true);
        bar_toggle.SetActive(false);
        bar_search.SetActive(true);
    }

    void ClearEntries(){
        foreach(var kvp in entryList){
            Destroy(kvp.Value.gameObject);
        }
        entryList.Clear();
    }

    void Instantiate(List<BaseShortInfo> list){
        foreach(var i in list){
            var o = Instantiate(entryPrefab,entryParent);
            var se = o.GetComponent<SearchEntry>();
            se.Init(i,this);
            entryList.Add(i,se);
        }
    }

    List<BaseShortInfo> Search(string value){
        Debug.Log("Search for: " + value);
        List<BaseShortInfo> list = new List<BaseShortInfo>();
        //poi
        var pois = GETPointOfInterest.DownloadedInformationPois.infos.Where(x=>x.title.ToLower().Contains(value)).ToList<BaseShortInfo>();
        list.AddRange(pois);
        //shops
        var shops = GETShops.DownloadedInformationShop.shopInfos.Where(x=>x.title.ToLower().Contains(value)).ToList<BaseShortInfo>();
        list.AddRange(shops);
        Debug.Log("Found: " + list.Count.ToString());
        return list;
    }

    public void OnValueChange(string value){
        ClearEntries();
        if(value.Length > 2) {
            var list = Search(value.ToLower());
            Instantiate(list);
        }
    }

    public void OpenTab(BaseShortInfo info){
        if(info.tipo == "poi")
            markerTabScript.GetClickedNameByPoi(info.id);
        if(info.tipo == "negozio")
            shopMarkerTabScript.GetClickedNameByShop(info.id);
        Close();
    }

    public void Close(){
        //inp_search.text = "";
        //ClearEntries();
        uiManager.CallActiveSelector();
    }
}
