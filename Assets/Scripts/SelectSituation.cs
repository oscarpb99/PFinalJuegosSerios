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
    [SerializeField] DragImage dragImage;

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
        //GetComponent<UnityEngine.UI.Image>().sprite = GameManager.Instance.situationManager.imagenSituation.sprite; 
    }

    private void FixedUpdate()
    {
        if (isOverlapping(imageRect, text1Rect))
        {
            GameManager.Instance.showModifiedStats(GameManager.Instance.getStatsText(1)[0], GameManager.Instance.getStatsText(1)[1], GameManager.Instance.getStatsText(1)[2], GameManager.Instance.getStatsText(1)[3]);
            if (isLetGo)
            {
                if (GameManager.Instance.situationManager.tutorialCounter < GameManager.Instance.situationManager.tutorialCards.Length)
                {
                    GameManager.Instance.situationManager.applyCardTutorial(1);
                    Destroy(gameObject);
                    createNewImageInstance();
                }
                else
                {
                    GameManager.Instance.addorloseStats(0, GameManager.Instance.getStatsText(1)[0], GameManager.Instance.getStatsText(1)[1], GameManager.Instance.getStatsText(1)[2], GameManager.Instance.getStatsText(1)[3]);

                    //Sumamos uno a las veces que se coge la izquierda de una determinada situacion
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].nSelectedLeft++;

                    // Para las situaciones acumuladoras, restamos y sumamos 1 
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeLeft++;

                    if(GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeRight>0)
                        GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeRight--;

                    if (GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeDown > 0)
                        GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeDown--;


                    // Acumular para actualizar la racha actual
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakLeftNow++;
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakRightNow = 0;
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakDownNow = 0;

                    //Guardamos la mejor racha 
                    if(GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakLeftNow > GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].maxStreakLeft)
                    {
                        GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].maxStreakLeft = GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakLeftNow;
                    }
                    
                    // Desbloqueamos situaciones si coincide
                    GameManager.Instance.situationManager.unlockAndLockSituation(0);
                    
                    Destroy(gameObject);
                    createNewImageInstance();
                }
            }

        }
        else if (isOverlapping(imageRect, text2Rect))
        {
            GameManager.Instance.showModifiedStats(GameManager.Instance.getStatsText(0)[0], GameManager.Instance.getStatsText(0)[1], GameManager.Instance.getStatsText(0)[2], GameManager.Instance.getStatsText(0)[3]);
            if (isLetGo)
            {
                if (GameManager.Instance.situationManager.tutorialCounter < GameManager.Instance.situationManager.tutorialCards.Length)
                {
                    GameManager.Instance.situationManager.applyCardTutorial(2);
                    Destroy(gameObject);
                    createNewImageInstance();
                }
                else
                {
                    GameManager.Instance.addorloseStats(1, GameManager.Instance.getStatsText(0)[0], GameManager.Instance.getStatsText(0)[1], GameManager.Instance.getStatsText(0)[2], GameManager.Instance.getStatsText(0)[3]);

                    //Sumamos uno a las veces que se coge la derecha de una determinada situacion
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].nSelectedRight++;

                    // Para las situaciones acumuladoras, restamos y sumamos 1 
                    if (GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeLeft > 0)
                        GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeLeft--;

                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeRight++;

                    if (GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeDown > 0)
                        GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeDown--;


                    // Acumular para actualizar la racha actual
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakLeftNow = 0;
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakRightNow++;
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakDownNow = 0;

                    //Guardamos la mejor racha 
                    if (GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakRightNow > GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].maxStreakRight)
                    {
                        GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].maxStreakRight = GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakRightNow;
                    }

                    // Desbloqueamos situaciones si coincide
                    GameManager.Instance.situationManager.unlockAndLockSituation(1);

                    Destroy(gameObject);
                    createNewImageInstance();
                }
                
            }
        }
        else if (text3Rect.gameObject.activeSelf && isOverlapping(imageRect, text3Rect))
        {
            GameManager.Instance.showModifiedStats(GameManager.Instance.getStatsText(2)[0], GameManager.Instance.getStatsText(2)[1], GameManager.Instance.getStatsText(2)[2], GameManager.Instance.getStatsText(2)[3]);

            if (isLetGo)
            {
                if (GameManager.Instance.situationManager.tutorialCounter < GameManager.Instance.situationManager.tutorialCards.Length)
                {
                    GameManager.Instance.situationManager.applyCardTutorial(3);
                    Destroy(gameObject);
                    createNewImageInstance();
                }
                else
                {
                    GameManager.Instance.addorloseStats(2, GameManager.Instance.getStatsText(2)[0], GameManager.Instance.getStatsText(2)[1], GameManager.Instance.getStatsText(2)[2], GameManager.Instance.getStatsText(2)[3]);

                    //Sumamos uno a las veces que se coge la de abajo de una determinada situacion
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].nSelectedDown++;

                    // Para las situaciones acumuladoras, restamos y sumamos 1 
                    if (GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeLeft > 0)
                        GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeLeft--;

                    if (GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeRight > 0)
                        GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeRight--;

                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].acumulativeDown++;

                    // Acumular para actualizar la racha actual
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakLeftNow = 0;
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakRightNow = 0;
                    GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakDownNow++;

                    //Guardamos la mejor racha 
                    if (GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakDownNow > GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].maxStreakDown)
                    {
                        GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].maxStreakDown = GameManager.Instance.situationManager.numRepeteatSelections[GameManager.Instance.situationManager.getCurrentSituation()].streakDownNow;
                    }

                    // Desbloqueamos situaciones si coincide
                    GameManager.Instance.situationManager.unlockAndLockSituation(2);

                    Destroy(gameObject);
                    createNewImageInstance();
                }
            }
        }
        else
        {
            GameManager.Instance.showModifiedStats(0,0,0,0);
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
        yield return new WaitUntil(() => GameManager.Instance != null && GameManager.Instance.situationManager != null);

        // Esperar hasta que la imagen esté lista, si es necesario
        yield return new WaitUntil(() => GameManager.Instance.situationManager.imagenSituation != null);

        // Asignar el sprite
        imageComponent.sprite = GameManager.Instance.situationManager.imagenSituation.sprite;
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
        selectSituation.dragImage = image.GetComponent<DragImage>();
        selectSituation.imageRect = image.GetComponent<RectTransform>();
        selectSituation.canvas = canvas;
        image.GetComponent<UnityEngine.UI.Image>().sprite = GameManager.Instance.situationManager.imagenSituation.sprite;
        image.transform.SetParent(canvas.transform, false);
        image.transform.position = posIni;
        GameManager.Instance.situationManager.imagenSituation = image.GetComponent<UnityEngine.UI.Image>();
        GameManager.Instance.situationManager.manageSituations();

        //Situations.Instance.setAll();
    }

    public void OnLetGo()
    {
        isLetGo = true;
    }
}
