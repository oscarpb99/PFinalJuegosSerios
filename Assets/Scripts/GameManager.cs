using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject imagenPrefab;
    public SituationManager situationManager;
    public UIFill[] imagesStats;
    private float[] stats= new float[4];
    
    public float []valuesIni = new float [4];

    private int[]valuesTextLeft = new int[4];
    private int[]valuesTextRight=new int[4];
    private int[]valuesTextDown = new int[4];

    public int totalCredits = 240;
    int credits = 0;

    private bool winCondition;
    private bool isExamWeek;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        credits = 0;
        winCondition = false;
        isExamWeek = false;
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = valuesIni[i];
        }
       

}
    //id-> 0=izq, 1=der, 2=abajo
    public void addorloseStats(int id,int s1,int s2, int s3, int s4)
    {
        if (situationManager.getType() == SituationManager.Type.Acumulador)
        {
            if (id == 0)
            {
                stats[0] += s1 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectLeft * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat1);
                stats[1] += s2 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectLeft * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat2);
                stats[2] += s3 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectLeft * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat3);
                stats[3] += s4 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectLeft * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat4);
            }
            else if (id == 1)
            {
                stats[0] += s1 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectRight * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat1);
                stats[1] += s2 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectRight * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat2);
                stats[2] += s3 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectRight * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat3);
                stats[3] += s4 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectRight * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat4);
            }
            else if (id == 2)
            {
                stats[0] += s1 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectDown * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat1);
                stats[1] += s2 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectDown * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat2);
                stats[2] += s3 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectDown * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat3);
                stats[3] += s4 + (situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].nRepeatSelectDown * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat4);
            }
        }
        else
        {
            stats[0] += s1;
            stats[1] += s2;
            stats[2] += s3;
            stats[3] += s4;
        }

        //En la semana de exámenes se suman los créditos dependiendo de la situación
        if (isExamWeek)
        {
            switch (id)
            {
                case 0:
                    credits += 60;
                    break;
                case 1:
                    credits += 42;
                    break;
                case 2:
                    credits += 24;
                    break;
            }
            isExamWeek = false;
        }

        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] <= 0)
            {
                stats[i] = 0;
                winCondition = false;
                SceneManager.LoadScene("EndScene");
            }
            else if (stats[i] > 100)
            {
                stats[i] = 100;
            }

        }

        for (int i = 0; i < imagesStats.Length; i++)
        {
            imagesStats[i].addOrDeduct(stats[i]);
        }
        if(credits >= totalCredits)
        {
            winCondition = true;
            SceneManager.LoadScene("EndScene");
        }
    }

    public float getStat(int index)
    {
         return stats[index]; 
    }

    public int[] getStatsText(int nCard) {
        if(nCard == 1)
            return valuesTextLeft;
        else if (nCard == 0)
            return valuesTextRight;
        else
            return valuesTextDown;
    }

    public void setStatsText(int nCard,int s1,int s2,int s3,int s4)
    {
        if (nCard == 1)
        {
            valuesTextLeft[0] = s1;
            valuesTextLeft[1] = s2;
            valuesTextLeft[2] = s3;
            valuesTextLeft[3] = s4;
        }
        else if (nCard == 0)
        {
            valuesTextRight[0] = s1;
            valuesTextRight[1] = s2;
            valuesTextRight[2] = s3;
            valuesTextRight[3] = s4;
        }
        else if (nCard == 2)
        {
            valuesTextDown[0] = s1;
            valuesTextDown[1] = s2;
            valuesTextDown[2] = s3;
            valuesTextDown[3] = s4;
        }
    }

    public bool getWinCondition()
    {
        return winCondition;
    }

    public int getCredits()
    {
        return credits;
    }

    public void examWeek()
    {
        isExamWeek = true;
    }

    public void resetGame()
    {
        StartCoroutine(resetGameRoutine());
       
    }
    private IEnumerator resetGameRoutine()
    {
        // Cargar la escena de manera asincrónica
        SceneManager.LoadScene("GameScene");

        // Esperar un frame para asegurarte de que los objetos de la escena se inicialicen
        yield return null;

        // Reconfigurar el juego
        situationManager = GameObject.Find("SituationManager").GetComponent<SituationManager>();
        imagesStats[0] = GameObject.Find("Social").GetComponent<UIFill>();
        imagesStats[1] = GameObject.Find("Corazon").GetComponent<UIFill>();
        imagesStats[2] = GameObject.Find("Academic").GetComponent<UIFill>();
        imagesStats[3] = GameObject.Find("Money").GetComponent<UIFill>();
        credits = 0;
        winCondition = false;
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = valuesIni[i];
        }
        
    }

    //Se le llama para activar los circulos hijos dela UI de cada stat para mostrar que se van a modificar, pero no cómo
    public void showModifiedStats(int s1, int s2, int s3, int s4)
    {
        for (int i = 0; i < imagesStats.Length; i++)
        {
            imagesStats[i].showModifiedStat(false);
        }
        if (s1 > 0 || s1 < 0)
        {
            imagesStats[0].showModifiedStat(true);
        }
        if (s2 > 0 || s2 < 0)
        {
            imagesStats[1].showModifiedStat(true);
        }
        if (s3 > 0 || s3 < 0)
        {
            imagesStats[2].showModifiedStat(true);
        }
        if (s4 > 0 || s4 < 0)
        {
            imagesStats[3].showModifiedStat(true);
        }
    }
    


}
