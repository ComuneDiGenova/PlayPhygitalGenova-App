using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARLight : MonoBehaviour
{
    [SerializeField] ARCameraManager camManager;
    [SerializeField] Light mainLight;
    [SerializeField] float luminosity = 1.1f;

    private void Awake() {
        camManager.frameReceived += OnFrame;
    }

    void OnFrame(ARCameraFrameEventArgs args){
        if(args.lightEstimation.mainLightDirection.HasValue)
            mainLight.transform.rotation = Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);
        if (args.lightEstimation.mainLightIntensityLumens.HasValue)
            mainLight.intensity = args.lightEstimation.mainLightIntensityLumens.Value;
        if (args.lightEstimation.averageBrightness.HasValue)
            mainLight.intensity = args.lightEstimation.averageBrightness.Value;
        if (args.lightEstimation.averageColorTemperature.HasValue)
            mainLight.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
        if (args.lightEstimation.colorCorrection.HasValue)
            mainLight.color = args.lightEstimation.colorCorrection.Value;
    }
}
