using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class Resetter : MonoBehaviour
{
    [SerializeField] ARSession session;
    [SerializeField] ARSessionOrigin origin;
    ARAnchor anchor;
    Vector3 position;

    void Awake(){
        position = transform.localPosition;
    }

    void OnEnable(){
        //Invoke("Reset",0.5f);
        origin.transform.position = Vector3.zero;
        //anchor = gameObject.AddComponent<ARAnchor>();
        //anchor.destroyOnRemoval = false;
        transform.localPosition = position;
    }

    void Reset(){
        Debug.LogWarning("Reset");
        session.Reset();
    }

    void OnDisable(){
        //Destroy(anchor);
        //anchor = null;
        transform.localPosition = position;
    }

}
