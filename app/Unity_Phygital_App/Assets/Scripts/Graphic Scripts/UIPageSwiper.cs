using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 panelLocation;

    RectTransform rect;

    int currentPage = 1;

    [SerializeField] float percentThreshold = 0.2f;
    [SerializeField] float easing = 0.5f;

    [SerializeField] RectTransform[] pages = new RectTransform[0];

    public Button nextPageButton;
    public Button prevPageButton;
    public GameObject IMG_POI_Selettore;
    public GameObject IMG_Negozi_Selettore;
    // Start is called before the first frame update

    void Start()
    {
        rect = GetComponent<RectTransform>();
        panelLocation = rect.anchoredPosition;
        nextPageButton.onClick.AddListener(NextPage);
        prevPageButton.onClick.AddListener(PrevPage);
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].anchoredPosition = new Vector2(Screen.width * i,pages[i].anchoredPosition.y);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        rect.anchoredPosition = panelLocation - new Vector2(difference, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;

        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector2 newLocation = panelLocation;

            if (percentage > 0 && currentPage < pages.Length)
            {
                newLocation += new Vector2(-Screen.width, 0);
                currentPage++;
                IMG_Negozi_Selettore.SetActive(true);
                IMG_POI_Selettore.SetActive(false);
                
            }
            else if (percentage < 0 && currentPage > 1)
            {
                newLocation += new Vector2(Screen.width, 0);
                currentPage--;
                IMG_Negozi_Selettore.SetActive(false);
                IMG_POI_Selettore.SetActive(true);
            }

            StartCoroutine(SmoothMove(rect.anchoredPosition, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(rect.anchoredPosition, panelLocation, easing));
        }
    }

    IEnumerator SmoothMove(Vector2 startpos, Vector2 endpos, float seconds)
    {
        float t = 0f;

        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            rect.anchoredPosition = Vector2.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        rect.anchoredPosition = endpos;
    }

    private void NextPage()
    {
        if (currentPage < pages.Length)
        {
            Vector3 newLocation = panelLocation + new Vector2(-Screen.width, 0);
            StartCoroutine(SmoothMove(rect.anchoredPosition, newLocation, easing));
            panelLocation = newLocation;
            currentPage++;
            IMG_Negozi_Selettore.SetActive(true);
            IMG_POI_Selettore.SetActive(false);
        }
    }

    private void PrevPage()
    {
        if (currentPage > 1)
        {
            Vector3 newLocation = panelLocation + new Vector2(Screen.width, 0);
            StartCoroutine(SmoothMove(rect.anchoredPosition, newLocation, easing));
            panelLocation = newLocation;
            currentPage--;
            IMG_Negozi_Selettore.SetActive(false);
            IMG_POI_Selettore.SetActive(true);
        }
    }
}

