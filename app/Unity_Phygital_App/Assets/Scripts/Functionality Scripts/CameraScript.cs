using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using System.Threading.Tasks;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class CameraScript : MonoBehaviour
{
    [SerializeField] UIManager managerUIScript;
    [SerializeField] QRCodeReader readerQRScript;
    [SerializeField] Button closeButton;

    WebCamTexture webCam;

    [SerializeField] RawImage cameraQRImage;

    bool isCameraActive;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        isCameraActive = false;
    }


    async Task<bool> CheckCameraPermission()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
            AlertPanel.OpenAlert("I permessi della camera sono necessari per utilizzare l'applicazione", "ID_Camera");
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
            await Task.Yield();
            return Application.HasUserAuthorization(UserAuthorization.WebCam);
        }else{
            return true;
        }
#endif
#if UNITY_ANDROID  && !UNITY_EDITOR
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
                AlertPanel.OpenAlert("I permessi della camera sono necessari per utilizzare l'applicazione", "ID_Camera");
                Permission.RequestUserPermission(Permission.Camera);
                await Task.Yield();
                return Permission.HasUserAuthorizedPermission(Permission.Camera);
        }else{
            return true;
        }
#endif
        return true;
    }

    /////////////////////////////////////////////////////////////////
    // --------------------------LETTURA QR----------------------- //
    /////////////////////////////////////////////////////////////////

    public async void ActiveQRCamera()
    {
        closeButton.interactable = false;
        Invoke("ResetClose",3);
        cameraQRImage.texture = null;
        cameraQRImage.GetComponent<AspectRatioFitter>().aspectRatio = 16f/9;
        if (await CheckCameraPermission())
        {
            GetCamDevice();

            if (!isCameraActive && webCam != null)
            {
                webCam.Play();
                isCameraActive = true;
                cameraQRImage.texture = webCam;
                cameraQRImage.GetComponent<AspectRatioFitter>().aspectRatio = webCam.width * 1f / webCam.height;
            }
            else
            {
                Debug.LogError("NO CAMERA");
            }
        }
    }

    void ResetClose(){
        closeButton.interactable = true;
    }

    private void Update() {
        if (isCameraActive && webCam.didUpdateThisFrame)
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                // decode the current frame
                var result = barcodeReader.Decode(webCam.GetPixels32(), webCam.width, webCam.height);
                if (result != null)
                {
                    Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                    StopCamera();

                    readerQRScript.ReadQR(result.Text);

                    managerUIScript.CallCloseButton();
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
    }

    /////////////////////////////////////////////////////////////////
    // --------------------------LETTURA QR----------------------- //
    /////////////////////////////////////////////////////////////////

    public void Close(){
        StopCamera();
        //managerUIScript.CallCloseButton();
    }


    public void StopCamera()
    {
        if (isCameraActive)
        {
            webCam.Stop();
            webCam = null;
            isCameraActive = false;
        }
    }

    void GetCamDevice()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.LogWarning("Number of web cams connected: " + devices.Length);

        foreach (WebCamDevice device in devices)
        {
            Debug.LogWarning("camera: " + device.name);

            if (!device.isFrontFacing || device.kind == WebCamKind.WideAngle)
            {
                webCam = new WebCamTexture(device.name, 1280, 720);
                webCam.filterMode = FilterMode.Bilinear;
                if (webCam != null) return;
            }
        }
#if UNITY_EDITOR
        webCam = new WebCamTexture(1280, 720);
        webCam.filterMode = FilterMode.Bilinear;
#endif
    }
}
