using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPOIExtendedInfo : MonoBehaviour
{
    [SerializeField] GameObject infoTab;

    ShortInfo poiData = new ShortInfo();

    public void ActiveExtendedInfo()
    {
        //textUpdateUI.SetInfo("10", poiData);
        infoTab.SetActive(true);
    }
}
