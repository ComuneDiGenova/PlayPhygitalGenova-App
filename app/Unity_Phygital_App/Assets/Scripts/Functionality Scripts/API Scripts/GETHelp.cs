using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GETHelp : MonoBehaviour
{
    string helpURL = "/jsonapi/get_pagine_help_app_mobile";

    public static HelpInfo HelpInfo;
    public static HelpInfoList HelpInfoList = new HelpInfoList();

    //[SerializeField] HelpInfoList helpData; // DA RIMUOVERE

    [SerializeField] GameObject helpPrefab;
    [SerializeField] Transform helpContainer;
    [SerializeField] TMP_Text helpTitle;
    [SerializeField] TMP_Text helpText;

    List<GameObject> panels = new List<GameObject>();
    

    void Awake(){
        GameConfig.OnLanguageChange += (lang) => {
            InstantiateHelp();
        };
    }

    void Start()
    {
    }

    public void InstantiateHelp()
    {
        Debug.Log("Help");
        foreach(var g in panels){
            Destroy(g);
        }
        panels.Clear();
        var children = helpContainer.GetComponentsInChildren<Transform>();
        foreach(var c in children){
            if(c != helpContainer.transform)
                Destroy(c.gameObject);
        }
        //
        HelpInfo = null;
        //
        var url = helpURL + "/" + GameConfig.applicationLanguage.ToString().ToLower();
        WSO2.GET(url, (response)=>{
            if(!string.IsNullOrEmpty(response) || response != "[]"){
                string json = "{\"helpList\": " + response + "}";
                Debug.Log(json);
                try
                {
                    HelpInfoList = JsonUtility.FromJson<HelpInfoList>(json);
                    if (HelpInfoList == null)
                    {
                        var result = JsonUtility.FromJson<Result>(response);
                        if (result != null && result.result == false)
                            Debug.LogError(result.message);
                    }
                    else
                    {
                        //helpData = HelpInfoList;
                        Debug.LogWarning(HelpInfoList.helpList.Count);
                        if (HelpInfoList != null)
                        {
                            foreach (var h in HelpInfoList.helpList)
                            {
                                helpTitle.text = h.titolo;
                                var text = h.descrizione.Replace("<strong>", "<b>");
                                text = text.Replace("</strong>", "</b>");
                                helpText.text = text;
                                Debug.Log(h.ToString());
                                var p = Instantiate(helpPrefab, helpContainer);
                                panels.Add(p);
                            }
                        }
                    }
                }catch(System.Exception e){
                    Debug.LogError(e.Message);
                }
            }
        });
        
    }
}
