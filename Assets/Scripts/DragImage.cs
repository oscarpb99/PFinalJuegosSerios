using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragImage : MonoBehaviour,IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 offset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
  
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        //Actualiza la pos del objeto según el mov del ratón
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float newY = Mathf.Min(localPoint.y - offset.y,-20);
            rectTransform.localPosition = new Vector2(localPoint.x - offset.x,newY);
        }
    }


}
