using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragImage : MonoBehaviour,IPointerDownHandler,IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 offset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Calcula la diferencia entre la pos del ratón y la del objeto
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        //Actualiza la pos del objeto según el mov del ratón
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            rectTransform.localPosition = localPoint - offset;
        }
    }


}
