using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class LocalizationTextGenovini : LocalizationText
{
    public override void ChangeLanguage(Languages lang)
    {
        var text = CSVParser.GetTextFromID(key, (int)lang) + " " + GETUserInfo.DownloadedUserInfo.genovini;
        gameObject.GetComponent<TMP_Text>().text = text;
    }
}
