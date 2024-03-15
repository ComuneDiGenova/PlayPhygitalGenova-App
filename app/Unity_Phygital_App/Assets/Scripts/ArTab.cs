using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArTab : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject PreviewTabAR;

    public void ApriChiudiPopUp()
    {
        Animator animator = PreviewTabAR.GetComponent<Animator>();
        bool booleana = animator.GetBool("Open");
        if (booleana)
        {
            Debug.Log("Ho chiuso il popUp");
            animator.Play("TabArClose");
            animator.SetBool("Open", false);
        }
        else
        {
            Debug.Log("Ho aperto il popUp");
            animator.Play("TabArOpen");
            animator.SetBool("Open", true);
        }
    }
    
}
