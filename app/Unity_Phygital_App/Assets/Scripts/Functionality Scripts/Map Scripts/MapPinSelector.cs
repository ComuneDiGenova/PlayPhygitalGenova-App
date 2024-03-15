using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapPinSelector : MonoBehaviour
{

    [SerializeField] Toggle touristToggle;
    [SerializeField] Toggle nonTouristToggle;
    [SerializeField] Toggle shopToggle;
     [SerializeField] Toggle bottegheShopToggle;
    [SerializeField] Toggle allToggle;


    public void OnButtonEnable()
    {
        if (OnlineMapsMarkerManager.instance.items.Count == 0)
        {
            InitializeToggle();
        }

        //Debug.Log(touristToggle.isOn);
        //Debug.Log(nonTouristToggle.isOn);
        //Debug.Log(shopToggle.isOn);
        //Debug.Log(allToggle.isOn);
    }

    private void InitializeToggle()
    {      
        touristToggle.isOn = true;
        nonTouristToggle.isOn = false;
        shopToggle.isOn = true;
        //allToggle.isOn = false;
        bottegheShopToggle.isOn = true;
    }

    public void ToggleTourist()
    {

        if (!touristToggle.isOn)
        {
            foreach (var marker in OnlineMapsMarkerManager.instance.items)
            {
                if (marker.tags.Contains("Punto Storico"))
                {
                    marker.enabled = false;                    
                }
                
            }
            //allToggle.isOn = false;
        }

        if (touristToggle.isOn)
        {
            foreach (var marker in OnlineMapsMarkerManager.instance.items)
            {
                if (marker.tags.Contains("Punto Storico"))
                {
                    marker.enabled = true;
                }
                //CheckAllToggle();
            }
        }        
    }

    public void ToggleNonTourist()
    {      
        if (!nonTouristToggle.isOn)
        {
            foreach (var marker in OnlineMapsMarkerManager.instance.items)
            {
                if (marker.tags.Contains("Non Turistico"))
                {
                    marker.enabled = false;                   
                }        
            }
            //allToggle.isOn = false;
        }

        if (nonTouristToggle.isOn)
        {
            foreach (var marker in OnlineMapsMarkerManager.instance.items)
            {
                if (marker.tags.Contains("Non Turistico"))
                {
                    marker.enabled = true;
                }
                //CheckAllToggle();
            }
        }
    }

    public void ToggleShop()
    {
        if (!shopToggle.isOn)
        {
            foreach (var marker in OnlineMapsMarkerManager.instance.items)
            {
                if (marker.tags.Contains("Negozio"))
                {
                    marker.enabled = false;
                }            
            }
            //allToggle.isOn = false;
        }

        if (shopToggle.isOn)
        {
            foreach (var marker in OnlineMapsMarkerManager.instance.items)
            {
                if (marker.tags.Contains("Negozio"))
                    marker.enabled = true;
            }
            //CheckAllToggle();
        }
    }

        public void BottegheToggleShop()
    {
        if (!bottegheShopToggle.isOn)
        {
            foreach (var marker in OnlineMapsMarkerManager.instance.items)
            {
                if (marker.tags.Contains("Bottega"))
                {
                    marker.enabled = false;
                }            
            }
            //allToggle.isOn = false;
        }

        if (bottegheShopToggle.isOn)
        {
            foreach (var marker in OnlineMapsMarkerManager.instance.items)
            {
                if (marker.tags.Contains("Bottega"))
                    marker.enabled = true;
            }
            //CheckAllToggle();
        }
    }

    public void ToggleAll()
    {
        if (!allToggle.isOn)
        {
            foreach (var marker in OnlineMapsMarkerManager.instance.items)
            {
                marker.enabled = true;
            }

        }
    } 

    void CheckAllToggle()
    {
        if(touristToggle.isOn && nonTouristToggle.isOn && shopToggle.isOn)
        {
            allToggle.isOn = true;
        }
    }
}
