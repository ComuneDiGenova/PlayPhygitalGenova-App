using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SendRecipit : MonoBehaviour
{    
    [SerializeField] GameObject placeholderPanel;
    [SerializeField] GameObject sendButton;
    [SerializeField] GameObject closeButton;
    [SerializeField] TMP_Text actionResponseText;

    [SerializeField] TMP_InputField recipitNumber;
    [SerializeField] TMP_InputField recipitValue;
    [SerializeField] TMP_InputField recipitDate;
    [SerializeField] TMP_InputField clientID;

    public void SendRecipitInfo()
    {
        AddPoint AddPoint = new AddPoint();

        ///
        if(string.IsNullOrWhiteSpace(clientID.text)){
            AlertPanel.OpenAlert("Attenzione: ID utente mancante.");
            return;
        }if(string.IsNullOrWhiteSpace(recipitValue.text)){
            AlertPanel.OpenAlert("Attenzione: importo mancante.");
            return;
        }
        if(string.IsNullOrWhiteSpace(recipitNumber.text)){
            AlertPanel.OpenAlert("Attenzione: numero scontrino mancante.");
            return;
        }
        if(string.IsNullOrWhiteSpace(recipitDate.text)){
            AlertPanel.OpenAlert("Attenzione: data mancante.");
            return;
        }

        //
        AddPoint.user_id = clientID.text;
        AddPoint.action = "acquisto";
        AddPoint.content_type = "negozio";
        AddPoint.content_id = "7950"; // <---- CONTENT ID, ATTUALMENTE NON REPERIBILE, QUESTO ID PROVA 01
        AddPoint.data_scontrino = recipitDate.text;
        AddPoint.importo = recipitValue.text;
        AddPoint.numero_scontrino = recipitNumber.text;

        //StartCoroutine(GETUserInfo.ADDPoints(AddPoint, GetRecepitResponse));
        GETUserInfo.AddPoints(AddPoint, GetRecepitResponse);

        placeholderPanel.SetActive(true);
        closeButton.SetActive(true);
        sendButton.SetActive(false);
    }

    public void CloseRecipitTab()
    {
        if (placeholderPanel.activeSelf)
        {
            placeholderPanel.SetActive(false);
            closeButton.SetActive(false);
            sendButton.SetActive(true);
            actionResponseText.text = "Inviando informazioni transizione...";
        }
    }

    void GetRecepitResponse(ResponseAddPoint response)
    {
       if(response != null){
            if (response.result == true)
            {
                actionResponseText.text = response.message + ": " + response.points;
                clientID.text = null;
                recipitNumber.text = null;
                recipitValue.text = null;
                recipitDate.text = null;
                PopUpPanel.OpenLanguage(GETUserInfo.pointsIdKeyLanguage,true,response.points.ToString());
            }
            else if (response.result == false)
            {
                actionResponseText.text = response.message + ".";
                PopUpPanel.OpenLanguage(GETUserInfo.pointsIdKeyLanguage,false,response.points.ToString());
            }
        }else{
            actionResponseText.text = response.message + ".";
            PopUpPanel.OpenLanguage(GETUserInfo.pointsIdKeyLanguage,false,"Error in crediting points");
        }
    }
}
