using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static OnlineMapsGPXObject;
using Unity.VisualScripting;

public class TEST_API_POI : MonoBehaviour
{
    [SerializeField] ShortInfo infoPOI;
    [SerializeField] Info infoExtended;


    [SerializeField] Button button;

    [SerializeField] TMP_Text textID;
    [SerializeField] TMP_Text textNome;
    [SerializeField] TMP_Text textDescrizione;
    [SerializeField] TMP_Text textIndicazioni;
    [SerializeField] TMP_Text textOrario;
    [SerializeField] TMP_Text textSito;
    [SerializeField] TMP_Text textTelefono;
    [SerializeField] TMP_Text textMail;

    [SerializeField] RawImage textureImage;

    void Awake()
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log("Cliccato: " + infoPOI.title);
        });
    }

    public void SetPoi(Info poi)
    {
        infoExtended = poi;

        //textID.text = poi.id;
        textNome.text = poi.nome;
        textDescrizione.text = poi.descrizione;
        textIndicazioni.text = poi.indirizzo;
        textOrario.text = poi.orari.ToString();
        textSito.text = poi.url;
        textTelefono.text = poi.telefono;
        textMail.text = poi.email;
    }

    public void SetImage(Texture texture)
    {
        textureImage.texture = texture;
    }
}
