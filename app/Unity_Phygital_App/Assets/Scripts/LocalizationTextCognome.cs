using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizationTextCognome : LocalizationText
{
    public override void ChangeLanguage(Languages lang)
    {
        var text = CSVParser.GetTextFromID(key, (int)lang) + " " + GETUserInfo.DownloadedUserInfo.cognome;
        gameObject.GetComponent<TMP_Text>().text = text;
    }
}

