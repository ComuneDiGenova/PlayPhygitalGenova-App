using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeFavourite : MonoBehaviour
{
    /// <summary>
    /// RectTransform rectTransform;
    /// Left rectTransform.offsetMin.x;
    /// Right rectTransform.offsetMax.x;
    /// Top rectTransform.offsetMax.y;
    /// Bottom rectTransform.offsetMin.y;
    /// </summary>

    [SerializeField] GameObject gridBoxPreferiti;

    float gridChildCount;
    float gridRow;

    //float[] gridRowsSize = { 240, 480, 980, 1375, 1770, 2140, 2560 };
    
    public void ResizeGridFavourite()
    {
        gridChildCount = gridBoxPreferiti.transform.childCount;
        gridRow = gridChildCount;

        if (gridRow >= 5)
        {
            gridRow -= 5;

            float offsetY = 0; //= gridRowsSize[(int)gridRow];

            for (int i = 0; i <= gridRow; i++)
            {
                offsetY += 230.0f;
            }

            gridBoxPreferiti.GetComponent<RectTransform>().offsetMin = new Vector2(gridBoxPreferiti.GetComponent<RectTransform>().offsetMin.x, -offsetY);
        }
    }

}
