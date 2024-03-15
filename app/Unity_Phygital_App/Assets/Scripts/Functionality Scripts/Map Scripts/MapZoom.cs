using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoom : MonoBehaviour
{
    [SerializeField] GameObject map;
    [SerializeField] int zoom = 18;

    void Start()
    {
        SetZoom();
    }

    void SetZoom()
    {
        map.GetComponent<OnlineMaps>().zoom = zoom;
    }
}
