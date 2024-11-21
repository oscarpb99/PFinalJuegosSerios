using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject imagenPrefab;
    public SituationManager situationManager;
    [SerializeField] UIFill[] imagesStats;
    private float[] stats= new float[4];
    
    public float []valuesIni = new float [4];

    private int[]valuesTextLeft = new int[4];
    private int[]valuesTextRight=new int[4];
    private int[]valuesTextDown = new int[4];

    public int numMaxSituations = 30;
    int nSituation = 0;

    public float multVidaSocial=1;
    public float multBienestar = 1;
    public float multAcademic = 1;
    public float multMoney = 1;

    private bool winCondition;

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
        nSituation = 0;
        winCondition = false;
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = valuesIni[i];
        }
        multVidaSocial = 1;
        multBienestar = 1;
        multAcademic = 1;
        multMoney = 1;

}
    public void addorloseStats(int s1,int s2, int s3, int s4)
    {
        nSituation++;
        changeMultiplicates(s1,s2,s3,s4);
        stats[0] += s1*multVidaSocial;
        stats[1] += s2 *multBienestar;
        stats[2] += s3*multAcademic;
        stats[3] += s4*multMoney;

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
        if(nSituation>=numMaxSituations)
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
        nSituation = 0;
        winCondition = false;
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = valuesIni[i];
        }
        multVidaSocial = 1;
        multBienestar = 1;
        multAcademic = 1;
        multMoney = 1;
    }

    private void changeMultiplicates(int s1, int s2, int s3, int s4)
    {
        if (s1 < 0)
            multVidaSocial += 0.5f;
        else if (s2 > 0)
            multVidaSocial = 1;

        if (s2 < 0)
            multBienestar += 0.5f;
        else if (s2 > 0)
            multBienestar = 1;

        if (s3 < 0)
            multAcademic += 0.5f;
        else if (s3 > 0)
            multAcademic = 1;

        if (s4 < 0)
            multMoney += 0.5f;
        else if (s4 > 0)
            multMoney = 1;
    }


}
