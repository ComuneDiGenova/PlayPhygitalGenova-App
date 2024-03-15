using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legenda : MonoBehaviour
{
    [SerializeField] private GameObject CTNlegenda;

    public void CloseLegenda()
    {
        CTNlegenda.SetActive(false);
    }

    public void ButtonLegenda()
    {
        if (CTNlegenda.activeSelf)
        {
            CTNlegenda.SetActive(false);
        }
        else CTNlegenda.SetActive(true);
    }
 
}
