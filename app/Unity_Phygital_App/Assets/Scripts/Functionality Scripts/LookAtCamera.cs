using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool flatXZLook = false;
    public Camera targetCamera;

    void Update()
    {
        Vector3 dir;
        if(targetCamera == null)
            dir = Camera.main.transform.position - transform.position;
        else
            dir = targetCamera.transform.position - transform.position;
        //
        if(flatXZLook){
            dir.y = 0;
        }
        dir.Normalize();
        if(dir.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}
