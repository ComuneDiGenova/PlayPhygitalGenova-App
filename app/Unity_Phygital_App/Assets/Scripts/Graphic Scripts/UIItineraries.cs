using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItineraries : MonoBehaviour
{
    [SerializeField] GameObject itinerariesTab; 
    [SerializeField] GameObject defaultItinerariesTab;
    [SerializeField] GameObject customItinerariesTab;
    [SerializeField] GameObject minimizedItinerariesTab;

    [SerializeField] GameObject closeItinerariesPanel;


    public void OpenItineraries(){
        //Debug.Log("Open");
        if (ItinerariesManager.itinerarioSelezionato == null)
        {
            itinerariesTab.SetActive(true);
            customItinerariesTab.SetActive(false);
            defaultItinerariesTab.SetActive(false);
            minimizedItinerariesTab.SetActive(false);
        }else{
            itinerariesTab.SetActive(false);
            customItinerariesTab.SetActive(false);
            defaultItinerariesTab.SetActive(false);
            minimizedItinerariesTab.SetActive(true);
        }
    }

    public void ActiveDefaultItineraries()
    {
        //Debug.Log("Default");
        defaultItinerariesTab.SetActive(!defaultItinerariesTab.activeSelf);
        //itinerariesTab.SetActive(false);
        /*
        if (defaultItinerariesTab.activeSelf == false)
        {
            defaultItinerariesTab.SetActive(true);
            //itinerariesTab.SetActive(false);
        }
        else
        {
            defaultItinerariesTab.SetActive(false);
            //itinerariesTab.SetActive(false);
        }*/
    }

    public void ActiveCustomItineraries()
    {
        //Debug.Log("custom");
        customItinerariesTab.SetActive(true);
        //itinerariesTab.SetActive(false);
        /*
        if (customItinerariesTab.activeSelf == false)
        {
            customItinerariesTab.SetActive(true);
            itinerariesTab.SetActive(false);
        }
        else
        {
            customItinerariesTab.SetActive(false);
            itinerariesTab.SetActive(false);
        }
        */
    }

    public void MinimizeItineraries()   //chiamato da selezione itinerario
    {
        //Debug.Log("select minimize");
        itinerariesTab.SetActive(false);
        defaultItinerariesTab.SetActive(false);
        customItinerariesTab.SetActive(false);
        minimizedItinerariesTab.SetActive(true);
        closeItinerariesPanel.SetActive(false);
    }

    public void SwitchMinimizedItineraries() //chiamato dalle frecce
    {
        //Debug.Log("open/close mini");
        if(itinerariesTab.activeSelf || defaultItinerariesTab.activeSelf || customItinerariesTab.activeSelf){
            itinerariesTab.SetActive(false);
            defaultItinerariesTab.SetActive(false);
            customItinerariesTab.SetActive(false);
        }else{
            itinerariesTab.SetActive(true);
            customItinerariesTab.SetActive(false);
            defaultItinerariesTab.SetActive(false);
        }
    }

    public void ResetItinerariesTab()
    {
        //Debug.Log("RESET");
        if (ItinerariesManager.itinerarioSelezionato == null)
        {
            itinerariesTab.SetActive(true);
            defaultItinerariesTab.SetActive(false);
            customItinerariesTab.SetActive(false);
            minimizedItinerariesTab.SetActive(false);
        }
        else
        {
            minimizedItinerariesTab.SetActive(true);
        }
    }
}
