using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class BackClose : MonoBehaviour
{
    
    UnityEngine.UI.Button button;
    private void Awake() {
        button = GetComponent<UnityEngine.UI.Button>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            button.onClick?.Invoke();
        }
    }
}
