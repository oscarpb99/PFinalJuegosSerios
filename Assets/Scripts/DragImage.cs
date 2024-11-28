using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragImage : MonoBehaviour,IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 offset;
    private Vector2 posIni;
    private SelectSituation selectSituation;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        posIni = rectTransform.localPosition;
        selectSituation = GetComponent<SelectSituation>();
    }
  
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        //Actualiza la pos del objeto seg�n el mov del rat�n
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float newY = Mathf.Min(localPoint.y - offset.y,-40);
            rectTransform.localPosition = new Vector2(localPoint.x - offset.x,newY);
        }
    }

    //Comprobamos si el drag ha terminado
    public void OnEndDrag(PointerEventData eventData)
    {
        //Llamamos a la funci�n OnLetGo de SelectSituation
        if (selectSituation != null) {
            selectSituation.OnLetGo();
        }
        
    }

    public void resetPos()
    {
        transform.localPosition = posIni;
    }


}
