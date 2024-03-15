using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionDataHandler : MonoBehaviour
{
    [SerializeField] int maxTransaction = 5;
    [SerializeField] RectTransform transactionParent;
    [SerializeField] TMPro.TextMeshProUGUI transactionEntryPrefab;

    List<GameObject> entries = new List<GameObject>();

    public void OpenTransaction(){
        foreach(var e in entries){
            Destroy(e);
        }
        entries.Clear();
        GETUserInfo.GetLastTransactions((transactions)=>{
            int nt = 0;
            foreach(var tran in transactions){
                nt++;
                if(nt>maxTransaction) break;
                var t = Instantiate(transactionEntryPrefab.gameObject,transactionParent);
                var tt = t.GetComponent<TMPro.TextMeshProUGUI>();
                tt.text = $"{tran.title}: <b>{tran.genovini}</b>\n{tran.dettaglio}";
                entries.Add(t);
            }
        });
    }
}
