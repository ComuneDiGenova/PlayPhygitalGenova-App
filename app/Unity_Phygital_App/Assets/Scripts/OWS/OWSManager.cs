using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace OWS {
    public class OWSManager
    {
        public const string geoportalEndpoint = "https://mappe.comune.genova.it/geoserver/SITGEO/ows";  // TO REMOVE
        
        //lista features
        //https://mappe.comune.genova.it/geoserver/wms?service=wms&version=1.3.0&request=GetCapabilities
        //esempio
        //https://mappe.comune.genova.it/geoserver/SITGEO/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=SITGEO:V_BAGNI&outputFormat=json&srsName=CRS:84



        static async Task<Root> GetFeatures(string featuresCode){
            LoadingPanel.OpenPanel();
            string url = geoportalEndpoint + $"?service=WFS&version=1.0.0&request=GetFeature&typeName={featuresCode}&outputFormat=json&srsName=CRS:84";
            Debug.Log(url);
            using(UnityWebRequest uwr = UnityWebRequest.Get(url)){
                //uwr.timeout = 10;
                uwr.SendWebRequest();
                while(!uwr.isDone){
                    await Task.Yield();
                }
                if(uwr.result != UnityWebRequest.Result.Success){
                    Debug.LogError($"{url} | {uwr.result} | {uwr.error}");
                    if(uwr.result == UnityWebRequest.Result.ConnectionError)
                        WSO2.NoConnectionError(false);
                    LoadingPanel.ClosePanel();
                    return null;
                }else{
                    string json = uwr.downloadHandler.text;
                    //Debug.Log(json);
                    try{
                        Root obj = JsonUtility.FromJson<Root>(json);
                        LoadingPanel.ClosePanel();
                        return obj;
                    }catch(Exception ex){
                        Debug.LogError(ex.Message);
                        LoadingPanel.ClosePanel();
                        return null;
                    }
                }
            }
        }

        public static async Task<List<NTPoi>> GetOWSFeatures(FeatureType featureType, Action<List<NTPoi>> callBack){
            Debug.Log("NTPOI: " + featureType.ToString());
            string featureName = "SITGEO:";
            switch(featureType){
                case FeatureType.Bagni:
                    featureName += "V_BAGNI";
                    break;
                case FeatureType.BikeSharing:
                    featureName += "V_MOB_PARKS_BIKESHARING";
                    break;
                case FeatureType.Autobus:
                    featureName += "V_MOB_FERMATE_AMT";
                    break;
                default:
                    break;
            }
            var root = await GetFeatures(featureName);
            if(root == null) return null;
            List<NTPoi> ntlist = new List<NTPoi>();
            foreach(var f in root.features){
                var npoi = new NTPoi(f,featureType);
                ntlist.Add(npoi);
            }
            if(ntlist.Count > 0){
                callBack?.Invoke(ntlist);
                return ntlist;
            }else
                return null;
        }
    }
}