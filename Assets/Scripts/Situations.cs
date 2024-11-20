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
        }
        readJSON(Application.streamingAssetsPath + "/JSON/situations.json");
    }
    

    private void readJSON(string jsonFile)
    {
        string jsonData=File.ReadAllText(jsonFile, System.Text.Encoding.UTF8);
        SituationList list=JsonUtility.FromJson<SituationList>(jsonData);
        situationManager.situations = list.situations;
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
