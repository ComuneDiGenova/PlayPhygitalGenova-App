using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetTipologiePOI : MonoBehaviour
{
    public static event VoidDelegate OnTipologiePoiTrovate;
    [SerializeField] private GameObject ctnParent;
    [SerializeField] private GameObject tipologiaPOIPrefab;
    List<GameObject> poitypes = new List<GameObject>();
    Sprite spriteImgPrincipale;
    [SerializeField] Texture2D PoiSprite;
    public static Dictionary<string, Sprite> idToSprite = new Dictionary<string, Sprite>();
    private void Awake()
    {
        GETPointOfInterest.OnDownloadedType += IstanzioPunti;

    }

    private void IstanzioPunti()
    {
        Debug.Log("lista di TypePois : "+ GETPointOfInterest.DownloadedTypePois.typeList.Count);
        //StartCoroutine(IstanzioTipologiePoi());
        IstanzioTipologiePoi();
    }

    private async void IstanzioTipologiePoi()
    {
        //LoadingPanel.OpenPanel();
        foreach(var pt in poitypes){
            Destroy(pt);
        }
        poitypes.Clear();
        LoadingPanel.OpenPanel();
        foreach(var elemento in GETPointOfInterest.DownloadedTypePois.typeList)
        {
            GameObject newPrefab = Instantiate(tipologiaPOIPrefab);
            poitypes.Add(newPrefab);
            newPrefab.transform.SetParent(ctnParent.transform);
            newPrefab.transform.localScale = new Vector3(1, 1, 1);
            newPrefab.GetComponentInChildren<TMP_Text>().text = elemento.nome;
            string url = elemento.icona;

            if (string.IsNullOrEmpty(url))
            {
                Debug.Log("la stringa dell'icona è nulla");
                Texture2D texture = PoiSprite;
                spriteImgPrincipale = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                newPrefab.transform.GetChild(1).GetComponent<Image>().sprite = spriteImgPrincipale;
                AddSprite(elemento.id, spriteImgPrincipale);
                Debug.Log(" id categoria : " + elemento.id);
                continue;
            }

            bool wait = true;
            WSO2.GETImage(url, texture =>
            {
                if (texture != null)
                {
                    //TextureScale.Scale(ref texture,64,64);
                    TextureScale.ScaleGPU(ref texture,64,64);
                    texture.filterMode = FilterMode.Bilinear;
                    texture.anisoLevel = 2;
                    texture.Apply();
                    spriteImgPrincipale = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    newPrefab.transform.GetChild(1).GetComponent<Image>().sprite = spriteImgPrincipale;
                }
                wait = false;

            }, true);
            //yield return new WaitWhile(() => wait);
            while(wait){
                await System.Threading.Tasks.Task.Yield();
            }
            //aggiungo un elemento alla mia dictionary delle categorie
            AddSprite(elemento.id, spriteImgPrincipale);
            Debug.Log(" id categoria : "+elemento.id);
        }
        //Debug.Log("Numero di elementi nel dictionary: " + idToSprite.Count);
        OnTipologiePoiTrovate?.Invoke();
        LoadingPanel.ClosePanel();
    }

    // Aggiungi una nuova entry (id, sprite) nel dictionary
    public static void AddSprite(string id, Sprite sprite)
    {
        idToSprite[id] = sprite;
    }

    // Restituisci la sprite associata a un determinato ID
    public static Sprite GetSprite(string id)
    {
        if (idToSprite.ContainsKey(id))
        {
            return idToSprite[id];
        }
        else
        {
            Debug.LogWarning("ID non trovato nel dictionary: " + id);
            return null;
        }
    }
}


