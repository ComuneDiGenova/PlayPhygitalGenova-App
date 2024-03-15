using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeNearMe : MonoBehaviour
{

    /// <summary>
    /// RectTransform rectTransform;
    /// Left rectTransform.offsetMin.x;
    /// Right rectTransform.offsetMax.x;
    /// Top rectTransform.offsetMax.y;
    /// Bottom rectTransform.offsetMin.y;
    /// </summary>

    [SerializeField] GameObject gridBoxStorico;
    [SerializeField] GameObject gridBoxNegozio;
    [SerializeField] GameObject gridBoxBottega;

    float gridChildCount;
    float gridRow;

    float[] gridRowsSize = { 190, 580, 980, 1375, 1770, 2140, 2560 };


    public void ResizeAllGrids()
    {
        ResizeGridStorico();
        ResizeGridNegozio();
        ResizeGridBottega();
    }

    public void ResizeGridStorico()
    {
        gridChildCount = gridBoxStorico.transform.childCount;
        gridRow = gridChildCount / 3;

        if (gridChildCount % 3 != 0)
        {
            gridRow += 1;
        }

        if (gridRow >= 4)
        {
            gridRow -= 4;
            float offsetY = gridRowsSize[(int)gridRow];
            gridBoxStorico.GetComponent<RectTransform>().offsetMin = new Vector2(gridBoxStorico.GetComponent<RectTransform>().offsetMin.x, -offsetY);
        }
    }

    public void ResizeGridNegozio()
    {
        gridChildCount = gridBoxNegozio.transform.childCount;
        gridRow = gridChildCount / 3;

        if (gridChildCount % 3 != 0)
            gridRow += 1;
        
        if (gridRow >= 4)
        {
            gridRow -= 4;
            float offsetY = gridRowsSize[(int)gridRow];
            gridBoxNegozio.GetComponent<RectTransform>().offsetMin = new Vector2(gridBoxNegozio.GetComponent<RectTransform>().offsetMin.x, -offsetY);
        }
    }

    public void ResizeGridBottega()
    {
        gridChildCount = gridBoxBottega.transform.childCount;
        gridRow = gridChildCount / 3;

        if (gridChildCount % 3 != 0)
            gridRow += 1;

        if (gridRow >= 4)
        {
            gridRow -= 4;
            float offsetY = gridRowsSize[(int)gridRow];
            gridBoxBottega.GetComponent<RectTransform>().offsetMin = new Vector2(gridBoxBottega.GetComponent<RectTransform>().offsetMin.x, -offsetY);
        }
    }
}
   

