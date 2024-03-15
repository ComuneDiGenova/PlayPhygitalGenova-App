using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class LocalizationText : MonoBehaviour
{
    public string key;

    void Start()
    {
        ChangeLanguage(GameConfig.applicationLanguage);

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    public virtual void ChangeLanguage(Languages lang)
    {
        if(!string.IsNullOrWhiteSpace(key))
            gameObject.GetComponent<TMP_Text>().text = CSVParser.GetTextFromID(key, (int)lang);
    }

    void OnEnable()
    {
        GameConfig.OnLanguageChange += ChangeLanguage;
        ChangeLanguage(GameConfig.applicationLanguage);
    }

    void OnDisable()
    {
        GameConfig.OnLanguageChange -= ChangeLanguage;
    }
}
