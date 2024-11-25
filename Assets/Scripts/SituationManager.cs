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
    public TextMeshProUGUI textSituation;
    public TextMeshProUGUI textElec1, textElec2, textElec3;
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
        setSituation();
    }

    private void setSituation()
    {


        currentSituation = Random.Range(0, situations.Length);
        while (currentSituation != lastSituation)
        {
            currentSituation = Random.Range(0, situations.Length);
        }
        textSituation.text = situations[currentSituation].situation;
        textElec1.text = situations[currentSituation].elec1;
        textElec2.text = situations[currentSituation].elec2;
        if (situations[currentSituation].elec3 == "" || situations[currentSituation].elec3 == "NULL"|| situations[currentSituation].tag=="Desactivar")
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
                type= Type.Acumulador;
            }
            else if(situations[currentSituation].tag == "Probabilidad")
            {
                type= Type.Probabilidad;
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
