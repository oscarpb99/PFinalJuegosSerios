using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct NumRepeteatSelection
{
     public int nRepeatSelectLeft;
     public int nRepeatSelectRight;
     public int nRepeatSelectDown;

}

public class SituationManager : MonoBehaviour
{
    public Situation[] situations;
    public Situation[] specificSituations;
    public TutorialCard[] tutorialCards = new TutorialCard[0];
    public int tutorialCounter;

    //array para guardar cuantas veces tiene que esperar una situacion para volver a salir
    private int[] sleepingSituations;

    //situaciones bloqueadas
    private bool[] lockedSituations;
    private bool[] lockedSpecificSituations;

    public NumRepeteatSelection[] numRepeteatSelections;
    int lastSituation;
    int currentSituation;
    int cartasAño;
    int cardCounter;
    int yearDifficulty;
    int specificCounter;
    bool isSpecific = false;
    public TextMeshProUGUI textSituation;
    public TextMeshProUGUI textElec1, textElec2, textElec3;
    [SerializeField] GameObject backgroundElec1;
    [SerializeField] GameObject backgroundElec2;
    [SerializeField] GameObject backgroundElec3;
    public UnityEngine.UI.Image imagenSituation;
    public enum Type { Acumulador, Desactivar, Probabilidad };
    public Type type;


    // Start is called before the first frame update
    void Start()
    {
        // Para cuando asignamos una situacion, ya se han cargado todas en situations
        numRepeteatSelections=new NumRepeteatSelection[situations.Length];
        for (int i = 0; i < situations.Length; i++)
        {
            numRepeteatSelections[i].nRepeatSelectLeft = 0;
            numRepeteatSelections[i].nRepeatSelectRight = 0;
            numRepeteatSelections[i].nRepeatSelectDown = 0;
        }
        lockedSituations = new bool[situations.Length];
        lockedSpecificSituations = new bool[specificSituations.Length];
        sleepingSituations = new int[situations.Length];
        blockSituations();
        blockSpecificSituations();
        for(int  i = 0;i < situations.Length;i++)
        {
            sleepingSituations[i] = 0;
        }
        cartasAño = Random.Range(15, 25);
        cardCounter = 0;
        yearDifficulty = 0;
        tutorialCounter = 0;
        specificCounter = 0;
        SetTutorial();
    }

    private void setSituation()
    {
        int specialCardProbability;
        bool nextIsSpecial = false;

        cardCounter++;
        specificCounter++;


        for (int i = 0; i < sleepingSituations.Length; i++)
        {
            if (sleepingSituations[i] > 0)
            {
                sleepingSituations[i]--;
            }
        }

        if (textElec1.gameObject.activeSelf == false)
        {
            backgroundElec1.SetActive(true);
            textElec1.gameObject.SetActive(true);
        }
        if (textElec2.gameObject.activeSelf == false)
        {
            backgroundElec2.SetActive(true);
            textElec2.gameObject.SetActive(true);
        }

        //Si ya se han sacado todas las cartas del año, se resetea el contador y sale la carta de los exámenes
        if (cardCounter >= cartasAño)
        {
            cardCounter = 0;
            cartasAño = Random.Range(15, 25);
            currentSituation = 0;
            textSituation.text = specificSituations[currentSituation].situation;
            int maxDifficulty = 70 + yearDifficulty;
            int midDifficulty = 40 + yearDifficulty;
            if(maxDifficulty > 100)
            {
                maxDifficulty = 100;
            }
            if (midDifficulty > 80)
            {
                midDifficulty = 80;
            }
            if (GameManager.Instance.getStat(2) >= maxDifficulty) 
            {
                textElec1.text = specificSituations[currentSituation].elec1;
                backgroundElec2.SetActive(false);
                textElec2.gameObject.SetActive(false);
                backgroundElec3.SetActive(false);
                textElec3.gameObject.SetActive(false);
            }
            else if(GameManager.Instance.getStat(2) >= midDifficulty)
            {
                backgroundElec1.SetActive(false);
                textElec1.gameObject.SetActive(false);
                textElec2.text = specificSituations[currentSituation].elec2;
                backgroundElec3.SetActive(false);
                textElec3.gameObject.SetActive(false);
                yearDifficulty += 10;
            }
            else
            {
                backgroundElec1.SetActive(false);
                textElec1.gameObject.SetActive(false);
                backgroundElec2.SetActive(false);
                textElec2.gameObject.SetActive(false);
                textElec3.text = specificSituations[currentSituation].elec3;
                yearDifficulty += 20;
            }
            GameManager.Instance.examWeek();
            GameManager.Instance.setStatsText(1, specificSituations[currentSituation].stat1Left, specificSituations[currentSituation].stat2Left, specificSituations[currentSituation].stat3Left, specificSituations[currentSituation].stat4Left);
            GameManager.Instance.setStatsText(0, specificSituations[currentSituation].stat1Right, specificSituations[currentSituation].stat2Right, specificSituations[currentSituation].stat3Right, specificSituations[currentSituation].stat4Right);
            GameManager.Instance.setStatsText(2, specificSituations[currentSituation].stat1Down, specificSituations[currentSituation].stat2Down, specificSituations[currentSituation].stat3Down, specificSituations[currentSituation].stat4Down);

            imagenSituation.sprite = Situations.Instance.GetSprite(specificSituations[currentSituation].image);
        }
        else
        {
            //Comprueba que una stat es baja para tener la posibilidad de que salga una carta especial
            if (GameManager.Instance.getStat(0) <= 20 || GameManager.Instance.getStat(1) <= 20 || GameManager.Instance.getStat(2) <= 20 || GameManager.Instance.getStat(3) <= 20)
            {
                specialCardProbability = Random.Range(0, 5);
                if (specialCardProbability == 1)
                {
                    nextIsSpecial = true;
                }
            }
            if (!nextIsSpecial)
            {
                if (specificCounter >= 6)
                {
                    specificCounter = 0;
                    currentSituation = Random.Range(1, specificSituations.Length - 4);
                    while (lockedSpecificSituations[currentSituation])
                    {
                        currentSituation = Random.Range(1, specificSituations.Length - 4);
                    }
                    isSpecific = true;
                }
                else
                {
                    currentSituation = Random.Range(0, situations.Length);
                    while (lockedSituations[currentSituation] || sleepingSituations[currentSituation] > 0)
                    {
                        currentSituation = Random.Range(0, situations.Length);
                    }
                }
            }
            //Por diseño, metemos las cartas especiales al final
            else
            {
                int index = Random.Range(0, 4);
                while (GameManager.Instance.getStat(index) > 20)
                {
                    index = Random.Range(0, 4);
                }
                currentSituation = specificSituations.Length - 4 + index;
                type = Type.Probabilidad;
                isSpecific = true;
            }
            if (isSpecific)
            {
                textSituation.text = specificSituations[currentSituation].situation;
                textElec1.text = specificSituations[currentSituation].elec1;
                textElec2.text = specificSituations[currentSituation].elec2;
                textElec3.text = specificSituations[currentSituation].elec3;
                GameManager.Instance.setStatsText(1, specificSituations[currentSituation].stat1Left, specificSituations[currentSituation].stat2Left, specificSituations[currentSituation].stat3Left, specificSituations[currentSituation].stat4Left);
                GameManager.Instance.setStatsText(0, specificSituations[currentSituation].stat1Right, specificSituations[currentSituation].stat2Right, specificSituations[currentSituation].stat3Right, specificSituations[currentSituation].stat4Right);
                GameManager.Instance.setStatsText(2, specificSituations[currentSituation].stat1Down, specificSituations[currentSituation].stat2Down, specificSituations[currentSituation].stat3Down, specificSituations[currentSituation].stat4Down);
                imagenSituation.sprite = Situations.Instance.GetSprite(specificSituations[currentSituation].image);
                isSpecific = false;
            }
            else
            {
                textSituation.text = situations[currentSituation].situation;
                textElec1.text = situations[currentSituation].elec1;
                textElec2.text = situations[currentSituation].elec2;
                if (situations[currentSituation].elec3 == "" || situations[currentSituation].elec3 == "NULL" || situations[currentSituation].tag == "Desactivar")
                {
                    type = Type.Desactivar;
                    backgroundElec3.SetActive(false);
                    textElec3.gameObject.SetActive(false);
                }
                else
                {
                    backgroundElec3.SetActive(true);
                    textElec3.gameObject.SetActive(true);
                    textElec3.text = situations[currentSituation].elec3;
                    if (situations[currentSituation].tag == "Acumulador")
                    {
                        type = Type.Acumulador;
                    }

                }
                GameManager.Instance.setStatsText(1, situations[currentSituation].stat1Left, situations[currentSituation].stat2Left, situations[currentSituation].stat3Left, situations[currentSituation].stat4Left);
                GameManager.Instance.setStatsText(0, situations[currentSituation].stat1Right, situations[currentSituation].stat2Right, situations[currentSituation].stat3Right, situations[currentSituation].stat4Right);
                GameManager.Instance.setStatsText(2, situations[currentSituation].stat1Down, situations[currentSituation].stat2Down, situations[currentSituation].stat3Down, situations[currentSituation].stat4Down);
                sleepingSituations[currentSituation] = situations[currentSituation].spawnRate;

                imagenSituation.sprite = Situations.Instance.GetSprite(situations[currentSituation].image);
            }
        }

    }

    private void SetTutorial()
    {
        // Con las cartas de tutorial ya leidas, se leen de manera ordenada
        if (tutorialCounter < tutorialCards.Length)
        {
            textSituation.text = tutorialCards[tutorialCounter].situation;
            textElec1.text = tutorialCards[tutorialCounter].elec1;
            textElec2.text = tutorialCards[tutorialCounter].elec2;
            textElec3.text = tutorialCards[tutorialCounter].elec3;
            GameManager.Instance.setStatsText(1, tutorialCards[tutorialCounter].stat1Left, tutorialCards[tutorialCounter].stat2Left, tutorialCards[tutorialCounter].stat3Left, tutorialCards[tutorialCounter].stat4Left);
            GameManager.Instance.setStatsText(0, tutorialCards[tutorialCounter].stat1Right, tutorialCards[tutorialCounter].stat2Right, tutorialCards[tutorialCounter].stat3Right, tutorialCards[tutorialCounter].stat4Right);
            GameManager.Instance.setStatsText(2, tutorialCards[tutorialCounter].stat1Down, tutorialCards[tutorialCounter].stat2Down, tutorialCards[tutorialCounter].stat3Down, tutorialCards[tutorialCounter].stat4Down);
            imagenSituation.sprite = Situations.Instance.GetSprite(tutorialCards[tutorialCounter].image);
        }
        else
        {
            setSituation();
        }

    }

    public void applyCardTutorial(int option)
    {
        switch (tutorialCounter)
        {
            default:
                break;
            case 0:
                // Si elige NO continuar el tutorial
                if (option != 2)
                    tutorialCounter = tutorialCards.Length + 1;
                break;
            case 4:
                // Enseñamos que se puede modificar las stats
                if (option == 1)
                {
                    GameManager.Instance.addorloseStats(0, GameManager.Instance.getStatsText(1)[0], GameManager.Instance.getStatsText(1)[1], GameManager.Instance.getStatsText(1)[2], GameManager.Instance.getStatsText(1)[3]);
                }
                else if (option == 2)
                {
                    GameManager.Instance.addorloseStats(1, GameManager.Instance.getStatsText(0)[0], GameManager.Instance.getStatsText(0)[1], GameManager.Instance.getStatsText(0)[2], GameManager.Instance.getStatsText(0)[3]);
                }
                else
                {
                    GameManager.Instance.addorloseStats(2, GameManager.Instance.getStatsText(2)[0], GameManager.Instance.getStatsText(2)[1], GameManager.Instance.getStatsText(2)[2], GameManager.Instance.getStatsText(2)[3]);
                }
                break;
            case 6:
                // Reseteamos los stats
                GameManager.Instance.resetStats();
                break;

        }

        // Pasamos a la siguiente carta del tutorial
        tutorialCounter++;
        SetTutorial();
    }

    public int getLeftRepeated(int index)
    {
        return numRepeteatSelections[index].nRepeatSelectLeft;
    }
    public int getRightRepeated(int index)
    {
        return numRepeteatSelections[index].nRepeatSelectRight;
    }
    public int getDownRepeated(int index)
    {
        return numRepeteatSelections[index].nRepeatSelectDown;
    }
    public int getCurrentSituation()
    {
        return currentSituation;
    }

    public Type getType()
    {
        return type;
    }

    public void manageSituations()
    {
        // Si seguimos con los tutoriales, llamamos a setTutorial
        if (tutorialCounter < tutorialCards.Length)
        {
            SetTutorial();
        }
        else
        {
            // Seleccionamos una situacion. Si la situacion es la misma que la anterior, se vuelve a seleccionar otra
            lastSituation = currentSituation;

            setSituation();
        }
    }

    public void blockSituations()
    {
        //Aqui están las situaciones generales que empiezan bloqueadas
        for(int i = 0; i < situations.Length; i++)
        {
            switch (i)
            {
                default:
                    lockedSituations[i] = false;
                    break;
                case 5:
                    lockedSituations[i] = true;
                    break;
                case 6:
                    lockedSituations[i] = true;
                    break;
                case 7:
                    lockedSituations[i] = true;
                    break;
            }
        }
    }

    public void blockSpecificSituations()
    {
        //Aqui están las situaciones específicas que empiezan bloqueadas
        for (int i = 0; i < specificSituations.Length; i++)
        {
            switch (i)
            {
                default:
                    lockedSpecificSituations[i] = false;
                    break;
                case 2:
                    lockedSpecificSituations[i] = true;
                    break;
                case 6:
                    lockedSpecificSituations[i] = true;
                    break;
                case 7:
                    lockedSpecificSituations[i] = true;
                    break;
                case 8:
                    lockedSpecificSituations[i] = true;
                    break;
            }
        }
    }
}
