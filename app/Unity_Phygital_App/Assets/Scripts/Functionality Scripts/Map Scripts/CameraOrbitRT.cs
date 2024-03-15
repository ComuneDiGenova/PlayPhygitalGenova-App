#if (!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR
#define USE_MOUSE_ROTATION
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[OnlineMapsPlugin("Camera Orbit", typeof(OnlineMapsControlBaseDynamicMesh), true)]
public class CameraOrbitRT : OnlineMapsCameraOrbit
{

    protected override void Start(){}
    
    protected override void OnEnable() {
        _instance = this;
    }

    protected override void Update(){}

    protected override void LateUpdate() {
        UpdateCameraRotation();
    }

    private void UpdateCameraRotation()
    {
        transform.rotation = Quaternion.Euler(0,0,rotation.y);;
    }

    public static void ResetRotation(){
        _instance.rotation = Vector2.zero;
    }
}
