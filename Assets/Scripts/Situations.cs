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

[System.Serializable]
public class TutorialList
{
    public TutorialCard[] cards;
}

public class Situations : MonoBehaviour
{
    [SerializeField] SituationManager situationManager;
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
           // DontDestroyOnLoad(gameObject);
        }
        readJSON(Application.streamingAssetsPath + "/JSON/situations.json");
        readJSON(Application.streamingAssetsPath + "/JSON/specificSituations.json");
        readJSONTutorial(Application.streamingAssetsPath + "/JSON/tutorial.json");

    }
    

    private void readJSON(string jsonFile)
    {
        string jsonData=File.ReadAllText(jsonFile, System.Text.Encoding.UTF8);
        SituationList list=JsonUtility.FromJson<SituationList>(jsonData);
        if(situationManager.situations.Length <= 0)
        {
            situationManager.situations = list.situations;
        }
        else
        {
            situationManager.specificSituations = list.situations;
        }
    }

    private void readJSONTutorial(string jsonFile)
    {
        string jsonData = File.ReadAllText(jsonFile, System.Text.Encoding.UTF8);
        TutorialList list = JsonUtility.FromJson<TutorialList>(jsonData);
        situationManager.tutorialCards = list.cards;
    }

    public Sprite GetSprite(string imageName)
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
