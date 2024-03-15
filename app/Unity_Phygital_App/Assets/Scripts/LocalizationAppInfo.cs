using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizationAppInfo : LocalizationText
{
    public override void ChangeLanguage(Languages lang)
    {
        if(!string.IsNullOrWhiteSpace(key)){
            var text = CSVParser.GetTextFromID(key, (int)lang) + $"\nrev: {Application.version}";
            gameObject.GetComponent<TMP_Text>().text = text;
        }
    }
}
