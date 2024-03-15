using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizationTextName : LocalizationText
{
    public override void ChangeLanguage(Languages lang)
    {
        Debug.LogWarning(GETUserInfo.DownloadedUserInfo.nome);
        var text = CSVParser.GetTextFromID(key, (int)lang) + " " + GETUserInfo.DownloadedUserInfo.nome;
        gameObject.GetComponent<TMP_Text>().text = text;
    }
}
