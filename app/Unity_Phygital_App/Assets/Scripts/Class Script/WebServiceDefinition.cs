using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Globalization;
using GeoCoordinate;
using static OnlineMapsGPXObject;

//DELEGATI//

public delegate void VoidDelegate();

// //////////////////////////////////////////////////// //
// ////////////////////// UTENTE ////////////////////// //
// //////////////////////////////////////////////////// //
[System.Serializable]
public class InfoUser
{
    public string codice_utente;
    public string nome;
    public string cognome;
    public string email;   
    public string genovini;
    public string ruoli;
    public string[] ruoloTag = new string[0]; // "Amministatore,Visitatore,Esercente"
    public string lingua;
    public string sesso;
    public string nazionalita;
    public string interessi;
    public string tipologia;
    public string privacy;
    public string consenso_email_dati_acquisto;
    public string consenso_email_esercenti_phygital;
    public string consenso_messaggio_appio;
    public string consenso_email_comune;
    public string consenso_email_partecipate_comune;
    public string consenso_email_eventi_comune;
    public string consenso_email_eventi_esercenti;
    public string consenso_email_notizie_servizio;

    // ??????????????????????? //
    public string preferiti;
    public string preferiti_categorie_negozio;
    public string spostamento_da; //???????????????????
    public string spostamento_in; //???????????????????

    public void EvalRuoli(){
        ruoloTag = ruoli.Split(",");
    }

    public override string ToString()
    {
        return $"{codice_utente}, {nome}, {cognome}, {email}, {ruoli}, {genovini}, {lingua}, {sesso}, {nazionalita}, {interessi}, {tipologia}";
    }
}

// POST
[System.Serializable]
public class AddPoint
{
    public string user_id;
    public string action;
    public string content_type;
    public string content_id;
    public string data_scontrino; // swagger data scontrino
    public string importo; // swagger segnato come number
    public string numero_scontrino;
}

// GET
[System.Serializable]
public class ResponseAddPoint
{
    public bool result;
    public string message;
    public int points;

    public override string ToString()
    {
        return $"{result}, {message}, {points}";
    }
}

[System.Serializable]
public class UserFavourite
{
    public string id;
    public string nome;
    public string tipo;
    public string categoria;

    public override string ToString()
    {
        return $"{id}, {nome}, {tipo}, {categoria}";
    }
}

[System.Serializable]
public class FavouriteList
{
    public List<UserFavourite> favourites = new List<UserFavourite>();
}

[System.Serializable]
public class AddFavouriteResponse
{
    public bool result;
    public string message;
}

public class RemoveFavouriteResponse
{
    public bool result;
    public string message;
}

// //////////////////////////////////////////////////// //
// ///////////////// FINE UTENTE ////////////////////// //
// //////////////////////////////////////////////////// //

// //////////////////////////////////////////////////// //
// //////////////////// INFO POI  ///////////////////// //
// //////////////////////////////////////////////////// //

[System.Serializable]
public class Info
{
    public string id;
    public string nome;
    public string accessibilita;
    public int agevolazioni;
    public string audio;
    public int avatar;  
    public string cellulare;
    public string coordinate;
    public string descrizione_audio;
    public string email;
    public string indirizzo;
    public string orari;
    public string servizi;
    public string tags;
    public string telefono;
    public string descrizione;
    public string tipologia;
    public string immagini360;
    public string gallery;
    public List<string> gallery_list = new List<string>();
    public string immagine_di_copertina;
    public string url;

    public Languages language = Languages.IT;  //settata da doi durante la call per controllare se dettaglio va riscaricato

    [System.NonSerialized] public Texture2D immagine_di_copertinaTexture;

    public override string ToString()
    {
        return $"{id}, {nome}, {coordinate}, {descrizione}, {cellulare}, {coordinate}, {email}";
    }
}

[System.Serializable]
public class InfoList
{
    public List<Info> infos = new List<Info>();
}

[System.Serializable]
public class ShortInfo : BaseShortInfo
{
    public override string ToString()
    {
        return $"PoiShortInfo: {id}, {lon}, {lat}, {id_tipologia}, {tipologia}, {tipo}";
    }  
}

[System.Serializable]
public class InformationList
{
    public List<ShortInfo> infos = new List<ShortInfo>();

    public void PurgeNullCoordinate()
    {
        var purgedList = new List<ShortInfo>();

        foreach (var p in infos)
        {
            if (p.lon != 0 && p.lat != 0)
            {
                purgedList.Add(p);
            }
        }

        infos = purgedList;
    }

    public void PurgeBotteghe()
    {
        var purgedList = new List<ShortInfo>();

        foreach (var p in infos)
        {
            if (p.tipo == "poi")
            {
                purgedList.Add(p);
            }
        }

        infos = purgedList;
    }
}

// //////////////////////////////////////////////////// //
// //////////////// FINE INFO POI ///////////////////// //
// //////////////////////////////////////////////////// //

[System.Serializable]
public class BaseShortInfo
{
    public string id;
    public string title;
    public string tipologia; // shops // "Bottega Storica", "Locali di tradizione", "Negozi" , "Esercizi di pregio"
    public string id_tipologia; // pois
    public string tipologia_img_icona; // pois
    public string tipo; // pois // "poi","negozio",
    public double lat;
    public double lon;

    protected GeoCoordinate.Coordinate coordinate = null;

    public GeoCoordinate.Coordinate ToCoordinate()
    {
        
        if (coordinate == null && (lat != 0 && lon != 0))
        {
            coordinate = new GeoCoordinate.Coordinate(lat, lon);
        }
        return coordinate;
    }

    public override string ToString()
    {
        return $"BaseShortInfo: {id}, {title}, {tipologia},{lon}, {lat}";
    }
}

// //////////////////////////////////////////////////// //
// ////////////////// INFO SHOP /////////////////////// //
// //////////////////////////////////////////////////// //

[System.Serializable]
public class ShopShortInfo : BaseShortInfo
{
    public override string ToString()
    {
        return $"ShopShortInfo: {id}, {title}, {tipologia},{lon}, {lat}";
    }
}

[System.Serializable]
public class ShopInfoList
{
    public List<ShopShortInfo> shopInfos = new List<ShopShortInfo>();

    public void PurgeNullCoordinate()
    {
        var purgedList = new List<ShopShortInfo>();

        foreach (var s in shopInfos) 
        {
            if (s.lon != 0 && s.lat != 0)
            {
                purgedList.Add(s);
            }
        }

        shopInfos = purgedList;
    }

    public void SetType(){
        foreach (var s in shopInfos) 
        {
            s.tipo = "negozio";
        }
    }
}

[System.Serializable]
public class ShopType
{
    public string id;
    public string nome;
}

[System.Serializable]
public class ShopInformaitions
{
    public string id;
    public string nome;
    public double latitudine;
    public double longitudine;
    public ShopType categoria_del_negozio;
    public string cellulare;
    public string email;
    public string indirizzo;
    public string orari;
    public string tags;
    public string telefono;
    public string descrizione;
    public string tipologia;
    public string immagine_di_copertina;
    public string immagini360;
    public string immagini_gallery;
    public string audio;
    public string url;

    public Languages language = Languages.IT;  //settata da doi durante la call per controllare se dettaglio va riscaricato

    [System.NonSerialized] public Texture2D immagine_di_copertinaTexture;
}

[System.Serializable]
public class ShopInformationsList
{
    public List<ShopInformaitions> shopInfoList = new List<ShopInformaitions>();
}


// //////////////////////////////////////////////////// //
// //////////////// FINE INFO SHOP //////////////////// //
// //////////////////////////////////////////////////// //

// //////////////////////////////////////////////////// //
// /////////////////// INFO TIPO ////////////////////// //
// //////////////////////////////////////////////////// //

[System.Serializable]
public class TypeInfo
{
    public string id;
    public string nome;
    public string icona;
}

[System.Serializable]
public class TypeList
{
    public List<TypeInfo> typeList = new List<TypeInfo>();
}

// //////////////////////////////////////////////////// //
// ///////////////// FINE INFO TIPO /////////////////// //
// //////////////////////////////////////////////////// //

// //////////////////////////////////////////////////// //
// /////////////// INFO ITINERARIO //////////////////// //
// //////////////////////////////////////////////////// //
/*
[System.Serializable]
public class Itinerary
{
    public string id;
    public string title;
    public int predefinito;
}

[System.Serializable]
public class ItineraryDetails
{
    public string id;
    public string nome;
    public string sferiche;
    public string url;
}
*/
[System.Serializable]
public class InfoJS // questo ï¿½ il valore salvato nella pagina javascript che l'app WebGL va a LEGGERE
{
    public ItinerarioJS itinerario;
}

[System.Serializable]
public class ItinerarioJS // Classe necessaria per costruire il percorso nella app360 WebGL e che va salvata nelle API 
{
    public string nome;
    public double id;
    public int lingua;
    public string rgb;
    public List<PoiJS> lista_poi;

    public override string ToString() => $"ItinerarioJs {id} {nome} {lista_poi.Count}";
}

[System.Serializable]
public class PoiJS
{
    public string id_poi;
}
/*
[System.Serializable]
public class ItineraryList
{
    public List<Itinerary> itineraryList = new List<Itinerary>();
}
*/
[System.Serializable]
public class ItinerarioDettaglio // questa ï¿½ la classe riportata dall dettaglio dell'itinerario 
{
    public string id;
    public string nome;
    public string url;
    public string descrizione;
    public string sferiche; // Questo qua salviamo il nostro itineraruio JS in formato JSON
    
}

[System.Serializable]
public class ItinerarioShort// questa ï¿½ la classe dentro listaItinerari dalle API 
{
    public int predefinito;
    public string id;
    public string title;
}

[System.Serializable]
public class ListaItinerari// questa ï¿½ la classe riportata dalla lista degli itinerari delle API 
{
    public List<ItinerarioShort> itinerari = new List<ItinerarioShort>(); 
}

[System.Serializable]
public class ListaItinerariDettaglio// questa ï¿½ la classe riportata dalla lista degli itinerari delle API 
{
    public List<ItinerarioDettaglio> itinerario = new List<ItinerarioDettaglio>();
}

// //////////////////////////////////////////////////// //
// ////////////// FINE INFO ITINERARIO //////////////// //
// //////////////////////////////////////////////////// //

// //////////////////////////////////////////////////// //
// ///////////////// ACCREDITO PUNTI ////////////////// //
// //////////////////////////////////////////////////// //      

[System.Serializable]
public class AddUserPoint
{

}

// //////////////////////////////////////////////////// //
// ////////////// FINE ACCREDITO PUNTI //////////////// //
// //////////////////////////////////////////////////// //

// //////////////////////////////////////////////////// //
// /////////////////// PAGINE HELP //////////////////// //
// //////////////////////////////////////////////////// //

[System.Serializable]
public class HelpInfo
{
    public int id;
    public string titolo;
    public string descrizione;
    public string immagine_di_copertina;

    public override string ToString() { return $"Hepl: {id} {titolo} {immagine_di_copertina}"; }
}

[System.Serializable]
public class HelpInfoList
{
    public List<HelpInfo> helpList = new List<HelpInfo>();
}

// //////////////////////////////////////////////////// //
// //////////////// FINE PAGINE HELP ////////////////// //
// //////////////////////////////////////////////////// //



////////////////////// TRANSAZIONI ////////////////////

[System.Serializable]
public class TransactionList
{
    public List<Transaction> transazioni = new List<Transaction>();
}
[System.Serializable]
public class Transaction
{
    public string id;
    public string title;
    public string genovini;
    public string dettaglio;
}

//////////////////////////// GENERICO ERROR RESULT ////////////////

[System.Serializable]
public class Result{
    public bool result;
    public string message;
}


///////////////////////////////////////////////////////////////////////







/*

[
    {
        "id": "419",
        "nome": "Attrazioni"
    },
    {
        "id": "420",
        "nome": "Biblioteche"
    },
    {
        "id": "35",
        "nome": "Botteghe\nStoriche"
    },
    {
        "id": "186",
        "nome": "Forti"
    },
    {
        "id": "189",
        "nome": "I parchi e le\nville"
    },
    {
        "id": "38",
        "nome": "Mare"
    },
    {
        "id": "417",
        "nome": "Monumenti e luoghi sacri"
    },
    {
        "id": "53",
        "nome": "Mura e\nforti"
    },
    {
        "id": "14",
        "nome": "Musei"
    },
    {
        "id": "131",
        "nome": "Outdoor"
    },
    {
        "id": "188",
        "nome": "Palazzi dei Rolli - Patrimonio\nUnesco"
    },
    {
        "id": "418",
        "nome": "Parchi, ville e orti botanici"
    },
    {
        "id": "421",
        "nome": "POI360"
    },
    {
        "id": "134",
        "nome": "Punti\npanoramici"
    },
    {
        "id": "15",
        "nome": "Quartieri"
    },
    {
        "id": "54",
        "nome": "Sport"
    },
    {
        "id": "132",
        "nome": "Storia e\ntradizioni"
    },
    {
        "id": "52",
        "nome": "Teatri"
    }
]

*/