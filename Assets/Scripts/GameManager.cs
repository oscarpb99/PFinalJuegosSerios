using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject imagenPrefab;
    [SerializeField] UIFill[] imagesStats;
    private int[] stats= new int[4];
    
    public int []valuesIni = new int [4];

    private int[]valuesTextLeft = new int[4];
    private int[]valuesTextRight=new int[4];

    
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

    public int[] getStatsText(bool isLeft) {
        if(isLeft)
        {
            return valuesTextLeft;
        }
        else
        {
            return valuesTextRight;
        }
    }

    public void setStatsText(bool isLeft,int s1,int s2,int s3,int s4)
    {
        if (isLeft)
        {
            valuesTextLeft[0] = s1;
            valuesTextLeft[1] = s2;
            valuesTextLeft[2] = s3;
            valuesTextLeft[3] = s4;
        }
        else
        {
            valuesTextRight[0] = s1;
            valuesTextRight[1] = s2;
            valuesTextRight[2] = s3;
            valuesTextRight[3] = s4;
        }
    }


}
