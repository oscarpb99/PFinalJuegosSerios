using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject imagenPrefab;
    public SituationManager situationManager;

    // Al final de cada partida, guardamos los datos de cada situacion
    public StadisticsOfSelections[] lastGameDataSaved;
    public int indexGameDataSaved = 0;

    
    public void saveSituationData(StadisticsOfSelections[] data)
    {
        lastGameDataSaved = data;
        indexGameDataSaved = 0;
    }

    
    public UIFill[] imagesStats;
    public float[] stats= new float[4];
    
    public float []valuesIni = new float [4];

    private int[]valuesTextLeft = new int[4];
    private int[]valuesTextRight=new int[4];
    private int[]valuesTextDown = new int[4];

    public int totalCredits = 240;
    public int credits = 0;
    public int year = 1;

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

    public void resetStats()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = valuesIni[i];
            imagesStats[i].addOrDeduct(stats[i]);
        }
    }


    //id-> 0=izq, 1=der, 2=abajo
    public void addorloseStats(int option,int s1,int s2, int s3, int s4)
    {
        // Si una situacion tiene un tag, aplicamos sus efectos especiales
        if (situationManager.getType() == SituationManager.Type.Acumulador)
        {
            // Segun la eleccion que elija, modificamos los stats correspondientes
            if (option == 0)
            {
                // Sacamos la cantidad de veces que se ha seleccionado la option 0 (izquierda), opcion 1 (derecha) y opcion 2 (abajo) de la situacion actual
                //situationManager.numRepeteatSelections[situationManager.getCurrentSituation()].acumulativeXXXX

                // Miramos el accumulative stat, para saber que tanto puede afectar segun la eleccion
                if (!situationManager.getIsSpecific())
                {
                    stats[0] += s1 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeLeft * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat1Left);
                    stats[1] += s2 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeLeft * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat2Left);
                    stats[2] += s3 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeLeft * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat3Left);
                    stats[3] += s4 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeLeft * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat4Left);
                }
                else
                {
                    stats[0] += s1 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeLeft * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat1Left);
                    stats[1] += s2 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeLeft * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat2Left);
                    stats[2] += s3 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeLeft * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat3Left);
                    stats[3] += s4 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeLeft * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat4Left);
                }
            }
            else if (option == 1)
            {
                if (!situationManager.getIsSpecific())
                {
                    stats[0] += s1 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeRight * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat1Right);
                    stats[1] += s2 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeRight * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat2Right);
                    stats[2] += s3 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeRight * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat3Right);
                    stats[3] += s4 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeRight * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat4Right);
                }
                else
                {
                    stats[0] += s1 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeRight * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat1Right);
                    stats[1] += s2 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeRight * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat2Right);
                    stats[2] += s3 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeRight * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat3Right);
                    stats[3] += s4 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeRight * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat4Right);
                }
            }
            else if (option == 2)
            {
                if (!situationManager.getIsSpecific())
                {
                    stats[0] += s1 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeDown * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat1Down);
                    stats[1] += s2 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeDown * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat2Down);
                    stats[2] += s3 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeDown * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat3Down);
                    stats[3] += s4 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeDown * situationManager.situations[situationManager.getCurrentSituation()].acumulativeStat4Down);
                }
                else
                {
                    stats[0] += s1 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeDown * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat1Down);
                    stats[1] += s2 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeDown * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat2Down);
                    stats[2] += s3 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeDown * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat3Down);
                    stats[3] += s4 + (situationManager.numRepeatedSelections[situationManager.getCurrentSituation()].acumulativeDown * situationManager.specificSituations[situationManager.getCurrentSituation()].acumulativeStat4Down);
                }
            }
        }
        else
        {
            stats[0] += s1;
            stats[1] += s2;
            stats[2] += s3;
            stats[3] += s4;
        }

        //En la semana de ex�menes se suman los cr�ditos dependiendo de la situaci�n
        if (isExamWeek)
        {
            switch (option)
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
            year += 1;
            isExamWeek = false;
        }

        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] <= 0)
            {
                stats[i] = 0;
                winCondition = false;
                imagenPrefab.GetComponent<SelectSituation>().addStadistics(option);
                saveSituationData(situationManager.numRepeatedSelections);
                situationManager.exportStatisticsToJson(NameData.PlayerName);
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
            imagenPrefab.GetComponent<SelectSituation>().addStadistics(option);
            saveSituationData(situationManager.numRepeatedSelections);
            situationManager.exportStatisticsToJson(NameData.PlayerName);
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

    public int getYear()
    {
        return year;
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
        // Cargar la escena de manera asincr�nica
        SceneManager.LoadScene("GameScene");

        // Esperar un frame para asegurarte de que los objetos de la escena se inicialicen
        yield return null;

        // Reconfigurar el juego
        situationManager = GameObject.Find("SituationManager").GetComponent<SituationManager>();
        credits = 0;
        winCondition = false;
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = valuesIni[i];
        }
        
    }

    //Se le llama para activar los circulos hijos dela UI de cada stat para mostrar que se van a modificar, pero no c�mo
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
