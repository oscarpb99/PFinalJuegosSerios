using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectSituation : MonoBehaviour
{
    public RectTransform text1Rect, text2Rect;
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
        GetComponent<UnityEngine.UI.Image>().sprite = Situations.Instance.imagenSituation.sprite; 
    }

    private void FixedUpdate()
    {
        if (isOverlapping(imageRect, text1Rect) )
        {

            GameManager.Instance.addorloseStats(GameManager.Instance.getStatsText(true)[0], GameManager.Instance.getStatsText(true)[1], GameManager.Instance.getStatsText(true)[2], GameManager.Instance.getStatsText(true)[3]);
            Destroy(gameObject);
            createNewImageInstance();


        }
        
        if (isOverlapping(imageRect, text2Rect))
        {
            GameManager.Instance.addorloseStats(GameManager.Instance.getStatsText(false)[0], GameManager.Instance.getStatsText(false)[1], GameManager.Instance.getStatsText(false)[2], GameManager.Instance.getStatsText(false)[3]);
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
        selectSituation.imageRect = image.GetComponent<RectTransform>();
        selectSituation.canvas = canvas;
        image.GetComponent<UnityEngine.UI.Image>().sprite = Situations.Instance.imagenSituation.sprite;
        image.transform.SetParent(canvas.transform, false);
        image.transform.position = posIni;
        Situations.Instance.setAll();
    }
}
