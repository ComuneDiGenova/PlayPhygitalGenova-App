using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageDropdown : MonoBehaviour
{
    //static public Action<int> ChangeLanguage;
    public TMP_Dropdown dropdown;
    public TMP_Text label;

    bool lockSet;

    public void LanguageChanged() //chiamato da dropdown
    {
        if(lockSet) return;
        Debug.LogWarning("LanguageChanged");
        GameConfig.ChangeLanguage((Languages)dropdown.value);
    }

    private void Awake() {
        lockSet = true;
        PopulateDropdown();
        lockSet = false;
        GameConfig.OnLanguageChange += (lang)=> {
            lockSet = true;
            Debug.Log("lang: " + (int)GameConfig.applicationLanguage + " | "+ GameConfig.applicationLanguage);
            dropdown.value = (int)GameConfig.applicationLanguage;
            dropdown.captionText.text = CSVParser.GetAvailableLanguages()[dropdown.value];
            label.text = dropdown.captionText.text;
            lockSet = false;
        };
    }

    void Start()
    {
        //PopulateDropdown();
    }

    void PopulateDropdown()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(CSVParser.GetAvailableLanguages());
        dropdown.value = (int)GameConfig.applicationLanguage;
        dropdown.captionText.text = CSVParser.GetAvailableLanguages()[dropdown.value];
        label.text = dropdown.captionText.text;
    }
}
