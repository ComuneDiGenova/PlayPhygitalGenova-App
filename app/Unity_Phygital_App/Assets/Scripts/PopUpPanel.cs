using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpPanel : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] RectTransform parent;

    static PopUpPanel instance;
    private void Awake() {
        instance = this;
        prefab.SetActive(false);
    }

    //static List<GameObject> openPopUps = new List<GameObject>();

    public static void Open(string message, float visibilityTime = 10){
        var p = Instantiate(instance.prefab,instance.parent);
        p.SetActive(true);
        var pc = p.GetComponentInChildren<PopUpPrefab>();
        pc.SetText(message);
        instance.StartCoroutine(instance.CloseDelay(pc,visibilityTime));
    }

    public static void OpenLanguage(string IDKeyLanguage, bool replaceValue = false, string message = null, float visibilityTime = 10){
        var p = Instantiate(instance.prefab,instance.parent);
        p.SetActive(true);
        var pc = p.GetComponentInChildren<PopUpPrefab>();
        var text = CSVParser.GetTextFromID(IDKeyLanguage, (int)GameConfig.applicationLanguage);
        if(replaceValue)
            text = text.Replace("*", message);
        else
            text = message + text;
        pc.SetText(text);
        instance.StartCoroutine(instance.CloseDelay(pc,visibilityTime));
    }

    IEnumerator CloseDelay(PopUpPrefab p, float delay){
        yield return new WaitForSeconds(delay);
        Close(p);
    }

    public static void Close(PopUpPrefab popup){
        if(popup == null) return;
        popup.Close();
    }
    

}
