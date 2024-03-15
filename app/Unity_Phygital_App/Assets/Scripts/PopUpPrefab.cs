using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpPrefab : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text message_text;
    [SerializeField] float destroydelay = 2;

    public void SetText(string message){
        gameObject.GetComponent<Animator>().Play("Punti");
        message_text.text = message;
    }

    public void Close(){
        //METTERE CHIAMATA ANIMAZIONE
        Destroy(gameObject,destroydelay);
    }
}
