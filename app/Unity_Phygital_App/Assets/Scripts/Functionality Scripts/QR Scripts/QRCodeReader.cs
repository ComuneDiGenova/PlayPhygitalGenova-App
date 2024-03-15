using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QRCodeReader : MonoBehaviour
{
    [SerializeField] TMP_InputField userCode;

    public void ReadQR(string qrString)
    {
        userCode.text = qrString;
    }
}
