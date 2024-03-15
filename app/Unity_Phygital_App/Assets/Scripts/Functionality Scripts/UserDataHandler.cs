using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserDataHandler : MonoBehaviour
{
    [SerializeField] TMP_Text userID;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text genoviniText;

    // Utobile
    [SerializeField] TMP_Text optionsUserID;
    //[SerializeField] TMP_Text optionsNameText;
    //[SerializeField] TMP_Text optionsSurnameText;
    [SerializeField] TMP_Text optionsEmailText;
    //[SerializeField] TMP_Text optionsGenoviniText;
    [SerializeField] TMP_Text qrGenoviniText;

    // Commericabile
    [SerializeField] TMP_Text optionsProUserID;
    [SerializeField] TMP_Text optionsNameProText;
    [SerializeField] TMP_Text surnameProText;
    [SerializeField] TMP_Text emailProText;


    public void SetData(InfoUser infouser)
    {
        if(infouser == null){
            userID.text = null;
            nameText.text = "Anonimo";        
            genoviniText.text = "0";
            optionsUserID.text = "ID: 0";
            optionsProUserID.text = "ID: 0";
            //optionsNameText.text = "Nome: Anonimo";
            //optionsSurnameText.text = "Cognome: ";
            optionsEmailText.text = "Email: ";
            //optionsGenoviniText.text = "Hai accumulato: 0 genovini";
        }else{
            userID.text = GameConfig.userID;
            nameText.text = infouser.nome;        
            genoviniText.text = infouser.genovini;

            optionsNameProText.text = infouser.nome;
            surnameProText.text = infouser.cognome;
            emailProText.text = infouser.email;

            optionsUserID.text = "ID: " + GameConfig.userID;
            optionsProUserID.text = "ID: " + GameConfig.userID;
            //optionsNameText.text = "Nome: " + infouser.nome;
            //optionsSurnameText.text = "Cognome: " + infouser.cognome;
            optionsEmailText.text = "Email: " + infouser.email;
            //optionsGenoviniText.text = "Hai accumulato: " + infouser.genovini + " genovini";
            qrGenoviniText.text = infouser.genovini;
        }
    }

    public void SetLanguage(string language)
    {   
        switch (language)
        {
            case "Italiano":
                GameConfig.ChangeLanguage(Languages.IT);
                break;
            case "Inglese":
                GameConfig.ChangeLanguage(Languages.EN);
                break;
            case "Francese":
                GameConfig.ChangeLanguage(Languages.FR);
                break;
            case "Tedesco":
                GameConfig.ChangeLanguage(Languages.DE);
                break;
            case "Spagnolo":
                GameConfig.ChangeLanguage(Languages.ES);
                break;
            case "Russo":
                GameConfig.ChangeLanguage(Languages.RU);
                break;
            default:
                GameConfig.ChangeLanguage(Languages.IT);
                break;

        }
    }


}
