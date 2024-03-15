using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchEntry : MonoBehaviour
{
    BaseShortInfo info;
    [SerializeField] Image icona;
    [SerializeField] TextMeshProUGUI testo;
    [SerializeField] Sprite bottega;
    [SerializeField] Sprite negozio;
    FilterSearch filtersearch;

    public void Init(BaseShortInfo info, FilterSearch filtersearch){
        this.info = info;
        this.filtersearch = filtersearch;
        testo.text = info.title;
        if(info.tipo == "negozio"){
            icona.sprite = bottega;
        }else
        {
            if (!string.IsNullOrEmpty(info.id_tipologia))
                icona.sprite = GetTipologiePOI.GetSprite(info.id_tipologia);
        }
    }

    public void Select(){
        Debug.Log("Selected: " + info.ToString());
        OnlineMaps.instance.SetPositionAndZoom(info.lon, info.lat, 20);
        filtersearch.OpenTab(info);
    }
}
