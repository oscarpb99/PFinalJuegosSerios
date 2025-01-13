using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NextSituation : MonoBehaviour
{
    public RectTransform text1Rect, text2Rect, text3Rect;
    private RectTransform imageRect;
    private Vector2 posIni;
    public Canvas canvas;
    [SerializeField] DragImage dragImage;
    public ProcessSituationData processSituationData;

    private bool isLetGo = false;


    private UnityEngine.UI.Image imageComponent;


    private void Awake()
    {
        imageRect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        posIni = transform.position;
        imageComponent = GetComponent<UnityEngine.UI.Image>();

        processSituationData.ProcessData();
        createNewImageInstance();
    }

    private void FixedUpdate()
    {
        if ((text1Rect.gameObject.activeSelf && isOverlapping(imageRect, text1Rect))
            || (text2Rect.gameObject.activeSelf && isOverlapping(imageRect, text2Rect))
            || (text3Rect.gameObject.activeSelf && isOverlapping(imageRect, text3Rect)))
        {
            // Si no hemos llegado al final de las situaciones a analizar
            if (isLetGo && GameManager.Instance.indexGameDataSaved < GameManager.Instance.lastGameDataSaved.Length)
            {
                    // Procesamos la siguiente situación
                    processSituationData.ProcessData();
                    createNewImageInstance();
            }
        }
        else
        {
            if (isLetGo)
            {
                dragImage.resetPos();
            }
            isLetGo = false;
        }


    }

    private IEnumerator WaitAndSetSprite()
    {
        // Esperar hasta que GameManager.Instance y situationManager estén inicializados
        yield return new WaitUntil(() => GameManager.Instance != null);

        // Esperar hasta que la imagen esté lista, si es necesario
        yield return new WaitUntil(() => GameManager.Instance.situationManager.imagenSituation != null);

        // Asignar el sprite
        imageComponent.sprite = GameManager.Instance.situationManager.imagenSituation.sprite;
    }
    private bool isOverlapping(RectTransform rect1, RectTransform rect2)
    {
        return (RectTransformUtility.RectangleContainsScreenPoint(rect1, rect2.position) ||
                RectTransformUtility.RectangleContainsScreenPoint(rect2, rect1.position));

    }

    private void createNewImageInstance()
    {
        // Cambiamos la imagen por la de la siguiente situacion a analizar
        if(GameManager.Instance.indexGameDataSaved < GameManager.Instance.lastGameDataSaved.Length)
            GetComponent<UnityEngine.UI.Image>().sprite = GameManager.Instance.lastGameDataSaved[GameManager.Instance.indexGameDataSaved].image;
        
        
        transform.SetParent(canvas.transform, false);
        transform.position = posIni;
    }

    public void OnLetGo()
    {
        isLetGo = true;
    }
}
