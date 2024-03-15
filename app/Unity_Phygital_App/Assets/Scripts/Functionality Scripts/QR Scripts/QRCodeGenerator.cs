using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing.QrCode;
using ZXing;
using System;

public class QRCodeGenerator : MonoBehaviour
{
    [SerializeField] RawImage qrImage;

    Texture2D encodedTexture;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        encodedTexture = new Texture2D(256, 256);
        EncodeTextToQR();
    }

    Color32[] Encode(string text, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width,
            }
        
        };

        return writer.Write(text);
    }

    void EncodeTextToQR()
    {
        string textWrite = string.IsNullOrEmpty(GameConfig.userID) ? "Nessun dato disponibile" : GameConfig.userID.ToString();

        Color32[] convertToTexture = Encode(GameConfig.userID, encodedTexture.width, encodedTexture.height);
        encodedTexture.SetPixels32(convertToTexture);
        encodedTexture.Apply();

        qrImage.texture = encodedTexture;
    }
}

