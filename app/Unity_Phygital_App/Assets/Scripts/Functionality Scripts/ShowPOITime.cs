using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPOITime : MonoBehaviour
{
    [SerializeField] GameObject hourTab;

    public void ActiveHourTab()
    {
        hourTab.SetActive(true);
    }

    public void CloseHourTab()
    {
        hourTab.SetActive(false);
    }
}
