using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Situations : MonoBehaviour
{
    public Situation[] situations;
    // Start is called before the first frame update
    void Start()
    {
        situations=new Situation[1];
        readJSON(Application.dataPath + "/JSON/situations.json");
    }

    private void readJSON(string jsonFile)
    {
        string jsonData=File.ReadAllText(jsonFile);
        for (int i = 0; i < situations.Length; i++)
        {
            situations[i]=JsonUtility.FromJson<Situation>(jsonData);
          
        }
    }

    
}
