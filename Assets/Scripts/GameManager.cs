using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject imagenPrefab;
    public SituationManager situationManager;
    [SerializeField] UIFill[] imagesStats;
    private int[] stats= new int[4];
    
    public int []valuesIni = new int [4];

    private int[]valuesTextLeft = new int[4];
    private int[]valuesTextRight=new int[4];
    private int[]valuesTextDown = new int[4];

    // WinCondition sera 0 si podemos seguir jugando, 1 si hemos perdido y 2 si ya se ha ganado
    public int winCondition;

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
        winCondition = 0;

        for (int i=0; i<stats.Length; i++)
        {
            stats[i]=valuesIni[i];
        }
        
    }
    public void addorloseStats(int s1,int s2, int s3, int s4)
    {
        stats[0] += s1;
        stats[1] += s2;
        stats[2] += s3;
        stats[3] += s4;

        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] < 0)
            {
                stats[i] = 0;
                
                // Puede sustituirse por un simple cambio de escena a la escena de fin de partida
                winCondition = 1;
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
    }

    public int getStat(int index)
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


}
