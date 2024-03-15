using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data.Common;
using System.Threading.Tasks;
using System.Drawing;

public class FakeInjector : MonoBehaviour
{
    [SerializeField] bool InjectFakeData = false;
    [SerializeField] int avatar=0;
    [SerializeField] string audio = "";
    [SerializeField] ShortInfo[] poiShorts = new ShortInfo[2]{
        new ShortInfo(){
            id="FP1",
            title="FakePoi",
            lat=0,
            lon=0,
            id_tipologia="417",
            tipo="poi"
        },new ShortInfo(){
            id="FP2",
            title="FakePoi",
            lat=0,
            lon=0,
            id_tipologia="35",
            tipo="poi"
        }
    };

    List<Info> poiInfos = new List<Info>();

    ItinerarioJS itinerarioJs;
    ItinerarioDettaglio itinerarioDett;
    [SerializeField] ItinerarioShort itinerario = new ItinerarioShort(){
        predefinito = 0,
        id = "FI",
        title = "FakeItinerario"
    };

    private void Awake() {
        if(!InjectFakeData)
            return;
        Init();
        GETPointOfInterest.OnDownloadedInfos+= () => {
            foreach(var i in poiShorts){
                GETPointOfInterest.DownloadedInformationPois.infos.Add(i);
            }
            /*
            foreach(var i in poiShorts){
                var info = poiInfos.Where(x=>x.id == i.id).FirstOrDefault();
                GETPointOfInterest.DownloadedExtendedInfo.Add(i,info);
            }
            */
            Debug.LogWarning("POI Injected");
        };
        
        ItinerariesManager.OnDownloadedItineraries += () => {
            ItinerariesManager.DownloadedItineraries.itinerari.Add(itinerario);
            ItinerariesManager.DownloadedDettagli.Add(itinerario,itinerarioDett);
            Debug.LogWarning("Itinerario Injected");
        };
        
         
        GameConfig.OnLanguageChange += (l) => {
            foreach(var i in poiShorts){
                var info = poiInfos.Where(x=>x.id == i.id).FirstOrDefault();
                if(GETPointOfInterest.DownloadedExtendedInfo.ContainsKey(i))
                    GETPointOfInterest.DownloadedExtendedInfo[i] = info;
                else
                    GETPointOfInterest.DownloadedExtendedInfo.Add(i,info);
            }
        };
    }

    void Init(){
        //creo infodett
        foreach(var i in poiShorts){
            poiInfos.Add(new Info(){
                id = i.id,
                nome = i.title,
                avatar = avatar,
                audio = audio
            });
        };
        //creo itinerario
        itinerarioJs = new ItinerarioJS(){
            nome = itinerario.title,
            id=9999,
            lista_poi = new List<PoiJS>()
        };
        foreach(var i in poiShorts){
            itinerarioJs.lista_poi.Add(new PoiJS(){id_poi = i.id});
        }
        itinerarioDett = new ItinerarioDettaglio(){
            id=itinerario.id,
            nome = itinerario.title,
            sferiche = JsonUtility.ToJson(itinerarioJs)
        };
    }
}
