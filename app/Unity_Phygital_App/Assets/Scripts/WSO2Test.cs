using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSO2Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        WSO2.GetUserId((uid)=>{
            Debug.Log(uid);
        });
        */
        WSO2.GET("/jsonapi/get_poi_list", (json) => {
            //
        });
        WSO2.GET("/jsonapi/user-get-info/"+(16892599758).ToString(),(uid)=>{
            Debug.Log(uid);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
