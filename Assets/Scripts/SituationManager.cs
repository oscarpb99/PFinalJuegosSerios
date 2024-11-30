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
    public NumRepeteatSelection[] numRepeteatSelections ;
    int lastSituation;
    int currentSituation;
    int cartasAño;
    int cardCounter;
    int yearDifficulty;
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
        cartasAño = Random.Range(20, 35);
        cardCounter = 0;
        yearDifficulty = 0;
        setSituation();
    }

    private void setSituation()
    {
        int specialCardProbability;
        bool nextIsSpecial = false;

        cardCounter++;

        if(textElec1.gameObject.activeSelf == false)
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
            cartasAño = Random.Range(20, 35);
            currentSituation = situations.Length -1;
            textSituation.text = situations[currentSituation].situation;
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
                textElec1.text = situations[currentSituation].elec1;
                backgroundElec2.SetActive(false);
                textElec2.gameObject.SetActive(false);
                backgroundElec3.SetActive(false);
                textElec3.gameObject.SetActive(false);
            }
            else if(GameManager.Instance.getStat(2) >= midDifficulty)
            {
                backgroundElec1.SetActive(false);
                textElec1.gameObject.SetActive(false);
                textElec2.text = situations[currentSituation].elec2;
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
                textElec3.text = situations[currentSituation].elec3;
                yearDifficulty += 20;
            }
            GameManager.Instance.examWeek();
        }
        else
        {
            //Comprueba que una stat es baja para tener la posibilidad de que salga una carta especial
            if (GameManager.Instance.getStat(0) <= 20 || GameManager.Instance.getStat(1) <= 20 || GameManager.Instance.getStat(2) <= 20 || GameManager.Instance.getStat(3) <= 20)
            {
                specialCardProbability = Random.Range(1, 11);
                if (specialCardProbability == 1)
                {
                    nextIsSpecial = true;
                }
            }
            if (!nextIsSpecial)
            {
                currentSituation = Random.Range(0, situations.Length - 4);
                while (currentSituation == lastSituation)
                {
                    currentSituation = Random.Range(0, situations.Length - 4);
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
                currentSituation = situations.Length - 5 + index;
                type = Type.Probabilidad;
            }

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
        }
        GameManager.Instance.setStatsText(1, situations[currentSituation].stat1Left, situations[currentSituation].stat2Left, situations[currentSituation].stat3Left, situations[currentSituation].stat4Left);
        GameManager.Instance.setStatsText(0, situations[currentSituation].stat1Right, situations[currentSituation].stat2Right, situations[currentSituation].stat3Right, situations[currentSituation].stat4Right);
        GameManager.Instance.setStatsText(2, situations[currentSituation].stat1Down, situations[currentSituation].stat2Down, situations[currentSituation].stat3Down, situations[currentSituation].stat4Down);

        imagenSituation.sprite = Situations.Instance.GetSprite(situations[currentSituation].image);
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
        // Por hacer
        setSituation();
    }
}
