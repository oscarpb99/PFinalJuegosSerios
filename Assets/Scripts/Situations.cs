using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

[System.Serializable]
public class SituationList
{
    public Situation[] situations;
}
public class Situations : MonoBehaviour
{
    public Situation[] situations;

    public GameManager gameManager;
    public TextMeshProUGUI textSituation;
    public TextMeshProUGUI textElec1;
    public TextMeshProUGUI textElec2;
    // Start is called before the first frame update
    void Start()
    {
        readJSON(Application.dataPath + "/JSON/situations.json");
        setAll();
    }

    private void readJSON(string jsonFile)
    {
        string jsonData=File.ReadAllText(jsonFile, System.Text.Encoding.UTF8);
        SituationList list=JsonUtility.FromJson<SituationList>(jsonData);
        situations = new Situation[list.situations.Length];
        situations = list.situations;
    }

    public void setAll()
    {
        int i = Random.Range(0, situations.Length);
        textSituation.text = situations[i].situation;
        textElec1.text = situations[i].elec1;
        textElec2.text = situations[i].elec2;
        gameManager.setStatsText(true, situations[i].stat1Left, situations[i].stat2Left, situations[i].stat3Left, situations[i].stat4Left);
        gameManager.setStatsText(false, situations[i].stat1Right, situations[i].stat2Right, situations[i].stat3Right, situations[i].stat4Right);

    }


}
