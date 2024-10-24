using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int[] stats= new int[4];
    
    public int []valuesIni = new int [4];

   // public TextMeshProUGUI[] texts = new TextMeshProUGUI[4]; 

    private void Start()
    {
        for (int i=0; i<stats.Length; i++)
        {
            stats[i]=valuesIni[i];
            //texts[i].text=stats[i].ToString();
        }
    }
    public void addorloseStats(int s1,int s2, int s3, int s4)
    {
        stats[0] += s1;
        stats[1] += s2;
        stats[2] += s3;
        stats[3] += s4;
    }

    public int getStat(int index)
    {
         return stats[index]; 
    }
    

}
