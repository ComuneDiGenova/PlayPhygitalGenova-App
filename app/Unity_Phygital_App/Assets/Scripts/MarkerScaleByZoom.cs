using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerScaleByZoom : MonoBehaviour
{
  
    [SerializeField] [Range(1,22)] int defaultZoom = 18;
    [SerializeField] [Range(1,22)] int minZoom = 10;
    [SerializeField] [Range(0.1f,1f)] float minScale = 0.5f;
    Dictionary<OnlineMapsMarker, float> markerScales = new Dictionary<OnlineMapsMarker, float>();


    private void Start()
    {
        minZoom = minZoom > defaultZoom ? defaultZoom : minZoom;
        if(minZoom < defaultZoom){
            OnlineMaps.instance.OnChangeZoom += OnChangeZoom;
        }
    }

    private void OnChangeZoom()
    {
        foreach(var m in OnlineMapsMarkerManager.instance.items){
            if(!markerScales.ContainsKey(m)) markerScales.Add(m,m.scale);
        }
        float scale = 1;
        if(OnlineMaps.instance.zoom < minZoom) scale = minScale;
        else if(OnlineMaps.instance.zoom > defaultZoom) scale = 1;
        else{
            var t = ((OnlineMaps.instance.zoom - minZoom) * 1f / (defaultZoom - minZoom));
            scale = Mathf.Lerp(minScale,1,t);
        }
        Debug.Log("scale: "  + scale);
        foreach(var m in OnlineMapsMarkerManager.instance.items){
            m.scale = markerScales[m] * scale;
        }
    }
}
