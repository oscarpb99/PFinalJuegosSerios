using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSituation : MonoBehaviour
{
    public RectTransform text1Rect, text2Rect;
    private RectTransform imageRect;
    public GameManager gameManager;
    private void Awake()
    {
        imageRect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (isOverlapping(imageRect, text1Rect))
        {
            gameManager.addorloseStats(10, 10, -10, -10);
            Destroy(gameObject);

        }
        if (isOverlapping(imageRect, text2Rect))
        {
            gameManager.addorloseStats(10, 10, -10, -10);
            Destroy(gameObject);
        }
    }
    private bool isOverlapping(RectTransform rect1,RectTransform rect2)
    {
        return (RectTransformUtility.RectangleContainsScreenPoint(rect1, rect2.position) ||
                RectTransformUtility.RectangleContainsScreenPoint(rect2, rect1.position));
            
    }
}
