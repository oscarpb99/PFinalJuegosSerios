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

    public TextMeshProUGUI textSituation;
    public TextMeshProUGUI textElec1;
    public TextMeshProUGUI textElec2;
    public UnityEngine.UI.Image imagenSituation;

    private Dictionary<string, Sprite> mapSprite = new Dictionary<string, Sprite>();

    public static Situations Instance { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        readJSON(Application.streamingAssetsPath + "/JSON/situations.json");
        setAll();
    }
    

    private void readJSON(string jsonFile)
    {
        string jsonData=File.ReadAllText(jsonFile, System.Text.Encoding.UTF8);
        SituationList list=JsonUtility.FromJson<SituationList>(jsonData);
        situations = list.situations;
    }

    public void setAll()
    {
        int i = Random.Range(0, situations.Length);
        textSituation.text = situations[i].situation;
        textElec1.text = situations[i].elec1;
        textElec2.text = situations[i].elec2;
        GameManager.Instance.setStatsText(true, situations[i].stat1Left, situations[i].stat2Left, situations[i].stat3Left, situations[i].stat4Left);
        GameManager.Instance.setStatsText(false, situations[i].stat1Right, situations[i].stat2Right, situations[i].stat3Right, situations[i].stat4Right);
        imagenSituation.sprite = GetSprite(situations[i].image);
        
    }

    private Sprite GetSprite(string imageName)
    {
        if (!mapSprite.ContainsKey(imageName))
        {
            Sprite sprite = Resources.Load<Sprite>(imageName);
            if (sprite != null)
            {
                mapSprite[imageName] = sprite; // Almacenar en el diccionario
            }
            else
            {
                Debug.LogWarning("Imagen no encontrada en los recursos: " + imageName);
            }
        }
        return mapSprite.ContainsKey(imageName) ? mapSprite[imageName] : null;
    }


}
