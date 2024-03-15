using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.WebRequestMethods;
using System.Linq;
using System.Globalization;
using System.Net.Mail;
using System;

public class UIActivateMarkerTab : MonoBehaviour
{
    //[SerializeField] GETPointOfInterest getPoi;

    [SerializeField] GameObject prefabTab;
    [SerializeField] GameObject poiExpandedTab;

    [SerializeField] GameObject menuButton;
    [SerializeField] GameObject helpButton;
    [SerializeField] GameObject arButton;
    [SerializeField] GameObject closeButton;


    [SerializeField] Button button;
    [SerializeField] TMP_Text textType;
    [SerializeField] TMP_Text textName;
    [SerializeField] RawImage textureImage;

    [SerializeField] GameObject hoursButton;
    [SerializeField] string poiID;
    [SerializeField] TMP_Text poiTextName;
    [SerializeField] TMP_Text poiTextType;
    [SerializeField] TMP_Text poiTextIndications;
    [SerializeField] TMP_Text poiTextHour;
    [SerializeField] TMP_Text poiTextPhone;
    [SerializeField] TMP_Text poiTextSite;
    [SerializeField] TMP_Text poiTextMail;
    [SerializeField] TMP_Text poiTextDescrizione;
    [SerializeField] RawImage poiTextureImage;

    // SPRITE PREFERITI
    [SerializeField] Image favouriteImage;
    [SerializeField] Sprite notFavouriteSprite;
    [SerializeField] Sprite favouriteSprite;

    string charsToRemove = "";     // TO REMOVE

    static string url = null;
    static string mailTo = null;

    private void OnEnable()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void ActiveMarkerTabExt(string category, Info info,string type){
        if (info == null) return;
        textType.text = type;
        textName.text = info.nome;

        textureImage.enabled=false;

        prefabTab.SetActive(true);

        Debug.Log("La categoria cliccata: " + category);
        Debug.Log("info.immagine_di_copertina: " + info.immagine_di_copertina);

        if (!string.IsNullOrEmpty(info.immagine_di_copertina))
        {
            if(info.immagine_di_copertinaTexture == null){
                GETPointOfInterest.GetImagePoi(info,(texture) => {
                    
                    if(texture != null){
                        textureImage.GetComponentInParent<Mask>().enabled = true;
                        textureImage.enabled=true;
                        textureImage.texture = texture;
                    }
                    else
                    {
                        textureImage.GetComponentInParent<Mask>().enabled = false;
                        textureImage.enabled = false;
                        textureImage.texture = null;
                    }
                });
            }else{
                textureImage.GetComponentInParent<Mask>().enabled = true;
                textureImage.texture = info.immagine_di_copertinaTexture;
                textureImage.enabled=true;
            }
        }
    }
     public void ActiveMarkerTabExt(string category, ShopInformaitions info){
        if (info == null) return;
        textType.text = info.tipologia;
        textName.text = info.nome;
        textureImage.enabled=false;

        if (textType.text == "Botteghe Storiche")
        {
            textType.text = "Bottega Storica";
        }

        prefabTab.SetActive(true);

        Debug.Log("La categoria cliccata : " + category);
        Debug.Log("info.immagine_di_copertina: " + info.immagine_di_copertina);

        if (!string.IsNullOrEmpty(info.immagine_di_copertina))
        {
            if (category == "Bottega")
            {
                Debug.Log("Cliccato su una bottega storica");
                if(info.immagine_di_copertinaTexture == null){
                /*
                    StartCoroutine(GETPointOfInterest.GETImage(null, info.immagine_di_copertina, (texture) => {
                        info.immagine_di_copertinaTexture = texture;
                        textureImage.texture = texture;
                    }));*/
                    GETPointOfInterest.GetImagePoi(info, (texture) => {
                        
                        if(texture != null){
                            textureImage.GetComponentInParent<Mask>().enabled = true;
                            textureImage.texture = texture;
                            textureImage.enabled=true;
                        }
                        else
                        {
                            textureImage.GetComponentInParent<Mask>().enabled = false;
                            textureImage.enabled = false;
                            textureImage.texture = null;
                        }
                            
                    });
                }else{
                   textureImage.GetComponentInParent<Mask>().enabled = true;
                   textureImage.texture = info.immagine_di_copertinaTexture;
                   textureImage.enabled=true;
                }
            }

            if (category == "Negozio")
            {
                Debug.Log("Cliccato su un negozio");
                if(info.immagine_di_copertinaTexture == null){
                /*
                    StartCoroutine(GETShops.GETShopImage(null, info.immagine_di_copertina,  (texture) => {
                        info.immagine_di_copertinaTexture = texture;
                        textureImage.texture = texture;
                    }));*/
                      GETPointOfInterest.GetImagePoi(info, (texture) => {
                         if(texture != null){
                              textureImage.GetComponentInParent<Mask>().enabled = true;
                              textureImage.texture = texture;
                              textureImage.enabled=true;
                         }
                          else
                          {
                              textureImage.GetComponentInParent<Mask>().enabled = false;
                              textureImage.enabled = false;
                              textureImage.texture = null;
                          }
                    });
                }else{
                    textureImage.GetComponentInParent<Mask>().enabled = true;
                    textureImage.texture = info.immagine_di_copertinaTexture;
                    textureImage.enabled=true;
                }
            }
        }
    
    }


    public void ActivePOITabExt(string id, string name, string type, string indications, string hour, string phone, string site, string mail, string description,Texture2D texture)
    {
        Debug.Log("POI TAB");
        if(GETUserInfo.FavouriteResponse != null){
            var favouriteTrovato = GETUserInfo.FavouriteResponse.favourites.Where((x) => (x.id.ToString() == id)).FirstOrDefault();

            Debug.Log("Marker: " + AddFavourite.favouriteClickedMarkerLabel);
            if (favouriteTrovato != null)
            {
                Debug.Log("ho trovato un poi preferito");
                favouriteImage.sprite = favouriteSprite;
                //favouriteTrovato = null;
            }
            else
            {
                Debug.Log("non ho trovato un poi preferito");
                favouriteImage.sprite = notFavouriteSprite;
                //favouriteTrovato = null;
            }
        }else{
            favouriteImage.sprite = notFavouriteSprite;
        }

        menuButton.SetActive(false);
        helpButton.SetActive(false);
        arButton.SetActive(false);
        closeButton.SetActive(true);

        poiExpandedTab.SetActive(true);

        //url = null;
        //Debug.Log("SITO URL VUOTO: " + url);
        //Debug.Log("SITO URL SITE: " + site);
        string shortSite;
        url = site;

        Debug.Log("SITO URL: " + url);

        CleanURL(site, out shortSite);

        Debug.Log("SITO URL SHORT: " + shortSite);

        mailTo=null;
        mailTo = mail;

        poiID = id;

        if(!string.IsNullOrEmpty(indications)){
            for (int i = 1; i <= 100; i++)
            {
                indications = indications.Replace("  ", " ");
            }      
        }

        if (hour == "0" || hour == null || hour.Count() < 5)
        {
            hour = "";
            hoursButton.SetActive(false);
        }
        else
        {
            hoursButton.SetActive(true);
        }

        
        if (type == "Botteghe Storiche")
        {
            type = "Bottega Storica";
        }

        poiTextName.text = name;
        poiTextType.text = type;

        //controllo i vari campi testuali e verifico se hanno un valore dentro
        ControlloStringaVuota(indications, poiTextIndications);
        ControlloStringaVuota(phone, poiTextPhone);
        ControlloStringaVuota(shortSite, poiTextSite);
        ControlloStringaVuota(mail, poiTextMail);
        //poiTextIndications.text = indications;
        if (string.IsNullOrEmpty(hour)) hoursButton.transform.parent.transform.gameObject.SetActive(false);
        else
        {
            poiTextHour.text = hour;
            hoursButton.transform.parent.transform.gameObject.SetActive(true);
        }
            if (!hoursButton.transform.parent.transform.gameObject.activeInHierarchy && !poiTextPhone.transform.parent.transform.gameObject.activeInHierarchy)
        {
            hoursButton.transform.parent.transform.parent.transform.gameObject.SetActive(false);
        } else hoursButton.transform.parent.transform.parent.transform.gameObject.SetActive(true);

        //poiTextPhone.text = phone;
        //poiTextSite.text = shortSite;
        //poiTextMail.text = mail;
        //BOLT

            //controllo se esiste il testo e ne eseguo la formattazione 
        if (string.IsNullOrEmpty(description)) poiTextDescrizione.transform.parent.transform.parent.gameObject.SetActive(false);
        else
        {
            poiTextDescrizione.transform.parent.transform.parent.gameObject.SetActive(true);
            //poiTextDescrizione.text = TextDecoder.DecodeText(description);
            poiTextDescrizione.text = System.Text.RegularExpressions.Regex.Replace(description, "<.*?>", String.Empty);
        }

        if(texture != null){

            //var downloadedHeight = texture.height;
            //var downloadedWidth = texture.width;

            //float aspectRatio = downloadedWidth / downloadedHeight;
            poiTextureImage.GetComponentInParent<Mask>().enabled = true;
            poiTextureImage.texture = texture;
            poiTextureImage.enabled=true;

            poiTextureImage.GetComponent<AspectRatioFitter>().aspectRatio = 1.5f;
        }else{
            poiTextureImage.GetComponentInParent<Mask>().enabled = false;
            poiTextureImage.enabled=false;
            poiTextureImage.texture = null;
        }
    }

    private void ControlloStringaVuota(string stringa, TMP_Text text)
    {
        if (string.IsNullOrEmpty(stringa)) { text.transform.parent.transform.gameObject.SetActive(false);}
        else
        {
            text.transform.parent.transform.gameObject.SetActive(true);
            text.text = stringa;
        }
        Debug.Log("controllo se la stringa "+ text + " "+ string.IsNullOrEmpty(stringa));
    }
    public void CloseMarkerTab()
    {
        CoordinateControl.isMarkerActive = false;
        if(ARPOIBehaviour.activeArPoi != null)
            ARPOIBehaviour.activeArPoi.Disable();
        prefabTab.SetActive(false);
    }

    void CleanURL(string site, out string cleanSite)
    {
        if(string.IsNullOrEmpty(site)){
            cleanSite = null;
            return;
        }
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        string shortString = site.Replace(charsToRemove, "");
        string cleanedString = shortString.Replace("-", " ");
        string capitalizedString = textInfo.ToTitleCase(cleanedString);
        cleanSite = capitalizedString;
        cleanSite = cleanSite.Replace("%25C3%25A0", "à");
        cleanSite = cleanSite.Replace("%25E2%2580%2593", "-");
        cleanSite = cleanSite.Replace("%25E2%2580%2599", "'");
    }

    public void OpenLink()
    {
        if(!string.IsNullOrEmpty(url)){
            string urlSito = System.Uri.UnescapeDataString(url);
            Debug.Log("SITO URL CLICK: " + url);
            Debug.Log("SITO URL CLICKSTRING: " + urlSito);
            Uri outUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out outUri) && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
            {
            Debug.Log("OK URI: " + outUri.AbsoluteUri);
            Application.OpenURL(outUri.AbsoluteUri);
            }else{
                Debug.LogError("Wrong URI");
                Application.OpenURL(urlSito);
            }
            //Application.OpenURL(urlSito);
        }
    }

    public void OpenDefaultMailApp()
    {
        if(!string.IsNullOrEmpty(mailTo)){
            string mailtoLink = "mailto:" + System.Uri.EscapeDataString(mailTo);
            Application.OpenURL(mailtoLink);
        }
    }

    public void OpenDefaultPhoneDialer()
    {
        // Make sure to remove any non-numeric characters from the phone number
        string numericPhoneNumber = new string(poiTextPhone.text.Where(char.IsDigit).ToArray());

        // Create the tel link
        string telLink = "tel:" + numericPhoneNumber;

        // Open the default phone dialer application
        Application.OpenURL(telLink);
    }

    // TABELLA POI AR
    public void ActiveMarkerARTab(string category, Info info, string tipologia)
    {
        textType.text = tipologia;
        textName.text = info.nome;

        prefabTab.SetActive(true);

        textureImage.enabled=false;

        Debug.Log("La categoria cliccata: " + category);

        if (!string.IsNullOrEmpty(info.immagine_di_copertina))
        {
            if (category == "Punto Storico")
            {
                Debug.Log("Cliccato su un punto storico");
                if(info.immagine_di_copertinaTexture == null){
                /*
                    StartCoroutine(GETPointOfInterest.GETImage(null, info.immagine_di_copertina, (texture) => {
                        info.immagine_di_copertinaTexture = texture;
                        textureImage.texture = texture;
                    }));*/
                      GETPointOfInterest.GetImagePoi(info, (texture) => {
                          if (texture != null)
                          {
                              textureImage.GetComponentInParent<Mask>().enabled = true;
                              textureImage.texture = texture;
                              textureImage.enabled = true;
                          }
                          else
                          {
                              textureImage.GetComponentInParent<Mask>().enabled = false;
                              textureImage.enabled = false;
                              textureImage.texture = null;
                          }
                      });
                }else{
                    textureImage.GetComponentInParent<Mask>().enabled = true;
                    textureImage.texture = info.immagine_di_copertinaTexture;
                   textureImage.enabled=true;
                   }
            }

            if (category == "Negozio")
            {
                Debug.Log("Cliccato su un negozio");
                if(info.immagine_di_copertinaTexture == null){/*
                    StartCoroutine(GETShops.GETShopImage(null, info.immagine_di_copertina, (texture) => {
                        info.immagine_di_copertinaTexture = texture;
                        textureImage.texture = texture;
                    }));*/
                      GETPointOfInterest.GetImagePoi(info, (texture) => {
                          if (texture != null)
                          {
                              textureImage.GetComponentInParent<Mask>().enabled = true;
                              textureImage.texture = texture;
                              textureImage.enabled = true;
                          }
                          else
                          {
                              textureImage.GetComponentInParent<Mask>().enabled = false;
                              textureImage.enabled = false;
                              textureImage.texture = null;
                          }
                      });
                }else{
                    textureImage.GetComponentInParent<Mask>().enabled = true;
                    textureImage.texture = info.immagine_di_copertinaTexture;
                   textureImage.enabled=true;
                }
            }
        }
    }

    public void ActiveMarkerARTab(string category, ShopInformaitions info)
    {
        textType.text = info.tipologia;
        textName.text = info.nome;

        textureImage.enabled=false;

        if (textType.text == "Botteghe Storiche")
        {
            textType.text = "Bottega Storica";
        }

        prefabTab.SetActive(true);

        Debug.Log("La categoria cliccata �: " + category);

        if (!string.IsNullOrEmpty(info.immagine_di_copertina))
        {
            if (category == "Bottega")
            {
                Debug.Log("Cliccato su una bottega");
                if (info.immagine_di_copertinaTexture == null){/*
                    StartCoroutine(GETShops.GETShopImage(null, info.immagine_di_copertina, (texture) => {
                        info.immagine_di_copertinaTexture = texture;
                        textureImage.texture = texture;
                    }));
                    */
                      GETPointOfInterest.GetImagePoi(info, (texture) => {
                          if (texture != null)
                          {
                              textureImage.GetComponentInParent<Mask>().enabled = true;
                              textureImage.texture = texture;
                              textureImage.enabled = true;
                          }
                          else
                          {
                              textureImage.GetComponentInParent<Mask>().enabled = false;
                              textureImage.enabled = false;
                              textureImage.texture = null;
                          }
                      });
                }else{
                    textureImage.GetComponentInParent<Mask>().enabled = true;
                    textureImage.texture = info.immagine_di_copertinaTexture;
                    textureImage.enabled=true;
                }
            }

            if (category == "Negozio")
            {
                Debug.Log("Cliccato su un negozio");
                if (info.immagine_di_copertinaTexture == null){/*
                    StartCoroutine(GETShops.GETShopImage(null, info.immagine_di_copertina, (texture) => {
                        info.immagine_di_copertinaTexture = texture;
                        textureImage.texture = texture;
                    }));*/
                      GETPointOfInterest.GetImagePoi(info, (texture) => {
                          if (texture != null)
                          {
                              textureImage.GetComponentInParent<Mask>().enabled = true;
                              textureImage.texture = texture;
                              textureImage.enabled = true;
                          }
                          else
                          {
                              textureImage.GetComponentInParent<Mask>().enabled = false;
                              textureImage.enabled = false;
                              textureImage.texture = null;
                          }
                      });
                }else{
                   textureImage.GetComponentInParent<Mask>().enabled = true;
                   textureImage.texture = info.immagine_di_copertinaTexture;
                   textureImage.enabled=true;
                }
            }
        }
    }

}
