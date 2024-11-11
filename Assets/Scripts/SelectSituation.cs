using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectSituation : MonoBehaviour
{
    public RectTransform text1Rect, text2Rect;
    private RectTransform imageRect;
    public GameManager gameManager;
    private Vector2 posIni;

    public GameObject imagenPrefab;
    public Canvas canvas;
    public Situations situations;

    private void Awake()
    {
        imageRect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        posIni = transform.position;
    }

    private void FixedUpdate()
    {
        if (isOverlapping(imageRect, text1Rect) )
        {
           
            gameManager.addorloseStats(gameManager.getStatsText(true)[0], gameManager.getStatsText(true)[1], gameManager.getStatsText(true)[2], gameManager.getStatsText(true)[3]);
            Destroy(gameObject);
            GameObject image=Instantiate(imagenPrefab);
            image.GetComponent<SelectSituation>().text1Rect = text1Rect;
            image.GetComponent<SelectSituation>().text2Rect = text2Rect;
            image.GetComponent<SelectSituation>().imageRect = image.GetComponent<RectTransform>();
            image.GetComponent<SelectSituation>().gameManager = gameManager;
            image.GetComponent<SelectSituation>().imagenPrefab = imagenPrefab;
            image.GetComponent<SelectSituation>().canvas = canvas;
            image.GetComponent<SelectSituation>().situations = situations;
            image.transform.SetParent(canvas.transform, false);
            image.transform.position = posIni;
            situations.setAll();


        }
        
        if (isOverlapping(imageRect, text2Rect))
        {
            gameManager.addorloseStats(gameManager.getStatsText(false)[0], gameManager.getStatsText(false)[1], gameManager.getStatsText(false)[2], gameManager.getStatsText(false)[3]);
            Destroy(gameObject);
            GameObject image=Instantiate(imagenPrefab);
            image.GetComponent<SelectSituation>().text1Rect = text1Rect;
            image.GetComponent<SelectSituation>().text2Rect = text2Rect;
            image.GetComponent<SelectSituation>().imageRect = image.GetComponent<RectTransform>(); 
            image.GetComponent<SelectSituation>().gameManager = gameManager;
            image.GetComponent <SelectSituation>().imagenPrefab = imagenPrefab;
            image.GetComponent<SelectSituation>().canvas = canvas;
            image.GetComponent<SelectSituation>().situations = situations;
            image.transform.SetParent(canvas.transform, false);
            image.transform.position = posIni;
            situations.setAll();

        }
     
    }
    private bool isOverlapping(RectTransform rect1,RectTransform rect2)
    {
        return (RectTransformUtility.RectangleContainsScreenPoint(rect1, rect2.position) ||
                RectTransformUtility.RectangleContainsScreenPoint(rect2, rect1.position));
            
    }
}
