using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectSituation : MonoBehaviour
{
    public RectTransform text1Rect, text2Rect, text3Rect;
    private RectTransform imageRect;
    private Vector2 posIni;
    public Canvas canvas;
   

    private void Awake()
    {
        imageRect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        posIni = transform.position;
        GetComponent<UnityEngine.UI.Image>().sprite = GameManager.Instance.situationManager.imagenSituation.sprite; 
    }

    private void FixedUpdate()
    {
        if (isOverlapping(imageRect, text1Rect))
        {

            GameManager.Instance.addorloseStats(GameManager.Instance.getStatsText(1)[0], GameManager.Instance.getStatsText(1)[1], GameManager.Instance.getStatsText(1)[2], GameManager.Instance.getStatsText(1)[3]);
            Destroy(gameObject);
            createNewImageInstance();


        }
        else if (isOverlapping(imageRect, text2Rect))
        {
            GameManager.Instance.addorloseStats(GameManager.Instance.getStatsText(0)[0], GameManager.Instance.getStatsText(0)[1], GameManager.Instance.getStatsText(0)[2], GameManager.Instance.getStatsText(0)[3]);
            Destroy(gameObject);
            createNewImageInstance();
        }
        else if (isOverlapping(imageRect, text3Rect))
        {
            GameManager.Instance.addorloseStats(GameManager.Instance.getStatsText(2)[0], GameManager.Instance.getStatsText(2)[1], GameManager.Instance.getStatsText(2)[2], GameManager.Instance.getStatsText(2)[3]);
            Destroy(gameObject);
            createNewImageInstance();
        }


    }
    private bool isOverlapping(RectTransform rect1,RectTransform rect2)
    {
        return (RectTransformUtility.RectangleContainsScreenPoint(rect1, rect2.position) ||
                RectTransformUtility.RectangleContainsScreenPoint(rect2, rect1.position));
            
    }

    private void createNewImageInstance()
    {
        GameObject image = Instantiate(GameManager.Instance.imagenPrefab);
        SelectSituation selectSituation=image.GetComponent<SelectSituation>();
        selectSituation.text1Rect = text1Rect;
        selectSituation.text2Rect = text2Rect;
        selectSituation.text3Rect = text3Rect;
        selectSituation.imageRect = image.GetComponent<RectTransform>();
        selectSituation.canvas = canvas;
        image.GetComponent<UnityEngine.UI.Image>().sprite = GameManager.Instance.situationManager.imagenSituation.sprite;
        image.transform.SetParent(canvas.transform, false);
        image.transform.position = posIni;
        GameManager.Instance.situationManager.manageSituations();

        //Situations.Instance.setAll();
    }
}
